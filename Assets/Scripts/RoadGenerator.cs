using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadGenerator
{
    public static GameObject ConnectWithSideRoad(Vector3 position1, Vector3 position2, GameObject sideRoadPrefab, GameObject sideRoadParent, float lowerSize)
    {
        float offset = 9f;
        GameObject newRoad = GameObject.Instantiate(sideRoadPrefab, position1, Quaternion.identity);

        newRoad.name = "Small Road";
        newRoad.transform.LookAt(position2);
        newRoad.transform.localScale = new Vector3(lowerSize, sideRoadPrefab.transform.localScale.y, Vector3.Distance(position1, position2) - offset);
        newRoad.transform.position += newRoad.transform.forward * ((newRoad.transform.localScale.z / 2) + offset/2);
        newRoad.transform.parent = sideRoadParent.transform;

        return newRoad;
    }

    public static GameObject ConnectWithMainRoad(Vector3 position1, Vector3 position2, GameObject roadPrefab)
    {
        GameObject newRoad = GameObject.Instantiate(roadPrefab, position1, Quaternion.identity);

        newRoad.transform.LookAt(position2);
        newRoad.transform.localScale = new Vector3(roadPrefab.transform.localScale.x, roadPrefab.transform.localScale.y, Vector3.Distance(position1, position2));
        newRoad.transform.position += newRoad.transform.forward * (newRoad.transform.localScale.z / 2);

        return newRoad;
    }
}
