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

    private bool touchStarted = false;
    private float startTouchY;
    public float zThresh = 15f;
    public float zSpeed = .5f;
    private bool hasSwiped = false;

    private int newtouches = 0;

    void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void Update() {
        if (carrying) {
            carry(carriedObject);
            //checkDrop();
        } else {
            pickup();
        }
    }

    void rotateObject() {
        carriedObject.transform.Rotate(5, 10, 15);
    }

    void carry(Pickupable o) {
        if (Input.touchCount >= 1) {
            Touch currentTouch = Input.GetTouch(0);
            hasSwiped = false;
            switch (currentTouch.phase) {
                case TouchPhase.Began:
                    newtouches++;
                    touchStarted = !touchStarted;
                    startTouchY = currentTouch.position.y;
                    break;
                case TouchPhase.Moved:
                    if (Mathf.Abs(startTouchY - currentTouch.position.y) > zThresh) {
                        hasSwiped = true;
                        if (startTouchY > currentTouch.position.y) {
                            currentDistance -= zSpeed;
                        } else {
                            currentDistance += zSpeed;
                        }
                    }
                    break;
                case TouchPhase.Stationary:
                    if (Mathf.Abs(startTouchY - currentTouch.position.y) > zThresh) {
                        hasSwiped = true;
                        if (startTouchY > currentTouch.position.y) {
                            currentDistance -= zSpeed;
                        } else {
                            currentDistance += zSpeed;
                        }
                    }
                    break;
                case TouchPhase.Ended:
                    if (!hasSwiped && !touchStarted) {
                        dropObject();
                    }
                    break;
            }

            if (currentDistance < minDistance) {
                currentDistance = minDistance;
            }

            debug.SetText("startTouchY " + startTouchY + " currentTouchY" + currentTouch.position.y + "Touch Started: " + touchStarted);
        }

        
        o.transform.position = mainCamera.transform.position + mainCamera.transform.forward * currentDistance;
        o.transform.rotation = Quaternion.identity;
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
                    startTouchY = Input.GetTouch(0).position.y;
                    touchStarted = true;
                    carriedObject = HighlightedObject;
                    carriedObject.setSelected();
                    carriedObject.ClearRb();
                    carriedObject.ActivateSnapTriggers();

                    carriedObject.UseGravity(false);
                }
            } else {
                HighlightedObject.Highlight(false);
                HighlightedObject = null;
            }
        }
    }

    void checkDrop() {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended && !hasSwiped) {
            dropObject();
        }
    }

    void dropObject() {
        //touchStarted = false;
        carrying = false; 
        carriedObject.DropSelf();
        carriedObject = null;
    }
}