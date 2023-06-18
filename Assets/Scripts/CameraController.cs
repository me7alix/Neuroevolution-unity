using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject cam;
    public float sens, speed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * sens, 0);
        cam.transform.Rotate(-Input.GetAxis("Mouse Y") * sens, 0, 0);
        Vector3 dir = new Vector3();
        if (Input.GetKey(KeyCode.W)) dir.z = 1;
        if (Input.GetKey(KeyCode.D)) dir.x = 1;
        if (Input.GetKey(KeyCode.A)) dir.x = -1;
        if (Input.GetKey(KeyCode.S)) dir.z = -1;
        if (Input.GetKey(KeyCode.Space)) dir.y = 1;
        if (Input.GetKey(KeyCode.LeftShift)) dir.y = -1;
        transform.position += (transform.forward * dir.z + 
        transform.right * dir.x + 
        transform.up * dir.y).normalized * speed * Time.deltaTime;
    }
}
