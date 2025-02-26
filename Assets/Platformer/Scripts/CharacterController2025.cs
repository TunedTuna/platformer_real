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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        show = false;
        manager = GetComponent<GameManager>();
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

        Collider c = GetComponent<Collider>();
        Vector3 startPoint = transform.position;
        float castDistance = c.bounds.extents.y;

        isGrounded = Physics.Raycast(startPoint, Vector3.down, castDistance);

        Color color = (isGrounded) ? Color.green : Color.red;
        if ((!show))
        {
            
            Debug.DrawLine(startPoint, startPoint + castDistance * Vector3.down, color, 0f, false);
        }


        //head
        Vector3 top= transform.position + new Vector3(0, c.bounds.extents.y, 0);
        if (!show) 
        {
            headButt = Physics.Raycast(top, Vector3.up, castDistance);
            Color color2 = (headButt) ? Color.yellow : Color.blue;
            Debug.DrawLine(top, top + castDistance * Vector3.up, color2, 0f, false);
        }



        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //apply force upward

            rb.AddForce(Vector3.up * jumpImpulse, ForceMode.VelocityChange);
        }


        else if (Input.GetKey(KeyCode.Space) && !isGrounded)
        {
            if(rb.linearVelocity.y>0)
            {
            rb.AddForce(Vector3.up * jumpBoostForce, ForceMode.Acceleration);
            }
        }

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
    }
}
