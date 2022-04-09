using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine.SceneManagement;

public class ControllerBase : MonoBehaviour
{
    public float rotationSpeed;
    public float impulseAmplitude;
    public float impulseSmooth;
    public GameObject radar;
    public bool showLine;
    public Axies axies;
    protected Locator locator;
    public static float[] signal = new float[5];
    protected float[] outt = new float[5];
    public static float time;

    protected GameObject phantom;
    protected Window_Graph[] graphScripts;

    void Start()
    {
        locator = FindObjectOfType<Locator>();
        graphScripts = FindObjectsOfType<Window_Graph>();
        foreach (var item in graphScripts)
        {
            Debug.Log(item.name);
        }
        for (int i = 0; i < 5; i++)
        {
            outt[i] = 0;
            signal[i] = 0;
        }
        OnStart();

    }

    void FixedUpdate()
    {

        radar.transform.Rotate(Vector3.up * rotationSpeed);
        locator.arrow.Rotate(Vector3.back * rotationSpeed);

        for (int i = 0; i < graphScripts.Length; i++)
        {
            outt[i] = Mathf.Lerp(outt[i], signal[i], Time.deltaTime * impulseSmooth);
            graphScripts[i].AddNewPoint(outt[i] + 2 * Random.value - 1);
        }
        OnFixedUpdate();
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("menu");
            return;
        }
    }
    private void OnEnable()
    {
        SignalTrigger.CatchObj += Catch;
        SignalTrigger.LostObj += Lost;
    }
    private void OnDisable()
    {
        SignalTrigger.CatchObj -= Catch;
        SignalTrigger.LostObj -= Lost;
    }
    virtual public void OnFixedUpdate() { }
    virtual public void OnStart() { }
    virtual public void Catch(GameObject go, int id) { }
    virtual public void Lost(GameObject go, int id) { }

    protected void DrawArch(float angel)
    {
        var delta = 1f;
        var radius = 10f;
        int size = (int)(angel / delta);

        axies.angelLine.positionCount = (size);
        float angelstep = angel / size / 180 * Mathf.PI;
        // Debug.Log($"angel: {angel}  size: {size}  angstep {angelstep}");
        for (int i = 0; i < size; i++)
        {
            var Theta = angelstep * i;
            float x = radius * Mathf.Cos(Theta);
            float y = radius * Mathf.Sin(Theta);
            axies.angelLine.SetPosition(i, new Vector3(y, 0, x));
        }
        float ThetaText;
        if (angel < 40)
        {
            ThetaText = (angel) * 1.5f / 180 * Mathf.PI;
        }
        else ThetaText = (angel) / 2 / 180 * Mathf.PI;
        // var T = (angel) / 2 / 180 * Mathf.PI;
        // Debug.Log(ThetaText / Mathf.PI * 180);
        axies.angelValue.text = $"{Mathf.Round(angel)}°";
        float xt = 1.8f * radius * Mathf.Cos(ThetaText);
        float yt = 1.8f * radius * Mathf.Sin(ThetaText);
        axies.angelValue.transform.position = new Vector3(yt, 0.05f, xt);
    }

    public void ToggleSignal(int id, bool active)
    {
        // Debug.Log($"{id} - {active}");
        if (id >= signal.Length ||id <0) return;
        signal[id] = active ? impulseAmplitude : 0;
        time = 0;
        // Debug.Log($"{signal[id]}");
    }

    protected GameObject CreatePhantom(GameObject go)
    {
        var newgo = Instantiate(go);
        newgo.TryGetComponent<Collider>(out var collider);
        if (collider != null) collider.enabled = false;
        var meshes = newgo.GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshes)
        {
            var mats = mesh.materials;
            foreach (var mat in mats)
            {
                var c = mat.color;
                mat.color = new Color(c.r, c.g, c.b, 0.5f);
                ToFadeMode(mat);
            }
        }
        return newgo;
    }

    public static void ToFadeMode(Material material)
    {
        material.SetOverrideTag("RenderType", "Transparent");
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    protected void DetectPlane(GameObject go)
    {

        var reallen = 80f;
        var arrowlen = locator.arrow.sizeDelta.y;
        var factor = arrowlen / reallen;

        var locatorPos = new Vector2(go.transform.position.x, go.transform.position.z) * factor;
        locator.SetPoint(locatorPos);

        axies.targetLine.gameObject.SetActive(showLine);
        axies.angelLine.gameObject.SetActive(showLine);
        if (showLine)
        {
            axies.targetLine.SetPosition(2, go.transform.position);
            var grndPos = new Vector3(go.transform.position.x, 0, go.transform.position.z);
            axies.targetLine.SetPosition(1, grndPos);
            var angel = -Vector3.SignedAngle(go.transform.position, Vector3.forward, Vector3.up);
            if (angel < 0) angel += 360;
            DrawArch(angel);
            if (phantom == null)
            {
                phantom = CreatePhantom(go);
            }
            phantom.transform.position = go.transform.position;
            phantom.transform.rotation = go.transform.rotation;

        }
    }

}
