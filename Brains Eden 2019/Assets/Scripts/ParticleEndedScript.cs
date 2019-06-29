using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ParticleEndedScript : MonoBehaviour
{
    public GameObject Owner;

    public void OnParticleSystemStopped()
    {
        Destroy(Owner);
    }
}
