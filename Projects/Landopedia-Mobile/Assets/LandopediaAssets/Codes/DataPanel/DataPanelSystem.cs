using UnityEngine;

namespace Landopedia
{
    public class DataPanelSystem : MonoBehaviour
    {
        private static DataPanelSystem instance;
        public static DataPanelSystem Instance => instance;

        public static DataMessageHandler ErrorMessageHandler => (Resources.Load("ErrorMessageHandler") as DataMessageHandler);

        [SerializeField] private Transform dataPanelTransform;
        [SerializeField] private DataPanel dataPanelPrefab;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
        }

        public void ClearDataPanel()
        {
            foreach (Transform dataPanel in dataPanelTransform)
                Destroy(dataPanel.gameObject);
		}

        public DataPanel CreateDataPanel(DataPanelStruct dataPanelStruct)
		{
			DataPanel dataPanel = Instantiate(dataPanelPrefab, dataPanelTransform);
            dataPanel.SetDataPanel(dataPanelStruct);
            return dataPanel;
        }
    }
}
