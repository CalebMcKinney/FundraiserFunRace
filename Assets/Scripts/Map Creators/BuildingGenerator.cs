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

    public RoadGenerator roadGen;

    public int maxHeight;

    //public int[] buildingIDs;
    public GameObject[] generatedBuildings;
    public GameObject[] blankBuildingList;
    public int uniqueBuildings;

    public float buildingDistFromRoad;
    public float urbanBuildingSize;

    public void InitializeUniqueBuildings()
    {
        generatedBuildings = new GameObject[uniqueBuildings + 1];

        GameObject generatedBuildingContainer = new GameObject("BUILDING PREFABS");
        generatedBuildingContainer.transform.localPosition = new Vector3(0f, 0f, buildingDistFromRoad);


        Vector3 pos = new Vector3((-1.25f / 1.75f) * buildingSize, 0f, (-1.35f / 1.75f) * buildingSize);

        for (int i = 0; i <= uniqueBuildings; i++)
        {
            GameObject newBuilding = generateUrbanBuilding(pos, Vector3.zero, false);
            newBuilding.transform.parent = generatedBuildingContainer.transform;

            generatedBuildings[i] = newBuilding;
        }

        blankBuildingList = generatedBuildings;
    }

    public void ClearDefaultBuildings()
    {
        foreach(GameObject i in blankBuildingList)
        {
            Destroy(i);
            Destroy(i.transform.root.gameObject);
        }
    }

    public GameObject buildingAtVector3(Vector3 location, Vector3 rotation)
    {
        Vector3 noise = location / coordGen.roadDistance;
        float coordinateNoise = Mathf.PerlinNoise(coordGen.ruralMapOffset.x + noise.x, coordGen.ruralMapOffset.y + noise.z);
        coordinateNoise = 10 * Mathf.Clamp01(coordinateNoise);

        int areaRuralityInt = Mathf.FloorToInt(coordinateNoise);

        GameObject chosenContainer;
        GameObject tempGameObject;

        if(areaRuralityInt >= 7)
        {
            tempGameObject = Instantiate(cube, location, Quaternion.Euler(rotation));
            tempGameObject.transform.localScale = new Vector3(1f, (buildingStartingHeight + (Mathf.PerlinNoise(location.x, location.z) * buildingAmplitude)), 1f);
            tempGameObject.transform.GetChild(0).localScale = new Vector3(buildingSize, 1f, buildingSize);

            chosenContainer = ruralContainer;
        }
        else if(areaRuralityInt <= 3)
        {
            tempGameObject = Instantiate(generatedBuildings[UnityEngine.Random.Range(0,uniqueBuildings)], location, Quaternion.Euler(rotation + (Vector3.up * 90)));
            tempGameObject.transform.localScale = Vector3.one * urbanBuildingSize;

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

        GameObject door = Instantiate(prefabList.DoorPrefabs[UnityEngine.Random.Range(0, prefabList.DoorPrefabs.Length)], position, Quaternion.identity);
        door.transform.parent = tempParent.transform;
        door.transform.name = "Door";

        if (useAwning)
        {
            for (int i = 0; i <= awningHeight; i++)
            {
                GameObject lowerWall = Instantiate(prefabList.WindowPrefabs[UnityEngine.Random.Range(0, prefabList.WindowPrefabs.Length)], position + new Vector3(0f, 0.9375f * tempParent.transform.childCount, 0f), Quaternion.identity);
                lowerWall.transform.parent = tempParent.transform;
                lowerWall.transform.name = "Lower Wall";
            }

            GameObject awning = Instantiate(cityPrefabs.AwningPrefabs[UnityEngine.Random.Range(0, cityPrefabs.AwningPrefabs.Length)], position + new Vector3(0f, 0.9375f * tempParent.transform.childCount, 0f), Quaternion.identity);
            awning.transform.parent = tempParent.transform;
            awning.transform.name = "Awning";
        }

        float offset = useAwning ? 0.619f : 0f;

        for (int i = 0; i <= buildingHeight; i++)
        {
            GameObject wall = Instantiate(prefabList.WindowPrefabs[UnityEngine.Random.Range(0, prefabList.WindowPrefabs.Length)], position + new Vector3(0f, (0.9375f * tempParent.transform.childCount) - offset, 0f), Quaternion.identity);
            wall.transform.parent = tempParent.transform;
            wall.transform.name = "Wall";
        }

        GameObject chosenPrefab = prefabList.RoofPrefabs[UnityEngine.Random.Range(0, prefabList.RoofPrefabs.Length)];
        GameObject roof = Instantiate(chosenPrefab, position + new Vector3(0f, (0.9375f * tempParent.transform.childCount) - offset, 0f), Quaternion.identity);

        roof.transform.localPosition += chosenPrefab.transform.position;
        roof.transform.parent = tempParent.transform;
        roof.transform.name = "Roof";

        tempParent.transform.Rotate(rotation);
        return tempParent;
    }

    public Mesh randomMeshGen()
    {
        GameObject obj = generateUrbanBuilding(Vector3.zero, Vector3.zero, false);

        MeshFilter[] filters = obj.GetComponentsInChildren<MeshFilter>();
        Mesh finalMesh = new Mesh();
        CombineInstance[] combiners = new CombineInstance[filters.Length];

        for (int i = 0; i < filters.Length; i++)
        {
            if(filters[i].transform == obj.transform)
            {
                continue;
            }

            combiners[i].subMeshIndex = 0;
            combiners[i].mesh = filters[i].sharedMesh;
            combiners[i].transform = filters[i].transform.localToWorldMatrix;
        }

        finalMesh.CombineMeshes(combiners);
        Destroy(obj);

        return finalMesh;
    }

    public void GenerateBuildings()
    {
        foreach (GameObject[] currentGridCoordinates in roadGen.allGridSquares)
        {
            for (int i = 0; i < housesPerRoad; i++)
            {
                float interpolant = Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1);

                Vector3 pos1 = Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position, interpolant);
                Vector3 pos2 = Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position, interpolant);

                Vector3 rot1 = FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position);
                Vector3 rot2 = FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position);

                buildingAtVector3(pos1, rot1);
                buildingAtVector3(pos1, rot1 + new Vector3(0f, 180f));

                buildingAtVector3(pos2, rot2);
                buildingAtVector3(pos2, rot2 + new Vector3(0f, 180f));
            }
        }
    }

    public Vector3 FindPerpendicularAngle(Vector3 pos1, Vector3 pos2)
    {
        var temp = Quaternion.LookRotation(pos2 - pos1).eulerAngles;
        return new Vector3(temp.x, temp.y + 90f, temp.z);
    }
}