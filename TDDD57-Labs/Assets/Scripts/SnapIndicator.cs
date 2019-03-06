using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapIndicator : MonoBehaviour
{
    public float fadeSpeed = 200;
    [Range(0f, 255f)]
    public float opacityRange = 200;

    void Update()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
            renderer.material.color = new Color32(255, 255, 255, (byte)Mathf.PingPong(Time.time * fadeSpeed, opacityRange));
        }
        
    }
}
