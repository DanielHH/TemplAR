using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WayPointType { NORMAL, DANGER, FINISH }

public class WayPoint : MonoBehaviour
{
    public WayPointType wayPointType;
    public float reachDistance = 0.1f;
    public Transform GetTransform() {
        return this.transform;
    }
}
