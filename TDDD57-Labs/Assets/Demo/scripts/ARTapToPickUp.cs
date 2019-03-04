using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARTapToPickUp : MonoBehaviour
{
    GameObject mainCamera;
    GameObject carriedObject;
    GameObject selectedObject;
    Color32 regColor = new Color32(255, 255, 255, 255);
    Color32 highlightColor = new Color32(53, 255, 63, 255);
    public Transform theDest;
    bool carrying = false;
    public float distance;
    public float smooth;
    // Use this for initialization
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
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

    void carry(GameObject o)
    {
        o.transform.position = Vector3.Lerp(o.transform.position, mainCamera.transform.position + mainCamera.transform.forward * distance, Time.deltaTime * smooth);
        o.transform.rotation = Quaternion.identity;
    }

    void pickup()
    {

        var screenCast = mainCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(screenCast);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Pickupable p = hit.collider.GetComponent<Pickupable>();
            if (p != null)
            {
                if (selectedObject != null)
                {
                    selectedObject.GetComponent<Renderer>().material.color = regColor;
                }
                selectedObject = p.gameObject;
                p.gameObject.GetComponent<Renderer>().material.color = highlightColor;
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    carrying = true;
                    carriedObject = p.gameObject;
                    p.gameObject.GetComponent<Rigidbody>().useGravity = false;

                }
            }
            else
            {
                selectedObject.GetComponent<Renderer>().material.color = regColor;
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
        //carriedObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject.GetComponent<Renderer>().material.color = regColor;
        carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
        carriedObject = null;
    }
}
    /*
    public Transform theDest;

    private bool holdingCube = false;

    
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began){
            if (holdingCube == false)
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = theDest.position;
                this.transform.parent = GameObject.Find("Destination").transform;
                holdingCube = true;
            }
            else
            {
                GetComponent<BoxCollider>().enabled = true;
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                holdingCube = false;
            }
        }
    }
}
*/