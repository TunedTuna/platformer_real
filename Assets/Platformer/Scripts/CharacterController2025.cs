using System.Collections;
using UnityEditor.PackageManager;
using UnityEngine;

public class CharacterController2025 : MonoBehaviour
{
    Rigidbody rb;
    public float acceleration = 10f;
    public float maxSpeed = 10f;
    public float jumpImpulse = 8f;
    public float jumpBoostForce = 8f;


    

    public GameManager manager;

    [Header("debugg stuff")]
    public bool isGrounded;
    public bool headButt;
    public bool show;
    private bool isTriggered = false;
    private bool canRaycast = true;
    private float test;
    private float lastGroundedY;

    Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        show = false;
        animator = GetComponent<Animator>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontalAmount = Input.GetAxis("Horizontal");
        rb.linearVelocity += Vector3.right * (horizontalAmount * Time.deltaTime * acceleration);

        float horizontalSpeed = rb.linearVelocity.x;
        horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);

        Vector3 newVelocity = rb.linearVelocity;
        newVelocity.x = horizontalSpeed;

        rb.linearVelocity = newVelocity;
        //also clamp vertical velocity
        float verticalSpeed= rb.linearVelocity.y;
        verticalSpeed = Mathf.Clamp(verticalSpeed, -maxSpeed, jumpImpulse);
        newVelocity.y = verticalSpeed;
        rb.linearVelocity= newVelocity;

        Collider c = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        //feet======================================================================================================
        float castDistance = c.bounds.extents.y;

        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);

        Color color = (isGrounded) ? Color.green : Color.red;
            
        Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false);


      


        //head======================================================================================================
        Vector3 top = transform.position + new Vector3(0, c.bounds.extents.y, 0);

        
        headButt = Physics.Raycast(top, Vector3.up,out RaycastHit headHit, castDistance);
        Color color2 = (headButt) ? Color.yellow : Color.blue;
        Debug.DrawLine(top, top + castDistance * Vector3.up, color2, 0f, false);


            if (headButt && !isGrounded && headHit.transform.gameObject.CompareTag("Brick"))
            {

                GameObject block = headHit.transform.gameObject;
                manager.breakBrick(block);

                //Debug.Log("Break!");
                //Debug.Log(brick);
            }
            if (headButt && !isGrounded && headHit.transform.gameObject.CompareTag("Question") && !isTriggered && canRaycast)
            {

                GameObject block = headHit.transform.gameObject;
                manager.exhaustQuestion(block.transform);
                StartCoroutine(ResetTriggerAfterDelay(0.5f));
                isTriggered = true;
                canRaycast = false;

                //Debug.Log("Break!");
                //Debug.Log(brick);
            }


        test = 0;
        //inputs======================================================================================================
        //jumpin
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //apply force upward

            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }


        else if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            //TODO: if holding down jump, increment number up  to max Jump impule, else 

            //class code
            if (rb.linearVelocity.y > 0)
            {
                test += Time.deltaTime;
                Debug.Log($"test:{test}");
                rb.AddForce(Vector3.up * jumpBoostForce, ForceMode.Acceleration);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space) && !isGrounded)
        {
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.3f, rb.linearVelocity.z);
            }
        }


        //turnin
        if (horizontalAmount == 0) 
        {
            Vector3 decayVelocity = rb.linearVelocity;
            decayVelocity.x *= 1f - Time.deltaTime*2f;
            rb.linearVelocity= decayVelocity;
        }
        else
        {
            float yawRotation = (horizontalAmount > 0f) ? 90f : -90f;
            Quaternion rotation = Quaternion.Euler(0f, yawRotation, 0f);
            transform.rotation = rotation;
        }

        UpdateAnimation();
    }

    IEnumerator ResetTriggerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset the flag to allow future triggers
        isTriggered = false;

        // Optionally, you can also re-enable the raycast here if needed
        canRaycast = true;
    }

    void UpdateAnimation()
    {
        animator.SetFloat("Speed",Mathf.Abs(rb.linearVelocity.x));
        animator.SetBool("Jumping", !isGrounded);
    }


    }
