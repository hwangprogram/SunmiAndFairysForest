using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class a_fox : MonoBehaviour
{

    void Start()
    {
        if (Hub.PlayerStatus.currentDirection == Direct.left) this.transform.localScale = new Vector3(-1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
