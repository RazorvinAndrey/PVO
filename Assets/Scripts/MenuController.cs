using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
       
    private void Update()
    {

    }

    public void RunScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
