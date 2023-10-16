using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SNGames.CommonModule;
using TMPro;

namespace SNGames.BubbleShooter
{
    public class BubbleShooterController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro bubbleShotsLeftCountText;
        [SerializeField] private int bubbleShotsLeftCount;
        [SerializeField] private InGameBubblesData inGameBubbleData;
        [SerializeField] private LineRenderer initialPathRenderer;
        [SerializeField] private Transform currentBubbleLaunchPoint;
        [SerializeField] private Transform nextBubblePoint;
        [SerializeField] private LayerMask ignoreLayerMask;

        private Bubble currentlyPlacedBubble = null;
        private Bubble nextBubble;

        private void Start()
        {
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.MoveNextBubbleToCurrentBubble, MoveNextShootBubbleToCurrentShootBubble);
            bubbleShotsLeftCountText.text = bubbleShotsLeftCount.ToString();

            PlaceCurrentShootBubble();
            PlaceNextShootBubble();
        }

        void Update()
        {
            if (Input.GetMouseButton(0)
                || Input.GetMouseButtonUp(0))
                RayCastToBubblesOnBoardAndCheckForLaunchInput();
        }

        public void PlaceCurrentShootBubble()
        {
            //Choose a random color
            Bubble randomColorBubblePrefab = inGameBubbleData.GetRandomBubbleColorPrefab();
            //Bubble randomColorBubblePrefab = inGameBubbleData.GetBubbleOfAColor(BubbleType.PowerUp_Bomb);

            //Spawn current bubble shoot!! 
            currentlyPlacedBubble = Instantiate(randomColorBubblePrefab, currentBubbleLaunchPoint.position, Quaternion.identity);
            currentlyPlacedBubble.transform.parent = currentBubbleLaunchPoint.transform;
            currentlyPlacedBubble.isLaunchBubble = true;
            currentlyPlacedBubble.gameObject.layer = LayerMask.NameToLayer("LaunchBubble");
        }

        public void PlaceNextShootBubble()
        {
            Bubble randomColorBubblePrefab = inGameBubbleData.GetRandomBubbleColorPrefab();

            nextBubble = Instantiate(randomColorBubblePrefab, nextBubblePoint.position, Quaternion.identity);
            nextBubble.transform.parent = nextBubblePoint.transform;
            nextBubble.isLaunchBubble = true;
            nextBubble.gameObject.layer = LayerMask.NameToLayer("LaunchBubble");
        }

        public void MoveNextShootBubbleToCurrentShootBubble()
        {
            StartCoroutine(PlaceNextBubbleToCurrent());

            IEnumerator PlaceNextBubbleToCurrent()
            {
                yield return new WaitForSeconds(0.1f);
                nextBubble.transform.DOMove(currentBubbleLaunchPoint.position, 0.2f);

                yield return new WaitForSeconds(0.2f);
                currentlyPlacedBubble = nextBubble;
                PlaceNextShootBubble();
                initialPathRenderer.enabled = true;
                RayCastToBubblesOnBoardAndCheckForLaunchInput();
            }
        }

        private void RayCastToBubblesOnBoardAndCheckForLaunchInput()
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 rayCastDirection = (mouseWorldPos - currentBubbleLaunchPoint.position).normalized;

            RaycastHit2D hit;
            hit = Physics2D.Raycast(currentBubbleLaunchPoint.position, rayCastDirection, Mathf.Infinity,~ignoreLayerMask);
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<Bubble>() != null)
                {
                    Vector3 finalPositionIfCurrentBubbleShot = BubbleShooter_HelperFunctions.GetNearestNeighbourBubblePoint(hit.collider.GetComponent<Bubble>(), hit.point);

                    if (Input.GetMouseButtonUp(0))
                    {
                        OnShootingTheCurrentBubble(finalPositionIfCurrentBubbleShot);
                    }
                }

                initialPathRenderer.SetPosition(0, currentBubbleLaunchPoint.position + new Vector3(0, 0, 2));
                initialPathRenderer.SetPosition(1, (Vector3)hit.point + new Vector3(0, 0, 2));
            }
        }

        private void OnShootingTheCurrentBubble(Vector3 currentlyPlacedBallPosition)
        {
            if (currentlyPlacedBubble != null)
            {
                //Unparenting
                currentlyPlacedBubble.transform.parent = null;

                //Updating bubble left
                bubbleShotsLeftCount -= 1;
                bubbleShotsLeftCountText.text = bubbleShotsLeftCount.ToString();

                //Disabling the line renderer
                initialPathRenderer.enabled = false;

                //Moving the current bubble to final positions
                currentlyPlacedBubble.MoveLaunchBubbleToFinalPositionOnBoard(currentlyPlacedBallPosition);
                currentlyPlacedBubble = null;
            }
        }
    }
}