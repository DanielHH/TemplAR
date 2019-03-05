using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTrigger : MonoBehaviour
{
    public GameObject snappedIndicatorPrefab;

    private bool indicatorPlaced = false;
    private GameObject snappedIndicator;
    private Collider collisionObjectCollider;



    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Cube" && !indicatorPlaced) {
            collisionObjectCollider = other;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Cube" && !indicatorPlaced) {
            collisionObjectCollider = null;
        }
    }

    void Update() {
        if (collisionObjectCollider != null) {
            Vector3 center = collisionObjectCollider.bounds.center;
            if (!indicatorPlaced) {
                if (GetComponent<BoxCollider>().bounds.Contains(center)) {
                    snappedIndicator = Instantiate(snappedIndicatorPrefab, transform.position, transform.rotation);
                    indicatorPlaced = true;
                }
            } else {
                if (!GetComponent<BoxCollider>().bounds.Contains(center)) {
                    Destroy(snappedIndicator);
                    indicatorPlaced = false;
                }
            }          
        }
    }
}
