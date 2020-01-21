using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyControllerNew : CharacterStats
{
    public enum State {Idle, Chasing, Attacking};
    State currentState;

    public GameObject deathEffectPrefab;
    public float particleLifeTime = 2f;

    NavMeshAgent pathfinder;
    Transform target;
    CharacterStats targetStats;

    float meleeAttackdistance = 1.5f;
    float timeBetweenAttacks = 1f;
    float damage = 1f;

    float nextAvailableMeleeAttack;
    float myCollisionRadius;        //since the enemy is stopping in the center of the targets (players) positions, the enemys and the players radius of the collider are beeing taken into calculations;
    float targetCollisionRadius;

    public float refreshRate = 0.2f;

    bool hasTarget;

    private void Awake()
    {
        pathfinder = GetComponent<NavMeshAgent>();

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetStats = target.GetComponent<CharacterStats>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        if (hasTarget)
        {
            currentState = State.Chasing; //default state of the stateMachine

            targetStats.OnDeath += OnTargetDeath;

            StartCoroutine(UpdatePath());
        }
    }

    public void SetCharacteristics(float moveSpeed, int hitsToKillPlayer, float enemyHealth)
    {
        pathfinder.speed = moveSpeed;

        if (hasTarget)
        {
            damage = Mathf.Ceil(targetStats.startingHealth / hitsToKillPlayer);
        }

        startingHealth = enemyHealth;
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if(damage >= health && !dead)
        {
            Destroy(Instantiate(deathEffectPrefab, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)), particleLifeTime);
        }

        base.TakeHit(damage, hitPoint, hitDirection);
    }

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget)
        {
            if (Time.time > nextAvailableMeleeAttack)
            {
                //instead of calculating the Vector3 distance, which is very expensive, the sqr between the position of the target, and the player will be calculated instead
                float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;

                //the if below defines, when the enemy will start the attack procedure
                if (sqrDistanceToTarget < Mathf.Pow(meleeAttackdistance + myCollisionRadius + targetCollisionRadius, 2))    //the collisionRadiuses are added, so that the distance between the target and the enemy will be calculated from their capsuleCollider radius and not from their centers
                {
                    nextAvailableMeleeAttack = Time.time + timeBetweenAttacks;
                    StartCoroutine(MeleeAttack());
                }
            }
        }
    }

    IEnumerator MeleeAttack()
    {
        currentState = State.Attacking; //the current state of the stateMachine

        //since there will be an attack animation executed, the pathfinder should not work at this momet
        pathfinder.enabled = false;

        //here the enemy will start the attack from the origin position, close the player, hit (attackPosition) and then go back to the starting position (origin). And repeat.
        Vector3 directionToTheTarget = (target.position - transform.position).normalized;
        Vector3 originalPosition = transform.position;
        Vector3 attackPosition = target.position - directionToTheTarget * meleeAttackdistance;// + myCollisionRadius);// + targetCollisionRadius);


        float attackSpeed = 3;
        //now we determine, how far in the launch animation we are
        float percent = 0;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if(percent >= 0f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetStats.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;

            //the enemy starts at the originalPosition, then goes to the attackPosition and then back to the originalPosition. Therefore a function (parabell) is needed that go through 0 and 1, like y = 4(-x^2 + x)
            float interpolation = (-percent * percent + percent) * 4;
            //if percent is = 0, then transform.position is at the originalPosition, if percent is = 1, then transform.position is at attackPosition
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation); //the enemies position is equal to a value between his originalPosition and the attackPosition, which is determined by the interpolation (interpolation shows, where the enemy is betwenn the originalPosition and the AttackPosition)

            yield return null; //null to skip a frame in the while loop
        }

        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = .25f;

        while (hasTarget)
        {
            if (currentState == State.Chasing)
            {
                Vector3 directionToTheTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - directionToTheTarget * (myCollisionRadius + targetCollisionRadius + meleeAttackdistance/1.1f);

                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
