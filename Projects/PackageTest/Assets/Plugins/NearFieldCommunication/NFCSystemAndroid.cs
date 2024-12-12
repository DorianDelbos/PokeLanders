#if PLATFORM_ANDROID
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace dgames.nfc
{
    public partial class NFCSystem
    {
        private const string NfcTechMifareClassic = "android.nfc.tech.MifareClassic";
        private const string NfcTagExtra = "android.nfc.extra.TAG";
        private static readonly byte[] DefaultKey = { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
        private static readonly Int32 FLAG_RECEIVER_REPLACE_PENDING = 0x20000000;
        private static readonly Int32 FLAG_MUTABLE = 0x02000000;

        private static AndroidJavaObject activity;
        private static AndroidJavaObject nfcReader;
        private static AndroidJavaObject nfcAdapter;

        public async Task<byte[]> ReadTagAsync(CancellationToken cancellationToken)
        {
            InitializeAndroidNFC();

            AndroidJavaObject tag = await WaitForNfcTagAsync(cancellationToken);
            byte[] tagId = tag?.Call<byte[]>("getId") ?? throw new Exception("Failed to retrieve NFC tag ID.");

            ResetNfcProcess();

            return tagId;
        }

        public async Task<byte[]> ReadBlockAsync(int block, int sector, CancellationToken cancellationToken)
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
                ResetNfcProcess();
            }

            return blockData;
        }

        private async Task<AndroidJavaObject> WaitForNfcTagAsync(CancellationToken cancellationToken)
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

        private async Task<AndroidJavaObject> WaitForMifareBlockAsync(int block, int sector, CancellationToken cancellationToken)
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

        private bool TryGetNfcTag(out AndroidJavaObject tag)
        {
            AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getIntent");
            tag = intent?.Call<AndroidJavaObject>("getParcelableExtra", NfcTagExtra);
            return tag != null;
        }

        private bool TryGetMifareBlock(int block, int sector, out AndroidJavaObject mifareClassic)
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
            nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
            nfcAdapter = activity.Call<AndroidJavaObject>("getSystemService", "nfc");

            nfcReader.Call("setContext", activity);
            nfcReader.Call("onResume");

            //// ON RESUME
            //AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", activity, activity.Call<AndroidJavaObject>("getClass"));
            //intent.Call<AndroidJavaObject>("addFlags", FLAG_RECEIVER_REPLACE_PENDING);

            //using (AndroidJavaClass pendingIntentClass = new AndroidJavaClass("android.app.PendingIntent"))
            //{
            //    AndroidJavaObject pendingIntent = pendingIntentClass.CallStatic<AndroidJavaObject>("getActivity", activity, 0, intent, FLAG_MUTABLE);

            //    AndroidJavaObject[] intentFilters = new AndroidJavaObject[1];
            //    intentFilters[0] = new AndroidJavaObject("android.content.IntentFilter", "android.nfc.action.TAG_DISCOVERED");

            //    if (nfcAdapter != null)
            //        nfcAdapter.Call("enableForegroundDispatch", activity, pendingIntent, intentFilters, null);
            //}
        }

        private static void ResetNfcProcess()
        {
            //if (nfcAdapter != null)
            //    nfcAdapter.Call("disableForegroundDispatch", activity);

            nfcReader?.Call("onPause");
            activity?.Call("setIntent", new AndroidJavaObject("android.content.Intent"));
        }
    }
}
#endif
