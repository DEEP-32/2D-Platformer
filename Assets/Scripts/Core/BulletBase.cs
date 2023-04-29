using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] protected float speed = 20f;
    [SerializeField] protected float damage = 20f;
    [SerializeField] protected float impactForce = 20f;
    [SerializeField] protected float bulletLifeTime = 5;
    [SerializeField] protected GameObject bulletImpact;
    protected Rigidbody2D rb;
    protected float timer = 0;
    protected Vector3 bulletRot;

    protected virtual void Awake()
    {
        ////Debug.Log("Awake is called ");
        rb = GetComponent<Rigidbody2D>();
    }


    protected virtual void Update()
    {
        ////Debug.Log("Update is called");
        ////Debug.Log($"current velocity is: {rb.velocity}");
        timer += Time.deltaTime;
        if (timer > bulletLifeTime) gameObject.SetActive(false);
        else rb.velocity = transform.right * speed;
    }



    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        //bulletRot = other.contacts[0].normal;
        //bulletRot = other.contacts[0].normal;

    }
    protected virtual void OnEnable()
    {
        ////Debug.Log("On enable is called");
        timer = 0;
        rb.velocity = transform.right * speed;
    }

    protected virtual void OnbulletImpact(Vector2 pos, Vector2 rot)
    {
        Deactivate();
        //rot.y = -bulletRot.x;
    
        Debug.Log(rot);
        if(rot.x < 0)
           rot = new Vector3(0,0,0);
        else
        {
            rot = new Vector3(0,180f,0);   
        }
        if(bulletImpact != null)
          Destroy(Instantiate(bulletImpact, pos, Quaternion.Euler(rot)), 1f);


    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    } 
}
