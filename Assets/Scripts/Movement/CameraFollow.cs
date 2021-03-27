using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Tooltip("Максимальное расстояние по оси Y от игрока до края камеры")]
    [SerializeField] private float yOffset;
    [Tooltip("Расстояние, при удалении на которое от центра камеры игрок умирает")]
    [SerializeField] private float lowerBorder = 6;

    public delegate void GameOver();
    public static event GameOver OutOfBounds;
    private bool dead = false;

    void FixedUpdate()
    {
        Vector3 playerPos = player.position;
        if (playerPos.y + yOffset > transform.position.y)
        {
            transform.position = new Vector3(0, playerPos.y + yOffset, transform.position.z);
        }
        else if  (playerPos.y + lowerBorder <= transform.position.y && !dead)
        {
            dead = true;
            OutOfBounds();
        }
        
    }
}
