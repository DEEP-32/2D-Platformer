using UnityEngine;

public class FireScript : MonoBehaviour
{
    [SerializeField] private float fireCooldown = 0.3f;
    private GameObject projectilePrefab;

    ObjectPooler pooler;
    private float lastTimeFire = 0f;
    // Start is called before the first frame update
    void Start()
    {
        pooler = ObjectPooler.instance;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localRotation = Quaternion.Euler(0, getPlayerOrientation(), 0);
        if (PlayerController2.instance.isShootPressed && Time.time > fireCooldown+lastTimeFire)
        {
            projectilePrefab = pooler.GetPooledObjects();
            
            if (projectilePrefab == null)
                return;


            projectilePrefab.transform.SetPositionAndRotation(gameObject.transform.position, gameObject.transform.rotation);
            projectilePrefab.SetActive(true);

            lastTimeFire = Time.time;
        }
    }

    private float getPlayerOrientation()
    {
        //Debug.Log($"Is facing right {PlayerController2.instance.isFacingRight}");
        return PlayerController2.instance.isFacingRight ? 0 : 180;
    }
}
