using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Commonly shared accross all classes for this particular level!!
/// </summary>
public static class LevelData
{
    public static Dictionary<Vector3, Bubble> bubblesLevelData = new Dictionary<Vector3, Bubble>();

    public static void ResetLevelData()
    {
        bubblesLevelData.Clear();
        bubblesLevelData = new Dictionary<Vector3, Bubble>();
    }
}
