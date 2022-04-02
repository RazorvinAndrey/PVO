using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class Controller1 : ControllerBase
{

    protected Window_Graph graphScript;

    override public void OnStart()
    {
        graphScript = FindObjectOfType<Window_Graph>();

    }

    override public void OnFixedUpdate()
    {
        // outt = Mathf.Lerp(outt, signal[0], Time.deltaTime * impulseSmooth);
        // graphScript.AddNewPoint(outt + 2 * Random.value - 1);
    }

    override public void Catch(GameObject go, int id)
    {
        ToggleSignal(id, true);
        DetectPlane(go);
    }

    override public void Lost(GameObject go, int id)
    {
        ToggleSignal(id, false);
    }


}
