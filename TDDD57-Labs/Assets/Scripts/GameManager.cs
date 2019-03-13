using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{


    public TMPro.TMP_Text debug;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetLevel() {
        debug.SetText("NUUGG");
        GameObject level = GameObject.FindGameObjectWithTag("LevelObject");
        Destroy(level);
    }
}
