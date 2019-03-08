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
    public float distance;
    public float smooth;

    private Touch firstTouch;
    public float zThresh = 15f;

    void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void Update() {
        if (carrying) {
            carry(carriedObject);
            checkDrop();
            //rotateObject();
        } else {
            pickup();
        }
    }

    void rotateObject() {
        carriedObject.transform.Rotate(5, 10, 15);
    }

    void carry(Pickupable o) {
        //o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
        Touch CurrentTouch = Input.GetTouch(0);
        if (CurrentTouch.phase == TouchPhase.Began) {
            firstTouch = CurrentTouch;
        }
        Vector3 firstTouchPosition = firstTouch.position;
        Vector3 currentTouchPosition = CurrentTouch.position;
        float yDistance = Mathf.Abs(firstTouchPosition.y - currentTouchPosition.y);
        if (yDistance > zThresh) {
            if (firstTouchPosition.y > currentTouchPosition.y) {
                distance -= .5f;
            } else {
                distance += .5f;
            }
        }

        o.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
        o.transform.rotation = Quaternion.identity;

        debug.SetText("first Touch Y = " + firstTouchPosition.y + "Current: " + currentTouchPosition.y);
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
        carrying = false; 
        carriedObject.DropSelf();
        carriedObject = null;
    }
}