using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyState currentState = EnemyState.idle;

    private void ChangeState(EnemyState newState)
    {
        if (currentState == newState) return;
        Debug.Log($"Changing state from {currentState} to {newState}");
        currentState = newState;
    }

    [Header("Player detectioon")]
    [SerializeField] private float detectRange;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float colliderDistance;


    [Header("References")]
    [SerializeField] private Transform gfxObject;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private Transform player;



    #region Unity Methods
    private void Awake()
    {/*
        gfxObject = transform.GetChild(0);
        boxCollider = gfxObject.GetComponent<BoxCollider2D>();*/
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.idle:
                HandleIdleState();
                break;
            case EnemyState.Patrol:
                HandlePatrolState();
                break;
            case EnemyState.Chasing:
                HandleChasingState();
                break;
            case EnemyState.Attack:
                HandleAttackState();
                break;
            default:
                break;
        }

        
    }
    #endregion

    #region Helpe function
    private bool PlayerInRange()
    {
        var boxSize = getBoxColliderSize();
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * detectRange * colliderDistance, boxSize,
                      0f, Vector2.right, 0, whatIsPlayer);

        return hit.collider != null;
    }
    private Vector3 getBoxColliderSize()
    {
        return new Vector3(boxCollider.bounds.size.x * detectRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z);
    }

    private void OnDrawGizmos()
    {
        var boxSize = getBoxColliderSize();
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * transform.localScale.x * detectRange * colliderDistance, boxSize);
    }


    private bool isFacingRight => transform.eulerAngles.x == 180f;

    private void RotateEnemyToDir(Vector2 dir)
    {
        if (dir.x > 0)
        {
           // Debug.Log(dir);
            transform.eulerAngles = Vector2.zero;
        }
        else if (dir.x < 0)
        {   
            //Debug.Log(dir + " from else if");
            transform.eulerAngles = new Vector2(0f, 180f);
        }
  
    }
    #endregion

    #region Idle

    [Header("Idle State")]
    [SerializeField] private float idleWaitTime = .5f;
    private void HandleIdleState()
    {
        StartCoroutine(WaitAndGo());
    }

    private IEnumerator WaitAndGo()
    {
        yield return new WaitForSeconds(idleWaitTime);
        if(PlayerInRange()) ChangeState(EnemyState.Chasing);
        else ChangeState(EnemyState.Patrol);
    }

    #endregion

    #region Patrolling
    [Header("Patrol State")]
    [SerializeField] private float patrolSpeed = 1f;
    [SerializeField] private List<Transform> wayPoints = null;
    int currentIndex = 0;
    private void HandlePatrolState()
    {
        if (transform.position != wayPoints[currentIndex].position)
        {
            
            MoveToPoint(currentIndex);
            //Debug.Log(wayPoints[currentIndex].name);
            RotateEnemyToDir((transform.position - wayPoints[currentIndex].position).normalized);
           
            
        }

        else
        {
            if (currentIndex + 1 < wayPoints.Count)
                currentIndex++;
            else
            {
                //Debug.Log("Changed to zero");
                currentIndex = 0;
            }
            ChangeState(EnemyState.idle); 
        }

        if (PlayerInRange()) ChangeState(EnemyState.Chasing);
        
    }

    

    private void MoveToPoint(Vector2 pos)
    {
        transform.position = Vector2.MoveTowards(transform.position, pos, chaseSpeed * Time.deltaTime);
    }

    private void MoveToPoint(int index)
    {
        
        transform.position = Vector2.MoveTowards(transform.position,  wayPoints[index].position, patrolSpeed * Time.deltaTime);
    }

    #endregion

    #region Chasing

    [Header("Chasing State")]
    [SerializeField] private float chaseSpeed = 2f;
    [SerializeField] private float idleTimeBeforeChasing = 1f;
    private void HandleChasingState()
    {
        StartCoroutine(WaitAndChase());
    }

    private IEnumerator WaitAndChase()
    {
        yield return new WaitForSeconds(idleTimeBeforeChasing);

        if (!PlayerInAttackRange())
        {
            //Debug.Log("Moving towards player in chasing state");
            MoveToPoint(new Vector2(player.position.x, transform.position.y));
        }
        else
        {
            //Debug.Log("Changing state to attack");
            ChangeState(EnemyState.Attack);
        }

        if (!PlayerInRange())
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private bool PlayerInAttackRange()
    {
        //Debug.Log((player.position - transform.position).magnitude);
        return (player.position - transform.position).magnitude <= attackRange ;
    }


    #endregion

    #region Attack
    [Header("Attack State")]
    [SerializeField] private float attackCooldown = .2f;
    [SerializeField] private float attackDamage = 2f;
    [SerializeField] private LayerMask whatToDamage; 
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private Transform attackPoint;
    private float lastTimeAttack = 0f;
    private void HandleAttackState()
    {
        if(Time.time > attackCooldown + lastTimeAttack && PlayerInAttackRange())
        {
            Attack();
        }

        else if (PlayerInRange() && !PlayerInAttackRange())
        {
            Debug.Log((player.position - transform.position).magnitude);
           
            ChangeState(EnemyState.Chasing);
        }

        else if(!PlayerInRange())
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void Attack()
    {
        Debug.Log("Attacking the player");

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whatToDamage);
        if (hitObjects == null)
        {
            //Debug.Log("Didnt hit anything");
            return;
        }

        foreach (Collider2D hitObject in hitObjects)
        {
            var health = hitObject.GetComponent<PlayerHealth>();
            var rb = hitObject.GetComponent<Rigidbody2D>();

            if (health == null)
            {
                continue;
            }
            health.TakeDamage(attackDamage);

            lastTimeAttack = Time.time;
        }
    }

    #endregion

    
}
