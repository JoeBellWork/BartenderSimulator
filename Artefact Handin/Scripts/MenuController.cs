using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public Canvas menu; //final validation menu for leaving the simulation while in free roam scene. 

    //function on object startup that sets object to disabled.
    private void Awake()
    {
        menu.enabled = false;
    }

    //function used for UI button press that closes simulation.
    public void quitSim()
    {
        Application.Quit();
    }

    //function used for UI button press that closes this UI menu.
    public void closeMenu()
    {
        menu.enabled = false;
    }

    //function used for UI button press that enables this UI menu.
    public void openMenu()
    {
        menu.enabled = true;
    }

    
}
