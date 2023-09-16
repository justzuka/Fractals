using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Rotate : MonoBehaviour
{
    public Slider Xrot;
    public Slider Yrot;
    public Slider Zrot;
    Vector3 rot;

    // Update is called once per frame
    void Update()
    {
        rot.x = Xrot.value;
        rot.y = Yrot.value;
        rot.z = Zrot.value;
        transform.eulerAngles = rot;
    }
}
