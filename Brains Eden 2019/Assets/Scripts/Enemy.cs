using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Rigidbody RigidBody;


    public void GoTo(Transform transform) {
        Agent.SetDestination(transform.position);
    }

    public void GoTo(Vector3 location) {
        Agent.SetDestination(location);
    }
}
