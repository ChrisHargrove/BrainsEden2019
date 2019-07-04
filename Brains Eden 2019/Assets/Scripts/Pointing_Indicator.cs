using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointing_Indicator : MonoBehaviour
{
    [SerializeField] private Transform point_from;
    [SerializeField] private Transform point_to;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((point_from != null) && (point_to != null))
        {
            transform.position = point_from.position;
            transform.LookAt(point_to,Vector3.up);
        }
    }
}
