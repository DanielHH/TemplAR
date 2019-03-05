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

    bool carrying = false;
    public float distance;
    public float smooth;

    /*
    public string partnerTag;
    public float closeDist = 5f;
    private bool isSnapped;
    Color32 snapColor = new Color32(255, 211, 0, 255);
    GameObject partnerGO;

    float dist = Mathf.Infinity;
    */
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
    }

    void Update()
    {
        /*
        partnerGO = GameObject.FindGameObjectWithTag(partnerTag);
        */
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


        // Checks if the carried object is close enough to a snap to another object
        /*
        Vector3 partnerPos = mainCamera.GetComponent<Camera>().WorldToViewportPoint(partnerGO.transform.position);
        Vector3 carryPos = mainCamera.GetComponent<Camera>().WorldToViewportPoint(o.transform.position);

        dist = Vector3.Distance(partnerPos, carryPos);

        o.GetComponent<Renderer>().material.color = (dist < closeDist) ? snapColor : highlightColor;
        */
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