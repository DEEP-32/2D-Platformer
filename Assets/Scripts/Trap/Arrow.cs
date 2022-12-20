using UnityEngine;

public class Arrow : BulletBase
{
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Hurt player");
        base.OnCollisionEnter2D(other);
        if(other.transform.tag == "Player")
        {
            var health = other.transform.GetComponent<PlayerHealth>();
            //Debug.Log($"Giving damage of amount : {damage}");
            health.TakeDamage(damage);
            OnbulletImpact(Vector3.zero,Vector3.zero);
        }
    }


}
