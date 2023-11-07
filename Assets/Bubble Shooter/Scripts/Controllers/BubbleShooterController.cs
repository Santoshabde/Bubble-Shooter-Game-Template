using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SNGames.CommonModule;
using TMPro;
using System;

namespace SNGames.BubbleShooter
{
    public class BubbleShooterController : MonoBehaviour
    {
        [SerializeField] private LevelGenerator levelGenerator;
        [SerializeField] private TextMeshPro bubbleShotsLeftCountText;
        [SerializeField] private int bubbleShotsLeftCount;
        [SerializeField] private float minYTouchMousePoint;
        [SerializeField] private InGameBubblesData inGameBubbleData;
        [SerializeField] private LineRenderer initialPathRenderer;
        [SerializeField] private float distanceToMaintain;
        [SerializeField] private Transform distanceCalculationTransform;
        [SerializeField] private Transform nonPowerupsPlatform;
        [SerializeField] private Transform currentBubbleLaunchPoint;
        [SerializeField] private Transform nextBubblePoint;
        [SerializeField] private Transform powerupsPlatform;
        [SerializeField] private Transform currentPowerUpLaunchPoint;
        [SerializeField] private LayerMask ignoreLayerMask;

        private Bubble currentlyPlacedBubble = null;
        private Bubble catchedCurrentlyPlacedBubble = null;
        private Bubble nextBubble;
        private bool powerUpActivated = false;
        private bool currentPlaceBubbleInMovingProcess = false;
        private RaycastHit2D[] hit = new RaycastHit2D[1];

        //Variable related to, wall reflections for bubble, when you aim for the wall
        private List<Vector3> allWallHitPoints = new List<Vector3>();
        private Bubble finalHitBubble = null;
        private RaycastHit2D finalBubbleHit;

        public List<Bubble> allBubblesShot = new List<Bubble>();

