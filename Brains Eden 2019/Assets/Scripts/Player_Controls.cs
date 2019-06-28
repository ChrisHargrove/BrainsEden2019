using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    CharacterController char_con;

    [SerializeField] float move_speed = 1.0f;
    [SerializeField] float gravity_force = 9.8f;
    [SerializeField] float max_jump_force = 2.0f;
    float current_jump_force = 0.0f;
    [SerializeField] float jump_falloff = 0.1f;

    float current_fall_speed = 0.0f;


    private Vector3 movement_direction = new Vector3();


    // Start is called before the first frame update
    void Start()
    {
        char_con = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        movement_direction = new Vector3(0.0f, 0.0f, 0.0f);

        if (Input.GetKey(KeyCode.W))
        {
            movement_direction += transform.forward * Time.deltaTime * move_speed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movement_direction -= transform.forward * Time.deltaTime * move_speed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement_direction += transform.right * Time.deltaTime * move_speed;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            movement_direction -= transform.right * Time.deltaTime * move_speed;
        }

        Debug.Log(char_con.isGrounded);

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
        movement_direction.y -= (current_fall_speed * Time.deltaTime);

        if (current_jump_force > 0)
        {
            current_jump_force -= jump_falloff;
            movement_direction += transform.up * Time.deltaTime * current_jump_force;
        }

        char_con.Move(movement_direction);

    }
}
