using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCameraSwitcher : MonoBehaviour
{
   [SerializeField] private CinemachineVirtualCamera firstPersonCamera;
   [SerializeField] private CinemachineVirtualCamera thirdPersonCamera;

    private void Start() {
        firstPersonCamera.m_Priority = 11;
        thirdPersonCamera.m_Priority = 10;
    }

   private void Update() {
    if (Input.GetKeyDown(KeyCode.C))
    {
        if (firstPersonCamera.m_Priority > thirdPersonCamera.m_Priority) {
            firstPersonCamera.m_Priority = 10;
            thirdPersonCamera.m_Priority = 11;
        }

        else {
            firstPersonCamera.m_Priority = 11;
            thirdPersonCamera.m_Priority = 10;
        }
    }


   }
}
