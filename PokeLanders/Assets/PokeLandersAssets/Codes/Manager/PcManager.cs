using UnityEngine;
using UnityEngine.SceneManagement;

public class PcManager : MonoBehaviour
{
    private LanderData LanderData { get => GameManager.instance.Landers[0]; set => GameManager.instance.Landers[0] = value; }

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
        LanderData = new LanderData(data);
        landerDisplayHandler.SetMesh(LanderData.Mesh);
    }

    private void ResetData(LanderDataNFC data)
    {
        LanderData = null;
		landerDisplayHandler.SetMesh(null);
    }

    public void StartTrainLander()
    {
        GameManager.instance.Landers[1] = LanderData.CreateRandomLander(LanderData.Level);
        SceneManager.LoadScene("FightScene");
    }
}
