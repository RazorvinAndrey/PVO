using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour
{

    /*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/


    [SerializeField] float mainSpeed = 100.0f; //regular speed
    [SerializeField] float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    [SerializeField] float maxShift = 1000.0f; //Maximum speed when holdin gshift
    [SerializeField] float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(0, 0, 0); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;
    private GameObject radar;

    void Start()
    {
        radar = GameObject.Find("radar");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            transform.position = new Vector3(-80, 80, -30);
            transform.LookAt(radar.transform);

            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            lastMouse = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
            transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
        }

        //Mouse  camera angle done.  

        //Keyboard commands
        Vector3 p = GetBaseInput();

        p = p* Time.unscaledDeltaTime * mainSpeed;
        transform.Translate(p);

    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        { 
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        return p_Velocity;
    }

    void TranslateNoTime(Vector3 pos){
        transform.localPosition +=pos;
    }
}