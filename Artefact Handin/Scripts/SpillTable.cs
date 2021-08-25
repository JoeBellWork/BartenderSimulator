using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpillTable : MonoBehaviour
{
    public GameObject[] spillZone;
    public bool[] hasSpill;
    private bool found;
    public GameObject spillage;


    private void Awake()
    {
        found = false;
        hasSpill = new bool[spillZone.Length];
    }

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < hasSpill.Length; i++)
        {
            hasSpill[i] = false;
        }
        StartCoroutine(timer());
    }


    //random number generator that acts as a probability chance for a spillage spawning at a table where a spillzone is currently free.
    private void randomNumber()
    {
        found = false;
        int J = Random.Range(0, 5);
        if(J == 0)
        {
            for(int i = 0; i < hasSpill.Length; i++)
            {
                if (hasSpill[i] == false && found == false)
                {
                    hasSpill[i] = true;
                    found = true;
                    Instantiate(spillage, spillZone[i].transform, false);
                }
            }
        }
    }

    //Ienumerator function that counters before another attempt at spillage occurs
    private IEnumerator timer()
    {
        randomNumber();
        yield return new WaitForSeconds(10);
        StartCoroutine(timer());
    }

}
