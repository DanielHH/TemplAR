using UnityEngine;

public class SnapTrigger : MonoBehaviour
{
    public GameObject snappedIndicatorPrefab;
    public float proximityDistance = 1f;
    public WayPoint wayPoint;
    public float angleLimit = 30f;

    private TMPro.TextMeshProUGUI debug;

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
        debug = GameObject.FindGameObjectWithTag("Debug").GetComponent<TMPro.TextMeshProUGUI>();
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
                Vector3 pickupableCenter = pickupableObject.GetComponent<Collider>().bounds.center;
                Vector3 indicatorCenter = GetComponent<Collider>().bounds.center;
                float pickupableYRotation = pickupableObject.transform.eulerAngles.y;
                float indicatorYRotation = transform.eulerAngles.y;
                float rotationDiff = Mathf.Abs(Mathf.DeltaAngle(pickupableYRotation, indicatorYRotation));
                float symmRotationDiff;
                if (pickupableObject.isSymmetric) {
                    symmRotationDiff = Mathf.Abs(Mathf.DeltaAngle(pickupableYRotation + 180, indicatorYRotation));
                } else {
                    symmRotationDiff = rotationDiff;
                }

                //debug.SetText("pY " + pickupableYRotation + " iY " + indicatorYRotation + " rD " + rotationDiff + " sRD " + symmRotationDiff);

                if (!indicatorPlaced) {
                    if (Vector3.Distance(indicatorCenter, pickupableCenter) <= proximityDistance && (rotationDiff < angleLimit || symmRotationDiff < angleLimit)) {
                        if (wayPoint.wayPointType == WayPointType.DANGER) {
                            snappedIndicator = Instantiate(snappedIndicatorPrefab, transform.position, transform.rotation);
                            snappedIndicator.transform.parent = transform;
                            pickupableObject.Snap();
                            indicatorPlaced = true;
                        }
                    }
                } else {
                    if (Vector3.Distance(indicatorCenter, pickupableCenter) > proximityDistance || (rotationDiff >= angleLimit && symmRotationDiff >= angleLimit)) {
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
