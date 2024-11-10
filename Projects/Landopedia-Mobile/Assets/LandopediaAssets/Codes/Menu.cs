using UnityEngine;

namespace Landopedia
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private string menuTag;

        public string MenuTag => menuTag.ToLower();

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}
