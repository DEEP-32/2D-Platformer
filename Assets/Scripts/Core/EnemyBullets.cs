using UnityEngine;

public class EnemyBullets
    : BulletBase
{
    protected override void OnCollisionEnter2D(Collision2D other)
    {
       if(other.transform.tag == "Player")
       {
            base.OnCollisionEnter2D(other);
            var health = other.transform.GetComponent<PlayerHealth>();
            var rb = other.transform.GetComponent<Rigidbody>();
            health.TakeDamage(base.damage);
            Vector2 v1 = new(transform.localPosition.x, transform.localPosition.y);
            Vector2 normal = other.contacts[0].normal;
            base.OnbulletImpact(v1,normal);
       }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //Debug.Log(base.rb.velocity + " inside on enable");
    }
}
