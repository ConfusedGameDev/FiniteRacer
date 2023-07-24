using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EnemyPatroller : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(0, 2)]
    public float delta;
    public Transform target1, target2;
    void Start()
    {
        delta = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target1 || !target2) return;
        if(delta<1f)
        {
            transform.position = Vector3.Lerp(transform.position, target1.position,delta);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, target2.position,delta-1f);
        }
        delta += Time.deltaTime;
    }
}
