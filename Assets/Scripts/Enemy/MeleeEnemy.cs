using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{

    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float onAttackForce = 10f;
    [SerializeField] private float attackRange = 0.3f;
    [SerializeField] private float damage;


    [Header("Player Detection")]
    [SerializeField] private float detectRange;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask playerLayer;

    [Header("Setting Up")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Patrol enemyPatrol;


    private float coolDownTimer = Mathf.Infinity;
    

    private void Awake()
    {
        enemyPatrol = GetComponentInParent<Patrol>();
    }
    private void Update()
    {
        coolDownTimer += Time.deltaTime;


        if (!PlayerInsight())
        {
            enemyPatrol.enabled = true;
            return;
        }

        else
        {
            enemyPatrol.enabled = !PlayerInsight();
            if (coolDownTimer >= attackCooldown)
            {
                coolDownTimer = 0;
                Attack();
            }
        }


       
    }

    private bool PlayerInsight()
    {
        Vector3 boxSize = getBoxColliderSize();
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * transform.localScale.x * detectRange * colliderDistance, boxSize,
                      0f,Vector2.left,0,playerLayer);

        return hit.collider != null;
    }

   

    private void OnDrawGizmos()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Vector3 boxSize = getBoxColliderSize();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * transform.localScale.x * detectRange * colliderDistance, boxSize);
    }

    private Vector3 getBoxColliderSize()
    {
        return new Vector3(boxCollider.bounds.size.x * detectRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z);
    }

    private void Attack()
    {
        //Debug.Log("Attacking");
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,playerLayer);
        if(hitObjects == null){
            //Debug.Log("Didnt hit anything");
            return;
        }

        foreach(Collider2D hitObject in hitObjects)
        {
            var health = hitObject.GetComponent<PlayerHealth>();
                        
            if(health == null)
            {
                continue;
            }
            health.TakeDamage(damage);

            /*if(rb != null)
            {
                //Debug.Log("Adding force to the player");
                rb.AddForce(Vector2.right * -onAttackForce,ForceMode2D.Force);
            }*/
        }
    }

    private void OnDrawGizmosSelected()
    {
      
    }
}
