using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapGenerator : MonoBehaviour {

    [Tooltip("The number of grid squares on the map")] [Range(2, 100)]
    public int size;

    public Dictionary<int, GameObject> coordinateDictionary = new Dictionary<int, GameObject>();

    public CoordinateGenerator coordGen;
    public RoadGenerator roadGen;
    public BuildingGenerator buildingGen;

    private void Start()
    {
        GenerateRoads();
    }

    public void GenerateRoads()
    {
        coordGen.CreateCoordinates();
        coordGen.CreateCoordinateGameObjects();
        roadGen.CreateRoadGameObject();
    }
}
