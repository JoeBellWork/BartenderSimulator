using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer = null;
    private Vector3 targetPosition = Vector3.zero;
    private ParticleSystem splashParticle = null;
    private Coroutine pourRoutine = null;
    public LayerMask pourIgnoreLayerMask;

    //collects and sets varibles to this objects components. 
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
    }

    // sets position of start and end point
    private void Start()
    {
        MoveToPos(0, transform.position);
        MoveToPos(1, transform.position);
    }

    //set begin pour ienumerator to start stream
    public void Begin()
    {
        pourRoutine = StartCoroutine(BeginPour());
    }

    //the pouring ienumerator that sends start and end positions as well as animate between the two to show the liquid falling.
    private IEnumerator BeginPour()
    {
        while(gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();
            MoveToPos(0, transform.position);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }        
    }

    public void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    //function that ends the pour and animates the liquid to show the last of it being poured
    private IEnumerator EndPour()
    {
        while(!hasReachedPos(0, targetPosition))
        {
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }
        Destroy(gameObject);
    }

    //function that returns a point in world space to act as end point, found by using a raycast
    public Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);

        Physics.Raycast(ray, out hit, 2.0f, ~pourIgnoreLayerMask);

        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);
        return endPoint;
    }

    //sets line renderer position.
    private void MoveToPos(int index, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(index, targetPosition);
    }

    //animates line renderer to flow to end point.
    private void AnimateToPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 2);
        lineRenderer.SetPosition(index, newPosition);
    }


    // bool check that checks to see if end position has been found.
    private bool hasReachedPos(int index, Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);

        return currentPosition == targetPosition;
    }
}
