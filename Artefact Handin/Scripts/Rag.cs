using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rag : MonoBehaviour
{
    private bool grabebd = false;
    private Collider thisCollider;
    public Collider handCollider;
    public Transform hipHolder;
    public float speed = 5;


    // Start is called before the first frame update
    void Start()
    {
        thisCollider = this.GetComponent<Collider>();
    }

    // function that uses bool check to let system know the user is holding the rag.
    private void OnTriggerEnter(Collider other)
    {
        if (other == handCollider)
        {
            grabebd = true;
        }
    }

    //function that uses bool check to let system know the user isnt holding the rag anymore.
    private void OnTriggerExit(Collider other)
    {
        if(other == handCollider)
        {
            grabebd = false;
        }
    }

    // function that occurs when the rag isnt grabbed by the user. Will slowly lerp to the rang holder position on each frame.
    private void FixedUpdate()
    {
        if (grabebd == false)
        {
            this.gameObject.transform.position = Vector3.Lerp(transform.position, hipHolder.position, Time.deltaTime * speed);
        }
        
    }
}
