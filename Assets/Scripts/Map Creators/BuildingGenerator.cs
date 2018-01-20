using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BuildingGenerator : MonoBehaviour {
    public int housesPerRoad;
    public GameObject suburbContainer;
    public GameObject ruralContainer;
    public GameObject urbanContainer;

    [Space(5)]

    public GameObject[] ruralPrefabs;
    public GameObject[] suburbPrefabs;
    public GameObject[] urbanPrefabs;
    public GameObject schoolPrefab;

    public string seedInput;
    public List<int[]> seedInputSplit = new List<int[]>();

    private MapGenerator mapGen;
    public RoadGenerator roadGen;

    private void Start()
    {
        foreach(string i in seedInput.Split('/'))
        {
            List<int> temp = new List<int>();
            foreach(string x in i.Split('.'))
            {
                temp.Add(Convert.ToInt16(x));
            }

            seedInputSplit.Add(temp.ToArray());
        }
    }

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
