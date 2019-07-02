using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    private bool IsPaused = false;
    
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private bool IsDebug = true;

    [SerializeField] private string MenuSceneName;

    // Update is called once per frame
    void Update()
    {
        KeyCode pauseKey;
        if (IsDebug) pauseKey = KeyCode.P;
        else pauseKey = KeyCode.Escape;

        if (Input.GetKeyDown(pauseKey) || Input.GetKeyDown(KeyCode.Joystick1Button7)) {
            Pause(!IsPaused);
        }
    }

    public void Pause(bool pause) {
        IsPaused = pause;
        if (IsPaused) {
            PauseMenu.SetActive(true);
            Time.timeScale = 0.0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else {
            PauseMenu.SetActive(false);
            Time.timeScale = 1.0f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void QuitToMenu() {
        Pause(false);
        SceneManager.LoadScene(MenuSceneName);
    }

    public void QuitToDesktop() {
        Application.Quit();
    }
}
