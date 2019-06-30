using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_System : MonoBehaviour
{
    [SerializeField] ChainManager chain_man;
    [SerializeField] GameObject stick_text;
    [SerializeField] GameObject stick_bombs_text;
    [SerializeField] GameObject damage_text;
    [SerializeField] GameObject end_text;

    [SerializeField] GameObject normal_gremil;
    [SerializeField] GameObject bomb_gremil;
    private GameObject gremil_loner;

    [SerializeField] Player_Controls player_con;

    [SerializeField] private float end_message_time;
    [SerializeField] private string main_game_scene_name;
    private float current_time;

    enum States {BLANK,STICK_ENEMIES,STICK_BOMBS,DAMAGE_SPELLS,END_MESSAGE,START }
    [SerializeField] States current_state = States.BLANK;

    private bool has_state_inited = false;
    // Start is called before the first frame update
    void Start()
    {
        stick_text.SetActive(false);
        stick_bombs_text.SetActive(false);
        damage_text.SetActive(false);
        end_text.SetActive(false);
    }

    void Stick_State()
    {
        stick_text.SetActive(true);

        if (chain_man.Chains.Count >= 1)
        {
            current_state = States.STICK_BOMBS;
            has_state_inited = false;
        }
    }

    void Stick_Bombs_State()
    {
        if (!has_state_inited)
        {
            GameObject temp = Instantiate(bomb_gremil, new Vector3(0.0f, 21.0f, -57.0f),new Quaternion());
            temp.GetComponent<Enemy>().Initialize(null, chain_man);
            has_state_inited = true;
        }

        stick_text.SetActive(false);
        stick_bombs_text.SetActive(true);

        if (chain_man.Chains.Count <= 0)
        {
            current_state = States.DAMAGE_SPELLS;
            has_state_inited = false;
        }
    }

    void Damage_Spells_State()
    {
        if (!has_state_inited)
        {
            GameObject temp = Instantiate(normal_gremil, new Vector3(0.0f, 21.0f, -43.0f), new Quaternion());
            temp.GetComponent<Enemy>().Initialize(null, chain_man);
            gremil_loner = temp;
            player_con.is_secondary_attack_enabled = true;
            has_state_inited = true;
        }

        stick_bombs_text.SetActive(false);
        damage_text.SetActive(true);

        if (gremil_loner == null)
        {
            current_state = States.END_MESSAGE;
            has_state_inited = false;
            current_time = 0.0f;
        }
    }

    void End_Message_State()
    {
        current_time += Time.deltaTime;

        damage_text.SetActive(false);
        end_text.SetActive(true);

        if (current_time >= end_message_time)
        {
            current_state = States.START;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(current_state)
        {
            case States.BLANK:
                {
                    current_state = States.STICK_ENEMIES;
                    break;
                }
            case States.STICK_ENEMIES:
                {
                    Stick_State();
                    break;
                }
            case States.STICK_BOMBS:
                {
                    Stick_Bombs_State();
                    break;
                }
            case States.DAMAGE_SPELLS:
                {
                    Damage_Spells_State();
                    break;
                }
            case States.END_MESSAGE:
                {
                    End_Message_State();
                    break;
                }
            case States.START:
                {
                    SceneManager.LoadScene(main_game_scene_name);
                    break;
                }
        }
    }
}
