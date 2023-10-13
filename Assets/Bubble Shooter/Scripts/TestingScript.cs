using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using SNGames.BubbleShooter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestingScript : MonoBehaviour
{
    [SerializedDictionary]
    public SerializedDictionary<Vector3, Bubble> bubblesLevelData = new SerializedDictionary<Vector3, Bubble>();

    //Testing BFS
    [Foldout("BFS Testing")]
    [SerializeField] private Bubble bubbleToTestBFS;
    [Foldout("BFS Testing")]
    [SerializeField] private List<Bubble> allNodes;
    [Foldout("BFS Testing")]
    [SerializeField] private List<Bubble> allVisitedNodes;
    [Foldout("BFS Testing")]
    [SerializeField] private List<Bubble> leftOutNodes;

    private void Update()
    {
        bubblesLevelData = new SerializedDictionary<Vector3, Bubble>();
        foreach (var item in LevelData.bubblesLevelDataDictionary)
        {
            bubblesLevelData.Add(item.Key, item.Value);
        }
    }

    [Button]
    public void TestBFSAlgo()
    {
        allNodes = new List<Bubble>();
        foreach (var item in LevelData.bubblesLevelDataDictionary)
        {
            allNodes.Add(item.Value);
        }

        allVisitedNodes = BubbleShooter_HelperFunctions.GetAllReachableNodesOfAnyColor(bubbleToTestBFS);

        leftOutNodes = new List<Bubble>();
        leftOutNodes = allNodes.Except(allVisitedNodes).ToList();
    }
}
