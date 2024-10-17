using UnityEngine;
using UnityEngine.SceneManagement;

public class PcManager : MonoBehaviour
{
    private LanderData LanderData => GameManager.instance.Landers[0];

    [SerializeField] LanderDisplayHandler landerDisplayHandler;

    private void OnEnable()
    {
        NfcModule.onNewNfcDetect += SetData;
        NfcModule.onNfcRemove += ResetData;
    }

    private void OnDisable()
    {
        NfcModule.onNewNfcDetect -= SetData;
        NfcModule.onNfcRemove -= ResetData;
    }

    private void SetData(LanderDataNFC data)
    {
        LanderData.Nfc = data;
        landerDisplayHandler.SetMesh(LanderData.mesh);
    }

    private void ResetData(LanderDataNFC data)
    {
        LanderData.Nfc = null;
        LanderData.name = string.Empty;
        LanderData.description = string.Empty;
        LanderData.types.Clear();
        landerDisplayHandler.SetMesh(null);
    }

    public void StartTrainLander()
    {
        GameManager.instance.Landers[1].SetRandom(LanderData.Nfc.currentLevel);
        SceneManager.LoadScene("FightScene");
    }
}
