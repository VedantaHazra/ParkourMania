using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentChecker : MonoBehaviour
{
   public Vector3 rayOfFeet;
   public float rayLength = 0.9f;
   public float heightRayLength = 6f;
   public LayerMask obstacleLayer;

   public ObstacleInfo CheckObstacle()
   {
    var hitData = new ObstacleInfo();
    var rayOrigin = transform.position + rayOfFeet;
    hitData.hitFound = Physics.Raycast(rayOrigin, transform.forward, out hitData.hitInfo, rayLength, obstacleLayer);
    Debug.DrawRay(rayOrigin, transform.forward*rayLength, (hitData.hitFound)? Color.red : Color.green);

    if(hitData.hitFound)
    {
        var heightOrigin = hitData.hitInfo.point + Vector3.up * heightRayLength;
        hitData.heightHitFound = Physics.Raycast(heightOrigin, Vector3.down, out hitData.heightInfo, heightRayLength, obstacleLayer);
        Debug.DrawRay(heightOrigin, Vector3.down * heightRayLength, (hitData.heightHitFound)? Color.blue : Color.green);
    }
    return hitData;
   }

}

public struct ObstacleInfo
   {
    public bool hitFound;
    public bool heightHitFound;
    public RaycastHit hitInfo;
    public RaycastHit heightInfo;
   }
