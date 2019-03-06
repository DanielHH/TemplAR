using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTrigger : MonoBehaviour
{
    public GameObject snappedIndicatorPrefab;
    public float proximityDistance = 1f;

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
                    if (Vector3.Distance(GetComponent<BoxCollider>().bounds.center, center) <= proximityDistance) {
                        snappedIndicator = Instantiate(snappedIndicatorPrefab, transform.position, transform.rotation);
                        snappedIndicator.transform.parent = transform;
                        pickupableObject.Snap();
                        indicatorPlaced = true;
                    }
                }
                else {
                    if (Vector3.Distance(GetComponent<BoxCollider>().bounds.center, center) > proximityDistance) {
                        Destroy(snappedIndicator);
                        pickupableObject.UnSnap();
                        indicatorPlaced = false;
                    }
                }
            }        
        }
    }

    public void nullifyPO() {
        pickupableObject = null;
    }
}
