using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wave_Display_Logic : MonoBehaviour
{
    [SerializeField] private float max_time_display = 30.0f;
    [SerializeField] private EnemyManager enemy_man;
    [SerializeField] private int current_time = 0;
    [SerializeField] private Text wave_text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        current_time = Mathf.CeilToInt(enemy_man.SpawnInterval - enemy_man.ElapsedTime);

        if (current_time <= max_time_display)
        {
            wave_text.text = "Next Wave: " + current_time;
        }
        else
        {
            wave_text.text = "";
        }
    }
}
