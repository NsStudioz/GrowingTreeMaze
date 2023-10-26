using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Vector Elements")]
    private Vector3 cameraPos;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 10f;

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        transform.position = cameraPos;

        HandleMovementVertical();
        HandleMovementHorizontal();
    }

    private void HandleMovementHorizontal()
    {
        if (Input.GetKey(KeyCode.D))
            cameraPos += transform.right * movementSpeed * Time.deltaTime;

        else if (Input.GetKey(KeyCode.A))
            cameraPos += transform.right * -movementSpeed * Time.deltaTime;
    }

    private void HandleMovementVertical()
    {
        if (Input.GetKey(KeyCode.W))
            cameraPos += transform.forward * movementSpeed * Time.deltaTime;

        else if (Input.GetKey(KeyCode.S))
            cameraPos += transform.forward * -movementSpeed * Time.deltaTime;
    }

}
