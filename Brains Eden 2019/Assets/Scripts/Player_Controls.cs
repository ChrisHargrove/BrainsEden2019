using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private bool is_shooting = false;


    private Vector3 movement_direction = new Vector3();


    // Start is called before the first frame update
    void Start()
    {
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
        }

        if ((Input.GetMouseButtonDown(1)) || ((Input.GetAxis("Shoot_Secondary") > 0) && (!is_shooting)))
        {
            is_shooting = true;
            Attack_Spell(secondary_attack_projectile,transform.forward);
        }

        if (Input.GetAxis("Shoot") == 0 && Input.GetAxis("Shoot_Secondary") == 0)
        {
            is_shooting = false;
        }
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
    }

    Vector3 Controller_Movement_Controls()
    {
        return (transform.forward * Time.deltaTime * move_speed * -Input.GetAxis("Y Movement"))
            + (transform.right * Time.deltaTime * move_speed * Input.GetAxis("X Movement"));
    }

    void Movement_Controls()
    {
        movement_direction = Controller_Movement_Controls();

        // Forward and back
        if (Input.GetKey(KeyCode.W))
        {
            movement_direction += transform.forward * Time.deltaTime * move_speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement_direction -= transform.forward * Time.deltaTime * move_speed;
        }

        // Left and right
        if (Input.GetKey(KeyCode.D))
        {
            movement_direction += transform.right * Time.deltaTime * move_speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement_direction -= transform.right * Time.deltaTime * move_speed;
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
