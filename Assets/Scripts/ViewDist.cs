using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ViewDist : MonoBehaviour {

    //number of chunks the player can see
    public float chunkViewDistance;
    float chunkSize;

    public Transform viewer;

    private Vector2 viewerPosition;
    public Vector2 viewerCoordinate;

    public GameObject buildingContainer;
    public GameObject roadContainer;


    private void Start()
    {
        CoordinateGenerator coordGen = gameObject.GetComponent<CoordinateGenerator>();
        chunkSize = coordGen.roadDistance;

        foreach (Renderer i in buildingContainer.GetComponentsInChildren<Renderer>())
        { i.enabled = false; }

        foreach (Renderer i in roadContainer.GetComponentsInChildren<Renderer>())
        { i.enabled = false; }

        NewChunkCenter(new Vector2(0,0));

        //buildingContainer = gameObject.GetComponent<BuildingGenerator>().suburbContainer.transform.parent.gameObject;
        //roadContainer = gameObject.GetComponent<RoadGenerator>().horizontalContainer.transform.parent.gameObject;
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

        Collider[] allCollidersInChunk = Physics.OverlapSphere(viewer.position, chunkViewDistance * chunkSize);
        Collider[] renderersToCheck = Physics.OverlapSphere(viewer.position, chunkViewDistance * chunkSize * 1.5f);

        foreach (Collider currentCollider in allCollidersInChunk)
        {
            objectsInChunk.Add(currentCollider.gameObject);
        }

        foreach (Collider i in renderersToCheck)
        {
            i.gameObject.GetComponentInChildren<Renderer>().enabled = objectsInChunk.Contains(i.gameObject) || i.transform.root.CompareTag("Important") || i.transform.root.CompareTag("Player") || i.transform.root.CompareTag("MainCamera") && !i.transform.root.CompareTag("Container");
        }
    }
}
