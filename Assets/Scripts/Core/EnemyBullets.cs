using UnityEngine;

public class EnemyBullets
    : BulletBase
{
    protected override void OnCollisionEnter2D(Collision2D other)
    {
       if(other.transform.tag == "Player")
       {
            var health = other.transform.GetComponent<PlayerHealth>();
            health.TakeDamage(base.damage);
            Vector2 v1 = new(transform.localPosition.x, transform.localPosition.y);
            base.OnbulletImpact(v1,-v1);
       }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //Debug.Log(base.rb.velocity + " inside on enable");
    }
}
