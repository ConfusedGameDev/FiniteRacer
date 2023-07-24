using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FollowObject : MonoBehaviour
{
    public bool followPos, followRot;
    public Transform parent;
    public Vector3 posOffset;
    public Quaternion rotOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(parent)
        {
            if(followRot)
            {
                transform.rotation = parent.rotation * rotOffset;
            }
            if(followPos)
            {
                transform.position = parent.position + posOffset;
            }
        }
    }
}
