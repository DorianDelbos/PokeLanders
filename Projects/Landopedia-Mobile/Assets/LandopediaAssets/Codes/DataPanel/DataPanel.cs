using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Landopedia
{
    public class DataPanel : MonoBehaviour
    {
        [SerializeField] private Transform buttonTransform;
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private Button buttonDataPrefab;

        public void SetDataPanel(DataPanelStruct dataPanel)
        {
            textMesh.text = dataPanel.text;
            foreach (DataPanelStruct.Button button in dataPanel.buttons)
            {
                Button instance = Instantiate(buttonDataPrefab, buttonTransform);
                instance.transform.Find("Text").GetComponent<TMP_Text>().text = button.text;
                instance.onClick.AddListener(() => button.action?.Invoke());
            }
        }
    }
}
