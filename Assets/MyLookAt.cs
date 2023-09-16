using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLookAt : MonoBehaviour
{
    private Transform target;
    private void Start()
    {
        target = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
