using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Death_Menu : MonoBehaviour
{
    [SerializeField] private Button back_to_menu;
    [SerializeField] private string main_menu_name;

    // Start is called before the first frame update
    void Start()
    {
        back_to_menu.onClick.AddListener(Back_To_Menu_Click);
    }
    
    void Back_To_Menu_Click()
    {
        SceneManager.LoadScene(main_menu_name);
    }
}
