using dgames.http;
using dgames.Utils;
using Landers.API;
using Landers.Utils;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Landopedia
{
    public class LanderMenuHandler : MonoBehaviour
    {
        [Serializable]
        private struct BaseStatSlider
        {
            public Slider slider;
            public TMP_Text textMeshPro;
            public string text;
        }

        [Serializable]
        private class SubMenu
        {
            public void OpenMenu(SubMenu[] allMenu)
            {
                foreach (SubMenu menu in allMenu)
                {
                    if (menu == this)
                        menu.OpenMenu();
                    else
                        menu.CloseMenu();
                }
            }

            private void OpenMenu()
            {
                Color color = TypeRepository.Instance.GetByName(lander.types.First()).color.ToColor();

                ApplyColor(color);
                panel.SetActive(true);
            }

            private void CloseMenu()
            {
                Color color = new Color(1.0f, 0.9732704f, 0.9481132f, 1.0f);

                ApplyColor(color);
                panel.SetActive(false);
            }

            private void ApplyColor(Color color)
            {
                ColorBlock newColorBlock = button.colors;
                newColorBlock.normalColor = color;
                newColorBlock.pressedColor = color;
                newColorBlock.selectedColor = color;
                button.colors = newColorBlock;
            }

            public Button button;
            public GameObject panel;
        }

        public static LanderMenuHandler current;
        public static Lander lander;

        [Header("Sub Menu")]
        [SerializeField] private SubMenu[] subsMenu;

        [Header("Initialize objects")]
        [SerializeField] private TMP_Text speciesIdTextMesh;
        [SerializeField] private RawImage landerTextureImage;
        [SerializeField] private GameObject[] typesGameObject;
        [SerializeField] private TMP_Text descriptionTextMesh;
        [SerializeField] private TMP_Text heightWeightTextMesh;
        [SerializeField] private BaseStatSlider[] baseStatSlider;
        [SerializeField] private Transform movesContent;
        [SerializeField] private GameObject movePrefab;

        private void Awake()
        {
            current = this;
            InitializeLanderPage();
            InitializeMenu();
        }

        private void InitializeMenu()
        {
            subsMenu.First().OpenMenu(subsMenu);

            foreach (SubMenu menu in subsMenu)
            {
                menu.button.onClick.AddListener(() =>
                {
                    menu.OpenMenu(subsMenu);
                });
            }
        }

        private void InitializeLanderPage()
        {
            if (lander == null)
            {
                Debug.LogError("Lander is null !");
                return;
            }

            InitializeMain();
            InitializeAbout();
            InitializeStats();
            InitializeMoves();
        }

        private void InitializeMain()
        {
            speciesIdTextMesh.text = $"<sprite=0>\t{lander.name}\t#{lander.id.ToString("D3")}";
            InitializeTexture();
            for (int i = 0; i < lander.types.Count; i++)
                SetType(typesGameObject[i], TypeRepository.Instance.GetByName(lander.types[i]));
        }

        private void InitializeAbout()
        {
            descriptionTextMesh.text = lander.description;
            heightWeightTextMesh.text = $"Height<color=#000000>  \t{LanderUtils.GetHeightInInches(lander.base_height)}    \t{LanderUtils.GetHeightInMeters(lander.base_height)}</color>\r\nWeight<color=#000000> \t{LanderUtils.GetWeightInPounds(lander.base_weight)}  \t{LanderUtils.GetWeightInKilograms(lander.base_weight)}</color>";
        }

        private void InitializeStats()
        {
            byte minVal = byte.MaxValue;
            byte maxVal = 0;

            foreach (BaseStats stat in lander.stats)
            {
                if (maxVal < stat.base_stat) maxVal = stat.base_stat;
                if (minVal > stat.base_stat) minVal = stat.base_stat;
            }

            for (int i = 0; i < lander.stats.Count; i++)
            {
                byte val = lander.stats[i].base_stat;
                baseStatSlider[i].slider.value = RemapStatToRange(val, minVal, maxVal);
                baseStatSlider[i].textMeshPro.text = $"{baseStatSlider[i].text} : {val}";
            }
        }

        private void InitializeMoves()
        {
            foreach (Moves move in lander.moves)
            {
                GameObject instance = Instantiate(movePrefab, movesContent);
                instance.GetComponentInChildren<TMP_Text>().text = $"{move.move}\r\n<color=#F5F5F5><size=12>LEARN AT LEVEL {move.move_learned_details.level_learned_at.ToString("D3")}</size></color>";
            }
        }

        private async void InitializeTexture()
        {
            AsyncOperationWeb<Texture2D> op = WebService.AsyncRequestImage(lander.sprite);

            await op.AwaitCompletion();

            if (!op.IsError)
                landerTextureImage.texture = op.Result;
            else
                Debug.LogError(op.Exception.Message, this);
        }

        private void SetType(GameObject typeObject, Landers.API.Type type)
        {
            typeObject.SetActive(true);
            typeObject.GetComponent<Image>().color = type.color.ToColor();
            typeObject.GetComponentInChildren<TMP_Text>().text = type.name;
        }

        private float RemapStatToRange(byte value, byte minVal, byte maxVal)
        {
            float normalizedValue = (float)(value - minVal) / (maxVal - minVal);
            normalizedValue = Mathf.Clamp01(normalizedValue);
            float remappedValue = Mathf.Lerp(0.25f, 0.75f, normalizedValue);

            return remappedValue;
        }

        public void BackMain()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
