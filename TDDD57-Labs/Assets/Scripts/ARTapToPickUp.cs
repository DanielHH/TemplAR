using UnityEngine;

/// <summary>
/// This script handles the interaction of picking up, carrying and dropping a pickupable object.
/// It is in the 'AR Session Origin' object.
/// </summary>
public class ARTapToPickUp : MonoBehaviour {
    private GameObject mainCamera;
    private Pickupable carriedObject;
    private Pickupable HighlightedObject;

    public TMPro.TMP_Text debug;

    private  bool carrying = false;
    public float minDistance;
    private float currentDistance;

    private bool quickTouch = true;
    private float startTouchY;
    private float startTouchX;
    private float rotateAroundY;
    public float zThresh = 15f;
    public float zSpeed = .5f;
    public float rotationSpeed = 5f;
    private Quaternion target;

    void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera");
        currentDistance = minDistance;
        rotateAroundY = 0;
    }

    void Update() {
        if (carrying) {
            carry(carriedObject);
        } else {
            pickup();
        }
    }

    void rotateObject() {
        carriedObject.transform.Rotate(5, 10, 15);
    }

    void carry(Pickupable o) {
        bool reposition = true;
        if (Input.touchCount >= 1) {
            Touch currentTouch = Input.GetTouch(0);
            switch (currentTouch.phase) {
                case TouchPhase.Began:
                    startTouchY = currentTouch.position.y;
                    startTouchX = currentTouch.position.x;
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (Mathf.Abs(startTouchY - currentTouch.position.y) > zThresh && rotateAroundY == 0) {
                        quickTouch = false;
                        if (startTouchY > currentTouch.position.y) {
                            currentDistance -= zSpeed;
                        } else {
                            currentDistance += zSpeed;
                        }
                    }
                    else if (Mathf.Abs(startTouchX - currentTouch.position.x) > zThresh) {
                        quickTouch = false;
                        if (startTouchX > currentTouch.position.x) {
                            rotateAroundY = rotationSpeed;
                        } else {
                            rotateAroundY = -rotationSpeed;
                        }

                    }
                    break;
                case TouchPhase.Ended:
                    if (quickTouch) {
                        dropObject();
                        reposition = false;
                    } else {
                        rotateAroundY = 0;
                        quickTouch = true;
                    }
                    break;
            }

            if (currentDistance < minDistance) {
                currentDistance = minDistance;
            }

            debug.SetText("startTouchY " + startTouchY + " currentTouchY" + currentTouch.position.y);
        }

        if (reposition) {
            o.transform.position = mainCamera.transform.position + mainCamera.transform.forward * currentDistance;
            o.transform.Rotate(0, rotateAroundY, 0, Space.Self);
        }
    }

    void pickup() {
        var screenCast = mainCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(screenCast);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            
            if (hit.collider.tag == "Pickupable") {
                Pickupable p = hit.collider.GetComponent<Pickupable>();
     
                if (HighlightedObject != p && HighlightedObject != null) {
                    HighlightedObject.Highlight(false);
                }
                HighlightedObject = p;
                HighlightedObject.Highlight(true);

                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) {
                    carrying = true;
                    quickTouch = false;
                    startTouchY = Input.GetTouch(0).position.y;
                    startTouchX = Input.GetTouch(0).position.x;
                    carriedObject = HighlightedObject;
                    carriedObject.setSelected();
                    carriedObject.ActivateSnapTriggers();
                    carriedObject.UseGravity(false);
                    carriedObject.SetKinematic(true);
                    carriedObject.transform.rotation = Quaternion.identity;
                }
            } else {
                HighlightedObject.Highlight(false);
                HighlightedObject = null;
            }
        }
    }

    void dropObject() {
        carrying = false;
        carriedObject.ClearRb();
        carriedObject.DropSelf();
        carriedObject = null;
        currentDistance = minDistance;
    }
}