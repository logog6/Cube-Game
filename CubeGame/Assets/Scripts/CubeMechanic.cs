using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMechanic : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float drag = 0.5f;
    public float terminalRotationSpeed = 25.0f;
    public VirtualJoystick movelJoystick;

    public float boostSpeed = 5.0f;
    public float boostCooldown = 2.0f;
    private float lastBoost;

    private Rigidbody contorller;
    private Transform camTransform;

    private void Start()
    {
        lastBoost = Time.time - boostCooldown;

        contorller = GetComponent<Rigidbody>();
        contorller.maxAngularVelocity = terminalRotationSpeed;
        contorller.drag = drag;

        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 dir = Vector3.zero;

        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if (dir.magnitude > 1)
            dir.Normalize();

        if (movelJoystick.InputDirection != Vector3.zero)
        {
            dir = movelJoystick.InputDirection;
        }

        //Odwrócenie kierunku wektora z kamerą
        Vector3 rotatedDir = camTransform.TransformDirection(dir);
        rotatedDir = new Vector3(rotatedDir.x,0,rotatedDir.z);
        rotatedDir = rotatedDir.normalized * dir.magnitude;

        contorller.AddForce(rotatedDir * moveSpeed);
    }

    public void Boost()
    {
        if (Time.time - lastBoost > boostCooldown)
        {
            contorller.AddForce(contorller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);
        }
    }
}
