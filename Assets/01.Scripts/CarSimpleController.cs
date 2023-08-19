using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSimpleController : MonoBehaviour
{
    [System.Serializable]
    public struct wheelData
    {
        public WheelCollider wCollider;
        public bool useTorque;
        public bool useMotor;
    }
    public List<wheelData> wheels = new List<wheelData>();
    public float gas, handling,maxAngle,maxTorque, antirollForce;
    Rigidbody rb;
    public bool useAntiroll;
    public Transform shipMesh;
    public float maxShipAngle, maxShipBounce, shipBounceSpeed;
    public float initialShipYpos;
    public TMPro.TextMeshProUGUI speedLabel;

    Vector3 resetPosition;
    Quaternion resetQuaternion;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        resetPosition = transform.position;
        resetQuaternion = transform.rotation;
        initialShipYpos = shipMesh ? shipMesh.transform.localPosition.y : 0;
    }

    public void resetPlayer()
    {
        rb.velocity = Vector3.zero;
        foreach (var wheel in wheels)
        {
            wheel.wCollider.motorTorque = 0;
        }
        transform.position = resetPosition;
        transform.rotation = resetQuaternion;
    }



    // Update is called once per frame
    void Update()
    {
        gas = Input.GetAxis("Vertical");
        handling= Input.GetAxis("Horizontal");
        if(shipMesh)
        {
            shipMesh.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(-maxShipAngle, maxShipAngle, (handling + 1) / 2f));
            shipMesh.transform.localPosition = new Vector3(0, Mathf.Lerp(initialShipYpos-maxShipBounce, initialShipYpos+ maxShipBounce,Mathf.PingPong( Time.time* shipBounceSpeed, 1f)), 0);
        }
        if (rb && speedLabel)
            speedLabel.text ="Speed :"+ rb.velocity.magnitude.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
            resetPlayer();
    }

    private void FixedUpdate()
    {
        foreach (var wheel in wheels)
        {
            if(wheel.wCollider && wheel.useMotor)
            {
                wheel.wCollider.motorTorque = gas*maxTorque;
            }
            if (wheel.wCollider && wheel.useTorque)
            {
                wheel.wCollider.steerAngle= Mathf.Lerp(-maxAngle,maxAngle, (handling+1)/2f);
            }
        }
        if (useAntiroll)
            calculateAntiRoll();
    }
    public void addForce(float force)
    {
        if (rb)
            rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }
    public void calculateAntiRoll()
    {
        if (!rb || wheels.Count<2) return;
        WheelHit hit;
        float travelL = 1f;
        float travelR = 1f;
        var wheelL = wheels[0].wCollider;
        var wheelR = wheels[1].wCollider;
        bool groundedL = wheelL.GetGroundHit(out hit);
        if(groundedL)
        {
            travelL = (-wheelL.transform.InverseTransformPoint(hit.point).y - wheelL.radius) / wheelL.suspensionDistance;
        }
        bool groundedR = wheelR.GetGroundHit(out hit);
        if (groundedR)
        {
            travelR = (-wheelR.transform.InverseTransformPoint(hit.point).y - wheelR.radius) / wheelR.suspensionDistance;
        }
        if(groundedL)
        {
            rb.AddForceAtPosition(wheelL.transform.up * -(travelL - travelR) * antirollForce, wheelL.transform.position);

        }
        if (groundedR)
        {
            rb.AddForceAtPosition(wheelR.transform.up * -(travelL - travelR) * antirollForce, wheelR.transform.position);
        }

    }
}
