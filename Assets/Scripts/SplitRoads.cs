using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadSplits
{
    public static GameObject SplitRoads(GameObject upperRoad, GameObject lowerRoad, GameObject splitPrefab, float lowerSize)
    {
        GameObject newRoad = GameObject.Instantiate(splitPrefab, upperRoad.transform.position, Quaternion.identity);

        newRoad.name = "Small Road";
        newRoad.transform.LookAt(lowerRoad.transform.position);
        newRoad.transform.localScale = new Vector3(splitPrefab.transform.localScale.x, splitPrefab.transform.localScale.y, Vector3.Distance(upperRoad.transform.position, lowerRoad.transform.position));
        newRoad.transform.position += newRoad.transform.forward * (newRoad.transform.localScale.z / 2);

        return newRoad;
    }
}
