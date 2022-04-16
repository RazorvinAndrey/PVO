using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Square : MonoBehaviour
{
    public delegate void SquareEvent(GameObject sector, int zone);
    public static event SquareEvent OnSquareEnter;
    public static event SquareEvent OnSquareExit;
    public static event SquareEvent OnSquareClick;  

    public int size;
    public Transform tangent;
    private SpriteRenderer _sRenderer;
    private MeshRenderer _mRenderer;

    private CameraMover _cameraMover;
    void Start()
    {
        TryGetComponent<SpriteRenderer>(out _sRenderer);
        TryGetComponent<MeshRenderer>(out _mRenderer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (EarthController.currentSquare?[size] != this)
            {
                OnSquareClick?.Invoke(this.gameObject, size);
                SetOpacity(0f);
            }
        }
        // Debug.Log($"click {this.gameObject}");
    }
    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {

            if (EarthController.currentSquare?[size] != this.gameObject)
            {
                OnSquareEnter?.Invoke(this.gameObject, size);
                SetOpacity(0.5f);
            }

            Debug.Log($"enter {this.gameObject.name } {EarthController.currentSquare?[size] } {size}");
        }
    }
    void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            OnSquareEnter?.Invoke(this.gameObject, size);
            SetOpacity(0f);
            // Debug.Log($"exit {this.gameObject}");
        }
    }

    private void SetOpacity(float value)
    {
        if (_sRenderer != null)
        {
            var clr = _sRenderer.color;
            _sRenderer.color = new Color(clr.r, clr.g, clr.b, value);
        }
        if (_mRenderer != null)
        {
            EarthController.SetOpacity(_mRenderer.material,value);
        }
    }
}
