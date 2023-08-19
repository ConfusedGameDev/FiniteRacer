using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;

 public class SplineSampler : MonoBehaviour
{
    [System.Serializable]
    public struct SamplePosition
    {
        public float3 center;
        public float3 right;
        public float3 left;

        public SamplePosition(float3 center, float3 right, float3 left)
        {
            this.center = center;
            this.right = right;
            this.left = left;
        }
    }
    public List<SamplePosition> samples= new List<SamplePosition>();
    [SerializeField]
    public SplineContainer splineContainer;
    [SerializeField]
    private int splineIndex;
    [SerializeField]
    [Range(0f,1f)]
    private float time;
    [SerializeField]
    [Range(0.01f, 1f)]
    private float timeOffset;

    public float maxWidth;
   
    public int resolution = 10;
    public float debugSphereSize = 0.25f;

    public bool debug;

    public Transform FollowerObject, followerChild;
    public float yOffset;
    public float xOffset;
    public float maxXoffset = 1f;
    public float maxYoffset = 1f;
    public float speed = 0.01f;
    public float movementSpeed = 1f;
    Vector3 lastOffset = Vector3.zero;
    Vector3 playerTarget;
    public Vector2 movementVector;
    public float maxZAngle = 30;
    // Update is called once per frame
    void Update()
    {
        if (FollowerObject)
        {
            movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            
            playerTarget = getSplinePosition(time + timeOffset > 1f ? timeOffset : time + timeOffset)
                ;
            /*+
                                  FollowerObject.right * xOffset +
                                  FollowerObject.up * yOffset;
            */
            
            /*+
                                      FollowerObject.right * xOffset + 
                                      FollowerObject.up * yOffset;
            */


            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                xOffset += Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
                yOffset += Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed;
                var angleDelta = (Input.GetAxis("Horizontal") + 1) / 2;

                xOffset = Mathf.Min(Mathf.Max(-maxXoffset, xOffset), maxXoffset);
                yOffset = Mathf.Min(Mathf.Max(-maxYoffset, yOffset), maxYoffset);
                if (followerChild)
                {
                   
                    followerChild.localPosition = Vector3.right * xOffset +
                                          Vector3.up * yOffset;

                }

            }
            FollowerObject.LookAt(playerTarget);
            FollowerObject.position = getSplinePosition(time);
            time = Mathf.Clamp01(time+ speed * Time.deltaTime);
            if (time >= 1)
                time = 0;

        }
        if (Application.isPlaying) return;
        samples.Clear();

        if (!splineContainer) return;
        for (int i = 0; i < resolution; i++)
        {
            float3 position, tangent, upVector;
            float3 p1, p2;
            splineContainer.Evaluate(splineIndex, i/(float)resolution, out position, out tangent, out upVector);
            float3 right = Vector3.Cross(tangent, upVector).normalized * maxWidth / 2f;
            p1 = position + (right * maxWidth / 2f);
            p2 = position + (-right * maxWidth / 2f);
            position.y = 0;
            p1.y = 0;
            p2.y = 0;
            samples.Add(new SamplePosition(position, p1, p2));

        }

    }
    public Vector3 getSplinePosition(float t)
    {
        float3 pos=  Vector3.zero;
        float3 tang = Vector3.zero;
        float3 upV = Vector3.zero;
        splineContainer.Evaluate(splineIndex, t, out pos, out tang, out upV);
        Vector3 finalPos = pos;
        return  finalPos;
    }    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!debug) return;
        foreach (var sample in samples)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(sample.center, debugSphereSize);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(sample.left, debugSphereSize);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(sample.right, debugSphereSize);

            Gizmos.DrawLine(sample.left, sample.right) ;
        }
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(getSplinePosition(time),debugSphereSize);

        Gizmos.color = Color.red;

        Gizmos.DrawSphere(playerTarget, debugSphereSize);
        Gizmos.DrawLine(FollowerObject.position, playerTarget);

    }
#endif

    
}
