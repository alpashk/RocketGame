using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    [Tooltip("���������, � ������� �������� ������")]
    [SerializeField] float flightForce;
    [Tooltip("�������� �������� ������")]
    [SerializeField] float rotateSpeed;
    [Tooltip("������������ ������ �������� ��� ������")]
    [SerializeField] float maxAngle=90;
    [Tooltip("���������������� ����������")]
    [SerializeField] float rotateSensitivity = 1;

    [SerializeField] bool InvertSteering = true;


    private Rigidbody rocketRB;

    private void Awake()
    {
        Controls.RotationControl += RotateController;
        Controls.FlightControl += FlightController;

        rocketRB = GetComponent<Rigidbody>();

        if(InvertSteering)
        {
            rotateSensitivity *= -1;
        }

    }

    private void FlightController()
    {
        rocketRB.AddForce(transform.up * flightForce, ForceMode.Acceleration);
    }

    private void RotateController(float deltaZ) 
    {
        float degreeChange = deltaZ / rotateSensitivity;
        float currentRotationZ = transform.rotation.eulerAngles.z;
        if(currentRotationZ > 180)
        {
            currentRotationZ -= 360;
        }

        float newRotation = currentRotationZ + degreeChange;

        if(newRotation < -maxAngle)
        {
            newRotation = -maxAngle;
        }
        else if (newRotation > maxAngle)
        {
            newRotation = maxAngle;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, 
            Quaternion.Euler(new Vector3(0, 0, newRotation)), rotateSpeed);
    }
    private void OnDestroy()
    {
        Controls.RotationControl -= RotateController;
        Controls.FlightControl -= FlightController;
    }
    private void OnDisable()
    {
        Controls.RotationControl -= RotateController;
        Controls.FlightControl -= FlightController;
    }
}

