#if PLATFORM_ANDROID
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace dgames.nfc
{
    public static partial class NFCSystem
    {
		#region PROPERTIES

		private static readonly string NfcTechMifareClassic = "android.nfc.tech.MifareClassic";
        private static readonly string NfcTagExtra = "android.nfc.extra.TAG";
        private static readonly byte[] DefaultKey = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        private static readonly Int32 FLAG_RECEIVER_REPLACE_PENDING = 0x20000000;
        private static readonly Int32 FLAG_MUTABLE = 0x02000000;

        private static AndroidJavaObject activity;
        private static AndroidJavaObject nfcAdapter;

		#endregion

		#region MAIN

		private static async Task<byte[]> ReadTagAsync(CancellationToken cancellationToken)
        {
            try
            {
                InitializeAndroidNFC();

                AndroidJavaObject tag = await WaitForNfcTagAsync(cancellationToken);
                byte[] tagId = tag?.Call<byte[]>("getId") ?? throw new Exception("Failed to retrieve NFC tag ID.");

                return tagId;
            }
            finally
            {
                ResetNfcProcess();
            }
        }

        private static async Task<byte[]> ReadBlockAsync(int block, int sector, CancellationToken cancellationToken)
        {
            try
            {
                InitializeAndroidNFC();

                AndroidJavaObject mifareClassic = await WaitForMifareBlockAsync(block, sector, cancellationToken);
                byte[] blockData;

                try
                {
                    AuthenticateMifareClassic(mifareClassic, sector);
                    blockData = mifareClassic.Call<byte[]>("readBlock", block);
                }
                catch (Exception)
                {
                    throw new Exception($"Failed to read NFC block {block} in sector {sector}. Ensure the card remains on the reader.");
                }
                finally
                {
                    mifareClassic?.Call("close");
                }

                return blockData;
            }
            finally
            {
                ResetNfcProcess();
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private static async Task<byte[]> WriteBlockAsync(int block, int sector, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{
            throw new NotImplementedException();
		}

		#endregion

		#region COMMON

		private static async Task<AndroidJavaObject> WaitForNfcTagAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (TryGetNfcTag(out AndroidJavaObject tag))
                    return tag;

                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException("NFC tag detection was canceled or timed out.");

                await Task.Delay(100, cancellationToken);
            }
        }

        private static async Task<AndroidJavaObject> WaitForMifareBlockAsync(int block, int sector, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (TryGetMifareBlock(block, sector, out AndroidJavaObject mifareClassic))
                    return mifareClassic;

                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException("MIFARE block detection was canceled or timed out.");

                await Task.Delay(100, cancellationToken);
            }
        }

        private static void AuthenticateMifareClassic(AndroidJavaObject mifareClassic, int sector)
        {
            if (!IsAuthenticated(mifareClassic, sector))
                throw new Exception($"Failed to authenticate NFC sector {sector}.");
        }

        private static bool IsAuthenticated(AndroidJavaObject mifareClassic, int sector)
        {
            mifareClassic.Call("connect");
            return mifareClassic.Call<bool>("authenticateSectorWithKeyA", sector, DefaultKey);
        }

        private static bool TryGetNfcTag(out AndroidJavaObject tag)
        {
            AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getIntent");
            tag = intent?.Call<AndroidJavaObject>("getParcelableExtra", NfcTagExtra);
            return tag != null;
        }

        private static bool TryGetMifareBlock(int block, int sector, out AndroidJavaObject mifareClassic)
        {
            mifareClassic = null;

            if (!TryGetNfcTag(out AndroidJavaObject tag) || !IsMifareClassicTag(tag))
                return false;

            mifareClassic = new AndroidJavaClass(NfcTechMifareClassic).CallStatic<AndroidJavaObject>("get", tag);
            return mifareClassic != null;
        }

        private static bool IsMifareClassicTag(AndroidJavaObject tag)
        {
            string[] techList = tag.Call<string[]>("getTechList");
            return techList.Contains(NfcTechMifareClassic);
        }

        private static void InitializeAndroidNFC()
        {
            activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
			nfcAdapter = new AndroidJavaClass("android.nfc.NfcAdapter").CallStatic<AndroidJavaObject>("getDefaultAdapter", activity);

            // Initialize NFC
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", activity, activity.Call<AndroidJavaObject>("getClass"));
            intent.Call<AndroidJavaObject>("addFlags", FLAG_RECEIVER_REPLACE_PENDING);

            using (AndroidJavaClass pendingIntentClass = new AndroidJavaClass("android.app.PendingIntent"))
            {
                AndroidJavaObject pendingIntent = pendingIntentClass.CallStatic<AndroidJavaObject>("getActivity", activity, 0, intent, FLAG_MUTABLE);
                AndroidJavaObject[] intentFilters = new AndroidJavaObject[]
                {
                    new AndroidJavaObject("android.content.IntentFilter", "android.nfc.action.TAG_DISCOVERED")
                };

                if (nfcAdapter != null)
                    nfcAdapter.Call("enableForegroundDispatch", activity, pendingIntent, intentFilters, null);
			}
		}

        private static void ResetNfcProcess()
        {
            if (nfcAdapter != null)
                nfcAdapter.Call("disableForegroundDispatch", activity);

            activity?.Call("setIntent", new AndroidJavaObject("android.content.Intent"));
        }

		#endregion
	}
}
#endif
