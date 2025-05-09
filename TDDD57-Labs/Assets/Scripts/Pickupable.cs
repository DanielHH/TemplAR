﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class Pickupable : MonoBehaviour {
    public List<SnapTrigger> snapTriggers = new List<SnapTrigger>();
    public Color32 regColor = new Color32(255, 255, 255, 255);
    public Color32 highlightColor = new Color32(53, 255, 63, 255);
    public bool isSymmetric = false;

    private bool snap = false;
    private bool selected = false;
    private SnapTrigger currentSnapTrigger = null;

    private void Start() {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.SetColor("_EmissionColor", regColor);
        }
    }

    public void DropSelf() {
        Highlight(false);
        selected = false;
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

    public void SetKinematic(bool isKinematic) {
        GetComponent<Rigidbody>().isKinematic = isKinematic;
    }

    public void Highlight(bool highlighted) {

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
            if (highlighted) {
                renderer.material.SetColor("_EmissionColor", highlightColor);
            } else {
                renderer.material.SetColor("_EmissionColor", regColor);
            }  
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
            currentSnapTrigger.DestroyIndicator();
            snap = false;

        }
        foreach (SnapTrigger trigger in snapTriggers) {
            if (trigger.wayPoint.wayPointType == WayPointType.DANGER) {
                trigger.gameObject.SetActive(true);
            }
        }
    }

    public void Snap() {
        snap = true;
    }

    public void UnSnap() {
        snap = false;
    }

    public void setSelected() {
        selected = true;
    }

    public bool isSelected() {
        return selected;
    }

    public void ClearRb() {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}
