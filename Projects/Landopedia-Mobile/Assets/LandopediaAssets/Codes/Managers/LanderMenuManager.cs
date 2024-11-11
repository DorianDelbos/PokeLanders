using Lander.Module.API;
using Lander.Module.Utilities;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Landopedia
{
    public class LanderMenuManager : MonoBehaviour
    {
        private static LanderMenuManager instance;
        public static LanderMenuManager current => instance;

        // Background
        [SerializeField] private Image backgroundColor;
        // Species
        [SerializeField] private TMP_Text speciesTextMesh;
        // Sprite
        [SerializeField] private Image spriteLander;
        // Types
        [SerializeField] private Transform typeTransform;
        [SerializeField] private GameObject typePrefab;
        // Description
        [SerializeField] private TMP_Text descriptionTextMesh;
        // Height & Weight
        [SerializeField] private TMP_Text heightWeightTextMesh;

        private void Awake()
        {
            instance = this;
        }

        public async void SetLander(Lander.Module.API.Lander lander)
        {
            // Background color
            ColorUtility.TryParseHtmlString(DataFetcher<Type>.FetchData($"api/v1/type/name/{lander.types.First()}").color, out Color color);
            backgroundColor.color = color;
            // Species and ID
            speciesTextMesh.text = $"<sprite=0>\t{lander.name}     #{lander.id.ToString("D3")}";
            // Sprite
            spriteLander.sprite = null;
            spriteLander.sprite = await WebSpriteUtilities.LoadSpriteFromUrlAsync(lander.sprite);
            // Type
            foreach (Transform t in typeTransform) Destroy(t.gameObject); // Clean
            lander.types.ForEach(t => {
                GameObject instanceType = Instantiate(typePrefab, typeTransform);
                ColorUtility.TryParseHtmlString(DataFetcher<Type>.FetchData($"api/v1/type/name/{t}").color, out Color colorType);
                instanceType.GetComponent<Image>().color = colorType;
                instanceType.transform.Find("Text").GetComponent<TMP_Text>().text = t;
            });
            // Description
            descriptionTextMesh.text = lander.description;
            // Height & Weight
            heightWeightTextMesh.text = $"Height<color=#000000>  \\t{LanderUtilities.GetHeightInInches(lander.base_height)}    \\t{LanderUtilities.GetHeightInMeters(lander.base_height)}</color>\\r\\nWeight<color=#000000> \\t{LanderUtilities.GetWeightInPounds(lander.base_weight)}  \\t{LanderUtilities.GetWeightInKilograms(lander.base_weight)}</color>";
        }
    }
}
