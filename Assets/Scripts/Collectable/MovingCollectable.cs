using UnityEngine;

public class MovingCollectable : BaseCollectable
{
    [Header("Moving Parameters")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float amplitude = 1.2f;

    /*[SerializeField] private float minSpeed = 1.5f;
    [SerializeField] private float maxSpeed = 2f;   
    [SerializeField] private float minAmplitude = 1.2f;
    [SerializeField] private float maxAmpitude = 3f;*/

    private void Update()
    {
        float x = transform.localPosition.x;
        float y = Mathf.Sin(Time.time * speed) * amplitude;
        float z = transform.localPosition.z;

        transform.localPosition = new Vector3(x, y, z);
    }

   /* private float getRandomAmplitude()
    {
        return Random.Range(minAmplitude,maxAmpitude);
    }

    private float getRandomSpeed()
    {
        return Random.Range(minSpeed, maxSpeed);
    }*/

    /*protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        //OnCollect?.Invoke(base.collectScore);
    }
    */
}
