using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {
    public PathEditor pathToFollow;

    public int currentWayPointID = 0;
    public float originalSpeed = 2;
    public float reachDistance = .1f;
    public float rotationSpeed = .5f;

    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private float speed;

    void Start() {
        speed = originalSpeed;
        previousPosition = transform.position;
    }


    void Update() {
        WayPoint targetWayPoint = pathToFollow.wayPoints[currentWayPointID];

        Vector3 targetPosition = targetWayPoint.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        float distance = Vector3.Distance(targetPosition, transform.position);
        if (distance <= targetWayPoint.reachDistance) {
            if (targetWayPoint.wayPointType == WayPointType.DANGER) {
                speed = 0;
            } else {
                speed = originalSpeed;
                currentWayPointID++;
            }

        }

        if (currentWayPointID >= pathToFollow.wayPoints.Count) {
            currentWayPointID = 0;
        }
    }

    public float getSpeed() {

        if (pathToFollow.wayPoints.Count <= 0) {
            return 0;
        } else {
            return speed;
        }
    }
}
