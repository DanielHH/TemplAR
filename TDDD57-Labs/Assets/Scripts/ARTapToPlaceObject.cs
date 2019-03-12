using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;

public class ARTapToPlaceObject : MonoBehaviour {
    GameObject mainCamera;
    public GameObject objectToPlace;
    public GameObject placementIndicator;

    private ARSessionOrigin arOrigin;
    private Pose ARPlacementPose;
    private Pose IGlacementPose;
    private bool placementPoseIsValid = false;
    void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera");
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        Application.targetFrameRate = 30;
    }


    void Update() {
        updatePlacementPose();
        updatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Began) {
            PlaceObject();
        }
    }

    private void PlaceObject() {
        Instantiate(objectToPlace, ARPlacementPose.position, Quaternion.identity);
    }

    private void updatePlacementIndicator() {
        if (placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(IGlacementPose.position, ARPlacementPose.rotation);
        } else {
            placementIndicator.SetActive(false);
        }
    }

    private void updatePlacementPose() {
        var screenCast = mainCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(screenCast);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            IGlacementPose.position = hit.point;
        }

        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCast, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid) {
            ARPlacementPose = hits[0].pose;
            
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            ARPlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
            
        }
    }
}
