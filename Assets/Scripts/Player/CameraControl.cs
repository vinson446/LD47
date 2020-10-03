using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float mouseSensitivity = 100;
    public Transform playerBody;

    float xRot = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // rotate camera when moving mouse vertically
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);
        transform.localRotation = Quaternion.Euler(xRot, 0, 0);

        // rotate player when moving mouse horizontally
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
