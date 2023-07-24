using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyPatrol : MonoBehaviour
{
    public enum chaseState
    {
        aligningWithShip,
        chasingship,
        aligningwithTarget,
        chasingTarget,
        smash
    }
    public chaseState currentState= chaseState.aligningWithShip;
    Rigidbody rb;
    public float horizontalForce, aligningForce,smashForce;
    public ForceMode hfm, vfm;
    public CAR playerShip;
    public Transform  chargePostion;
    public float minAligningDistance,maxAlignDistance, minChaseDistance,maxChaseDist, minChargeDistance, minTargetDist;
    public float fwdMotor;
    public Transform shipMesh;

    public float distToship,distToChargepos;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        chargePostion = playerShip.getPatrolPosition();
    }
    public void FixedUpdate()
    {
        distToship = Vector3.Distance(transform.position, playerShip.transform.position);
        if (currentState== chaseState.aligningWithShip)
        {
            if (transform.position.x > playerShip.transform.position.x)
            {
                rb.AddForce(aligningForce * -transform.right, hfm);
            }
            else
            {
                rb.AddForce(aligningForce * transform.right, hfm);
            }
            if(Mathf.Abs(playerShip.transform.position.x-transform.position.x)<minAligningDistance)
            {
                 
                currentState = chaseState.chasingship;
            }
        }
        else if (currentState== chaseState.chasingship)
        {
            
           
            if( distToship >minChaseDistance)
            {
                rb.AddForce(transform.forward * fwdMotor, vfm);
                if (Mathf.Abs(playerShip.transform.position.x - transform.position.x) > maxAlignDistance)
                {
                    currentState = chaseState.aligningWithShip;
                }
            }
            else 
            {
                
                currentState = chaseState.aligningwithTarget;
            }
        }
        else if(currentState== chaseState.aligningwithTarget)
        {
            if (transform.position.x > chargePostion.transform.position.x)
            {
                rb.AddForce(aligningForce * -transform.right, hfm);
            }
            else
            {
                rb.AddForce(aligningForce * transform.right, hfm);
            }
            if (Mathf.Abs(chargePostion.transform.position.x - transform.position.x) < minAligningDistance)
            {

                currentState = chaseState.chasingTarget;
            }
            if(distToship>maxChaseDist)
            {
                currentState = chaseState.aligningWithShip;
            }
        }
        else if (currentState == chaseState.chasingTarget)
        {
            distToChargepos = Vector3.Distance(transform.position,  chargePostion.position);

            if (distToChargepos > minTargetDist)
            {
                rb.AddForce(transform.forward * fwdMotor, vfm);
                if (Mathf.Abs(chargePostion.transform.position.x - transform.position.x) > maxAlignDistance)
                {
                    currentState = chaseState.aligningwithTarget;
                }
            }
            else
            {

                currentState = chaseState.smash;
            }
        }
        else if(currentState== chaseState.smash)
        {
            var dir = transform.position.x > playerShip.transform.position.x ? -1 : 1;
            rb.AddForce(smashForce * transform.right*dir, hfm);
        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
