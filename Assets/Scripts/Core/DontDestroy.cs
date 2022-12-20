using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector] public static GameObject instance;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = gameObject;
        else
            Destroy(gameObject);

       
    }
}
