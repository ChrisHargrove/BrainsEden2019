using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_Menu_Control : MonoBehaviour
{
    [SerializeField] Button start_button;
    [SerializeField] Button scores_button;
    [SerializeField] Button quit_button;

    [SerializeField] GameObject highscore_board_obj;
    [SerializeField] Button back_button;

    [SerializeField] highscore_show score_shower;

    [SerializeField] string game_scene_name;

    [SerializeField] UnityEngine.EventSystems.EventSystem event_sys;

    private GameObject current_event_object;
    
    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(Start_Clicked);
        scores_button.onClick.AddListener(Scores_Clicked);
        quit_button.onClick.AddListener(Quit_Clicked);
        back_button.onClick.AddListener(Back_Clicked);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (event_sys.currentSelectedGameObject == null)
        {
            event_sys.SetSelectedGameObject(current_event_object);
        }
        else
        {
            current_event_object = event_sys.currentSelectedGameObject;
        }
    }

    void Start_Clicked()
    {
        // Change to main game scene
        SceneManager.LoadScene(game_scene_name);
    }
    void Scores_Clicked()
    {
        event_sys.SetSelectedGameObject(back_button.gameObject);
    }
    void Back_Clicked()
    {
        event_sys.SetSelectedGameObject(scores_button.gameObject);
    }
    void Quit_Clicked()
    {
        // Quit game
        Application.Quit();
    }
}
