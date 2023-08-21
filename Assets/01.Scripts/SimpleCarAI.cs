using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCarAI : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public float horizontalOffset = 2f;
    public int side=1;
    public CarSimpleController carController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x<target.position.x+horizontalOffset*side)
        {
            carController.handling = 1f;
        }
        else if(transform.position.x>target.position.x+(horizontalOffset*2)*side)
        {
            carController.handling = -1f;
        }
        else
        {
            carController.handling = 0;
        }

        if(target.transform.position.z>transform.position.z)
        {
            carController.gas = 1f;
        }
        else
        {
            carController.gas = -1f;
        }


    }
}
