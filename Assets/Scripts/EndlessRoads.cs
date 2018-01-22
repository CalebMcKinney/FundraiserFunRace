using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRoads : MonoBehaviour {

    //number of chunks the player can see
    public float chunkViewDistance;
    float chunkSize;

    public Transform viewer;

    private Vector2 viewerPosition;
    public Vector2 viewerCoordinate;

    public static List<GameObject> objectsInGame = new List<GameObject>();

    public Dictionary<Vector2, GameObject[]> endlessCoordDictionary;

    private void Start()
    {
        CoordinateGenerator coordGen = gameObject.GetComponent<CoordinateGenerator>();
        chunkSize = coordGen.roadDistance;
    }

    public void InitializeObjectList()
    {
        foreach(GameObject i in FindObjectsOfType<GameObject>())
        {
            objectsInGame.Clear();
            objectsInGame.Add(i);
        }
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.transform.position.x, viewer.transform.position.z);
        Vector2 newChunkCoords = new Vector2(Mathf.CeilToInt(viewerPosition.x / chunkSize), Mathf.CeilToInt(viewerPosition.y / chunkSize));

        if (viewerCoordinate != newChunkCoords)
        {
            NewChunkCenter(newChunkCoords);
        }

        viewerCoordinate = newChunkCoords;
    }

    public void NewChunkCenter(Vector2 chunkCenter)
    {
        List<GameObject> objectsInChunk = new List<GameObject>();

        var allCollidersInChunk = Physics.OverlapBox(new Vector3(chunkCenter.x * chunkSize + chunkSize / 2, 0f, chunkCenter.y * chunkSize + chunkSize / 2), (Vector3.one / 2) * chunkViewDistance * chunkSize);
        foreach (Collider currentCollider in allCollidersInChunk)
        {
            objectsInChunk.Add(currentCollider.gameObject);
        }

        foreach (GameObject i in objectsInGame)
        {
            if(!i.CompareTag("Coordinate"))
            {
                if (objectsInChunk.Contains(i) || i.CompareTag("Important") || i.CompareTag("Player") || i.CompareTag("MainCamera") || i.CompareTag("MainCamera"))
                {
                    i.SetActive(true);
                }
                else
                {
                    i.SetActive(false);
                }
            }
        }
    }
}
