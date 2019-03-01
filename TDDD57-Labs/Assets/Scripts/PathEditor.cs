using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathEditor : MonoBehaviour
{
    public Color pathColor = Color.black;
    public Color dangerWayPointColor = Color.red;
    public List<WayPoint> wayPoints = new List<WayPoint>();

    private WayPoint[] children;

    private void OnDrawGizmos() {
        Gizmos.color = pathColor;
        wayPoints.Clear();
        children = GetComponentsInChildren<WayPoint>();

        foreach (WayPoint wayPoint in children) {
            wayPoints.Add(wayPoint);
            
        }
        
        for (int i = 0; i < wayPoints.Count; i++) {
            Vector3 currentPosition = wayPoints[i].GetTransform().position;
            Gizmos.color = pathColor;
            if (i > 0) {
                Vector3 previousPosition = wayPoints[i - 1].GetTransform().position;
                
                Gizmos.DrawLine(previousPosition, currentPosition);
                if (wayPoints[i].wayPointType == WayPointType.DANGER) {
                    Gizmos.color = dangerWayPointColor;
                }
                Gizmos.DrawWireSphere(currentPosition, .15f);
            } else {
                Gizmos.DrawWireSphere(currentPosition, .15f);
            }
        }
        Gizmos.color = pathColor;
    }

}
