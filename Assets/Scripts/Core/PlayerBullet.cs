using UnityEngine;

public class PlayerBullet : BulletBase
{
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        ////Debug.Log("Colliding");
        if(other.transform.tag == "Enemy")
        {
            base.OnCollisionEnter2D(other);
            var health = other.transform.GetComponent<EnemyHealth>();
            health.TakeDamage(damage);
            Vector2 v1 = new(transform.localPosition.x, transform.localPosition.y);
            Vector2 normal = other.contacts[0].normal;
            base.OnbulletImpact(v1, normal);
        }

        else
        {
            Vector2 v1 = new(transform.localPosition.x, transform.localPosition.y);
            Vector2 normal = other.contacts[0].normal;
            base.OnbulletImpact(v1, normal);
        }
    }

}
