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
    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(Start_Clicked);
        scores_button.onClick.AddListener(Scores_Clicked);
        quit_button.onClick.AddListener(Quit_Clicked);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Start_Clicked()
    {
        // Change to main game scene
        SceneManager.LoadScene("Player_Test");
    }
    void Scores_Clicked()
    {
        // Show scores
    }
    void Quit_Clicked()
    {
        // Quit game
        Application.Quit();
    }
}
