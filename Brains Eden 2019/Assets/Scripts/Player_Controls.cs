using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    CharacterController char_con;

    [SerializeField] private float move_speed;
    [SerializeField] private float gravity_force;
    [SerializeField] private float max_jump_force;
    private float current_jump_force = 0.0f;
    [SerializeField] private float jump_falloff;
    private float current_fall_speed = 0.0f;

    [SerializeField] private float turn_speed;
    [SerializeField] private GameObject attack_projectile;
    [SerializeField] private float fire_speed;
    [SerializeField] private float spell_fire_offset; // The distance infront of the player which the projectile spawns

    [SerializeField] public int score;


    private Vector3 movement_direction = new Vector3();


    // Start is called before the first frame update
    void Start()
    {
        char_con = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement_Controls();
        Player_Rotation();

        if (Input.GetMouseButtonDown(0))
        {
            Attack_Spell(transform.forward);
        }
    }

    void Attack_Spell(Vector3 fire_direction)
    {
        GameObject created_obj = Instantiate(attack_projectile);
        Rigidbody rb = created_obj.GetComponent<Rigidbody>();
        created_obj.transform.position = transform.position + (fire_direction * spell_fire_offset);
        rb.AddForce(fire_direction * fire_speed);
    }

    void Player_Rotation()
    {
        transform.Rotate(new Vector3(0.0f, Input.GetAxis("Mouse X"), 0.0f) * Time.deltaTime * turn_speed);
    }

    void Movement_Controls()
    {
        movement_direction = new Vector3(0.0f, 0.0f, 0.0f);

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
            if (Input.GetKey(KeyCode.Space))
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
