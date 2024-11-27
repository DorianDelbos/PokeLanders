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

        [Header("General")]
        // Background
        [SerializeField] private Image backgroundColor;
        // Species
        [SerializeField] private TMP_Text speciesTextMesh;
        // Sprite
        [SerializeField] private Image spriteLander;
        // Types
        [SerializeField] private Transform typeTransform;
        [SerializeField] private GameObject typePrefab;

        [Header("About")]
        // Description
        [SerializeField] private TMP_Text descriptionTextMesh;
        // Height & Weight
        [SerializeField] private TMP_Text heightWeightTextMesh;

        [Header("Stats")]
        [SerializeField] private Slider hpSlider;
        [SerializeField] private TMP_Text hpTextMesh;
        [SerializeField] private Slider attackSlider;
        [SerializeField] private TMP_Text attackTextMesh;
        [SerializeField] private Slider defenseSlider;
        [SerializeField] private TMP_Text defenseTextMesh;
        [SerializeField] private Slider specialAttackSlider;
        [SerializeField] private TMP_Text specialAttackTextMesh;
        [SerializeField] private Slider specialDefenseSlider;
        [SerializeField] private TMP_Text specialDefenseTextMesh;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private TMP_Text speedTextMesh;

        [Header("Moves")]
        [SerializeField] private GameObject movePrefab;
        [SerializeField] private Transform moveTransform;

        private void Awake()
        {
            instance = this;
        }

        public void SetLander(Lander.Module.API.Lander lander)
        {
            SetGeneral(lander);
            SetAbout(lander);
            SetStats(lander);
            SetMoves(lander);
        }

        private async void SetGeneral(Lander.Module.API.Lander lander)
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
        }

        private void SetAbout(Lander.Module.API.Lander lander)
        {
            // Description
            descriptionTextMesh.text = lander.description;
            // Height & Weight
            heightWeightTextMesh.text = $"Height<color=#000000>  \\t{LanderUtilities.GetHeightInInches(lander.base_height)}    \\t{LanderUtilities.GetHeightInMeters(lander.base_height)}</color>\\r\\nWeight<color=#000000> \\t{LanderUtilities.GetWeightInPounds(lander.base_weight)}  \\t{LanderUtilities.GetWeightInKilograms(lander.base_weight)}</color>";
        }

        private void SetStats(Lander.Module.API.Lander lander)
        {
            byte minVal = byte.MaxValue;
            byte maxVal = 0;

            foreach (Stats stat in lander.stats)
            {
                if (maxVal < stat.base_stat)
                    maxVal = stat.base_stat;
                if (minVal > stat.base_stat)
                    minVal = stat.base_stat;
            }

            byte hpVal = lander.stats[0].base_stat;
            hpSlider.value = RemapStatToRange(hpVal, minVal, maxVal);
            hpTextMesh.text = $"Health point : {hpVal}";

            byte attackVal = lander.stats[1].base_stat;
            attackSlider.value = RemapStatToRange(attackVal, minVal, maxVal);
            attackTextMesh.text = $"Attack : {attackVal}";

            byte defenseVal = lander.stats[2].base_stat;
            defenseSlider.value = RemapStatToRange(defenseVal, minVal, maxVal);
            defenseTextMesh.text = $"Defense : {defenseVal}";

            byte specialAttackVal = lander.stats[3].base_stat;
            specialAttackSlider.value = RemapStatToRange(specialAttackVal, minVal, maxVal);
            specialAttackTextMesh.text = $"Special Attack : {specialAttackVal}";

            byte specialDefenseVal = lander.stats[4].base_stat;
            specialDefenseSlider.value = RemapStatToRange(specialDefenseVal, minVal, maxVal);
            specialDefenseTextMesh.text = $"Special Defense : {specialDefenseVal}";

            byte speedVal = lander.stats[5].base_stat;
            speedSlider.value = RemapStatToRange(speedVal, minVal, maxVal);
            speedTextMesh.text = $"Speed : {speedVal}";
        }

        private void SetMoves(Lander.Module.API.Lander lander)
        {
            foreach (Moves move in lander.moves)
            {
                GameObject instance = Instantiate(movePrefab, moveTransform);
                instance.GetComponentInChildren<TMP_Text>().text = $"{move.move}\r\n<color=#F5F5F5><size=12>Learn at level {move.move_learned_details.level_learned_at}</size></color>";
            }
        }

        private float RemapStatToRange(byte value, byte minVal, byte maxVal)
        {
            float normalizedValue = (float)(value - minVal) / (maxVal - minVal);
            normalizedValue = Mathf.Clamp01(normalizedValue);
            float remappedValue = Mathf.Lerp(0.25f, 0.75f, normalizedValue);

            return remappedValue;
        }
    }
}
