using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [Header("Attacking")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firepoint;
    private float lastTimeShot = 0f;


    [Header("Detection")]
    [SerializeField] private LayerMask whatToDetect;
    private bool inRange = false;

    [Header("Shooting Projectile")]
    private GameObject projectile;

    //Refernces.
    private ObjectPoolerTraps pooler;
    private Vector3 positionOffset;

    private void Start()
    {
        pooler = ObjectPoolerTraps.instance;
    }
    private void Shoot()
    {
        positionOffset = Vector2.right * gameObject.transform.localScale.x;
        //Debug.Log("Shooting");
        projectile = pooler.GetPooledObjects();


        if (projectile == null)
            return;


        projectile.transform.SetPositionAndRotation(gameObject.transform.position + positionOffset, gameObject.transform.rotation);
        projectile.SetActive(true);

        lastTimeShot = Time.time;
    }

    private void Update()
    {
        if(canShoot())
        {
            Shoot();
        }
    }

    private bool canShoot()
    {
        return Time.time >= attackCooldown + lastTimeShot && inRange;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
           inRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            inRange = false;
    }
}
