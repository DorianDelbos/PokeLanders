using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Landopedia
{
    public class DataPanel : MonoBehaviour
    {
        public static DataPanel current;

        [SerializeField] private GameObject myPanel;
        [SerializeField] private Transform buttonTransform;
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private Button buttonDataPrefab;

        public bool Active { get => myPanel.activeSelf; set => myPanel.SetActive(value); }

        private void Awake()
            => current = this;

        public void Clear()
        {
            foreach (Transform child in buttonTransform)
                Destroy(child.gameObject);
        }

        public void SetText(string text)
            => textMesh.text = text;

        public void AddButton(string text, Action action)
        {
            Button instance = Instantiate(buttonDataPrefab, buttonTransform);
            instance.transform.Find("Text").GetComponent<TMP_Text>().text = text;
            instance.onClick.AddListener(() => action?.Invoke());
        }
    }
}
