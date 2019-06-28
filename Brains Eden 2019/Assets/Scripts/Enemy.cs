using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    NONE = 0,
    MOVING,
    ATTACKING
}

public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Rigidbody RigidBody;
    public Animator Animator;

    private Building CurrentTarget;
    private BuildingManager BuildingManager;

    public bool IsDead = false;
    public int DamageDone = 2;

    private EnemyState State = EnemyState.NONE;

    public void Initialize(BuildingManager buildingManager) {
        BuildingManager = buildingManager;
    }

    void Start() {
        GoTo(BuildingManager.FindClosestHouse(transform.position).transform);
    }

    void Update() {

        if(State == EnemyState.ATTACKING) {
            if(CurrentTarget != null) CurrentTarget.Damage(DamageDone);
        }

        if(CurrentTarget == null) {
            GoTo(BuildingManager.FindClosestHouse(transform.position).transform);
        }
        Animator.SetInteger("State", (int)State);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<Building>() != null) {
            Agent.ResetPath();
            CurrentTarget = other.gameObject.GetComponent<Building>();
            State = EnemyState.ATTACKING;
        }
    }

    public void GoTo(Transform transform) {
        Agent.SetDestination(transform.position);
        State = EnemyState.MOVING;
    }

    public void GoTo(Vector3 location) {
        Agent.SetDestination(location);
        State = EnemyState.MOVING;
    }

}
