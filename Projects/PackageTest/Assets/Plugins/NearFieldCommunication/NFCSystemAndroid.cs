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

        public async Task<byte[]> ReadTagAsync(CancellationToken cancellationToken)
        {
            using AndroidJavaObject activity = GetCurrentActivity();
            using AndroidJavaObject nfcReader = InitializeNfcReader(activity);

            AndroidJavaObject tag = await WaitForNfcTagAsync(activity, cancellationToken);
            byte[] tagId = tag?.Call<byte[]>("getId") ?? throw new Exception("Failed to retrieve NFC tag ID.");

            ResetNfcProcess(nfcReader, activity);

            return tagId;
        }

        public async Task<byte[]> ReadBlockAsync(int block, int sector, CancellationToken cancellationToken)
        {
            using AndroidJavaObject activity = GetCurrentActivity();
            using AndroidJavaObject nfcReader = InitializeNfcReader(activity);
            AndroidJavaObject mifareClassic = await WaitForMifareBlockAsync(activity, block, sector, cancellationToken);
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
                ResetNfcProcess(nfcReader, activity);
            }

            return blockData;
        }

        private static AndroidJavaObject GetCurrentActivity()
            => new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");

        private static AndroidJavaObject InitializeNfcReader(AndroidJavaObject activity)
        {
            AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
            nfcReader.Call("setContext", activity);
            nfcReader.Call("onResume");
            return nfcReader;
        }

        private async Task<AndroidJavaObject> WaitForNfcTagAsync(AndroidJavaObject activity, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (TryGetNfcTag(activity, out AndroidJavaObject tag))
                    return tag;

                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException("NFC tag detection was canceled or timed out.");

                await Task.Delay(100, cancellationToken);
            }
        }

        private async Task<AndroidJavaObject> WaitForMifareBlockAsync(AndroidJavaObject activity, int block, int sector, CancellationToken cancellationToken)
        {
            while (true)
            {
                if (TryGetMifareBlock(activity, block, sector, out AndroidJavaObject mifareClassic))
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

        private static void ResetNfcProcess(AndroidJavaObject nfcReader, AndroidJavaObject activity)
        {
            nfcReader?.Call("onPause");
            activity?.Call("setIntent", new AndroidJavaObject("android.content.Intent"));
        }

        private bool TryGetNfcTag(AndroidJavaObject activity, out AndroidJavaObject tag)
        {
            AndroidJavaObject intent = activity.Call<AndroidJavaObject>("getIntent");
            tag = intent?.Call<AndroidJavaObject>("getParcelableExtra", NfcTagExtra);
            return tag != null;
        }

        private bool TryGetMifareBlock(AndroidJavaObject activity, int block, int sector, out AndroidJavaObject mifareClassic)
        {
            mifareClassic = null;

            if (!TryGetNfcTag(activity, out AndroidJavaObject tag) || !IsMifareClassicTag(tag))
                return false;

            mifareClassic = new AndroidJavaClass(NfcTechMifareClassic).CallStatic<AndroidJavaObject>("get", tag);
            return mifareClassic != null;
        }

        private static bool IsMifareClassicTag(AndroidJavaObject tag)
        {
            string[] techList = tag.Call<string[]>("getTechList");
            return techList.Contains(NfcTechMifareClassic);
        }
    }
}
#endif
