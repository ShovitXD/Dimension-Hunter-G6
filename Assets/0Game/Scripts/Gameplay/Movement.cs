using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPun
{
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float maxVelocityChange = 10f;

    private Vector2 input;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
    }

    private void FixedUpdate()
    {
        // Call CalculateMovement and apply the result to the Rigidbody
        Vector3 velocityChange = CalculateMovement(walkSpeed);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    Vector3 CalculateMovement(float speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= speed;

        Vector3 velocity = rb.linearVelocity;  // Corrected: use 'velocity' instead of 'linearVelocity'

        Vector3 velocityChange = targetVelocity - velocity;

        if (input.magnitude > 0.5f)
        {
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;  // Keep the Y velocity unaffected
        }

        return velocityChange;
    }
}
