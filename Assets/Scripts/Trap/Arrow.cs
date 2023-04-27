using UnityEngine;

public class Arrow : BulletBase
{
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        //Debug.Log("Hurt player");
        if(other.transform.tag == "Player")
        {
            base.OnCollisionEnter2D(other);
            var health = other.transform.GetComponent<PlayerHealth>();
            //Debug.Log($"Giving damage by arrow of amount : {damage}");
            health.TakeDamage(damage);
            Vector2 v1 = new(transform.localPosition.x, transform.localPosition.y);
            Vector2 normal = other.contacts[0].normal;

            base.OnbulletImpact(v1, normal);
        }
    }


}
