using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Logic : MonoBehaviour
{
    [SerializeField] private float hover_height; // Height the spell projectile will hover off of the floor

    private float current_time;
    [SerializeField] private float max_lifetime;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        current_time += Time.deltaTime;

        RaycastHit hit = new RaycastHit();
        Physics.Raycast(transform.position, Vector3.down, out hit);

        if (hit.collider != null)
        {
            transform.position = hit.point + (Vector3.up * hover_height);
        }

        if (current_time >= max_lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);

        var enemy = other.GetComponent<Enemy>();
        if(enemy != null)
        {
            if(enemy.Type > EnemyType.NORMAL && enemy.chain != null)
            {
                Debug.Log("Break The Chain! Fleetwood Mac");

                enemy.PopChain();
            }
        }
    }
}
