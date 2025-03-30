using GLTFast;
using Landers;
using Landers.Utils;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LanderFighter
{
    public class MainMenuHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private GltfAsset gltfAsset;

        private void Start()
        {
            TaskHandler.Instance.OnRepositoryLoaded += InitializeLanderData;
        }

        private void InitializeLanderData()
        {
            LanderData userLanderData = UserLanderManager.Instance.UserLanderData;
            descriptionText.text = $"Landopedia <size=9>No.</size>{userLanderData.Id}\\r\\n{userLanderData.Species} \\t {(userLanderData.IsMale ? "<sprite=1>" : "<sprite=2>")} {(userLanderData.IsShiny ? "<sprite=0>" : string.Empty)}\\r\\n{userLanderData.Types.ElementAtOrDefault(0) ?? string.Empty} {userLanderData.Types.ElementAtOrDefault(1) ?? string.Empty}\\r\\n<size=9><align=\"justified\">\\r\\n{userLanderData.Description}\\r\\n</align></size>\\r\\nHeight \\t {LanderUtils.GetHeightInInches(userLanderData.Height)}\\r\\nWeight \\t {LanderUtils.GetWeightInPounds(userLanderData.Weight)}";
            nameText.text = userLanderData.Name;
            healthBar.SetHealth(userLanderData.Pv, userLanderData.MaxHp);
            nameText.GetComponent<ContentSizeFitter>().SetLayoutHorizontal();

            TaskHandler.Instance.LoadModel3D(gltfAsset, userLanderData.ModelUrl);
        }

        public void LoadBattleScene()
        {
            SceneManager.LoadScene("BattleScene");
        }

        public void HealthLander()
        {
            UserLanderManager.Instance.UserLanderData.Pv = UserLanderManager.Instance.UserLanderData.MaxHp;
            healthBar.SetHealth(UserLanderManager.Instance.UserLanderData.Pv, UserLanderManager.Instance.UserLanderData.MaxHp);
        }
    }
}
