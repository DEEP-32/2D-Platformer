using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] protected float speed = 20f;
    [SerializeField] protected float damage = 20f;
    [SerializeField] protected float bulletLifeTime = 5;
    [SerializeField] protected GameObject bulletImpact;
    protected Rigidbody2D rb;
    protected float timer = 0;

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
        rot.y = PlayerController2.instance.isFacingRight ? 0 : -180f;
        if(bulletImpact != null)
          Destroy(Instantiate(bulletImpact, pos, Quaternion.Euler(rot)), 1f);


    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    } 
}