        private void Start()
        {
            //Move your next Bubble to current bubble on triggering this event
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.MoveNextBubbleToCurrentBubble, MoveNextShootBubbleToCurrentShootBubble);
            //You move the level(along with camera) up and down based on the last bubble distance to your shoot point. - NOTE: Never move bubbles, bubbles positions should always be fixed
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.MoveNextBubbleToCurrentBubble, () => AdjustShootPositionBasedOnLastBubbleIntheGrid(distanceCalculationTransform));

            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.OnLevelSuccess, ClearCurrentLevel);
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.OnLevelFail, ClearCurrentLevel);
            SNEventsController<InGameEvents>.RegisterEvent(InGameEvents.OnNewLevelStart, StartNewLevel);

            //Spawn respective power on clicking respective powerup
            PowerupController.OnPowerButtonClicked += OnPowerButtonClicked;

            //bubbleShotsLeftCountText.text = bubbleShotsLeftCount.ToString();

            //AdjustShootPositionBasedOnLastBubbleIntheGrid(distanceCalculationTransform, true);
        }

        private void OnDestroy()
        {
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.MoveNextBubbleToCurrentBubble, MoveNextShootBubbleToCurrentShootBubble);
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.MoveNextBubbleToCurrentBubble, () => AdjustShootPositionBasedOnLastBubbleIntheGrid(distanceCalculationTransform));
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.OnLevelSuccess, ClearCurrentLevel);
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.OnLevelFail, ClearCurrentLevel);
            SNEventsController<InGameEvents>.DeregisterEvent(InGameEvents.OnNewLevelStart, StartNewLevel);
        }

        void Update()
        {
            if (GameManager.Instance.currentGameStateIsInProgress)
            {
                initialPathRenderer.enabled = false;
                if (ShouldCastARayAndCheckForInput())
                {
                    initialPathRenderer.enabled = ShouldEnableLineRenderer();

                    //2 cases again here - ray cast directly to bubble (or) ray cast to wall
                    RayCastToBubblesOnBoardAndCheckForLaunchInput();  
                }
            }
        }

        private bool ShouldEnableLineRenderer() => !currentPlaceBubbleInMovingProcess && AdjustShootPositionBasedOnLastBubbleIntheGrid_Coroutine == null;

        //Placing the 'currentlyPlacedBubble' to shoot in its position
        public void PlaceCurrentShootBubble()
        {
            //Choose a random color
            Bubble randomColorBubblePrefab = inGameBubbleData.GetRandomBubbleColorPrefab();

            //Spawn current bubble shoot!! 
            currentlyPlacedBubble = Instantiate(randomColorBubblePrefab, currentBubbleLaunchPoint.position, Quaternion.identity);
            currentlyPlacedBubble.transform.parent = currentBubbleLaunchPoint.transform;
            currentlyPlacedBubble.isLaunchBubble = true;
            currentlyPlacedBubble.gameObject.layer = LayerMask.NameToLayer("LaunchBubble");

            allBubblesShot.Add(currentlyPlacedBubble);
        }

        //Moving the next shoot bubble to 'currentlyPlacedBubble'
        public void MoveNextShootBubbleToCurrentShootBubble()
        {
            if (powerUpActivated)
            {
                currentPlaceBubbleInMovingProcess = false;
                powerUpActivated = false;
                RayCastToBubblesOnBoardAndCheckForLaunchInput();
                currentlyPlacedBubble = catchedCurrentlyPlacedBubble;
                nonPowerupsPlatform.gameObject.SetActive(true);
                powerupsPlatform.gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(PlaceNextBubbleToCurrent());

                IEnumerator PlaceNextBubbleToCurrent()
                {
                    nextBubble.transform.DOMove(currentBubbleLaunchPoint.position, 0.2f);

                    yield return new WaitForSeconds(0.2f);
                    currentlyPlacedBubble = nextBubble;
                    PlaceNextShootBubble();

                    currentPlaceBubbleInMovingProcess = false;
                }
            }
        }

        //Spawning the next shoot bubble
        public void PlaceNextShootBubble()
        {
            Bubble randomColorBubblePrefab = inGameBubbleData.GetRandomBubbleColorPrefab();

            nextBubble = Instantiate(randomColorBubblePrefab, nextBubblePoint.position, Quaternion.identity);
            nextBubble.transform.parent = nextBubblePoint.transform;
            nextBubble.isLaunchBubble = true;
            nextBubble.gameObject.layer = LayerMask.NameToLayer("LaunchBubble");

            allBubblesShot.Add(nextBubble);
        }

        private void RayCastToBubblesOnBoardAndCheckForLaunchInput()
        {
            //If nothing is placed on the launch point - currenltyPlacedBubble == null - Return
            if (currentlyPlacedBubble == null) return;

            Vector3 rayCastDirection = GetRayCastDirectionToShoot();

            //Only cast if finger/mouse point is at correct Y distance - else catche the old values only.
            if (IsMousePointFingerInCorrectYPositionToCastARay())
                Physics2D.RaycastNonAlloc(currentBubbleLaunchPoint.position, rayCastDirection, hit, 100f, ~ignoreLayerMask);

            if (hit[0].collider != null)
            {
                //On Raycast hitting bubble
                #region On First Ray Hitting the Bubble
                Bubble firstHitBubble = hit[0].collider.GetComponent<Bubble>();
                if (firstHitBubble != null)
                {
                    initialPathRenderer.positionCount = 2;
                    initialPathRenderer.SetPosition(0, currentBubbleLaunchPoint.position + new Vector3(0, 0, 2));
                    initialPathRenderer.SetPosition(1, (Vector3)hit[0].point + new Vector3(0, 0, 2));

                    if (GetInputUp())
                    {
                        Vector3 finalPositionIfCurrentBubbleShot = BubbleShooter_HelperFunctions.GetNearestNeighbourBubblePoint(firstHitBubble, hit[0].point);
                        OnShootingTheCurrentBubble(finalPositionIfCurrentBubbleShot, firstHitBubble);
                    }
                }
                #endregion

                //First ray hit wall
                #region On First Ray Hitting the Wall
                if (hit[0].collider.transform.tag == "Wall")
                {
                    if (IsMousePointFingerInCorrectYPositionToCastARay())
                    {
                        finalHitBubble = null;
                        allWallHitPoints = new List<Vector3>();
                        allWallHitPoints.Add(currentBubbleLaunchPoint.position);
                        allWallHitPoints.Add(hit[0].point);

                        Vector3 reflectedRayDirection0 = Vector3.Reflect(rayCastDirection, hit[0].normal);
                        CastWallRay(hit[0].point, reflectedRayDirection0);
                    }

                    initialPathRenderer.positionCount = allWallHitPoints.Count + 1;
                    for (int i = 0; i < allWallHitPoints.Count; i++)
                    {
                        initialPathRenderer.SetPosition(i, allWallHitPoints[i] + new Vector3(0, 0, 2));
                    }
                    initialPathRenderer.SetPosition(allWallHitPoints.Count, (Vector3)finalBubbleHit.point + new Vector3(0, 0, 2));

                    if (GetInputUp())
                    {
                        Sequence moveSeq = DOTween.Sequence();
                        foreach (var item in allWallHitPoints)
                        {
                            moveSeq.Append(currentlyPlacedBubble.transform.DOMove(item, 0.07f).SetEase(Ease.InOutCubic));
                        }

                        Vector3 finalPositionIfCurrentBubbleShot = BubbleShooter_HelperFunctions.GetNearestNeighbourBubblePoint(finalHitBubble, finalBubbleHit.point);

                        moveSeq.OnComplete(() =>
                        {
                            OnShootingTheCurrentBubble(finalPositionIfCurrentBubbleShot, finalHitBubble);
                        });
                    }
                }
                #endregion
            }
        }

        bool IsMousePointFingerInCorrectYPositionToCastARay()
        {
            return GetMouseTouchWorldPoint().y - currentBubbleLaunchPoint.position.y > 0.1f;
        }

        //A recursive function, which recurively casts rays to the 2 walls, until it strickes a bubble
        private void CastWallRay(Vector3 origin, Vector3 direction)
        {
            if (finalHitBubble != null)
            {
                return; // Stop when a Bubble is hit.
            }

            RaycastHit2D[] hits = Physics2D.RaycastAll(origin + ((direction * 0.2f)), direction, 100f, ~ignoreLayerMask);
            if (hits != null && hits.Length > 0 && hits[0].collider != null)
            {
                if (hits[0].collider.GetComponent<Bubble>() != null)
                {
                    finalHitBubble = hits[0].collider.GetComponent<Bubble>();
                    finalBubbleHit = hits[0];
                    return;
                }
                else if (hits[0].collider.transform.tag == "Wall")
                {
                    allWallHitPoints.Add(hits[0].point);
                    Vector3 reflectionDirection = Vector3.Reflect(direction, hits[0].normal);
                    CastWallRay(hits[0].point, reflectionDirection);
                }
                else
                {
                    return;
                }
            }
        }

        private void OnShootingTheCurrentBubble(Vector3 currentlyPlacedBallPosition, Bubble bubbleWeAreShootingTo = null)
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
                currentlyPlacedBubble.MoveLaunchBubbleToFinalPositionOnBoard(currentlyPlacedBallPosition, bubbleWeAreShootingTo);
                currentlyPlacedBubble = null;
            }
        }

        private void OnPowerButtonClicked(BubbleType powerType)
        {
            if (!powerUpActivated && !currentPlaceBubbleInMovingProcess)
            {
                powerUpActivated = true;
                nonPowerupsPlatform.gameObject.SetActive(false);
                powerupsPlatform.gameObject.SetActive(true);

                catchedCurrentlyPlacedBubble = currentlyPlacedBubble;
                currentlyPlacedBubble = Instantiate(inGameBubbleData.GetBubbleOfAColor(powerType), currentPowerUpLaunchPoint.position, Quaternion.identity);
                currentlyPlacedBubble.transform.parent = currentPowerUpLaunchPoint;
                currentlyPlacedBubble.isLaunchBubble = true;
                currentlyPlacedBubble.gameObject.layer = LayerMask.NameToLayer("LaunchBubble");
            }
        }

        private Coroutine AdjustShootPositionBasedOnLastBubbleIntheGrid_Coroutine;
        private void AdjustShootPositionBasedOnLastBubbleIntheGrid(Transform refPoint, bool debug = false)
        {
            if (AdjustShootPositionBasedOnLastBubbleIntheGrid_Coroutine == null)
                AdjustShootPositionBasedOnLastBubbleIntheGrid_Coroutine = StartCoroutine(AdjustShootPositionBasedOnLastBubbleIntheGrid_IEnum());

            IEnumerator AdjustShootPositionBasedOnLastBubbleIntheGrid_IEnum()
            {
                yield return new WaitForSeconds(0.2f);

                Bubble nearestRowBubbleInTheGrid = levelGenerator.GetNearestRowBubbleInTheGrid(refPoint);
                if (nearestRowBubbleInTheGrid != null)
                {
                    float distanceBetween = Mathf.Abs(nearestRowBubbleInTheGrid.PositionID.y - distanceCalculationTransform.position.y);

                    if (debug)
                        Debug.Log("Initial Distance:" + distanceBetween);

                    float distanceToMove = distanceBetween - distanceToMaintain;
                    transform.DOMove(transform.position + new Vector3(0, distanceToMove, 0), 0.45f);
                }

                yield return new WaitForSeconds(0.45f);

                RayCastToBubblesOnBoardAndCheckForLaunchInput();
                AdjustShootPositionBasedOnLastBubbleIntheGrid_Coroutine = null;
            }
        }

        private void ClearCurrentLevel()
        {
            foreach (var item in allBubblesShot)
            {
                if(item != null)
                {
                    Destroy(item.gameObject);
                }
            }

            allBubblesShot.Clear();
            allBubblesShot = null;
            allBubblesShot = new List<Bubble>();

            transform.position = Vector3.zero;
            powerupsPlatform.gameObject.SetActive(false);
            nonPowerupsPlatform.gameObject.SetActive(false);
            initialPathRenderer.transform.gameObject.SetActive(false);
        }

        private void StartNewLevel()
        {
            //Place current and next bubble at the start of the level
            PlaceCurrentShootBubble();
            PlaceNextShootBubble();

            powerupsPlatform.gameObject.SetActive(true);
            nonPowerupsPlatform.gameObject.SetActive(true);
            initialPathRenderer.transform.gameObject.SetActive(true);
        }

        #region Input Type Based Helper Functions

        private bool ShouldCastARayAndCheckForInput()
        {
#if UNITY_EDITOR
            if ((Input.GetMouseButton(0)
                || Input.GetMouseButtonUp(0)) && IsMousePointFingerInCorrectYPositionToCastARay())
                return true;
#else
            if ((Input.touchCount > 0)
 && (Input.GetTouch(0).phase == TouchPhase.Began
 || Input.GetTouch(0).phase == TouchPhase.Moved
 || Input.GetTouch(0).phase == TouchPhase.Ended) && IsMousePointFingerInCorrectYPositionToCastARay())
                return true;
#endif

            return false;
        }

        private Vector3 GetRayCastDirectionToShoot()
        {
#if UNITY_EDITOR
            Vector3 mouseWorldPos = GetMouseTouchWorldPoint();

            Vector3 rayCastDirection = (mouseWorldPos - currentBubbleLaunchPoint.position).normalized;

            return rayCastDirection;
#else
            // Get the touch position
            Vector3 rayCastDirection = new Vector3(0, 1, 0);
            if (Input.touchCount > 0)
            {
                Vector3 touchWorldPos = GetMouseTouchWorldPoint();

                // Calculate the raycast direction
                rayCastDirection = (touchWorldPos - currentBubbleLaunchPoint.position).normalized;
            }

            return rayCastDirection;
#endif
        }

        private bool GetInputUp()
        {
#if UNITY_EDITOR
            bool mouseInput = Input.GetMouseButtonUp(0);
            if (mouseInput)
            {
                currentPlaceBubbleInMovingProcess = true;

                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlayAudioClipWithAutoDestroy("shoot");
            }

            return mouseInput;
#else

            bool touchInput = (Input.touchCount > 0) && Input.GetTouch(0).phase == TouchPhase.Ended;
            if (touchInput)
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlayAudioClipWithAutoDestroy("shoot");
            }
            return touchInput;
#endif
        }

        private Vector3 GetMouseTouchWorldPoint()
        {
#if UNITY_EDITOR
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            return mouseWorldPos;
#else
            if (Input.touchCount > 0)
            {
                Vector3 touchScreenPos = Input.touches[0].position;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(touchScreenPos);
                worldPos.z = 0;

                return worldPos;
            }

            return new Vector3(0, 2, 0);
#endif
        }

        #endregion
    }
}