using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool rotationInX;
    [SerializeField] private bool rotationInY;
    [SerializeField] private bool rotationInZ;

    private void Update()
    {
        transform.Rotate(rotationInX ? rotationSpeed * Time.deltaTime : 0f,
            rotationInY ? rotationSpeed * Time.deltaTime : 0f,
            rotationInZ ? rotationSpeed * Time.deltaTime : 0f);
    }
}
