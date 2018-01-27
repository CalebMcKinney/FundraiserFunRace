using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingGenerator : MonoBehaviour {

    public CityPrefabs cityPrefabs;

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

    private MapGenerator mapGen;
    public RoadGenerator roadGen;

    public int maxHeight;


    public GameObject buildingAtVector3(float urban, Vector3 location, Vector3 rotation)
    {
        urban = Mathf.Clamp01(urban);
        GameObject buildingToInstantiate;
        int chosenPercent = 50; //UnityEngine.Random.Range(0, 100);

        int ruralPercent = 10; //seedInputSplit[(int)(urban * 10)][0];
        int urbanPercent = 10; //seedInputSplit[(int)(urban * 10)][2];

        GameObject chosenContainer;

        if(chosenPercent >= 100 - ruralPercent)
        {
            buildingToInstantiate = ruralPrefabs[0]; //UnityEngine.Random.Range(0,ruralPrefabs.Length)];
            chosenContainer = ruralContainer;
        }
        else if(chosenPercent <= urbanPercent)
        {
            buildingToInstantiate = urbanPrefabs[0]; //UnityEngine.Random.Range(0, urbanPrefabs.Length)];
            chosenContainer = urbanContainer;
        }
        else
        {
            buildingToInstantiate = suburbPrefabs[UnityEngine.Random.Range(0, suburbPrefabs.Length)];
            chosenContainer = suburbContainer;
        }

        var temp = Instantiate(buildingToInstantiate, location, Quaternion.Euler(rotation));
        temp.transform.parent = chosenContainer.transform;
        return temp;
    }

    public GameObject generateUrbanBuilding(Vector3 position, Vector3 rotation)
    {
        GameObject tempParent = Instantiate(urbanContainer, Vector3.zero, Quaternion.identity);
        tempParent.name = "Building";

        bool roundShape = UnityEngine.Random.Range(0, 2) == 0;  //50:50 chance round or square, max value is exclusive, so it will never be 2
        bool useAwning = UnityEngine.Random.Range(0, 3) == 0;   //1 in 3 chance of no awning in square;

        int buildingHeight = UnityEngine.Random.Range(2, Mathf.Max(maxHeight, 4));
        int awningHeight = UnityEngine.Random.Range(1, 3);

        Vector3 oldRotation = rotation;
        rotation += roundShape ? Vector3.up * 45f : Vector3.zero;
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

            GameObject awning = Instantiate(cityPrefabs.AwningPrefabs[UnityEngine.Random.Range(0, cityPrefabs.AwningPrefabs.Length)], position + new Vector3(0f, 0.9375f * tempParent.transform.childCount, 0f), Quaternion.Euler(oldRotation));
            awning.transform.parent = tempParent.transform;
            awning.transform.name = "Awning";
        }

        float offset = useAwning ? 0.62f : 0f;

        for (int i = 0; i <= buildingHeight; i++)
        {
            GameObject wall = Instantiate(prefabList.WindowPrefabs[UnityEngine.Random.Range(0, prefabList.WindowPrefabs.Length)], position + new Vector3(0f, (0.9375f * tempParent.transform.childCount) - offset, 0f), Quaternion.Euler(rotation));
            wall.transform.parent = tempParent.transform;
            wall.transform.name = "Wall";
        }

        GameObject roof = Instantiate(prefabList.RoofPrefabs[UnityEngine.Random.Range(0, prefabList.RoofPrefabs.Length)], position + new Vector3(0f, (0.9375f * tempParent.transform.childCount) - offset, 0f), Quaternion.Euler(rotation));
        roof.transform.parent = tempParent.transform;
        roof.transform.name = "Roof";

        return tempParent;
    }

    public void GenerateBuildings()
    {
        foreach (GameObject[] currentGridCoordinates in roadGen.allGridSquares)
        {
            for (int i = 0; i < housesPerRoad; i++)
            {
                buildingAtVector3(0.5f, Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position, Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1)), FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position));
                buildingAtVector3(0.5f, Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position, Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1)), FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position) + new Vector3(0f, 180f));

                buildingAtVector3(0.5f, Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position, Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1)), FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position));
                buildingAtVector3(0.5f, Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position, Mathf.InverseLerp(0f, housesPerRoad + 1, i + 1)), FindPerpendicularAngle(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position) + new Vector3(0f, 180f));
            }
        }
    }

    public Vector3 FindPerpendicularAngle(Vector3 pos1, Vector3 pos2)
    {
        var temp = Quaternion.LookRotation(pos2 - pos1).eulerAngles;
        return new Vector3(temp.x, temp.y + 90f, temp.z);
    }
}
