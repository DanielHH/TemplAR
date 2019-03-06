using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {
    public List<SnapTrigger> snapTriggers = new List<SnapTrigger>();
    public Color32 regColor = new Color32(255, 255, 255, 255);
    public Color32 highlightColor = new Color32(53, 255, 63, 255);

    private bool snap = false;
    private int currentSnapTrigger = 0;

    public void DropSelf() {
        UnSelect();
        if (snap) {
            transform.position = snapTriggers[currentSnapTrigger].transform.position;
            transform.rotation = snapTriggers[currentSnapTrigger].transform.rotation;         
            GetComponent<Rigidbody>().isKinematic = true;
        } else {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            
        }
        DeActivateSnapTriggers();
    }

    private void DeActivateSnapTriggers() {
        foreach (SnapTrigger trigger in snapTriggers) {
            trigger.gameObject.SetActive(false);
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

    public void setCurrentSnapTrigger(int trigger) {
        currentSnapTrigger = trigger;
    }

    public int getCurrentSnapTrigger() {
        return currentSnapTrigger;
    }

    public void ActivateSnapTriggers() {
        foreach (SnapTrigger trigger in snapTriggers) {
            trigger.gameObject.SetActive(true);
        }
    }

    public void Snap() {
        snap = true;
    }

    public void UnSnap() {
        snap = false;
    }
}
