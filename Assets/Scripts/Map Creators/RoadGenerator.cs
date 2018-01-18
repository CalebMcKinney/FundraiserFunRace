﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RoadGenerator : MonoBehaviour {
    public MapGenerator mapGen;

    public float roadWidth;

    public GameObject splitRoadContainer;
    public GameObject horizontalContainer;
    public GameObject verticalContainer;

    public GameObject roadPrefab;
    public GameObject splitPrefab;

    public List<GameObject[]> allGridSquares = new List<GameObject[]>();

    public void CreateRoadGameObject()
    {
        GameObject[] allCoordinates = GameObject.FindGameObjectsWithTag("Coordinate");

        foreach (GameObject currentObject in allCoordinates)
        {
            if (currentObject.GetComponent<Identifier>().location <= mapGen.size * mapGen.size)
            {
                GameObject roadHorizontal;
                GameObject roadVertical;

                if (currentObject.GetComponent<Identifier>().locationY < mapGen.size - 1)
                {
                    roadHorizontal = ConnectWithMainRoad(currentObject.transform.position, mapGen.coordinateDictionary[currentObject.GetComponent<Identifier>().location + 1].transform.position, roadPrefab);
                    roadHorizontal.transform.parent = horizontalContainer.transform;
                    roadHorizontal.name = "Horizontal Road";
                }

                if (currentObject.GetComponent<Identifier>().locationX < mapGen.size - 1)
                {
                    roadVertical = ConnectWithMainRoad(currentObject.transform.position, mapGen.coordinateDictionary[currentObject.GetComponent<Identifier>().location + (Convert.ToInt16(mapGen.size))].transform.position, roadPrefab);
                    roadVertical.transform.parent = verticalContainer.transform;
                    roadVertical.name = "Vertical Road";
                }
            }

            if (currentObject.GetComponent<Identifier>().locationY < mapGen.size - 1 && currentObject.GetComponent<Identifier>().locationX < mapGen.size - 1)
            {
                allGridSquares.Add(new GameObject[4] {
                    mapGen.coordinateDictionary[currentObject.GetComponent<Identifier>().location],                              //One at original point
                    mapGen.coordinateDictionary[currentObject.GetComponent<Identifier>().location + 1],                          //One to right
                    mapGen.coordinateDictionary[currentObject.GetComponent<Identifier>().location + (Convert.ToInt16(mapGen.size))],    //One down
                    mapGen.coordinateDictionary[currentObject.GetComponent<Identifier>().location + (Convert.ToInt16(mapGen.size) + 1)] //One to lower right
                });
            }
        }

        foreach (GameObject[] currentGridCoordinates in allGridSquares)
        {
            ConnectWithSideRoad(Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[1].transform.position, 0.5f), Vector3.Lerp(currentGridCoordinates[2].transform.position, currentGridCoordinates[3].transform.position, 0.5f), splitPrefab, splitRoadContainer, roadWidth * 0.75f);
            ConnectWithSideRoad(Vector3.Lerp(currentGridCoordinates[1].transform.position, currentGridCoordinates[3].transform.position, 0.5f), Vector3.Lerp(currentGridCoordinates[0].transform.position, currentGridCoordinates[2].transform.position, 0.5f), splitPrefab, splitRoadContainer, roadWidth * 0.75f);
        }
    }



    public static GameObject ConnectWithSideRoad(Vector3 position1, Vector3 position2, GameObject sideRoadPrefab, GameObject sideRoadParent, float lowerSize)
    {
        float offset = 9f;
        GameObject newRoad = Instantiate(sideRoadPrefab, position1, Quaternion.identity);

        newRoad.name = "Small Road";
        newRoad.transform.LookAt(position2);
        newRoad.transform.localScale = new Vector3(lowerSize, sideRoadPrefab.transform.localScale.y, Vector3.Distance(position1, position2) - offset);
        newRoad.transform.position += newRoad.transform.forward * ((newRoad.transform.localScale.z / 2) + offset/2);
        newRoad.transform.parent = sideRoadParent.transform;

        return newRoad;
    }

    public static GameObject ConnectWithMainRoad(Vector3 position1, Vector3 position2, GameObject roadPrefab)
    {
        GameObject newRoad = Instantiate(roadPrefab, position1, Quaternion.identity);

        newRoad.transform.LookAt(position2);
        newRoad.transform.localScale = new Vector3(roadPrefab.transform.localScale.x, roadPrefab.transform.localScale.y, Vector3.Distance(position1, position2));
        newRoad.transform.position += newRoad.transform.forward * (newRoad.transform.localScale.z / 2);

        return newRoad;
    }
}