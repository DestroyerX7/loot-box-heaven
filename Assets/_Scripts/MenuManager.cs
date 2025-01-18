using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Menu[] _menus;

    public void OpenMenu(string menuName)
    {
        foreach (Menu menu in _menus)
        {
            menu.gameObject.SetActive(menu.Name == menuName);
        }
    }

    public void OpenMenu(Menu menu)
    {
        OpenMenu(menu.Name);
    }
}
