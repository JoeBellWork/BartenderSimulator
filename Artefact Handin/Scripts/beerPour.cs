using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beerPour : MonoBehaviour
{
    // declare public and private varibles

    public Transform origin = null; //origin of pour
    public GameObject streamPrefab = null; //specific prefab for liquid
    public GameObject spillage; // spillage prefab for mistakes
    public Vector3 endPoint; // end point for line renderer
    public LayerMask pourIgnoreLayerMask; //what layers are ignored by the line renderer
    public GlassContents glassUI; // glass ui

    private bool isPouring = false; //bool check to trigger pouring
    private Stream currentStream = null; //current stream prefab being used
    private bool pourCheck; // bool check used alongside is pouring as a step validation
    private bool rayCheckTag; // bool check to see if raycast has hit object
    private bool isGlass; // bool check to see if ray has collided with glass
    private bool canPour = true; //final bool check for pourin system that prevents multiple pouring of the same bottle






    // a function that begins the pouring of the liquid, generates stream prefab by name of bottle/liquid, begins spillageCheck coroutine that acts as a timed pouring system.
    private void StartPour()
    {
        currentStream = CreateStream();
        currentStream.Begin();
        if (canPour)
        {
            canPour = false;
            StartCoroutine(spillageCheck());
        }
    }

    // function that ends pouring system
    private void EndPour()
    {
        currentStream.End();
        currentStream = null;
    }

    // function that instanciates new stream object at the position of the bottle origin
    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, origin.transform);
        return streamObject.GetComponent<Stream>();
    }



    //raycast function to detect objects below bottle origin to provide information for pouring
    public void spillRay()
    {
        RaycastHit hit;
        Ray ray = new Ray(origin.transform.position, Vector3.down);

        Physics.Raycast(ray, out hit, 2.0f, ~pourIgnoreLayerMask);


        if (hit.collider.gameObject.tag != "Glass")
        {
            isGlass = false;
            if (hit.collider.gameObject.tag == "Spill" || hit.collider.gameObject.tag == "Coaster")
            {
                rayCheckTag = false;
            }
            else
            {
                rayCheckTag = true;
            }
        }
        else
        {
            rayCheckTag = false;
            isGlass = true;
            glassUI = hit.collider.gameObject.GetComponent<GlassContents>();
        }
        endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);
    }




    // Semi-self contained ienumerator that collects all information provided above so that is may perform bahavior basedd on the location of the endpoint for pouring the liquid, whether it creates spillages or fills a glass.
    private IEnumerator spillageCheck()
    {
        yield return new WaitForSeconds(0.5f);
        if (currentStream != null)
        {
            spillRay();
            if (rayCheckTag)
            {
                Instantiate(spillage, endPoint, Quaternion.identity);
            }
            if (isGlass)
            {
                Debug.Log("Glass");
                if (glassUI.nameString == "Empty" || glassUI.nameString == this.gameObject.name)
                {
                    glassUI.nameString = this.gameObject.name;
                    glassUI.fillINT1 = glassUI.fillINT1 + 5;
                    glassUI.InitialiseDrinkStats();
                }
                else if (glassUI.nameString == "Beer" & this.gameObject.name == "Scotch" || glassUI.nameString == "Scotch" && this.gameObject.name == "Beer" || glassUI.nameString == "Boiler Maker" & this.gameObject.name == "Scotch" || glassUI.nameString == "Boiler Maker" && this.gameObject.name == "Beer")
                {
                    glassUI.nameString = "Boiler Maker";
                    glassUI.isCocktail = true;

                    if (this.gameObject.name == "Scotch")
                    {
                        glassUI.fillINT2 = glassUI.fillINT2 + 5;
                    }
                    else
                    {
                        glassUI.fillINT1 = glassUI.fillINT1 + 5;
                    }
                    glassUI.InitialiseDrinkStats();
                }



                else if (glassUI.nameString == "Bourbon" & this.gameObject.name == "Rum" || glassUI.nameString == "Rum" && this.gameObject.name == "Bourbon" || glassUI.nameString == "Admiral Schley" & this.gameObject.name == "Rum" || glassUI.nameString == "Admiral Schley" && this.gameObject.name == "Bourbon")
                {
                    glassUI.nameString = "Admiral Schley";
                    glassUI.isCocktail = true;

                    if (this.gameObject.name == "Bourbon")
                    {
                        glassUI.fillINT2 = glassUI.fillINT2 + 5;
                    }
                    else
                    {
                        glassUI.fillINT1 = glassUI.fillINT1 + 5;
                    }
                    glassUI.InitialiseDrinkStats();
                }

                else
                {
                    glassUI.nameString = "Ruined";
                    glassUI.InitialiseDrinkStats();
                }

            }
            StartCoroutine(spillageCheck());
        }
        canPour = true;
    }






    // Update is called once per frame
    //checks for rotations in bottles and controls bool checks
    void Update()
    {
        if (CalculateAngleZ() <360 && CalculateAngleZ() > 300)
        {
            pourCheck = true;
        }
        else
        {
            pourCheck = false;
        }


        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;

            if (isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }

   

    // provides rotation of bottle
    private float CalculateAngleZ()
    {
        return this.gameObject.transform.rotation.eulerAngles.z;
    }
}
