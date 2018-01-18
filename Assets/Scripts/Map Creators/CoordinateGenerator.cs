using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoordinateGenerator : MonoBehaviour {
    public MapGenerator mapGen;
    public Vector2 ruralMapOffset;
    private Vector2[,] roadIntersections;
    public GameObject coordinatePrefab;
    public float roadDistance;
    [Tooltip("How much to offset country roads")]
    public float countryOffset;

    private void Start()
    {
        mapGen = GetComponent<MapGenerator>();
    }

    public void CreateCoordinates()
    {
        roadIntersections = new Vector2[mapGen.size, mapGen.size];
        ruralMapOffset = new Vector2(UnityEngine.Random.Range(-10000f, 10000f), UnityEngine.Random.Range(-10000f, 10000f));

        for (int x = 0; x < roadIntersections.GetLength(0); x++)
        {
            for (int y = 0; y < roadIntersections.GetLength(1); y++)
            {
                roadIntersections[x, y] = new Vector2(x - mapGen.size / 2, y - mapGen.size / 2);
                float currentLocationValue = Mathf.PerlinNoise(x + ruralMapOffset.x, y + ruralMapOffset.y);

                if (currentLocationValue < 0.5)
                {
                    roadIntersections[x, y] = roadIntersections[x, y] + Vector2.one * UnityEngine.Random.Range(-1 * (Mathf.Lerp(1, 0, currentLocationValue * 2) * countryOffset), Mathf.Lerp(1, 0, currentLocationValue * 2) * countryOffset);
                }
            }
        }
    }

    public void CreateCoordinateGameObjects()
    {
        int i = 0;

        for (int x = 0; x < roadIntersections.GetLength(0); x++)
        {
            for (int y = 0; y < roadIntersections.GetLength(1); y++)
            {
                var createdObject = Instantiate(coordinatePrefab, new Vector3(roadIntersections[x, y].x, 0, roadIntersections[x, y].y) * roadDistance, Quaternion.identity);
                createdObject.GetComponent<Identifier>().locationX = x;
                createdObject.GetComponent<Identifier>().locationY = y;
                createdObject.GetComponent<Identifier>().location = i;

                mapGen.coordinateDictionary.Add(i, createdObject);
                i++;
            }
        }
    }

    public void ClearCoordinates() { foreach (GameObject i in GameObject.FindGameObjectsWithTag("Coordinate")) { Destroy(i); } }

}
