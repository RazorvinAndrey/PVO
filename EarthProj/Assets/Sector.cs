using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sector : MonoBehaviour
{
    public delegate void SectorEvent(GameObject sector, int zone);
    public static event SectorEvent OnSectorEnter;
    public static event SectorEvent OnSectorExit;
    public static event SectorEvent OnSectorClick;

    public int zone;
    private MeshRenderer _mRenderer;
    private CameraMover _cameraMover;
    void Start()
    {
        _mRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (EarthController.currentSector != this)
        {
            OnSectorClick?.Invoke(this.gameObject, zone);
            EarthController.SetOpacity(_mRenderer.material, 0f);
        }
    }
    void OnMouseEnter()
    {
        if (EarthController.currentSector != this)
        {
            OnSectorEnter?.Invoke(this.gameObject, zone);
            EarthController.SetOpacity(_mRenderer.material, 0.5f);
        }

    }
    void OnMouseExit()
    {
        OnSectorEnter?.Invoke(this.gameObject, zone);
        EarthController.SetOpacity(_mRenderer.material, 0f);

    }
}
