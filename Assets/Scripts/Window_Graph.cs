using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Window_Graph : MonoBehaviour
{

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private float maxPtsCount;
    [SerializeField] private float xInterval;
    [SerializeField] private float yMaximum;
    [SerializeField] private Color graphColor;
    [SerializeField] private float lineWeight;


    private RectTransform graphContainer;

    private List<RectTransform> pts = new List<RectTransform>();
    private List<RectTransform> lines = new List<RectTransform>();
    float graphHeight;

    private void Awake()
    {
        graphContainer = GetComponent<RectTransform>();
        graphHeight = graphContainer.sizeDelta.y;
        List<int> valueList = new List<int>() { 0 };
        ShowGraph(valueList);
    }

    private void MoveRight()
    {
        foreach (var pt in pts)
        {
            pt.anchoredPosition += Vector2.right * xInterval;
        }
        foreach (var line in lines)
        {
            line.anchoredPosition += Vector2.right * xInterval;
        }

    }

    public void AddNewPoint(float value)
    {
        float xPosition = 0;
        float yPosition = value / yMaximum * graphHeight;
        GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
        pts.Add(circleGameObject.GetComponent<RectTransform>());
        CreateDotConnection(pts[pts.Count - 2].anchoredPosition, pts[pts.Count - 1].anchoredPosition);
        MoveRight();
        if (pts.Count > maxPtsCount)
        {
            Destroy(pts[0].gameObject);
            Destroy(lines[0].gameObject);
            pts.RemoveAt(0);
            lines.RemoveAt(0);
        }
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(List<int> valueList)
    {

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xInterval + i * xInterval;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null)
            {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
            pts.Add(circleGameObject.GetComponent<RectTransform>());

        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = graphColor;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, lineWeight);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, GetAngleFromVectorFloat(dir));
        lines.Add(gameObject.GetComponent<RectTransform>());
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }


    void Update()
    {
    }
}
