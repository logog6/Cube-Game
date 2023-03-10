using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMechanic : MonoBehaviour
{
    private const float timeBefor = 3.0f;

    public float moveSpeed = 2.0f;
    public float drag = 0.5f;
    public float terminalRotationSpeed = 15.0f;
    public VirtualJoystick movelJoystick;

    public float boostSpeed = 7.0f;
    public float boostCooldown = 4.0f;
    private float lastBoost;

    private Rigidbody contorller;
    private Transform camTransform;

    private float startTime;

    private void Start()
    {
        lastBoost = Time.time - boostCooldown;
        startTime = Time.time;

        contorller = GetComponent<Rigidbody>();
        contorller.maxAngularVelocity = terminalRotationSpeed;
        contorller.drag = drag;

        camTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (Time.time - startTime < timeBefor)
        {
            return;
        }

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
        if (Time.time - startTime < timeBefor)
        {
            return;
        }

        if (Time.time - lastBoost > boostCooldown)
        {
            lastBoost = Time.time;
            contorller.AddForce(contorller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);
        }
    }
}
