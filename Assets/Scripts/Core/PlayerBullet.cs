using UnityEngine;

public class PlayerBullet : BulletBase
{
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        ////Debug.Log("Colliding");
        base.OnCollisionEnter2D(other);
        if(other.transform.tag == "Enemy")
        {
            var health = other.transform.GetComponent<EnemyHealth>();
            health.TakeDamage(damage);
            Vector2 v1 = new(transform.localPosition.x, transform.localPosition.y);
            base.OnbulletImpact(v1, -v1);
        }

        else
        {
            Vector2 v1 = new(transform.localPosition.x, transform.localPosition.y);
            base.OnbulletImpact(v1, -v1);
        }
    }

}
