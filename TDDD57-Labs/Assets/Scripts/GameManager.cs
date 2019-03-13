using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void ResetLevel() {

        GameObject level = GameObject.FindGameObjectWithTag("LevelObject");
        Destroy(level);
    }
}
