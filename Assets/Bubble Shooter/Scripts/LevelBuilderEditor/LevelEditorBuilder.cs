using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using SNGames.BubbleShooter;

[System.Serializable]
public class BubbleLevelJson
{
    public List<BubblePositionID> bubbles;
}

[System.Serializable]
public class BubblePositionID
{
    public BubbleType bubbleType;
    public Vector3 bubbleSpawnedPosition;
}


public class LevelEditorBuilder : MonoBehaviour
{

    public static float bubbleGap = 0.5f;

    [SerializeField, ResizableTextArea] private string generatedJson;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private float startX;
    [SerializeField] private float startY;
    [SerializeField] private BubbleSlot bubbleSlot;
    [SerializeField] private InGameBubblesData inGameBubblesData;

    public List<BubbleSlot> listOfBubbleSlots = new List<BubbleSlot>();

    public Bubble currenlyActiveBubble;

    [Button]
    public void GenerateRandomLevel()
    {
        listOfBubbleSlots = new List<BubbleSlot>();
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                float xOffset = 0;
                if (i % 2 == 1)
                    xOffset = bubbleGap / 2;

                Vector3 positionBubbleShouldSpawn = new Vector3(startX + (j * bubbleGap) + xOffset, startY + (i * bubbleGap), 0);
                BubbleSlot tempBubbleSlot = Instantiate(bubbleSlot, positionBubbleShouldSpawn, Quaternion.identity);
                tempBubbleSlot.transform.SetParent(transform);
                listOfBubbleSlots.Add(tempBubbleSlot);
            }
        }
    }

    [Button]
    public void GenerateJSON()
    {
        List<BubblePositionID> bubbles = new List<BubblePositionID>();
        foreach (var item in listOfBubbleSlots)
        {
            if (item.isOccupiedWithBubble)
            {
                bubbles.Add(new BubblePositionID() { bubbleSpawnedPosition = item.occupiedBubble.transform.position, bubbleType = item.occupiedBubble.BubbleColor });
            }
        }

        BubbleLevelJson bubbleLevelJson = new BubbleLevelJson() { bubbles = bubbles };
        generatedJson = JsonUtility.ToJson(bubbleLevelJson);
    }

    private void Update()
    {
        if (currenlyActiveBubble != null)
        {
            Vector2 mousePointWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePointWorld, Vector2.zero);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<BubbleSlot>() != null)
                {
                    currenlyActiveBubble.transform.position = hit.collider.GetComponent<BubbleSlot>().transform.position;
                }

                if (Input.GetMouseButtonDown(0))
                {
                    if (!hit.collider.GetComponent<BubbleSlot>().isOccupiedWithBubble)
                    {
                        hit.collider.GetComponent<BubbleSlot>().isOccupiedWithBubble = true;
                        Bubble spawned = Instantiate(currenlyActiveBubble, hit.collider.GetComponent<BubbleSlot>().transform.position, Quaternion.identity);
                        hit.collider.GetComponent<BubbleSlot>().occupiedBubble = spawned;
                    }
                }

                if(Input.GetMouseButtonDown(1))
                {
                    if (hit.collider.GetComponent<BubbleSlot>().isOccupiedWithBubble)
                    {
                        hit.collider.GetComponent<BubbleSlot>().isOccupiedWithBubble = false;
                        Destroy(hit.collider.GetComponent<BubbleSlot>().occupiedBubble.gameObject);
                        hit.collider.GetComponent<BubbleSlot>().occupiedBubble = null;
                    }
                }
            }
        }
    }

    public void OnButtonClick(int bubblrTypess)
    {
        BubbleType bubbleTypeClicked = (BubbleType)bubblrTypess;
        currenlyActiveBubble = Instantiate(inGameBubblesData.GetBubbleOfAColor(bubbleTypeClicked));
        currenlyActiveBubble.GetComponent<Collider2D>().enabled = false;
    }
}
