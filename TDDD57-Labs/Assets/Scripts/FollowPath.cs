using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public PathEditor pathToFollow;

    public int currentWayPointID = 0;
    public float speed;
    public float reachDistance = 1.0f;
    public float rotationSpeed = .5f;
    public string pathName;

    private Vector3 previousPosition;
    private Vector3 currentPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        //pathToFollow = GameObject.Find(pathName).GetComponent<PathEditor>();
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nextPoint = pathToFollow.pathNodeTransforms[currentWayPointID].position;
        float distance = Vector3.Distance(nextPoint, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, nextPoint, Time.deltaTime * speed);

        Quaternion rotation = Quaternion.LookRotation(nextPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

        if (distance <= reachDistance) {
            currentWayPointID++;
        }
        
        if (currentWayPointID >= pathToFollow.pathNodeTransforms.Count) {
            currentWayPointID = 0;
        }
    }
}
