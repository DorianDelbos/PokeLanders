using dgames.nfc;
using dgames.Tasks;
using Landers;
using Landers.API;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LanderFighter
{
    public class ReadNfcTask : TaskBase
    {
        /// <inheritdoc/>
        public override async Task Run()
        {
            List<byte> bytes = new List<byte>();

            // Read the NFC tag
            var tagResult = NFCSystem.ReadTag();
            await tagResult.WaitOperation();
            bytes.AddRange(tagResult.Result);

            // Read the first block
            var block1Result = NFCSystem.ReadBlock(1, 0);
            await block1Result.WaitOperation();
            bytes.AddRange(block1Result.Result);

            // Read the second block
            var block2Result = NFCSystem.ReadBlock(2, 0);
            await block2Result.WaitOperation();
            bytes.AddRange(block2Result.Result);

            // Read the third block
            var block3Result = NFCSystem.ReadBlock(0, 1);
            await block3Result.WaitOperation();
            bytes.AddRange(block3Result.Result);

            LanderDataNFC dataNFC = new LanderDataNFC(bytes.ToArray());

            UserLanderManager.Instance.SetLander(new LanderData(dataNFC, LanderRepository.Instance.GetById(dataNFC.Id)));
        }
    }
}
