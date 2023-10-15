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
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Vector3 rayCastDirection = (mouseWorldPos - initialLaunchPoint.position).normalized;

            Debug.DrawRay(initialLaunchPoint.position, rayCastDirection * 20f);

            RaycastHit2D hit;
            hit = Physics2D.Raycast(initialLaunchPoint.position, rayCastDirection, Mathf.Infinity,~ignoreLayerMask);
            if (hit.collider != null)
            {
                if (!initialPathRenderer.enabled)
                    initialPathRenderer.enabled = true;

                initialPathRenderer.SetPosition(0, initialLaunchPoint.position);
                initialPathRenderer.SetPosition(1, hit.point);
            }
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

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(initialLaunchPoint.position, 0.1f);

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            Gizmos.DrawSphere(mouseWorldPos, 0.1f);
        }
    }
}