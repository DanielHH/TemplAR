using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARTapToPickUp : MonoBehaviour
{
    GameObject mainCamera;
    Pickupable carriedObject;
    Pickupable selectedObject;

    public TMPro.TMP_Text debug;

    bool carrying = false;
    public float distance;
    public float smooth;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void Update()
    {
        if (carrying)
        {
            carry(carriedObject);
            checkDrop();
            //rotateObject();
        }
        else
        {
            pickup();
        }
    }

    void rotateObject()
    {
        carriedObject.transform.Rotate(5, 10, 15);
    }

    void carry(Pickupable o)
    {
        //o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
  
        o.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
        o.transform.rotation = Quaternion.identity;
    }

    void pickup()
    {
        var screenCast = mainCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(screenCast);
        RaycastHit hit;
        Debug.DrawRay(screenCast, ray.direction);
        if (Physics.Raycast(ray, out hit))
        {
            
            if (hit.collider.tag == "Pickupable") {
                Pickupable p = hit.collider.GetComponent<Pickupable>();
     
                if (selectedObject != p && selectedObject != null) {
                    selectedObject.UnSelect();
                }
                selectedObject = p;
                selectedObject.Select();

                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began) {
                    carrying = true;
                    carriedObject = selectedObject;
                    carriedObject.ActivateSnapTriggers();
                    carriedObject.UseGravity(false);
                }
            } else
            {
                selectedObject.UnSelect();
                selectedObject = null;
            }
        }
    }

    void checkDrop()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            dropObject();
        }
    }

    void dropObject()
    {
        carrying = false; 
        carriedObject.DropSelf();
        carriedObject = null;
    }
}