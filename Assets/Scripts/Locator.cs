using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Locator : MonoBehaviour
{
    public RectTransform arrow;
    public float fadeSpeed;

    [SerializeField] private Sprite circleSprite;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPoint(Vector2 pos)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(transform, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = new Color(0,1,0.3f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pos;
        rectTransform.sizeDelta = new Vector2(11, 11);

        StartCoroutine(FadePoint(gameObject.GetComponent<Image>()));
    }

    IEnumerator FadePoint(Image point){
        var appacity = point.color.a;
        while (appacity>0)
        {
            appacity-= Time.deltaTime*fadeSpeed;
            point.color = new Color(point.color.r,point.color.g,point.color.b,appacity);
            yield return new WaitForEndOfFrame();
        }
        Destroy(point.gameObject);
        
    }
}
