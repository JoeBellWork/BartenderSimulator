using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourDetect : MonoBehaviour
{
    public int pourThreshhold = 45;
    public Transform origin = null;
    public GameObject streamPrefab= null;
    public GameObject spillage;
    public Vector3 endPoint;
    public LayerMask pourIgnoreLayerMask;
    public GlassContents glassUI;
    public AudioManagerScript audioManager;
    public GameObject cork;

    private bool isPouring = false;
    private Stream currentStream = null;
    private bool pourCheck;
    private bool rayCheckTag;
    private bool isGlass;
    private bool canPour = true;


    // Update is called once per frame
    //checks for rotations in bottles and controls bool checks
    private void Update()
    {
        if(CalculateAngleX() >= pourThreshhold|| CalculateAngleX() <= -pourThreshhold)
        {
            pourCheck = true;
        }
        else if(CalculateAngleZ() >= pourThreshhold || CalculateAngleZ() <= -pourThreshhold)
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

            if(isPouring)
            {
                StartPour();
            }
            else
            {
                EndPour();
            }
        }
    }

    // a function that begins the pouring of the liquid, generates stream prefab by name of bottle/liquid, begins spillageCheck coroutine that acts as a timed pouring system.
    private void StartPour()
    {
        audioManager.soundPlay("WaterPour");
        currentStream = CreateStream();
        currentStream.Begin();
        if(canPour)
        {
            canPour = false;
            StartCoroutine(spillageCheck());
        }
    }

    private void EndPour()   // function that ends pouring system
    {
        audioManager.soundStop("WaterPour");
        currentStream.End();
        currentStream = null;
    }

    private float CalculateAngleX()  // provides rotation of bottle
    {     
        return origin.transform.rotation.x * Mathf.Rad2Deg;        
    }
    private float CalculateAngleZ()  // provides rotation of bottle
    {
        return origin.transform.rotation.z * Mathf.Rad2Deg;
    }



    // function that instanciates new stream object at the position of the bottle origin
    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }


    //raycast function to detect objects below bottle origin to provide information for pouring
    public void spillRay()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 2.0f, ~pourIgnoreLayerMask);
        if(hit.collider.gameObject.tag != "Glass")
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
        yield return new WaitForSeconds(0.25f);
        if(currentStream != null)
        {
            spillRay();
            if(rayCheckTag)
            {
                Instantiate(spillage, endPoint, Quaternion.identity);
            }
            if(isGlass)
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
}
