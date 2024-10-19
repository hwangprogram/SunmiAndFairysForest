using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyThese2 : MonoBehaviour
{
    void Awake()
    {
        if (GameObject.FindObjectsOfType<DontDestroyThese2>().Length > 1) Destroy(this.gameObject);
        else DontDestroyOnLoad(this.gameObject);
    }
}

