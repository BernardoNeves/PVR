using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] Menu[] menus;
    public static MenuManager Instance { get; private set; }

    void Awake()
    {
        if (Instance)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        foreach (Menu menu in menus)
        {
            if (menu.name == menuName)
                menu.Open();
            else
                menu.Close();
        }
    }

    public void CursorToggle(bool visible)
    {
        Cursor.visible = visible;

        if (visible)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
