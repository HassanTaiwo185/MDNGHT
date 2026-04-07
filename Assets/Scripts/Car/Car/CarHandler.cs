using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    // Car rigid body that handles physics movement
    [SerializeField]
    Rigidbody rigidBody;

    // Visual car model that is use for rotation effect
    [SerializeField]
    Transform gameModel;

    // Forward acceleration force
    float accelerate = 3;

    // Brake force
    float brakeForce = 15;

    // Steering force
    float steer = 2;

    // Maximum sideways movement speed
    float maximumSteeringVelocity = 1;

    // Maximum forward speed
    float maximumAcceleratorVelocity = 30;

    // Player input which are in 2 dimension(x& y)
    Vector2 input = Vector2.zero;

    CarCollisionHandler collisionHandler;

    float roadHalfWidth = 1f;

    void Start()
    {
        collisionHandler = GetComponent<CarCollisionHandler>();

        // get road width automatically
        GameObject road = GameObject.FindGameObjectWithTag("Road");
        if (road != null)
        {
            roadHalfWidth = road.GetComponentInChildren<Renderer>().bounds.size.x / 2f;
        }

    }

    void Update()
    {
        // Tilt the car model visually based on side velocity
        gameModel.transform.rotation = Quaternion.Euler(0, rigidBody.velocity.x * 5, 0);
    }

    private void FixedUpdate()
    {

        // stop all car physics if crashed
        if (collisionHandler != null && collisionHandler.hasCrashed)
            return;


        // If y equal 1 accelerate car or move car forwaard
        if (input.y > 0)
        {
            AccelerateCar();
        }
        else
        {
            // Small drag when not accelerating
            rigidBody.drag = 0.3f;
        }

        // if y equal -1 brake the car or stop it
        if (input.y < 0)
        {
            BrakeCar();
        }

        // Handle left/right movement oof the car
        SteerCar();

        // Prevent car from going backward
        if (rigidBody.velocity.z <= 0)
        {
            rigidBody.velocity = Vector3.zero;
        }
    }

    // Handles forward acceleration of the car
    void AccelerateCar()
    {
        // Remove drag while accelerating the car
        rigidBody.drag = 0;

        // Stop accelerating if maximum speed reached
        if (rigidBody.velocity.z >= maximumAcceleratorVelocity)
        {
            return;
        }

        // Add forward force to the car i.e move car forward
        rigidBody.AddForce(rigidBody.transform.forward * accelerate * input.y);
    }

    // Handles braking the car
    void BrakeCar()
    {
        // If already going backward, stop braking
        if (rigidBody.velocity.z < 0)
        {
            return;
        }

        // Apply backward force to stop the car
        rigidBody.AddForce(rigidBody.transform.forward * brakeForce * input.y);
    }

    // Handles steering (left/right) of the car
    void SteerCar()
    {
        // If player is steering
        if (Mathf.Abs(input.x) > 0)
        {
            // Get current forward speed of the car
            float currentSpeed = rigidBody.velocity.z;

            // Minimum car speed the car can steer
            float steerAmount = currentSpeed / 5.0f;
            steerAmount = Mathf.Clamp01(steerAmount);

            // Apply side force to the car
            rigidBody.AddForce(rigidBody.transform.right * steer * input.x * steerAmount);

            // Limit sideways speed
            float currentSideSpeed = rigidBody.velocity.x / maximumSteeringVelocity;
            currentSideSpeed = Mathf.Clamp(currentSideSpeed, -1.0f, 1.0f);
            float newSideSpeed = currentSideSpeed * maximumSteeringVelocity;
            float forwardSpeed = rigidBody.velocity.z;

            // Apply limited velocity
            rigidBody.velocity = new Vector3(newSideSpeed, 0, forwardSpeed);

        }
        else
        {
            // If not steering, smoothly return to center
            float forwardSpeed = rigidBody.velocity.z;
            Vector3 targetVelocity = new Vector3(0, 0, forwardSpeed);
            rigidBody.velocity = Vector3.Lerp(rigidBody.velocity, targetVelocity, Time.fixedDeltaTime * 10); 
        }

        // clamp car position to road boundaries
        float roadLimit = roadHalfWidth - 0.5f; // 0.5f so car stays fully on road
        float clampedX = Mathf.Clamp(rigidBody.position.x, -roadLimit, roadLimit);
        rigidBody.position = new Vector3(clampedX, rigidBody.position.y, rigidBody.position.z);

    }

    // Called from input system to set movement input
    public void SetInput(Vector2 inputVector)
    {
        input = inputVector;
    }

    
}