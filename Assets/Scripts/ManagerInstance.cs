using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInstance : MonoBehaviour {
    public GameObject prefab;

    void Start()
    {
        PoolManager.instance.CreatePool(prefab, 3);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PoolManager.instance.ReuseObject(prefab, Vector3.one, Quaternion.identity);
        }
    }
}
