using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed=1;
    void FixedUpdate()
    {
        Vector3 coinRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(coinRotation.x, coinRotation.y + rotationSpeed, coinRotation.z));
    }
}
