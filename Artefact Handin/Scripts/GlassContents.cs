using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlassContents : MonoBehaviour
{
    //declare varibles
    public Collider glassParent; //glass collider.
    public GameObject UIcanvas; //glass UI.
    public Text drinkName, contentFill1, contentFill2, clean; //UI text objects' text properties.
    public string nameString; //a overwritable string varible used to rewrite drink names.
    public int fillINT1, fillINT2; // overwritable int varibles used to rewrite drink fill amounts.
    public bool isClean; //bool check to see if glass is clean.
    public bool isCocktail; //bool check to see if drink is cocktail.
    public Material[] glassMats; //array of materials to change if the glass is clean or dirty.
    private int targetFill = 50; //target int so that drinks should aim to be fillled to this amount.

    private bool inSocket, inHand; // bool that checks if object is being held or is in a socket




    // Start is called before the first frame update
    //set glass as a fresh item with no liquids inside
    void Start()
    {
        UIcanvas.SetActive(false);
        nameString = "Empty";
        fillINT1 = 0;
        fillINT2 = 0;
        isClean = true;
        isCocktail = false;        
        InitialiseDrinkStats();
    }


    // a function that resents glass UI values depending on multiple situations
    public void InitialiseDrinkStats()
    {
        if(nameString != "Ruined")
        {
            drinkName.text = "Drink: " + nameString;
            contentFill1.text = "Volume: " + fillINT1 + "ML /" + targetFill + "ML";
            colourChange(fillINT1, contentFill1);

            if (isCocktail)
            {
                contentFill2.text = "Volume: " + fillINT2 + "ML /" + targetFill + "ML";
                colourChange(fillINT2, contentFill2);
            }
            else
            {
                contentFill2.text = "";
            }
            if (isClean)
            {
                clean.text = "Glass is Clean";
                glassParent.gameObject.GetComponent<MeshRenderer>().material = glassMats[0];
            }
            else
            {
                clean.text = "Glass is Dirty";
                glassParent.gameObject.GetComponent<MeshRenderer>().material = glassMats[1];
            }

            if(fillINT1 >= targetFill + 10 || fillINT2 >= targetFill + 10)
            {
                nameString = "Ruined";
                drinkName.text = "Drink: " + nameString;
                contentFill1.text = "Glass Over flowing";
                contentFill1.color = Color.red;
                contentFill2.text = "Bin glass and start again";
                contentFill2.color = Color.red;
                clean.text = "Glass is Dirty";
                isClean = false;
                glassParent.gameObject.GetComponent<MeshRenderer>().material = glassMats[1];
            }
        }
        else
        {
            drinkName.text = "Drink: " + nameString;
            contentFill1.text = "Wrong ingredients";
            contentFill1.color = Color.red;
            contentFill2.text = "Bin glass and start again";
            contentFill2.color = Color.red;
            clean.text = "Glass is Dirty";
            isClean = false;
            glassParent.gameObject.GetComponent<MeshRenderer>().material = glassMats[1];           
           
        }
        


    }


    //a function that controls the colour of the Glass UI depending on how close the drink is to being filled
    private void colourChange(int C, Text T)
    {
        if (C >= targetFill + 10)
        {
            T.color = Color.red;
        }
        else if (C == targetFill + 5 || C == targetFill - 5)
        {
            T.color = Color.yellow;
        }
        else if (C == targetFill)
        {
            T.color = Color.green;
        }
        else if (C <= targetFill - 10)
        {
            T.color = Color.red;
        }
    }






    // function that activates when an object with a collider enters the space of an object with this script attached.
    //checks for the objects layer and tag. If it has the hand layer or coaster tag, the UI canvas is set to activate.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            inHand = true;
            UIcanvas.SetActive(true);
        }
        if(other.gameObject.tag == "Coaster")
        {
            inSocket = true;
            UIcanvas.SetActive(true);
        }

    }


    //reverse of function above, deactivates the canvas if object with hand layer or coaster tag leaves this objects space.
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 11)
        {
            inHand = false;
            if(!inHand && !inSocket)
            {
                UIcanvas.SetActive(false);
            }            
        }
        if (other.gameObject.tag == "Coaster")
        {
            inSocket = false;
            if (!inHand && !inSocket)
            {
                UIcanvas.SetActive(false);
            }
        }
    }

    
}
