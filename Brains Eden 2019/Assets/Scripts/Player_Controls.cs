using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controls : MonoBehaviour
{
    CharacterController char_con;

    [SerializeField] float move_speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        char_con = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            char_con.Move(transform.forward * Time.deltaTime * move_speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            char_con.Move(-(transform.forward * Time.deltaTime * move_speed));
        }

        if (Input.GetKey(KeyCode.D))
        {
            char_con.Move(transform.right * Time.deltaTime * move_speed);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            char_con.Move(-(transform.right * Time.deltaTime * move_speed));
        }
    }
}
