using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHandler : MonoBehaviour
{
    public CarSimpleController playerController;
    public float speedUpMultiplier, speedUpDuration, speedUpForce;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(playerController && other.CompareTag("PowerUp"))
        {
            playerController.addForce(speedUpForce);
           // StartCoroutine(speedUpPowerUp());
        }
    }
    public IEnumerator speedUpPowerUp()
    {
        var oldSpeed = playerController.maxTorque;
        playerController.maxTorque *= speedUpMultiplier;
        yield return new WaitForSeconds(speedUpDuration);
        playerController.maxTorque = oldSpeed;
    }
}
