using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class spillHealth : MonoBehaviour
{
    public int health;
    public Color damageFlash, original;
    public Collider rag;
    public SpillTable spillTable;

    private Material thisMat;
    public GameObject UICanvas;
    public Slider slider;
    public Gradient gradient;
    public Image fillImage;
    public Text text;


    //assign the values for spillage health and colour.
    void Start()
    {        
        gradient.Evaluate(1f);
        UICanvas.SetActive(false);
        rag = GameObject.Find("Rag").GetComponent<Collider>();
        health = 100;
        thisMat = this.GetComponent<MeshRenderer>().material;
        slider.maxValue = health;
        slider.value = health;

        spillTable = transform.parent.parent.gameObject.GetComponent<SpillTable>();
        text.text = "Spillage: " + health + "%";
    }





    //open spillage UI when being interacted with by detecting collider. Reduce spillage health on each interaction and delete if health reaches 0.

    private void OnTriggerEnter(Collider other)
    {
        if(other == rag)
        {
            UICanvas.SetActive(true);
            slider.value = health;
            if (health > 25)
            {
                health -= 25;
                thisMat.color = damageFlash;
                slider.value = health;
               fillImage.color = gradient.Evaluate(slider.normalizedValue);
                text.text = "Spillage: " + health + "%";

            }
            else
            {
                if(spillTable != null)
                {
                    for (int i = 0; i < spillTable.spillZone.Length; i++)
                    {
                        if (this.transform.parent.gameObject == spillTable.spillZone[i])
                        {
                            spillTable.hasSpill[i] = false;
                        }
                    }
                    Destroy(this.gameObject);
                }
                else 
                {
                    Destroy(this.gameObject);
                }
                
            }
        }
    }





    // close the Spillage UI when not being interacted with.

    private void OnTriggerExit(Collider other)
    {
        if(other == rag)
        {
            UICanvas.SetActive(false);
            thisMat.color = original;
        }
    }
}
