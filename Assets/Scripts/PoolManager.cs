using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour
{

    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    static PoolManager _instance;

    public static PoolManager instance
    {
<<<<<<< HEAD
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    public void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(newObject);
                newObject.SetParent(poolHolder.transform);
            }
        }
    }
=======
		get {
			if (_instance == null)
            {
				_instance = FindObjectOfType<PoolManager> ();
			}
			return _instance;
		}
	}

	public void CreatePool(GameObject prefab, int poolSize) {
		int poolKey = prefab.GetInstanceID ();

		if (!poolDictionary.ContainsKey (poolKey))
        {
			poolDictionary.Add (poolKey, new Queue<ObjectInstance> ());

			GameObject poolHolder = new GameObject (prefab.name + " pool");
			poolHolder.transform.parent = transform;

			for (int i = 0; i < poolSize; i++)
            {
				ObjectInstance newObject = new ObjectInstance(Instantiate (prefab) as GameObject);
				poolDictionary [poolKey].Enqueue (newObject);
				newObject.SetParent (poolHolder.transform);
			}
		}
	}
>>>>>>> 3b9e55fd1585932c0f32cf1f592d56cab016c422

    public void CreatePool(GameObject prefab, int poolSize, GameObject container)
    {
        int poolKey = prefab.GetInstanceID();

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            GameObject poolHolder = new GameObject(prefab.name + " pool");
            poolHolder.transform.parent = transform;

            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(newObject);
                newObject.SetParent(container.transform);
            }
        }
    }

<<<<<<< HEAD
    public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 size)
    {
        int poolKey = prefab.GetInstanceID();
        GameObject temp = new GameObject();

        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            temp = objectToReuse.Reuse(position, rotation);
        }

        return temp;
    }
=======
    public GameObject ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 size) {
		int poolKey = prefab.GetInstanceID ();
        GameObject temp = new GameObject();

		if (poolDictionary.ContainsKey (poolKey))
        {
			ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue ();
			poolDictionary[poolKey].Enqueue (objectToReuse);

			temp = objectToReuse.Reuse(position, rotation);
		}

        return temp;
	}
>>>>>>> 3b9e55fd1585932c0f32cf1f592d56cab016c422

    public GameObject ReuseObject(int instanceID, Vector3 position, Quaternion rotation, Vector3 size)
    {
        int poolKey = instanceID;
        GameObject temp = new GameObject();


        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            temp = objectToReuse.Reuse(position, rotation);
        }

        return temp;
    }


<<<<<<< HEAD
    public class ObjectInstance
    {

        GameObject gameObject;
        Transform transform;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);
        }

        public GameObject Reuse(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;
            return gameObject;
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
=======
    public class ObjectInstance {

		GameObject gameObject;
		Transform transform;

		public ObjectInstance(GameObject objectInstance) {
			gameObject = objectInstance;
			transform = gameObject.transform;
			gameObject.SetActive(false);
		}

        public GameObject Reuse(Vector3 position, Quaternion rotation) {
			gameObject.SetActive (true);
			transform.position = position;
			transform.rotation = rotation;
            return gameObject;
		}

		public void SetParent(Transform parent) {
			transform.parent = parent;
		}
	}
>>>>>>> 3b9e55fd1585932c0f32cf1f592d56cab016c422
}