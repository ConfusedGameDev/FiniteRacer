using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CAR : MonoBehaviour
{
    // Start is called before the first frame update
    float verticalInput,horizontalInput;
    public Rigidbody rb;
    public float fwdMotor=500f;
    public float reverseMotor = 500f;
    public float sideMotor = 150f;
    bool canLoose,gameOver;
    public GameObject win, loose;
    public TMPro.TextMeshProUGUI speedUI, counterUI;
    public float gravityForce, antiGravityForce;
    public float maxRay = 4f;
    public bool hasStarted;
    public float meshMaxRotation = 20;
    public Transform shipMesh, leftPatrolPos,rightPatrolPos;
    public int counterStart = 3;

    public Transform getPatrolPosition()
    {
        if (Random.Range(-100, 100) > 0)
            return rightPatrolPos;
        return leftPatrolPos;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CountDown());
    }
    IEnumerator CountDown()
    {
        int delta = counterStart;
        {
            while (delta >= 0)
            {
                counterUI.text = delta.ToString();
                yield return new WaitForSeconds(1f);
                delta--;
            }
            counterUI.gameObject.SetActive(false);
            hasStarted = true;
            AddForwardForce();
        }
    }
    // Update is called once per frame
    void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        if (!canLoose && rb.velocity.magnitude > 0)
            canLoose = false;
        shipMesh.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, meshMaxRotation),
               Quaternion.Euler(0, 0,  -meshMaxRotation), (horizontalInput+1)/2f);
    }
    private void AddForwardForce()
    {
        rb.AddForce(Vector3.forward *  fwdMotor, ForceMode.Acceleration);
    }
    private void AddReverseForwardForce()
    {
        rb.AddForce(Vector3.forward * reverseMotor, ForceMode.Acceleration);
    }
    private void AddHorizontalForce()
    {
        rb.AddForce(Vector3.right * horizontalInput * sideMotor, ForceMode.Acceleration);

    }
    private void AddVerticalForce()
    {
        Ray r = new Ray(transform.position, -transform.up);
        RaycastHit hito;
        if(Physics.Raycast(r,out hito, maxRay))
        {
            rb.AddForce(Vector3.up * antiGravityForce, ForceMode.Acceleration);
        }

        
        

        

    }
    private void FixedUpdate()
    {
        if (gameOver||!hasStarted) return;
        AddHorizontalForce();
        if (canLoose && rb.velocity.magnitude<=0)
        {
           // loose.SetActive(true);
           // StartCoroutine(ReloadGame());
        }
        AddVerticalForce();
        speedUI.text = "SPEED: "+ rb.velocity.magnitude.ToString();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameOver) return;
        if (other.transform.CompareTag("PowerUp"))
        {
            AddForwardForce();
        }
        if (other.transform.CompareTag("PowerDown"))
        {
            AddReverseForwardForce();
        }
        else if (other.transform.CompareTag("Finish"))
        {
            win.SetActive(true);
            StartCoroutine(ReloadGame());
        }
        else if (other.transform.CompareTag("Loose"))
        {
            loose.SetActive(true);
            StartCoroutine(ReloadGame());
        }

    }
    IEnumerator ReloadGame()
    {
        gameOver = true;
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
