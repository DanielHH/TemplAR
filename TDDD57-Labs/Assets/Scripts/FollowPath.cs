using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public PathEditor pathToFollow;

    public int currentWayPointID = 0;
    public float speed = 2;
    public float reachDistance = .1f;
    public float rotationSpeed = .5f;

    private Vector3 previousPosition;
    private Vector3 currentPosition;
    

    void Start()
    {   
        previousPosition = transform.position;
    }


    void Update()
    {
        WayPoint targetWayPoint = pathToFollow.wayPoints[currentWayPointID];
        
        Vector3 targetPosition = targetWayPoint.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        float distance = Vector3.Distance(targetPosition, transform.position);
        if (distance <= targetWayPoint.reachDistance) {
            if (Input.GetMouseButton(0)) {
                targetWayPoint.wayPointType = WayPointType.NORMAL;
            }
            if (targetWayPoint.wayPointType == WayPointType.DANGER) {
                speed = 0;
            } else {
                speed = 2;
                currentWayPointID++;
            }
            
        }
        
        if (currentWayPointID >= pathToFollow.wayPoints.Count) {
            currentWayPointID = 0;
        }
    }
}
