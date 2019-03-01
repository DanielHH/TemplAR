using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathEditor : MonoBehaviour
{
    public Color pathColor;
    public List<Transform> pathNodeTransforms = new List<Transform>();

    private Transform[] children;

    private void OnDrawGizmos() {
        Gizmos.color = pathColor;
        pathNodeTransforms.Clear();
        children = GetComponentsInChildren<Transform>();

        foreach (Transform nodeTransfrom in children) {
            // If statement makes sure not to add the parent's transform to the path
            if (nodeTransfrom != this.transform) {
                pathNodeTransforms.Add(nodeTransfrom);
            }
        }
        Debug.Log(pathNodeTransforms.Count);
        for (int i = 0; i < pathNodeTransforms.Count; i++) {
            Vector3 currentPosition = pathNodeTransforms[i].position;
            if (i > 0) {
                Vector3 previousPosition = pathNodeTransforms[i - 1].position;
                Gizmos.DrawLine(previousPosition, currentPosition);
                Gizmos.DrawWireSphere(currentPosition, .15f);
            } else {
                Gizmos.DrawWireSphere(currentPosition, .15f);
            }
        }
    }

}
