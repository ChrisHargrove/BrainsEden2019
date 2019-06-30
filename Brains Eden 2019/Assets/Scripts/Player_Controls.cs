using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    NONE = 0,
    IDLE,
    WALKING_FORWARD,
    WALKING_BACKWARD,
    STRAFE_LEFT,
    STRAFE_RIGHT,
    ATTACK,
}

public class Player_Controls : MonoBehaviour
{
    CharacterController char_con;

    [SerializeField] private GameObject score_storage_object_prefab;
    private Score_Transfer score_comp;
    [SerializeField] private float move_speed;
    [SerializeField] private float gravity_force;
    [SerializeField] private float max_jump_force;
    private float current_jump_force = 0.0f;
    [SerializeField] private float jump_falloff;
    private float current_fall_speed = 0.0f;

    [SerializeField] private float turn_speed;
    [SerializeField] private GameObject attack_projectile;
    [SerializeField] private GameObject secondary_attack_projectile;
    [SerializeField] private float fire_speed;
    [SerializeField] private float spell_fire_offset; // The distance infront of the player which the projectile spawns

    [SerializeField] public int score;
    [SerializeField] public bool is_secondary_attack_enabled = true;

    private bool is_shooting = false;

    private PlayerState State;

    public Animator Animator;
    public Transform Rotator;

    private Vector3 movement_direction = new Vector3();

    private SoundManager soundManager;


    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();

        char_con = GetComponent<CharacterController>();

        GameObject score_object_instance = GameObject.FindGameObjectWithTag("Data");
        GameObject score_object;

        if (score_object_instance == null)
        {
            Debug.Log("NULL");
            score_object = Instantiate(score_storage_object_prefab);
        }
        else
        {
            Debug.Log("NOT NULL");
            score_object = score_object_instance;
        }
        score_comp = score_object.GetComponent<Score_Transfer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement_Controls();
        Player_Rotation();

        if ((Input.GetMouseButtonDown(0)) || ((Input.GetAxis("Shoot") > 0) && (!is_shooting)))
        {
            is_shooting = true;
            Attack_Spell(attack_projectile,transform.forward);
            State = PlayerState.ATTACK;
            soundManager.PlayForcePush();
        }

        if ((Input.GetMouseButtonDown(1)) || ((Input.GetAxis("Shoot_Secondary") > 0) && (!is_shooting)))
        {
            if (is_secondary_attack_enabled)
            {
                is_shooting = true;
                Attack_Spell(secondary_attack_projectile,transform.forward);
                State = PlayerState.ATTACK;
                soundManager.PlayFireball();
            }
        }

        if (Input.GetAxis("Shoot") == 0 && Input.GetAxis("Shoot_Secondary") == 0)
        {
            is_shooting = false;
        }

        Animator.SetInteger("State", (int)State);
    }

    void Attack_Spell(GameObject i_proj,Vector3 fire_direction)
    {
        GameObject created_obj = Instantiate(i_proj);
        Rigidbody rb = created_obj.GetComponent<Rigidbody>();
        created_obj.transform.position = transform.position + (fire_direction * spell_fire_offset);
        rb.AddForce(fire_direction * fire_speed);
    }

    void Player_Rotation()
    {
        transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X") + Input.GetAxis("X Look"), 0.0f) * Time.deltaTime * turn_speed);
        Rotator.Rotate(new Vector3(Input.GetAxis("Mouse Y") + Input.GetAxis("Y Look"), 0.0f, 0.0f) * Time.deltaTime * turn_speed * 0.1f);


        float minRotation = 0;
        float maxRotation = 55;
        Vector3 currentRotation = Rotator.localRotation.eulerAngles;
        if(currentRotation.x > 200) {
            currentRotation.x = Mathf.Clamp(currentRotation.x, 315, 360);
        }
        else
        {
            currentRotation.x = Mathf.Clamp(currentRotation.x, minRotation, maxRotation);
        }

        Rotator.localRotation = Quaternion.Euler(currentRotation);

    }

    Vector3 Controller_Movement_Controls()
    {
        return (transform.forward * Time.deltaTime * (move_speed/2) * -Input.GetAxis("Y Movement"))
            + (transform.right * Time.deltaTime * (move_speed / 2) * Input.GetAxis("X Movement"));
    }

    void Movement_Controls()
    {
        movement_direction = Controller_Movement_Controls();
        State = PlayerState.IDLE;

        // Forward and back
        if (Input.GetKey(KeyCode.W) || Input.GetAxis("Y Movement") < 0)
        {
            movement_direction += transform.forward * Time.deltaTime * move_speed;
            State = PlayerState.WALKING_FORWARD;

        }
        else if (Input.GetKey(KeyCode.S) || Input.GetAxis("Y Movement") > 0)
        {
            movement_direction -= transform.forward * Time.deltaTime * move_speed;
            State = PlayerState.WALKING_BACKWARD;
        }

        // Left and right
        if (Input.GetKey(KeyCode.D) || Input.GetAxis("X Movement") > 0)
        {
            movement_direction += transform.right * Time.deltaTime * move_speed;
            State = PlayerState.STRAFE_RIGHT;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetAxis("X Movement") < 0)
        {
            movement_direction -= transform.right * Time.deltaTime * move_speed;
            State = PlayerState.STRAFE_LEFT;
        }

        // Jumping
        if (char_con.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                current_fall_speed = 0.0f;
                current_jump_force = max_jump_force;
            }
        }
        else
        {
            current_fall_speed += gravity_force;
        }

        // Falling
        movement_direction.y -= (current_fall_speed * Time.deltaTime);
        if (current_jump_force > 0)
        {
            current_jump_force -= jump_falloff;
            movement_direction += transform.up * Time.deltaTime * current_jump_force;
        }

        char_con.Move(movement_direction);

    }

}
