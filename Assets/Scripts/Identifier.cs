using UnityEngine;
using System.Collections.Generic;

public class Identifier : MonoBehaviour
{
    public int locationX;
    public int locationY;
    public int location;

    public List<Vector2> connectedTo = new List<Vector2>();
}