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
    private float firstTouchY;
    public float zThresh = 15f;
    public float zSpeed = .5f;

    void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void Update() {
        if (carrying) {
            carry(carriedObject);
            checkDrop();
        } else {
            pickup();
        }
    }

    void rotateObject() {
        carriedObject.transform.Rotate(5, 10, 15);
    }

    void carry(Pickupable o) {
        if (Input.touchCount >= 1) {
            Touch CurrentTouch = Input.GetTouch(0);
            if (!touchStarted) {
                firstTouchY = CurrentTouch.position.y;
                touchStarted = true;
            }

            float currentTouchPositionY = CurrentTouch.position.y;
            float yDistance = Mathf.Abs(firstTouchY - currentTouchPositionY);
            if (yDistance > zThresh) {
                if (firstTouchY > currentTouchPositionY) {
                    currentDistance -= zSpeed;
                } else {
                    currentDistance += zSpeed;
                }
            }
        }

        if (currentDistance < minDistance) {
            currentDistance = minDistance;
        }

        debug.SetText("distance: " + minDistance);
        o.transform.position = mainCamera.transform.position + mainCamera.transform.forward * currentDistance;
        o.transform.rotation = Quaternion.identity;
    }

    void pickup() {
        var screenCast = mainCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(screenCast);
        RaycastHit hit;
        Debug.DrawRay(screenCast, ray.direction);
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
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) {
            dropObject();
        }
    }

    void dropObject() {
        touchStarted = false;
        carrying = false; 
        carriedObject.DropSelf();
        carriedObject = null;
    }
}