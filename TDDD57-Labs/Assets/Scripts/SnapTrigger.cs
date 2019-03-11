using UnityEngine;

public class SnapTrigger : MonoBehaviour
{
    public GameObject snappedIndicatorPrefab;
    public float proximityDistance = 1f;
    public WayPoint wayPoint;

    private GameObject snappedIndicator;
    private Pickupable pickupableObject;
    private bool indicatorPlaced = false;
    private bool foundChild = false;

    private void OnTriggerEnter(Collider other) {
        if (wayPoint.wayPointType == WayPointType.DANGER && other.tag == "Pickupable") {
            pickupableObject = other.GetComponent<Pickupable>();
            foundChild = false;
        }
    }

    private void Start()  {
        gameObject.SetActive(false);
    }

    private void Update() {
        if (pickupableObject != null && pickupableObject.isSelected()) {

            for (int i = 0; i < pickupableObject.snapTriggers.Count; i++) {
                if (pickupableObject.snapTriggers[i] == this) {
                    pickupableObject.setCurrentSnapTrigger(this);
                    foundChild = true;
                    break;
                }
            }

            if (foundChild) {
                Vector3 center = pickupableObject.GetComponent<Collider>().bounds.center;
                if (!indicatorPlaced) {
                    if (Vector3.Distance(GetComponent<Collider>().bounds.center, center) <= proximityDistance) {
                        if (wayPoint.wayPointType == WayPointType.DANGER) {
                            snappedIndicator = Instantiate(snappedIndicatorPrefab, transform.position, transform.rotation);
                            snappedIndicator.transform.parent = transform;
                            pickupableObject.Snap();
                            indicatorPlaced = true;
                        }
                    }
                } else {
                    if (Vector3.Distance(GetComponent<Collider>().bounds.center, center) > proximityDistance) {
                        DestroyIndicator();
                        pickupableObject.UnSnap();
                    }
                }
            }
        } else {
            NullifyPO();
        }
    }

    public void NullifyPO() {
        pickupableObject = null;
    }

    public void SetWayPointType(WayPointType type) {
        wayPoint.setWaypointType(type);
    }

    public void DestroyIndicator() {
        Destroy(snappedIndicator);
        indicatorPlaced = false;
    }
}
