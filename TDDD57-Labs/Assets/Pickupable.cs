using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {
    public SnapTrigger snapTrigger;
    public Color32 regColor = new Color32(255, 255, 255, 255);
    public Color32 highlightColor = new Color32(53, 255, 63, 255);

    private bool snap = false;

    public void DropSelf() {
        UnSelect();
        if (snap) {
            transform.position = snapTrigger.transform.position;
            transform.rotation = snapTrigger.transform.rotation;         
            GetComponent<Rigidbody>().isKinematic = true;
            snapTrigger.gameObject.SetActive(false);
        } else {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            
        }
    }

    public void UseGravity(bool useGravity) {
        GetComponent<Rigidbody>().useGravity = useGravity;
    }

    public void UnSelect() {
        GetComponent<Renderer>().material.color = regColor;
    }

    public void Select() {
        GetComponent<Renderer>().material.color = highlightColor;
    }

    public void Snap() {
        snap = true;
    }

    public void UnSnap() {
        snap = false;
    }
}
