using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManger : MonoBehaviour {
	public GameObject prefab;
	
	// Use this for initialization
	void Start () {
		PoolManager.instance.CreatePool(prefab, 3);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			PoolManager.instance.ReuseObject(prefab, Vector3.one, Quaternion.identity);
		}
	}
}
