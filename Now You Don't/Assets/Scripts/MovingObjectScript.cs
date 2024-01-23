
/**
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MovingObjectScript : MonoBehaviour
{
    public float speed;
    public bool bounce = false;

    //The starting position
    private Vector3 originalPos;

    //The ending position
    public Vector3 destination;

    //the distance between point originalPos and the Destination
    private float journeyLength;
    private float startTime;


    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.localPosition;
        startTime = Time.time;
        journeyLength = Vector3.Distance(originalPos, destination);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (transform.localPosition == destination) { BounceOnEnd(); }

        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        transform.localPosition = Vector3.Lerp(originalPos, destination, fractionOfJourney);

        
    }


    private void BounceOnEnd() 
    {
        if (bounce) 
        {
            if(this.transform.localPosition == destination) 
            {
                startTime = Time.time;
                Vector3 temp = originalPos;
                destination = temp;
                originalPos = transform.localPosition;

            }
        }
    }

}
**/
//POSSIBLE VERSION USING NAV MESH
 using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MovingObjectScript : MonoBehaviour
{
    public List<Transform> waypoints;
    private NavMeshAgent _agent;
    [SerializeField]
    private int currentTarget;
    private bool _reverse;
    [SerializeField]
    private bool _targetreached;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        
        _agent = GetComponent<NavMeshAgent>();
        
        
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        _agent.destination = waypoints[currentTarget].position;
        float distance = Vector2.Distance(transform.position, waypoints[currentTarget].position);
        if(distance < 1f && _targetreached==false )
        {
            _targetreached = true;
            if(_reverse == false) 
            {
                currentTarget++;
            }
            else
            {
                currentTarget--;
            }

            if(currentTarget==waypoints.Count-1) 
            {
                _reverse = true;
            }
            else if (currentTarget == 0)
            {
                _reverse= false;
            }
        }
        else if (distance < 1f && _targetreached == true)
        {
            StartCoroutine(WaitBeforeMoving());
        }

        
    }

    IEnumerator WaitBeforeMoving()
    {
        if(currentTarget == waypoints.Count-1 || currentTarget == 0) 
        {
            yield return new WaitForSeconds(5f);
            _targetreached = false;
        }
        else
        { 
            _targetreached= false;
        }
    }

}
 
// **/