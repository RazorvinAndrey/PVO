using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalTrigger : MonoBehaviour
{
    [SerializeField]private int id;
    public delegate void LocatorEvent(GameObject go,int id);
    public static event LocatorEvent CatchObj;
    public static event LocatorEvent LostObj;


    private void OnTriggerEnter(Collider other)
    {
         CatchObj?.Invoke(other.gameObject,id);
    
    }

    private void OnTriggerExit(Collider other)
    {
        LostObj?.Invoke(other.gameObject,id);
    }
}
