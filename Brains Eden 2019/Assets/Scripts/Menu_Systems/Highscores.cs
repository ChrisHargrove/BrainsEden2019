using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highscores : MonoBehaviour
{
    [SerializeField] private List<Text> text_array;
    [SerializeField] private InputField name_field;
    [SerializeField] private Button name_add_button;
    private List<int> scores_array;
    private List<string> name_array;
    private bool are_scores_loaded = false;
    private bool has_score_updated = false;
    // Start is called before the first frame update
    void Start()
    {
        // Clears highscores on start, delete this when not testing or highscores will be lost
        PlayerPrefs.DeleteAll();

        // Unlocks the cursor incase it is locked
        Cursor.lockState = CursorLockMode.None;

        if (name_add_button != null)
        {
            name_add_button.onClick.AddListener(Add_Name_Clicked);
        }

        scores_array = new List<int>();
        name_array = new List<string>();

        foreach (var text_obj in text_array)
        {
            scores_array.Add(0);
            name_array.Add("");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!are_scores_loaded)
        {
            Load_Scores();
        }

        if (name_field.isFocused)
        {
            Debug.Log("Opening Keyboard");
            
            TouchScreenKeyboard.Open(name_field.text, TouchScreenKeyboardType.Default);
        }
    }

    private void Add_Name_Clicked()
    {
        GameObject score_saver = GameObject.FindGameObjectWithTag("Data");
        Score_Transfer score_t = score_saver.GetComponent<Score_Transfer>();

        if ((score_t != null) && (has_score_updated == false))
        {
            Add_Score(name_field.text, score_t.player_score);
            has_score_updated = true;
        }

        
    }

    public void Add_Score(string i_name, int i_score)
    {
        scores_array.Add(i_score);
        name_array.Add(i_name);

        // Sorting scores and names
        for (int i = 0; i < scores_array.Count - 1; i++)
        {
            for (int j = 0; j < scores_array.Count - 1; j++)
            {
                if (scores_array[j] <= scores_array[j + 1])
                {
                    
                    int temp_score = scores_array[j + 1];
                    scores_array[j + 1] = scores_array[j];
                    scores_array[j] = temp_score;

                    string temp_name = name_array[j + 1];
                    name_array[j + 1] = name_array[j];
                    name_array[j] = temp_name;
                }
            }
        }

        Set_Scores();
        Load_Scores();
    }

    private void Load_Scores()
    {
        for (int i = 0; i < text_array.Count; i++)
        {
            name_array[i] = PlayerPrefs.GetString("NAME:" + i, "BLANK");
            scores_array[i] = PlayerPrefs.GetInt("SCORE:" + i, 0);

            text_array[i].text = name_array[i] + ": " + scores_array[i];
        }

        are_scores_loaded = true;
    }

    private void Set_Scores()
    {
        for (int i = 0; i < text_array.Count; i++)
        {
            PlayerPrefs.SetString("NAME:" + i, name_array[i]);
            PlayerPrefs.SetInt("SCORE:" + i, scores_array[i]);
        }
        PlayerPrefs.Save();
    }
}
