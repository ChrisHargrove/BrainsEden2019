using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Boom : MonoBehaviour
{
    [SerializeField] private GameObject target_object;
    [SerializeField] private float camera_distance;
    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(target_object.transform.position, -((target_object.transform.forward*2) - target_object.transform.up), out hit, 20.0f);

        float current_distance = camera_distance;
        if (hit.collider != null)
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = target_object.transform.position + -((target_object.transform.forward * 2) - target_object.transform.up) * camera_distance;
        }

        transform.LookAt(target_object.transform);

        
    }
}
