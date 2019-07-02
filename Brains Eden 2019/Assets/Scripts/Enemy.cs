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
    BOMB,
    HANS
}

public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Rigidbody RigidBody;
    public Animator Animator;
    public GameObject Mesh;

    public Building CurrentTarget;
    private BuildingManager BuildingManager;
    [SerializeField] private ChainManager ChainManager;

    public bool IsDead = false;
    public int DamageDone = 2;

    public EnemyState State = EnemyState.MOVING;

    private CharacterJoint joint;
    public bool IsChainHead = false;
    public Chain chain = null;

    public EnemyType Type;
    public EnemyType DeathType;

    public GameObject ExplosionParticle;
    public GameObject DissolveParticle;

    public int ScoreGiven;
    public float chainScoreMultiplier = 2f;

    public float DistanceFromBuilding = 350f;

    private SoundManager soundManager;

    public void Initialize(BuildingManager buildingManager, ChainManager chainManager) {
        BuildingManager = buildingManager;
        ChainManager = chainManager;
    }

    void Start() {
        if (BuildingManager != null) {
            var building = BuildingManager.FindClosestHouse(transform.position);
            if (building != null) GoTo(building.transform);
        }
        chain = null;

        var soundManagerObj = GameObject.FindGameObjectWithTag("SoundManager");
        if (soundManagerObj != null) soundManager = soundManagerObj.GetComponent<SoundManager>();

    }

    void Update() {

        if (State == EnemyState.DIEING) {
            Death();
        }

        if (CurrentTarget == null && Agent.enabled && BuildingManager != null) {
            var building = BuildingManager.FindClosestHouse(transform.position);
            if (building != null) GoTo(building.transform);
        }
        Animator.SetInteger("State", (int)State);

        if(CurrentTarget != null) {
            if (!CurrentTarget.GetComponent<Collider>().bounds.Intersects(GetComponent<Collider>().bounds)) {
                CurrentTarget = null;
            }
        }
    }

    public void Attack()
    {
        switch (Type) {
            case EnemyType.NORMAL:
                if (CurrentTarget != null) CurrentTarget.Damage(DamageDone);
                break;
            case EnemyType.BOMB:
                if (CurrentTarget != null) CurrentTarget.Damage(DamageDone);

                Mesh.SetActive(false);
                ExplosionParticle.SetActive(true);
                if (soundManager != null) soundManager.PlayExplosion(transform.position);

                break;
            case EnemyType.HANS:
                if (CurrentTarget != null) CurrentTarget.Damage(DamageDone);

                Mesh.SetActive(false);
                DissolveParticle.SetActive(true);
                break;
        }
    }

    public void Death()
    {
        switch (DeathType)
        {
            case EnemyType.NORMAL:
                Destroy(this.gameObject);
                break;
            case EnemyType.BOMB:
                Mesh.SetActive(false);
                ExplosionParticle.SetActive(true);
                if(soundManager != null) soundManager.PlayExplosion(transform.position);
                break;
            case EnemyType.HANS:
                Mesh.SetActive(false);
                DissolveParticle.SetActive(true);
                break;
        }

        var ScoreTransfer = GameObject.FindGameObjectWithTag("Data").GetComponent<Score_Transfer>();
        if (ScoreTransfer != null) {
            var awaredScore = ScoreGiven;
            if (DeathType > EnemyType.NORMAL) awaredScore = (int)(awaredScore * chainScoreMultiplier);
            ScoreTransfer.player_score += ScoreGiven;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<Building>() != null) {
            if(Agent.enabled) Agent.ResetPath();
            CurrentTarget = other.gameObject.GetComponent<Building>();
            State = EnemyState.ATTACKING;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.GetComponent<Building>() == CurrentTarget) {
            CurrentTarget = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.GetComponent<Enemy>();
        //If there is an enemy
        if (enemy != null) {
            //If the enemy is not the chain head and not in a chain
            if (enemy.chain == null && chain == null) {
                //Then set the enemy to be the new head of the chain.
                enemy.IsChainHead = true;
                //Create a new chain to store the data.d
                enemy.chain = ChainManager.NewChain();
                enemy.chain.Add(enemy);
                enemy.RigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }

            //If the enemy hit is the chain head or it already has a chain
            if(enemy.chain != null && chain == null) {
                //Add a joint to this
                AddJoint(enemy.GetComponent<Rigidbody>(), collision.GetContact(0).point);
                //turn off the AI
                Agent.enabled = false;
                //Turn off physics contraints
                RigidBody.constraints = RigidbodyConstraints.None;
                //assign its chain to be that of the rest of the chain
                chain = enemy.chain;
                chain.Add(this);
            }
            else if(enemy.chain == null && chain != null)
            {
                //Add a joint to this
                enemy.AddJoint(GetComponent<Rigidbody>(), collision.GetContact(0).point);
                //turn off the AI
                enemy.Agent.enabled = false;
                //Turn off physics contraints
                RigidBody.constraints = RigidbodyConstraints.None;
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
        ChainManager.PopChain(chain, this);
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
