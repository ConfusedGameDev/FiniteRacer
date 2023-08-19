using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class SpaceShipController : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxZAngle;
    public float yOffset;
    public float xOffset;
    public float maxXoffset = 1f;
    public float maxYoffset = 1f;
    public Vector2 maxZSpeed=new Vector2(0.01f,10f);
    public float zSpeedMultiplier = 1f;
    public float movementSpeed = 1f;
    Vector3 moveVector;
    public SplineAnimate splineAnimator;
    public Transform meshHolder;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveVector= new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),Input.GetKey(KeyCode.Space)?1:0);
        xOffset += moveVector.x * Time.deltaTime * movementSpeed;
        yOffset += moveVector.x * Time.deltaTime * movementSpeed;

        var angleDelta = (moveVector.x+ 1) / 2;
        var gasDelta = (moveVector.z + 1) / 2;

        if (splineAnimator)
        {
            splineAnimator.MaxSpeed = Mathf.Lerp( maxZSpeed.x,maxZSpeed.y,gasDelta) * zSpeedMultiplier ;
        }



        xOffset = Mathf.Min(Mathf.Max(-maxXoffset, xOffset), maxXoffset);
        yOffset = Mathf.Min(Mathf.Max(-maxYoffset, yOffset), maxYoffset);
        transform.localPosition = new Vector3(xOffset, yOffset, transform.localPosition.z);
        if(meshHolder)
        meshHolder.localRotation = Quaternion.Euler(meshHolder.localRotation.eulerAngles.x,
                                                   Mathf.Lerp(-maxZAngle, maxZAngle,angleDelta),
                                                   meshHolder.localRotation.eulerAngles.z);
    }
}
