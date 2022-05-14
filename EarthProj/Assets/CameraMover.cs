using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI; 

public class CameraMover : MonoBehaviour
{
    [SerializeField] public MeshRenderer sphere;
    [SerializeField] private float camSpeed;
    [SerializeField] private EarthController EarthPlan;
    [SerializeField] private CinemachineDollyCart camFollowObj;
    [SerializeField] private float camSectorMultipiller;
    [SerializeField] private float camSquareMultipiller;
    [SerializeField] private float sectorOffsetMultipiller;
    [SerializeField] private float squareOffsetMultipiller;
    [SerializeField] private float minorSqrCamMultipiller;
    [SerializeField] private float[] minorSqrOffsetMultipiller;
    [SerializeField] private float[] zoneSqrCameraMulti;
    [SerializeField] private float sqrScaleFactor;
    [SerializeField] private BackButton backbutton;
    [SerializeField] public Text coordText;
    public GameObject lastClickedSquare;
    private bool inFlag = true;


    [SerializeField] private GameObject[] zoneGO;
    [SerializeField] private GameObject[] sqaresGO;
    [SerializeField] private GameObject zoneGSqrGO;
    [SerializeField] private Transform earth;
    private CinemachineSmoothPath path;
    public GameObject currentSectorGo;
    // public GameObject currentSquareGo;
    public GameObject[] currentSquareGo;

    private Vector3 currentNormalVector;
    private void OnEnable()
    {
        Sector.OnSectorClick += OnSectorClick;
        Square.OnSquareClick += OnSquareClick;

    }
    private void OnDisable()
    {
        Sector.OnSectorClick -= OnSectorClick;
        Square.OnSquareClick -= OnSquareClick;

    }
    private void Start()
    {
        path = GetComponent<CinemachineSmoothPath>();
        currentSquareGo = new GameObject[3];

        Debug.Log($"{currentSquareGo.Length} {currentSquareGo[0]}");

    }

    private void OnSectorClick(GameObject sector, int zone)
    {
        
        Vector3 pos = sector.GetComponent<Collider>().ClosestPoint(earth.position);

        currentNormalVector = (pos - earth.position).normalized;
        if (currentSectorGo != null)
        {
            Destroy(currentSectorGo);
        }
        DestroyMinorSqrs(0);
        path.m_Waypoints[1].position = pos + currentNormalVector * camSectorMultipiller;
        camFollowObj.m_Speed = camSpeed;
        coordText.text = sector.name;
        MouseRotation.allowRotation = false;
        StartCoroutine(SpawnSectorGO(pos, currentNormalVector, zone));
    }

