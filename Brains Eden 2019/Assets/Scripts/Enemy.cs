using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    NONE = 0,
    MOVING,
    ATTACKING,
    DIEING
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
    [SerializeField] private ChainManager ChainManager;

    public bool IsDead = false;
    public int DamageDone = 2;

    public EnemyState State = EnemyState.NONE;

    private CharacterJoint joint;
    public bool IsChainHead = false;
    public Chain chain = null;

    public EnemyType Type;

    //temp
    public Material DeathMaterial;

    public void Initialize(BuildingManager buildingManager, ChainManager chainManager) {
        BuildingManager = buildingManager;
        ChainManager = chainManager;
    }

    void Start() {
        if(BuildingManager != null) GoTo(BuildingManager.FindClosestHouse(transform.position).transform);
        chain = null;
    }

    void Update() {

        if(State == EnemyState.ATTACKING) {
            if(CurrentTarget != null) CurrentTarget.Damage(DamageDone);
        }

        if(CurrentTarget == null && Agent.enabled && BuildingManager != null) {
            GoTo(BuildingManager.FindClosestHouse(transform.position).transform);
        }
        Animator.SetInteger("State", (int)State);

        if(State == EnemyState.DIEING)
        {
            GetComponentInChildren<MeshRenderer>().material = DeathMaterial;
        }
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
        //If there is an enemy
        if (enemy != null) {
            if (enemy.chain == chain) return;
            //If the enemy is not the chain head and not in a chain
            if (!enemy.IsChainHead && enemy.chain == null && chain == null) {
                //Then set the enemy to be the new head of the chain.
                enemy.IsChainHead = true;
                //Create a new chain to store the data.
                enemy.chain = ChainManager.NewChain();
                enemy.chain.Add(enemy);
                enemy.RigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
            //If the enemy hit is the chain head or it already has a chain
            else if(enemy.chain != null && chain == null) {
                //Add a joint to this
                AddJoint(enemy.GetComponent<Rigidbody>(), collision.GetContact(0).point);
                //turn off the AI
                Agent.enabled = false;
                //assign its chain to be that of the rest of the chain
                chain = enemy.chain;
                chain.Add(this);
            }
            else if( enemy.chain == null && chain != null)
            {
                //Add a joint to this
                enemy.AddJoint(GetComponent<Rigidbody>(), collision.GetContact(0).point);
                //turn off the AI
                enemy.Agent.enabled = false;
                //assign its chain to be that of the rest of the chain
                enemy.chain = chain;
                chain.Add(enemy);
            }
            else {
                Debug.Log("Unhandled Case");
            }

        }
    }

    public void PopChain() {
        ChainManager.PopChain(chain, transform.position);
    }

    public void GoTo(Transform transform) {
        if (transform != null) {
            Agent.SetDestination(transform.position);
            State = EnemyState.MOVING;
        }
    }

    public void GoTo(Vector3 location) {
        if (location != null) {
            Agent.SetDestination(location);
            State = EnemyState.MOVING;
        }
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
