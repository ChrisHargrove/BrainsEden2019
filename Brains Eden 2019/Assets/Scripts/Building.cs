using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DestructionLevel
{
    NONE = 0,
    PARTIAL,
    SEVERE,
    DESTROYED,
    NUM_LEVELS
}

public class Building : MonoBehaviour
{
    private DestructionLevel CurrentDestruction = DestructionLevel.NONE;

    [Range(1, 100)] public int Health = 100;
    private int MaxHealth;

    void Start() {
        MaxHealth = Health;    
    }

    void Update()
    {
        switch (CurrentDestruction)
        {
            case DestructionLevel.PARTIAL:
                break;
            case DestructionLevel.SEVERE:
                break;
            case DestructionLevel.DESTROYED:
                Destroy(this.gameObject);
                break;
        }
    }

    public void Damage(int amount)
    {
        Health -= amount;

        if(Health < MaxHealth * 0.5f) {
            CurrentDestruction = DestructionLevel.PARTIAL;
        }
        if(Health < MaxHealth * 0.25f) {
            CurrentDestruction = DestructionLevel.SEVERE;
        }
        if (Health <= 0) {
            CurrentDestruction = DestructionLevel.DESTROYED;
        }
    }

}
