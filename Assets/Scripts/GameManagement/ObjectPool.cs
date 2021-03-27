using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    [Tooltip("Значения должны соотвествовать своему объекту во всех массивах")]
    [SerializeField] private GameObject[] objectsToPool;
    [SerializeField] private string[] objectNames;
    [SerializeField] private int[] amountToPool;

    public static ObjectPool SharedInstance;
    private Dictionary<string, List<GameObject>> pooledObjects;

    private void Awake()
    {
        SharedInstance = this;
        pooledObjects = new Dictionary<string, List<GameObject>>();
        GameObject tmp;
        for (int i = 0; i < objectsToPool.Length; i++)
        {
            List<GameObject> tmpList = new List<GameObject>();
            pooledObjects.Add(objectNames[i], tmpList);
            for (int y = 0; y < amountToPool[i]; y++)
            {
                tmp = Instantiate(objectsToPool[i]);
                tmp.SetActive(false);
                tmpList.Add(tmp);
            }
        }
    }

    public GameObject GetObject(string name)
    {
        List<GameObject> objectList;
        if(!pooledObjects.TryGetValue(name, out objectList))
        {
            return null;
        }

        for (int i = 0; i < objectList.Count; i++)
        {
            if (!objectList[i].activeInHierarchy)
            {
                return objectList[i];
            }
        }

        GameObject tmp;
        tmp = Instantiate(objectList[0]);
        tmp.SetActive(false);
        objectList.Add(tmp);
        return tmp;

    }

}

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject objectToPool;
    [Tooltip("Стартовое количество созданных объектов (может расширяться)")]
    [SerializeField] private int amountToPool;

    public static ObjectPool SharedInstance;
    private List<GameObject> pooledObject;

    private void Awake()
    {
        SharedInstance = this;
        pooledObject = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i<amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObject.Add(tmp);
        }
    }

    public GameObject GetObject()
    {
        for(int i = 0; i<amountToPool; i++)
        {
            if(!pooledObject[i].activeInHierarchy)
            {
                return pooledObject[i];
            }
        }

        GameObject tmp;
        tmp = Instantiate(objectToPool);
        tmp.SetActive(false);
        pooledObject.Add(tmp);
        amountToPool++;
        return tmp;

    }

}
*/
