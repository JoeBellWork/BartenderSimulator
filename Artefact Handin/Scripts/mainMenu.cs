using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    //function used for UI button press that closes simulation
    public void quitSim()
    {
        Application.Quit();
    }

    //function used for UI button press that enters the free roam scene
    public void freeRoam()
    {
        SceneManager.LoadScene(1);
    }   

}
