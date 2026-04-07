using UnityEngine;

public class NPCCar : MonoBehaviour
{
    Rigidbody playerRigidBody;

    void Start()
    {
        playerRigidBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    void Update()
    {
    
        float npcSpeed = playerRigidBody.velocity.z * 0.05f;

        // minimum speed of 0.5 so very slow
        npcSpeed = Mathf.Max(npcSpeed, 0.5f);

        // move toward player
        transform.Translate(Vector3.back * npcSpeed * Time.deltaTime);
    }
}