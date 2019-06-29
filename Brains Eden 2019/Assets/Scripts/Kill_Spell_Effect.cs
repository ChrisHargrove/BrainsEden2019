using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill_Spell_Effect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Damage_Spell")
        {
            Enemy enemy_script = other.gameObject.GetComponent<Enemy>();

            if (enemy_script != null)
            {
                enemy_script.State = EnemyState.DIEING;
            }
            
        }
    }
}
