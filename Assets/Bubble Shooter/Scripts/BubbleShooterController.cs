using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.BubbleShooter
{
    public class BubbleShooterController : MonoBehaviour
    {
        [SerializeField] private InGameBubblesData inGameBubbleData;
        [SerializeField] private LineRenderer initialPathRenderer;
        [SerializeField] private Transform initialLaunchPoint;
        [SerializeField] private LayerMask ignoreLayerMask;

        private Bubble currentlyPlacedBubble = null;
        private Vector3 currentlyPlacedBallPosition;
        private Bubble nextBubble;

        void Update()
        {
            DrawInitialLineRenderer();

            if (Input.GetKeyDown(KeyCode.Space))
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
            currentlyPlacedBubble.gameObject.layer = LayerMask.NameToLayer("LaunchBubble");
        }

        public void PlaceNextShootBubble()
        {

        }

        public void MoveNextShootBubbleToCurrentShootBubble()
        {

        }

        private void DrawInitialLineRenderer()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 rayCastDirection = (mouseWorldPos - initialLaunchPoint.position).normalized;

            Debug.DrawRay(initialLaunchPoint.position, rayCastDirection * 20f);

            RaycastHit2D hit;
            hit = Physics2D.Raycast(initialLaunchPoint.position, rayCastDirection, Mathf.Infinity,~ignoreLayerMask);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Bubble>() != null)
                    CalculateFinalBallSettlePoint(hit.collider.GetComponent<Bubble>(), hit.point);

                initialPathRenderer.SetPosition(0, initialLaunchPoint.position);
                initialPathRenderer.SetPosition(1, hit.point);
            }
        }

        private void CalculateFinalBallSettlePoint(Bubble hitBubble, Vector2 hitPoint)
        {
            currentlyPlacedBallPosition = BubbleShooter_HelperFunctions.GetNearestNeighbourBubblePoint(hitBubble, hitPoint);
        }

        private void ApplyForceOnCurrenlyPlacedBubble()
        {
            if (currentlyPlacedBubble != null)
            {
                currentlyPlacedBubble.MoveLaunchBubbleToFinalPositionOnBoard(currentlyPlacedBallPosition);
            }
        }
    }
}