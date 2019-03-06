using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTrigger : MonoBehaviour
{
    public GameObject snappedIndicatorPrefab;

    private bool indicatorPlaced = false;
    private GameObject snappedIndicator;
    private Pickupable pickupableObject;
    private bool foundChild = false;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Pickupable") {
            pickupableObject = other.GetComponent<Pickupable>();
            foundChild = false;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Pickupable") {
            pickupableObject = null;
            foundChild = false;
        }
    }

    private void Start()  {
        gameObject.SetActive(false);
    }

    private void Update() {
        if (pickupableObject != null) {

            for (int i = 0; i < pickupableObject.snapTriggers.Count; i++) {
                if (pickupableObject.snapTriggers[i] == this) {
                    pickupableObject.setCurrentSnapTrigger(i);
                    foundChild = true;
                    break;
                }
            }

            if (foundChild) {
                Vector3 center = pickupableObject.GetComponent<Collider>().bounds.center;
                if (!indicatorPlaced) {
                    if (GetComponent<BoxCollider>().bounds.Contains(center)) {
                        snappedIndicator = Instantiate(snappedIndicatorPrefab, transform.position, transform.rotation);
                        snappedIndicator.transform.parent = transform;
                        pickupableObject.Snap();
                        indicatorPlaced = true;
                    }
                }
                else {
                    if (!GetComponent<BoxCollider>().bounds.Contains(center)) {
                        Destroy(snappedIndicator);
                        pickupableObject.UnSnap();
                        indicatorPlaced = false;
                    }
                }
            }        
        }
    }
}
