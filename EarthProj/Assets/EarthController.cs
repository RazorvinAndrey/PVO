using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthController : MonoBehaviour
{

    public MeshRenderer sphere;
    public List<Texture> zonesTextures = new List<Texture>();
    public List<Color> zonesColors = new List<Color>();
    static public Sector currentSector;
    static public GameObject[] currentSquare;
    void Start()
    {
        currentSquare = new GameObject[3];
        SetOpacity(sphere.materials[2], 1f);

    }

    private void OnEnable()
    {
        Sector.OnSectorClick += OnSectorClick;
        Sector.OnSectorEnter += OnSectorEnter;
        Sector.OnSectorExit += OnSectorExit;
        Square.OnSquareClick += OnSquareClick;

    }
    private void OnDisable()
    {
        Sector.OnSectorClick -= OnSectorClick;
        Sector.OnSectorEnter -= OnSectorEnter;
        Sector.OnSectorExit -= OnSectorExit;
        Square.OnSquareClick += OnSquareClick;

    }
    public static void SetOpacity(Material mat, float opacity)
    {
        var color = mat.color;
        mat.color = new Color(color.r, color.g, color.b, opacity);
    }

    public void SelectZone(int id)
    {
        if (id < 0 || id > 3)
        {
            SetOpacity(sphere.materials[2], 0.0f);
            return;
        }
        sphere.materials[2].mainTexture = zonesTextures[id];
        sphere.materials[2].color = zonesColors[id];

    }

    public void OnSectorEnter(GameObject sector, int zone)
    {
        SelectZone(zone);
        
    }
    public void OnSectorExit(GameObject sector, int zone)
    {
        SelectZone(-1);
    }
    public void OnSectorClick(GameObject sector, int zone)
    {
        currentSector = sector.GetComponent<Sector>();
    }
    public void OnSquareClick(GameObject square, int size)
    {
        currentSquare[size] = square;
        // currentSquare[size].GetComponent<Collider>().enabled = false;
        // Debug.Log($"recorded {square} as sqr {size}");
    }
    public static int LastSqr(){
        for (int i = currentSquare.Length-1; i <=0; i--)
        {
            var j = i-1;
            if(currentSquare[i]!=null) return i;
        }
        return -1;
    }

}
