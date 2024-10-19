using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform followThis;

    private Vector2 pos;

    private void Awake()
    {
        pos = followThis.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        pos = followThis.transform.position;
        this.transform.position = pos;
    }
}
