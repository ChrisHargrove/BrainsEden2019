using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knockback_Effect : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float knockback_force;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spell")
        {
            if (rb != null)
            {
                Vector3 force_dir = (transform.position - other.transform.position).normalized * knockback_force;
                rb.AddForce(force_dir);
            }

        }
    }
}
