using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingGenerator : MonoBehaviour {

    public CoordinateGenerator coordGen;

    public float buildingStartingHeight = 7f;
    public float buildingAmplitude = 5f;
    public float buildingSize = 5f;

    public CityPrefabs cityPrefabs;
    public RandValues[] UrbanBoundsValues;

    [Header("Suburbs")]

    public int housesPerRoad;

    [Space(5)]

    public GameObject suburbContainer;
    public GameObject ruralContainer;
    public GameObject urbanContainer;

    [Space(5)]

    public GameObject[] ruralPrefabs;
    public GameObject[] suburbPrefabs;
    public GameObject[] urbanPrefabs;

    [Space(5)]

    public GameObject schoolPrefab;
    public GameObject cube;

    public GameObject test;

    private MapGenerator mapGen;
    public RoadGenerator roadGen;

    public int maxHeight;

    private void Start()
    {
        //generateUrbanBuilding(Vector3.zero, Vector3.zero, false);
        Instantiate(test).GetComponent<MeshFilter>().sharedMesh = randomMeshGen();
    }

    public GameObject buildingAtVector3(Vector3 location, Vector3 rotation)
    {
        /* Vector3 noise = location / coordGen.roadDistance;
        float coordinateNoise = Mathf.PerlinNoise(coordGen.ruralMapOffset.x + noise.x, coordGen.ruralMapOffset.y + noise.z);
        coordinateNoise = 10 * Mathf.Clamp01(coordinateNoise);

        int areaRuralityInt = Mathf.FloorToInt(coordinateNoise);

        Debug.Log(areaRuralityInt);
        

        int ruralValue = UrbanBoundsValues[areaRuralityInt].ruralValue;
        int urbanValue = UrbanBoundsValues[areaRuralityInt].urbanValue;
        */
        int chosenPercent = UnityEngine.Random.Range(0, 100);

        GameObject chosenContainer;
        GameObject tempGameObject;

        if(chosenPercent >= 100 - 15)
        {
            tempGameObject = Instantiate(cube, location, Quaternion.Euler(rotation));
            tempGameObject.transform.localScale = new Vector3(1f, (buildingStartingHeight + (Mathf.PerlinNoise(location.x, location.z) * buildingAmplitude)), 1f);
            tempGameObject.transform.GetChild(0).localScale = new Vector3(buildingSize, 1f, buildingSize);

            chosenContainer = ruralContainer;
        }
        else if(chosenPercent <= 30)
        {
            tempGameObject = Instantiate(cube, location, Quaternion.Euler(rotation));
            tempGameObject.transform.localScale = new Vector3(1f, (buildingStartingHeight + (Mathf.PerlinNoise(location.x, location.z) * buildingAmplitude)), 1f);
            tempGameObject.transform.GetChild(0).localScale = new Vector3(buildingSize, 1f, buildingSize);

            chosenContainer = urbanContainer;
        }
        else
        {
            tempGameObject = Instantiate(suburbPrefabs[UnityEngine.Random.Range(0, suburbPrefabs.Length)], location, Quaternion.Euler(rotation));
            chosenContainer = suburbContainer;
        }

        tempGameObject.transform.parent = chosenContainer.transform;
        return tempGameObject;
    }

    public GameObject generateUrbanBuilding(Vector3 position, Vector3 rotation, bool roundShape)
    {
        GameObject tempParent = Instantiate(urbanContainer, Vector3.zero, Quaternion.identity);
        tempParent.name = "Building";

        //bool roundShape = UnityEngine.Random.Range(0, 2) == 0;  //50:50 chance round or square, max value is exclusive, so it will never be 2
        bool useAwning = UnityEngine.Random.Range(0, 3) == 0 && !roundShape;   //1 in 3 chance of no awning in square;

        int buildingHeight = UnityEngine.Random.Range(2, Mathf.Max(maxHeight, 4));
        int awningHeight = UnityEngine.Random.Range(1, 2);

        UrbanPrefabList prefabList = roundShape ? cityPrefabs.RoundPrefabs : cityPrefabs.SquarePrefabs;

        GameObject door = Instantiate(prefabList.DoorPrefabs[UnityEngine.Random.Range(0, prefabList.DoorPrefabs.Length)], position, Quaternion.Euler(rotation));
        door.transform.parent = tempParent.transform;
        door.transform.name = "Door";

        if (useAwning)
        {
            for (int i = 0; i <= awningHeight; i++)
            {
                GameObject lowerWall = Instantiate(prefabList.WindowPrefabs[UnityEngine.Random.Range(0, prefabList.WindowPrefabs.Length)], position + new Vector3(0f, 0.9375f * tempParent.transform.childCount, 0f), Quaternion.Euler(rotation));
                lowerWall.transform.parent = tempParent.transform;
                lowerWall.transform.name = "Lower Wall";
            }

            GameObject awning = Instantiate(cityPrefabs.AwningPrefabs[UnityEngine.Random.Range(0, cityPrefabs.AwningPrefabs.Length)], position + new Vector3(0f, 0.9375f * tempParent.transform.childCount, 0f), Quaternion.Euler(rotation));
            awning.transform.parent = tempParent.transform;
            awning.transform.name = "Awning";
        }

        float offset = useAwning ? 0.619f : 0f;

        for (int i = 0; i <= buildingHeight; i++)
        {
            GameObject wall = Instantiate(prefabList.WindowPrefabs[UnityEngine.Random.Range(0, prefabList.WindowPrefabs.Length)], position + new Vector3(0f, (0.9375f * tempParent.transform.childCount) - offset, 0f), Quaternion.Euler(rotation));
            wall.transform.parent = tempParent.transform;
            wall.transform.name = "Wall";
        }

        GameObject chosenPrefab = prefabList.RoofPrefabs[UnityEngine.Random.Range(0, prefabList.RoofPrefabs.Length)];
        GameObject roof = Instantiate(chosenPrefab, position + new Vector3(0f, (0.9375f * tempParent.transform.childCount) - offset, 0f), Quaternion.Euler(rotation));

        roof.transform.localPosition += chosenPrefab.transform.position;
        roof.transform.parent = tempParent.transform;
        roof.transform.name = "Roof";

        return tempParent;
    }

    public Mesh randomMeshGen()
    {
        GameObject obj = generateUrbanBuilding(Vector3.zero, Vector3.zero, false);

        MeshFilter[] meshFilters = obj.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combine);

        Destroy(obj);
        return finalMesh;
    }

    public void GenerateBuildings()
    {
        foreach (GameObject[] currentGridCoordinates in roadGen.allGridSquares)
        {
            for (int i = 0; i < housesPerRoad; i++)
            {
                buildingAtVector3(Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position, Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1)), FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position));
                buildingAtVector3(Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position, Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1)), FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position) + new Vector3(0f, 180f));

                buildingAtVector3(Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position, Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1)), FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position));
                buildingAtVector3(Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position, Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1)), FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position) + new Vector3(0f, 180f));
            }
        }
    }

    public Vector3 FindPerpendicularAngle(Vector3 pos1, Vector3 pos2)
    {
        var temp = Quaternion.LookRotation(pos2 - pos1).eulerAngles;
        return new Vector3(temp.x, temp.y + 90f, temp.z);
    }
}
