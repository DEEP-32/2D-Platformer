using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float fireCooldown;
    [SerializeField] private float onAttackForce = 10f;
    [SerializeField] private float damage;

    [Header("Ranged Attacks")]
    [SerializeField] private Transform firePoint;

    [Header("Player Detection")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private LayerMask playerLayer;

    [Header("Setting Up")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Patrol enemyPatrol;


    private float lastTimeFire = 0f;
    private GameObject projectilePrefab;
    private ObjectPoolerEnemy pooler;

    private bool isFacingRight => transform.localScale.x == 1;

    private void Awake()
    {
        enemyPatrol = GetComponentInParent<Patrol>();
    }

    private void Start()
    {
        pooler = ObjectPoolerEnemy.instance;
    }

    private void Update()
    {

        if(isFacingRight && firePoint.transform.localRotation.y != 0 || !isFacingRight && firePoint.transform.localRotation.y != 180)
            firePoint.transform.localRotation = Quaternion.Euler(0,setAttackPointOrientation(),0);


        if (!PlayerInSight())
        {
            enemyPatrol.enabled = true;
            return;
        }

        else
        {
            enemyPatrol.enabled = !PlayerInSight();
            if (Time.time > fireCooldown + lastTimeFire)
            {
                //Debug.Log("Shooting");
                Shoot();
            }
        }

    }


    private bool PlayerInSight()
    {
         Vector3 boxSize = getBoxColliderSize();
         RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * transform.localScale.x * range * colliderDistance, boxSize,
                          0f, Vector2.left, 0, playerLayer);

         return hit.collider != null;
        
    }

    private Vector3 getBoxColliderSize()
    {
        return new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z);
    }

    private void Shoot()
    {
        projectilePrefab = pooler.GetPooledObjects();

        if (projectilePrefab == null)
            return;


        projectilePrefab.transform.SetPositionAndRotation(firePoint.transform.position, firePoint.transform.rotation);
        projectilePrefab.SetActive(true);

        lastTimeFire = Time.time;   
    }

    private void OnDrawGizmos()
    {
        Vector3 boxSize = getBoxColliderSize();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * transform.localScale.x * range * colliderDistance, boxSize);

    }

    private float  setAttackPointOrientation()
    {
        return isFacingRight ? 0f : 180f;
    }

}
