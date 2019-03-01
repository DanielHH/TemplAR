using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARTapToPickUp : MonoBehaviour
{
    public GameObject pickUpIndicator;

    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
    }


    void Update()
    {
        updatePlacementPose();
        updatePlacementIndicator();

        if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }

    private void updatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            pickUpIndicator.SetActive(true);
            pickUpIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            pickUpIndicator.SetActive(false);
        }
    }

    private void updatePlacementPose()
    {
        var screenCast = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arOrigin.Raycast(screenCast, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        /*
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;

            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);

        }
        */
    }
}
