using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class Pickupable : MonoBehaviour {
    public List<SnapTrigger> snapTriggers = new List<SnapTrigger>();
    public Color32 regColor = new Color32(204, 204, 204, 255);
    public Color32 highlightColor = new Color32(53, 255, 63, 255);

    private bool snap = false;
    private SnapTrigger currentSnapTrigger = null;

    public void DropSelf() {
        UnSelect();
        if (snap) {
            transform.position = currentSnapTrigger.transform.position;
            transform.rotation = currentSnapTrigger.transform.rotation;
            currentSnapTrigger.SetWayPointType(WayPointType.NORMAL);
            GetComponent<Rigidbody>().isKinematic = true;
        } else {
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false; 
        }
        if (currentSnapTrigger != null) {
            currentSnapTrigger.NullifyPO();
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
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.color = regColor;
        }      
    }

    public void Select() {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.color = highlightColor;
        }
    }

    public void setCurrentSnapTrigger(SnapTrigger trigger) {
        currentSnapTrigger = trigger;
    }

    public SnapTrigger getCurrentSnapTrigger() {
        return currentSnapTrigger;
    }

    public void ActivateSnapTriggers() {
        if (currentSnapTrigger != null && currentSnapTrigger.wayPoint.wayPointType == WayPointType.NORMAL) {
            currentSnapTrigger.SetWayPointType(WayPointType.DANGER);
        }
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
