using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public Transform lookAtObject;
    public float rotationSpeed = 8;  //This will determine max rotation speed, you can adjust in the inspector
    public static bool allowRotation;
    public Camera cam;  //Drag the camera object here
    private Vector3 prevMousePos = Vector3.zero;
    void Start()
    {
        cam = Camera.main;
        allowRotation = true;
    }
    private void OnEnable()
    {
        Sector.OnSectorClick += OnSectorClick;
    }
    private void OnDisable()
    {
        Sector.OnSectorClick -= OnSectorClick;
    }

    void Update()
    {

        if (allowRotation && Input.GetMouseButton(1))
        {
            var before = transform.localEulerAngles;
            var xRotation = new Vector3(0, -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotationSpeed;
            var yRotation = new Vector3(0, -Input.GetAxis("Mouse Y"), 0) * Time.deltaTime * rotationSpeed;
            // lookAtObject.Translate(yRotation);
            // var lp = lookAtObject.position;
            // if(lp.y>30) lookAtObject.position= new Vector3(lp.x,30,lp.z);
            // if(lp.y<-30) lookAtObject.position= new Vector3(lp.x,-30,lp.z);
            // // transform.LookAt(lookAtObject);
            // var after = transform.localEulerAngles;
            // transform.localEulerAngles = new Vector3(after.x,before.y,after.z);
            var onCamDir = (transform.position -cam.transform.position).normalized;
            var axX = Vector3.Cross(onCamDir,transform.up);
            transform.RotateAround(transform.up,xRotation.y);
            transform.RotateAround(axX,yRotation.y);

            // Debug.Log(onCamDir);
            // Debug.DrawRay(transform.position,axX*30,Color.red,10);
        }

    }


    void OnSectorClick(GameObject sector, int zone)
    {

    }

}
