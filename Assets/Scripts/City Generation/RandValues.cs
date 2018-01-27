using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandValues
{
    [Space(-10)]
    [Header("Value Pair")][Tooltip("Two percentage values, which represent rural and urban values.")]
    public int ruralValue;
    public int urbanValue;
}
