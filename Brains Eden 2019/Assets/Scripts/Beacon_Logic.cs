using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beacon_Logic : MonoBehaviour
{
    [SerializeField] private Building this_building;
    [SerializeField] private MeshRenderer beacon_mesh;
    private int last_health;
    private bool is_beacon_showing = false;

    [SerializeField] private float beacon_shine_time = 5.0f;
    private float current_time = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        last_health = this_building.Health;
    }

    // Update is called once per frame
    void Update()
    {
        if (this_building.Health != last_health)
        {
            is_beacon_showing = true;
        }

        if (is_beacon_showing)
        {
            current_time += Time.deltaTime;

            beacon_mesh.enabled = true;

            if (current_time >= beacon_shine_time)
            {
                is_beacon_showing = false;
            }

        }
        else
        {
            beacon_mesh.enabled = false;
            current_time = 0.0f;
        }
    }
}
