using System;
using UnityEngine;

public class BaseCollectable : MonoBehaviour
{
    [Header("On Collect")]
    [SerializeField] protected float collectScore = 4f;
    public static Action<float> OnCollect;

    protected CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();    
    }


    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            OnCollect?.Invoke(collectScore);
            Destroy(this.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Player")
        {
            OnCollect?.Invoke(collectScore);
            Destroy(this.gameObject);
            Destroy(transform.parent.gameObject);
        }
    }




}
