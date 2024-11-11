using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Landopedia
{
    public class MenuHandler : MonoBehaviour
    {
        [SerializeField] private List<Menu> menus = new List<Menu>();
        public List<Menu> Menus => menus;

        private void Start()
        {
            ChangeMenu(Menus.First());
        }

        public void ChangeMenu(Menu menu)
        {
            menus.ForEach(m => m.SetActive(m == menu ? true : false));
        }

        public void ChangeMenu(string menuTag)
        {
            menus.ForEach(m => m.SetActive(menuTag.ToLower() == m.MenuTag ? true : false));
        }
    }
}
