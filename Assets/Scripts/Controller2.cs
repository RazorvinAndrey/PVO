using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class Controller2 : ControllerBase
{

    protected Window_Graph[] graphScript;
    private int state;
    override public void OnStart()
    {
        state = -1;
        graphScript = FindObjectsOfType<Window_Graph>();

    }
    void OnEnable()
    {
        SignalTrigger.CatchObj += Catch;
        SignalTrigger.LostObj += Lost;
    }
    void OnDisable()
    {
        SignalTrigger.CatchObj -= Catch;
        SignalTrigger.LostObj -= Lost;
    }

    override public void OnFixedUpdate()
    {
        // for (int i = 0; i < 4 ; i++)
        // {
        //     outt[i] = Mathf.Lerp(outt[i], signal[i], Time.deltaTime * impulseSmooth);
        //     graphScript[i].AddNewPoint(outt[i] + 2 * Random.value - 1);
        // }

    }

    override public void Catch(GameObject go, int id)
    {
        // Debug.Log($"id {id}");
       
        if (id == -1)
        {
            // state++;
             DetectPlane(go);
        }else{
             ToggleSignal(id, true);
        }
        // TryDetectPlane(go);
    }

    override public void Lost(GameObject go, int id)
    {
        ToggleSignal(id, false);

    }

    private void TryDetectPlane(GameObject go)
    {
        if (state == 1)
        {
            DetectPlane(go);
        }
    }






}
