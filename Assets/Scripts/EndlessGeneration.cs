using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGeneration : MonoBehaviour
{
    [Header("Endless Generation")]
    public int uniqueBuildingNumber;
    public int[] buildingIDs;

    public Vector2[,] allCoordinates;

    public GameObject roads;
    public GameObject roadPrefab;

    public MapGenerator mapGen;
    public RoadGenerator roadGen;
    public BuildingGenerator buildingGen;
    public CoordinateGenerator coordGen;

    [Header("View Distance")]
    public int chunkViewDistance;
    float chunkSize;

    public Transform viewer;

    private Vector2 viewerPosition;
    public Vector2 oldChunkCoords;

    public Dictionary<Vector2, GameObject> coordinateDictionary = new Dictionary<Vector2, GameObject>();

    public GameObject urbanContainer;
    public GameObject roadContainer;
    public GameObject suburbContainer;

    public GameObject coordinate;

    public Vector2 ruralMapOffset;
    public Vector2 heightMapOffset;

    public float countryOffset;
    public float heightVariation;

    void Start()
    {
        ruralMapOffset = new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));
        heightMapOffset = new Vector2(Random.Range(-10000, 10000), Random.Range(-10000, 10000));

        buildingIDs = new int[uniqueBuildingNumber];

        List<Transform[]> altDiffBuildings = new List<Transform[]>();

        for (int i = 0; i < uniqueBuildingNumber; i++)
        {
            GameObject newBuilding = buildingGen.generateUrbanBuilding(Vector3.zero, Vector3.zero, false);
            altDiffBuildings.Add(newBuilding.GetComponentsInChildren<Transform>());

            PoolManager.instance.CreatePool(newBuilding, Mathf.Max(((chunkViewDistance + 1) ^ 2) * (buildingGen.housesPerRoad * 4), 15), urbanContainer);
            buildingIDs[i] = newBuilding.GetInstanceID();
            Destroy(newBuilding);
        }

        PoolManager.instance.CreatePool(roads, 256, roadContainer);

        chunkSize = coordGen.roadDistance;

        GenerateChunk(Vector2.zero, Vector2.zero);
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.transform.position.x, viewer.transform.position.z);
        Vector2 newChunkCoords = new Vector2(Mathf.CeilToInt(viewerPosition.x / chunkSize), Mathf.CeilToInt(viewerPosition.y / chunkSize));

        if (oldChunkCoords != newChunkCoords)
        {
            GenerateChunk(newChunkCoords, oldChunkCoords);
        }

        oldChunkCoords = newChunkCoords;
    }

    public void GenerateChunk(Vector2 chunkCenter, Vector2 previousChunk)
    {
        int gridSideLength = (2 * chunkViewDistance) + 1;
        Vector2[,] newCoordinateLocations = new Vector2[gridSideLength, gridSideLength];

        for (int y = 0; y < newCoordinateLocations.GetLongLength(0); y++)
        {
            for (int x = 0; x < newCoordinateLocations.GetLongLength(1); x++)
            {
                newCoordinateLocations[x, y] = new Vector2(x + chunkCenter.x, y + chunkCenter.y) * chunkSize;
            }
        }

        OffsetCountry(ref newCoordinateLocations);

        //Repeat through all coordinates again after coordinates are offset.
        for (int y = 0; y < newCoordinateLocations.GetLongLength(0); y++)
        {
            for (int x = 0; x < newCoordinateLocations.GetLongLength(1); x++)
            {
                var newCoordObj = Instantiate(coordinate, new Vector3(newCoordinateLocations[x, y].x, 0, newCoordinateLocations[x, y].y), Quaternion.identity);

                if(!coordinateDictionary.ContainsKey(new Vector2(x + chunkCenter.x, y + chunkCenter.y)))
                {
                    coordinateDictionary.Add(new Vector2(x + chunkCenter.x, y + chunkCenter.y), newCoordObj);
                }

                //Check all sides of the current coordinate
                for (int i = 0; i < 4; i++)
                {
                    Vector2 coordToCheck = new Vector2(x + chunkCenter.x, y + chunkCenter.y);
                    Vector3 pos1 = new Vector3(coordToCheck.x, 0, coordToCheck.y);

                    switch (i)
                    {
                        default:
                            coordToCheck += new Vector2(1, 0);
                            break;
                        case 1:
                            coordToCheck += new Vector2(-1, 0);
                            break;
                        case 2:
                            coordToCheck += new Vector2(0, 1);
                            break;
                        case 3:
                            coordToCheck += new Vector2(0, -1);
                            break;
                    }

                    if(coordinateDictionary.ContainsKey(coordToCheck))
                    {
                        //Continue stops everything below this condition from happening if this condition is true
                        if (coordinateDictionary[coordToCheck].GetComponent<Identifier>().connectedTo.Contains(new Vector2(x + chunkCenter.x, y + chunkCenter.y)))
                        {
                            continue;
                        }

                        GameObject objectOverOne = coordinateDictionary[coordToCheck];

                        Vector3 pos2 = objectOverOne.transform.position;

                        RoadGenerator.ConnectWithMainRoad(pos1, pos2, roadPrefab);

                        Identifier coordID = objectOverOne.GetComponent<Identifier>();

                        if (coordID != null)
                        {
                            coordID.connectedTo.Add(coordToCheck);
                        }
                    }
                }
            }
        }

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

    public void OffsetCountry(ref Vector2[,] coordinateGrid)
    {
        for (int y = 0; y < coordinateGrid.GetLongLength(0); y++)
        {
            for (int x = 0; x < coordinateGrid.GetLongLength(1); x++)
            {
                float currentLocationValue = Mathf.PerlinNoise(x + ruralMapOffset.x, y + ruralMapOffset.y);
                coordinateGrid[x, y] = coordinateGrid[x, y] + Vector2.one * Random.Range(Mathf.Lerp(1, 0, currentLocationValue * 2) * -countryOffset, Mathf.Lerp(1, 0, currentLocationValue * 2) * countryOffset);
            }
        }
    }
}
