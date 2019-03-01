using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;

public class ARTapToPickUp : MonoBehaviour
{
    GameObject mainCamera;
    public Transform theDest;
    bool carrying = false;
    GameObject carriedObject;
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
        o.transform.position = theDest.position;
        o.transform.rotation = Quaternion.identity;
    }

    void pickup()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var screenCast = mainCamera.GetComponent<Camera>().ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            Ray ray = mainCamera.GetComponent<Camera>().ScreenPointToRay(screenCast);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Pickupable p = hit.collider.GetComponent<Pickupable>();
                if (p != null)
                {
                    carrying = true;
                    carriedObject = p.gameObject;
                    p.gameObject.GetComponent<Rigidbody>().useGravity = false;
                }
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