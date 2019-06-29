using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Menu : MonoBehaviour
{
    private bool is_paused = false;
    [SerializeField] private Canvas pause_canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            is_paused = !is_paused;
        }

        if (is_paused)
        {
            pause_canvas.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            pause_canvas.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }
}
