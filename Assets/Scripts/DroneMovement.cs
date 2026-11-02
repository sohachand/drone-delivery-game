using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;
    public float liftSpeed = 8f;
    public float minHeight = 0f;
    public float maxHeight = 20f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Just log speed here, actual movement happens in FixedUpdate
        float currentSpeed = rb.linearVelocity.magnitude;
        Debug.Log("Drone speed magnitude: " + currentSpeed);
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        Vector3 movement = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;

        float liftInput = 0f;
        if (Input.GetKey(KeyCode.E)) liftInput = 1f;
        if (Input.GetKey(KeyCode.Q)) liftInput = -1f;
        movement += Vector3.up * liftInput * liftSpeed * Time.fixedDeltaTime;

        Vector3 newPosition = rb.position + movement;
        newPosition.y = Mathf.Clamp(newPosition.y, minHeight, maxHeight);

        rb.MovePosition(newPosition);

        float rotationAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion newRotation = rb.rotation * Quaternion.Euler(0, rotationAmount, 0);
        rb.MoveRotation(newRotation);
    }
}