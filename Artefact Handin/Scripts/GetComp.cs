using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetComp : MonoBehaviour
{
    // a script that allows the menu to follow the users camera so that it is facing them.
    private Canvas canvas;
    private Transform cam;
    void Start()
    {
        cam = GameObject.Find("VR Camera").GetComponent<Transform>();
        canvas = this.gameObject.GetComponent<Canvas>();
        canvas.worldCamera = cam.gameObject.GetComponent<Camera>();
    }


    //
    private void LateUpdate()
    {
        Vector3 v = canvas.worldCamera.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(canvas.worldCamera.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}
