using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{
    [SerializeField]private Text timeScaleText;
    [SerializeField]private Image pauseImage;
    [SerializeField]private Sprite playSprite;
    [SerializeField]private Sprite stopSprite;

    private float tScale;
    void Start()
    {
        tScale = 1;
        timeScaleText.text = $"x 1";
        TogglePauseBtnImage(false);

    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            TogglePause();
        }
        if (Input.GetKeyDown(KeyCode.Minus)||Input.GetKeyDown(KeyCode.KeypadMinus)){
            ModifyTimeScale(-0.25f);
        }
        if (Input.GetKeyDown(KeyCode.Plus) ||Input.GetKeyDown(KeyCode.KeypadPlus)){
            ModifyTimeScale(0.25f);
        }
    }

    public void TogglePause()
    {
        var paused = Time.timeScale == 0 ;
        
        Time.timeScale = paused? tScale : 0;
        timeScaleText.text = $"x {Time.timeScale}";
        TogglePauseBtnImage(!paused);
    }

    public void ModifyTimeScale(float delta)
    {
        if( Time.timeScale+delta < 0 || Time.timeScale+delta > 3) return;
        if( Time.timeScale == 0 ) {
             TogglePauseBtnImage(false);
             tScale=0;
        }
        tScale+=delta;


        Time.timeScale = tScale;
        timeScaleText.text = $"x {tScale}";
    }

    private void TogglePauseBtnImage(bool pause){
        pauseImage.sprite = pause?playSprite:stopSprite;
    }
}
