using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShooterController : MonoBehaviour
{
    [SerializeField] private InGameBubblesData inGameBubbleData;
    [SerializeField] private LineRenderer initialPathRenderer;
    [SerializeField] private Transform initialLaunchPoint;

    private Bubble currentlyPlacedBubble = null;
    private Bubble nextBubble;

    void Update()
    {
        DrawInitialLineRenderer();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlaceCurrentShootBubble();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ApplyForceOnCurrenlyPlacedBubble();
        }
    }

    public void PlaceCurrentShootBubble()
    {
        //Choose a random color
        Bubble randomColorBubblePrefab = inGameBubbleData.GetRandomBubbleColorPrefab();

        currentlyPlacedBubble = Instantiate(randomColorBubblePrefab, initialLaunchPoint.position, Quaternion.identity);
        currentlyPlacedBubble.isLaunchBubble = true;
        currentlyPlacedBubble.gameObject.AddComponent<Rigidbody2D>();
        Rigidbody2D bubbleRb = currentlyPlacedBubble.GetComponent<Rigidbody2D>();
        bubbleRb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        bubbleRb.gravityScale = 0f;
    }

    public void PlaceNextShootBubble()
    {

    }

    public void MoveNextShootBubbleToCurrentShootBubble()
    {

    }

    private Vector3 GetCurrentShootDirections()
    {
        Vector3 shootDirection = Vector3.zero;

        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // zero z

        shootDirection = mouseWorldPos - initialLaunchPoint.position;

        return shootDirection.normalized;
    }

    private void DrawInitialLineRenderer()
    {
        initialPathRenderer.SetPosition(0, initialLaunchPoint.position);

        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // zero z

        initialPathRenderer.SetPosition(1, mouseWorldPos);
    }

    private void ApplyForceOnCurrenlyPlacedBubble()
    {
        if (currentlyPlacedBubble != null)
        {
            Rigidbody2D bubbleRb = currentlyPlacedBubble.GetComponent<Rigidbody2D>();
            bubbleRb.AddForce(GetCurrentShootDirections() * 14f, ForceMode2D.Impulse);

            currentlyPlacedBubble = null;
        }
    }
}
