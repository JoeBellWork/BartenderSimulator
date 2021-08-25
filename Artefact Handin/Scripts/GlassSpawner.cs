using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSpawner : MonoBehaviour
{
    public Transform glassSocket; //varible of the glass socket location in world space.
    public GameObject glass; // prefab of glass object.

    //function to spawn glass at the glass socket location to be held by the socket.
    private void spawnGlass()
    {
        Instantiate(glass, glassSocket.position, Quaternion.identity);
    }

    // when another item enters this objects space, spawn a glass object.
    private void OnTriggerEnter(Collider other)
    {
        spawnGlass();
    }

    
}
