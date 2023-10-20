using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using SNGames.BubbleShooter;

public class BubbleShooterLevelDesign : EditorWindow
{
    private Bubble currenlySpawnedBubble;
    private string prefabName;

    [MenuItem("BubbleShooter/LevelDesigner")]
    public static void ShowLevelDesignWindow()
    {
        GetWindow<BubbleShooterLevelDesign>("Bubble Shooter Level Designer");
    }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    private void OnGUI()
    {
        //Bubbles
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Green Bubble"))
        {
            if (currenlySpawnedBubble != null)
                DestroyImmediate(currenlySpawnedBubble.gameObject);
            prefabName = "Bubble Green.prefab";
            SetAndSpawnCurrenlySpawnedBubble(prefabName);
        }
        if (GUILayout.Button("Pink Bubble"))
        {
            if (currenlySpawnedBubble != null)
                DestroyImmediate(currenlySpawnedBubble.gameObject);
            prefabName = "Bubble Pink.prefab";
            SetAndSpawnCurrenlySpawnedBubble(prefabName);
        }
        if (GUILayout.Button("Red Bubble"))
        {
            if (currenlySpawnedBubble != null)
                DestroyImmediate(currenlySpawnedBubble.gameObject);
            prefabName = "Bubble Red.prefab";
            SetAndSpawnCurrenlySpawnedBubble(prefabName);
        }
        if (GUILayout.Button("White Bubble"))
        {
            if (currenlySpawnedBubble != null)
                DestroyImmediate(currenlySpawnedBubble.gameObject);
            prefabName = "Bubble White.prefab";
            SetAndSpawnCurrenlySpawnedBubble(prefabName);
        }
        if (GUILayout.Button("Yellow Bubble"))
        {
            if (currenlySpawnedBubble != null)
                DestroyImmediate(currenlySpawnedBubble.gameObject);
            prefabName = "Bubble Yellow.prefab";
            SetAndSpawnCurrenlySpawnedBubble(prefabName);
        }

        GUILayout.EndHorizontal();
    }

    private void SetAndSpawnCurrenlySpawnedBubble(string prefabName)
    {
        Bubble asset = AssetDatabase.LoadAssetAtPath<BubbleColored>("Assets/Bubble Shooter/Prefabs/Bubbles/" + prefabName);
        if (asset != null)
        {
            currenlySpawnedBubble = (BubbleColored)PrefabUtility.InstantiatePrefab(asset);
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (currenlySpawnedBubble != null)
        {
            Event e = Event.current;
            Vector2 mousePointWorld = HandleUtility.GUIPointToWorldRay(e.mousePosition).origin;
            RaycastHit2D hit = Physics2D.Raycast(mousePointWorld, Vector2.zero);

            if (hit.collider != null)
            {
                currenlySpawnedBubble.transform.position = hit.transform.position;
            }

            if (e.type == EventType.MouseDown && e.button == 1)
            {
                currenlySpawnedBubble = null;
                SetAndSpawnCurrenlySpawnedBubble(prefabName);

                e.Use();
            }
        }
    }
}