using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InfoScript : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField]private GameObject infoGO;
     public void OnPointerEnter(PointerEventData eventData)
    {
        infoGO.SetActive(true);
    }
     public void OnPointerExit(PointerEventData eventData)
    {
        infoGO.SetActive(false);
    }

    
}
