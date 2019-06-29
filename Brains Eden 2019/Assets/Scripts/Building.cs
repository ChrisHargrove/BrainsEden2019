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
    public DestructionLevel CurrentDestruction = DestructionLevel.NONE;

    // Destruction animation stuff
    [SerializeField] AnimationCurve destruction_curve;
    [SerializeField] private float shake_severity;
    private Vector3 default_position;
    private float curve_progress = 0.0f;
    private float sink_progress = 0.0f;
    private bool has_destruction_begun = false;
    //

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
                Destruction_Event();
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

    private void Destruction_Event()
    {
        if (!has_destruction_begun)
        {
            has_destruction_begun = true;
            default_position = transform.position;
            StartCoroutine(Shake());

        }
    }

    private IEnumerator Shake()
    {
        while(sink_progress <= 5.0f)
        {
            curve_progress += 0.1f;
            sink_progress += 0.02f;
            transform.position = default_position + new Vector3((destruction_curve.Evaluate(curve_progress)-0.5f)* shake_severity, -sink_progress, 0.0f);

            if (curve_progress >= 1.0f)
            {
                curve_progress = 0.0f;
            }
            yield return null;
        }

        Destroy(this.gameObject);
        yield return null;
    }

}
