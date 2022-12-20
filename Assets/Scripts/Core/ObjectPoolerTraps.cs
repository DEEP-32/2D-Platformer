using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolerTraps : MonoBehaviour
{
    public static ObjectPoolerTraps instance;
    [SerializeField] private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    [SerializeField] private Transform bulletParent;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(objectToPool, bulletParent);
            temp.SetActive(false);
            pooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObjects()
    {
        for (int j = 0; j < amountToPool; j++)
        {
            if (!pooledObjects[j].activeInHierarchy)
            {
                return pooledObjects[j];
            }
        }
        return null;
    }
}
