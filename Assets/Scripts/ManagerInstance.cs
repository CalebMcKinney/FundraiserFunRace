using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerInstance : MonoBehaviour
{
    public GameObject prefab;
    public ViewDist viewDist;

    void Start()
    {
        PoolManager.instance.CreatePool(prefab, 144);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PoolManager.instance.ReuseObject(prefab, new Vector3((Time.fixedTime * 2), 0f, 0f), Quaternion.identity, Vector3.one * 2);
        }
    }
}
