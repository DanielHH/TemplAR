using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathEditor : MonoBehaviour
{
    public Color pathColor = Color.black;
    public Color dangerWayPointColor = Color.red;
    public Color normalWayPointColor = Color.green;
    public List<WayPoint> wayPoints = new List<WayPoint>();

    public float wayPointSphereRadius = .1f;

    private WayPoint[] children;

    private void OnDrawGizmos() {
        Gizmos.color = pathColor;
        wayPoints.Clear();
        children = GetComponentsInChildren<WayPoint>();

        foreach (WayPoint wayPoint in children) {
            wayPoints.Add(wayPoint);
            
        }
        
        for (int i = 0; i < wayPoints.Count; i++) {
            Vector3 currentPosition = wayPoints[i].transform.position;
            Gizmos.color = pathColor;
            if (i > 0) {
                Vector3 previousPosition = wayPoints[i - 1].transform.position;
                
                Gizmos.DrawLine(previousPosition, currentPosition);
                if (wayPoints[i].wayPointType == WayPointType.DANGER) {
                    Gizmos.color = dangerWayPointColor;
                } else {
                    Gizmos.color = normalWayPointColor;
                }
                Gizmos.DrawWireSphere(currentPosition, wayPointSphereRadius);
            } else {
                Gizmos.DrawWireSphere(currentPosition, wayPointSphereRadius);
            }
        }
        Gizmos.color = pathColor;
    }

    private void Start() {
        wayPoints.Clear();
        children = GetComponentsInChildren<WayPoint>();

        foreach (WayPoint wayPoint in children) {
            wayPoints.Add(wayPoint);

        }
    }
}
