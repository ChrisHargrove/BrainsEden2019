using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Initials_Setter : MonoBehaviour
{
    [SerializeField] private Text initial_text;
    [SerializeField] private Text underline_text;
    [SerializeField] private Highscores h_score;

    [SerializeField] private string main_menu_name;

    private bool is_focused = true;
    private int initial_index = 0;
    private int[] initials = new int[3];

    [SerializeField] private float flash_time;
    private float current_time;
    private bool is_char_visible = true;

    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private bool is_input_detected = false;

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime;

        // Char selection system
        if (!is_input_detected)
        {
            if (Input.GetAxis("Y Movement") > 0 || Input.GetKeyDown(KeyCode.S))
            {
                is_input_detected = true;
                initials[initial_index]--;
            }
            else if (Input.GetAxis("Y Movement") < 0 || Input.GetKeyDown(KeyCode.W))
            {
                is_input_detected = true;
                initials[initial_index]++;
            }

            if (Input.GetAxis("X Movement") > 0 || Input.GetKeyDown(KeyCode.D))
            {
                is_input_detected = true;
                initial_index++;
            }
            else if (Input.GetAxis("X Movement") < 0 || Input.GetKeyDown(KeyCode.A))
            {
                is_input_detected = true;
                initial_index--;
            }
        }
        
        if ((Input.GetAxis("X Movement") == 0 && Input.GetAxis("Y Movement") == 0) &&
                !(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)))
        {
            is_input_detected = false;
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            h_score.Add_Name_Clicked();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            SceneManager.LoadScene(main_menu_name);
        }


        if (initial_index >= initials.Length)
        {
            initial_index = 0;
        }
        else if (initial_index < 0)
        {
            initial_index = initials.Length - 1;
        }

        if (initials[initial_index] >= alphabet.Length)
        {
            initials[initial_index] = 0;
        }
        else if (initials[initial_index] < 0)
        {
            initials[initial_index] = alphabet.Length - 1;
        }

        if (current_time > flash_time)
        {
            current_time = 0.0f;
            is_char_visible = !is_char_visible;
        }

        underline_text.text = "";
        string text_to_draw = "";
        for(int i = 0; i < initials.Length; i++)
        {
            text_to_draw += alphabet[initials[i]].ToString();
            if (is_char_visible)
            {
                underline_text.text += "  ";
            }
            else if (i == initial_index)
            {
                underline_text.text += "_";
            }
            else
            {
                underline_text.text += "  ";
            }
        }

        initial_text.text = text_to_draw;
    }
}
