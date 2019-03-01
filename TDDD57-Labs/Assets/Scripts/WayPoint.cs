using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WayPointType { NORMAL, DANGER, FINISH }

public class WayPoint : MonoBehaviour
{
    public WayPointType wayPointType;

    public Transform GetTransform() {
        return this.transform;
    }
}
