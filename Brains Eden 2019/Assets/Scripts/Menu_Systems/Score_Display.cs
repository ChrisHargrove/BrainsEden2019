using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_Display : MonoBehaviour
{
    [SerializeField] private Text score_display;
    [SerializeField] private string score_prefix;

    private Score_Transfer score_system;
    private int current_score = 0; // The true current score the player has
    private int displayed_score = 0; // The displayed score used to increment the display to the correct score over time
    // Start is called before the first frame update
    void Start()
    {
        score_system = GameObject.FindGameObjectWithTag("Data").GetComponent<Score_Transfer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (score_system != null)
        {
            current_score = score_system.player_score;
            score_display.text = score_prefix + displayed_score.ToString();
        }

        if (current_score > displayed_score)
        {
            int increase_value = Mathf.CeilToInt((current_score - displayed_score) / 20);
            if (increase_value == 0)
            {
                increase_value = 1;
            }
            displayed_score += increase_value;
        }
    }
}
