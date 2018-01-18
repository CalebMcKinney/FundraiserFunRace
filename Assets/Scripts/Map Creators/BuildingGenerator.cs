using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour {
    public int housesPerRoad;
    public GameObject[] ruralPrefabs;
    public GameObject[] suburbPrefabs;
    public GameObject[] urbanPrefabs;
    public GameObject[] schoolPrefab;

    private MapGenerator mapGen;
    private RoadGenerator roadGen;

    private void Start()
    {
        roadGen = GetComponent<RoadGenerator>();
    }

    public static GameObject buildingAtVector3(float urban, Vector3 location, Vector3 rotation)
    {
        urban = Mathf.Clamp01(urban);
        return GameObject.FindGameObjectWithTag("road");
    }

    public void GenerateBuildings()
    {
        foreach (GameObject[] currentGridCoordinates in roadGen.allGridSquares)
        {
            for (int i = 0; i <= housesPerRoad; i++)
            {

            }
        }
    }
}
