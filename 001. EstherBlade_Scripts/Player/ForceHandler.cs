using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceHandler : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField] float dampingDrag = 0.3f;

    Vector3 dampingVelociy;
    Vector3 currentForce;
    float verticalVelocity;

    public Vector3 Movement => currentForce + Vector3.up * verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = 0f;  // Phsics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        currentForce = Vector3.SmoothDamp(currentForce, Vector3.zero, ref dampingVelociy, dampingDrag);
    }

    public void Reset()
    {
        currentForce = Vector3.zero;
        verticalVelocity = 0f;
    }

    public void AddForce(Vector3 force)
    {
        currentForce += force;
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}
