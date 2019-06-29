using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Logic : MonoBehaviour
{
    [SerializeField] private float hover_height; // Height the spell projectile will hover off of the floor

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position, Vector3.down, out hit);

        if (hit.collider != null)
        {
            transform.position = hit.point + (Vector3.up * hover_height);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
