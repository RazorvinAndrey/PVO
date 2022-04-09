using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    
    public bool flag = false;
    public void ToggleFlag()
    {
        if (flag) {
            flag = false;
        } else
        {
            flag = true;
        }
    }
}
