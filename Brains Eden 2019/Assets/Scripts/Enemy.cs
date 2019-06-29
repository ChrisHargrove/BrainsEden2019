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

public enum EnemyType
{
    NORMAL = 0,
    BOMB
}

public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Rigidbody RigidBody;
    public Animator Animator;

    private Building CurrentTarget;
    private BuildingManager BuildingManager;
    private ChainManager ChainManager;

    public bool IsDead = false;
    public int DamageDone = 2;

    private EnemyState State = EnemyState.NONE;

    private CharacterJoint joint;
    public bool IsChainHead = false;
    public Chain chain = null;

    public EnemyType Type;

    public void Initialize(BuildingManager buildingManager, ChainManager chainManager) {
        BuildingManager = buildingManager;
        ChainManager = chainManager;
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

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.GetComponent<Enemy>();
        if (enemy != null && chain != null && !IsChainHead) {
            if (!enemy.IsChainHead) {
                enemy.IsChainHead = true;
                enemy.chain = ChainManager.NewChain();
                enemy.chain.Add(enemy);
            }
            AddJoint(enemy.GetComponent<Rigidbody>(), collision.GetContact(0).point);
            Agent.enabled = false;
            chain = enemy.chain;
            chain.Add(this);
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

    private void AddJoint(Rigidbody ConnectedBody, Vector3 hitPoint)
    {
        //Add Character Joint to Enemy
        joint = gameObject.AddComponent<CharacterJoint>();
        joint.connectedBody = ConnectedBody;
        //Setup axis for the rotations
        joint.axis = new Vector3(0, 0, -1);
        joint.swingAxis = new Vector3(0, 1, 0);

        //Set Anchoor position for the joint
        joint.anchor = transform.InverseTransformPoint(hitPoint);

        //Set all swing limits
        joint.lowTwistLimit = new SoftJointLimit {
            limit = 0,
            bounciness = 0,
            contactDistance = 0
        }; 
        joint.highTwistLimit = new SoftJointLimit {
            limit = 0,
            bounciness = 0,
            contactDistance = 0
        };

        joint.swingLimitSpring = new SoftJointLimitSpring {
            spring = 500f,
            damper = 0
        };
        joint.swing1Limit = new SoftJointLimit {
            limit = 106,
            bounciness = 0,
            contactDistance = 0
        };
        joint.swing2Limit = new SoftJointLimit {
            limit = 0,
            bounciness = 0,
            contactDistance = 0
        };
    }

}
