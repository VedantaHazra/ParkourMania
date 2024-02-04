using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
    [Header("Camera Controller")]
   [SerializeField] private Transform target;
   [SerializeField] private float gap = 3f;
   [SerializeField] private float rotSpeed = 3f;

   [Header("Camera Handling")]
   [SerializeField] private Vector2 framingBalance;
   [SerializeField] private float minVerAngle = -14f;
   [SerializeField] private float maxVerAngle = 45f;
    [SerializeField] private bool invertX;
    [SerializeField] private bool invertY;
    float invertXValue;
    float invertYValue;

   float rotx;
   float roty;

   
   private void Start() {
    Cursor.lockState = CursorLockMode.Locked;
   }
   private void Update()
   {
    invertXValue = (invertX) ? -1:1;
    invertYValue = (invertY) ? -1:1;

    rotx += Input.GetAxis("Mouse Y")*invertYValue*rotSpeed;
    rotx = Mathf.Clamp(rotx, minVerAngle, maxVerAngle);

    roty += Input.GetAxis("Mouse X")*invertXValue*rotSpeed;

    var targetRotation = Quaternion.Euler(rotx, roty, 0);

    var focusPos = target.position + new Vector3(framingBalance.x, framingBalance.y);
    transform.position = focusPos - targetRotation*new Vector3(0,0,gap);
    transform.rotation = targetRotation;
   }

   public Quaternion flatRotation => Quaternion.Euler(0,roty,0);
}