    IEnumerator SpawnSectorGO(Vector3 pos, Vector3 normal, int zone)
    {

        yield return new WaitForEndOfFrame();
        currentSectorGo = Instantiate(zoneGO[zone], pos + normal * sectorOffsetMultipiller, Quaternion.LookRotation(-normal, earth.up), earth);
    }
    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)||backbutton.flag)
        {
            backbutton.flag = false;
            for (int i = currentSquareGo.Length - 1; i >= 0; i--)
            {
                if (currentSquareGo[i] != null)
                {
                   
                    DestroySqr(i);

                    if (i != 0)
                    {
                       
                     
                       
                        // var offset = camSquareMultipiller / (1 + minorSqrCamMultipiller * (i - 1));
                        path.m_Waypoints[1].position = currentSquareGo[i - 1].transform.position + currentNormalVector * (GetSqrCamOffset(i - 1) + GetSqrPlaceOffset(i - 1));
                    }
                    else
                    {
                        path.m_Waypoints[1].position = currentSectorGo.transform.position + currentNormalVector * camSectorMultipiller;
                    }
                    return;
                }
            }

            
                if (currentSectorGo != null)
            {
                
                EarthController.currentSector = null;
                coordText.text = "";
                Destroy(currentSectorGo);
                camFollowObj.m_Position = 0;
                camFollowObj.m_Speed = 0;
                MouseRotation.allowRotation = true;

            }
        }
    }

    private void OnSquareClick(Square square, int size)
    {
        var sizeReal = size;
        if (size >= 5) size = size - 5;
        var tr = square.transform;
        var pos = tr.position;
        var normalVector = (pos - earth.position).normalized;
        /*if (inFlag)
        {
            inFlag = false;
        }
        else
        {
            lastClickedSquare.gameObject.SetActive(true);
        }
        lastClickedSquare = square.gameObject;
        */

        DestroyMinorSqrs(size);
        EarthController.currentSquare[size] = square.gameObject;
        camFollowObj.m_Speed = camSpeed;
        MouseRotation.allowRotation = false;

        //if (square.textFlag)
        {
            coordText.text += "." + square.name;
            square.textFlag = false;
        }

        if (EarthController.currentSector.zone == 0 && size == 0)
        {
            pos = tr.parent.position;
            normalVector = (pos - earth.position).normalized;

            // path.m_Waypoints[1].position = pos + normalVector * (GetSqrCamOffset(size) + GetSqrPlaceOffset(size));
           
            StartCoroutine(SpawnZoneGSqr0(tr.parent, normalVector));
        }
        else
        {
            square.gameObject.SetActive(false);
            StartCoroutine(SpawnSquareGO(tr, normalVector, size,sizeReal));
        }
    
        path.m_Waypoints[1].position = pos + normalVector * (GetSqrCamOffset(size) + GetSqrPlaceOffset(size) );
    }

    IEnumerator SpawnSquareGO(Transform tr, Vector3 normal, int size,int sizeReal)
    {
        if (size >= 5) size = size - 5;
        yield return new WaitForEndOfFrame();
        Vector3 uport;
        if(size==0){
            uport = currentSectorGo.transform.up;
        }else{
            uport = currentSquareGo[size-1].transform.up;
            normal= -currentSquareGo[size-1].transform.forward;
        }
        GameObject go = Instantiate(sqaresGO[sizeReal], tr.position + normal*GetSqrPlaceOffset(size), Quaternion.LookRotation(-normal, uport), earth) as GameObject;
        go.transform.localScale = tr.lossyScale / sqrScaleFactor;
        currentSquareGo[size] = go;
        if (size == 0)
        {
            EarthController.SetOpacity(sphere.materials[3], 0.0f);
        }
        else if(size == 1) 
        {
            currentSectorGo.SetActive(false);
        } 
        else if (size > 1)
        {
            currentSquareGo[size - 2].gameObject.SetActive(false);
        }
    }
    IEnumerator SpawnZoneGSqr0(Transform tr, Vector3 normal)
    {

        yield return new WaitForEndOfFrame();
        var size = 0;
        GameObject go = Instantiate(zoneGSqrGO, tr.position + normal * squareOffsetMultipiller, Quaternion.LookRotation(-earth.up, tr.right), earth) as GameObject;
        // go.transform.localScale = tr.lossyScale / sqrScaleFactor;
        // 
        // go.transform.rotation = tr.rotation;
        currentSquareGo[size] = go;
    }

    private void DestroyMinorSqrs(int size)
    {
        if (size >= 5) size -= 5;
        for (int i = currentSquareGo.Length - 1; i >= size; i--)
        {
            // if (currentSquareGo[i] != null) Destroy(currentSquareGo[i]);
            DestroySqr(i);
        }
    }

 

    private float GetSqrCamOffset(int size)
    {
        if (size >= 5) size = size - 5;
        float mult = 1;
        if (EarthController.currentSector != null)
        {
            int zone = EarthController.currentSector.zone ;

            mult = zoneSqrCameraMulti[zone];
            
        }
        
        return mult * camSquareMultipiller / (1 + minorSqrCamMultipiller * size);
    }
    private float GetSqrPlaceOffset(int size)
    {
        if (size >= 5) size = size - 5;
        // return squareOffsetMultipiller / (1 + minorSqrOffsetMultipiller * size);
        return minorSqrOffsetMultipiller[size];
    }

    private void DestroySqr(int size )
    {
        if (size >= 5) size = size - 5;
        var i = size;
        if (currentSquareGo[i] != null)
        {
            //if (countSymbol(currentSquareGo[i].name, '.')-1 == 2)
           
            coordText.text = coordText.text.Remove(coordText.text.LastIndexOf("."));

            Destroy(currentSquareGo[i]);
            if (size == 0)
            {
                EarthController.SetOpacity(sphere.materials[3], 1.0f);
            }
            else if(size == 1)
            {
                currentSectorGo.SetActive(true);
            }
            else if (size>1)
            {
                currentSquareGo[i - 2].SetActive(true);
            }
        }

        // if (EarthController.currentSquare[i] != null)
        // 

        // if (EarthController.currentSquare[i].TryGetComponent<Collider>(out var collider))
        // {
        //     collider.enabled = true;
        // }

        EarthController.currentSquare[i] = null;
        //coordText.text.Remove(coordText.text.Length - currentSquareGo[i].name.Length + 1);
        // }

    }

    private int countSymbol(string str,char ch)
    {
        int k = 0;
        for(int i = 0; i < str.Length; i++)
        {
            if (str[i] == ch)
            {
                k++;
            }
        }
        return k;
    }



   
}
