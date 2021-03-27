using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    [Tooltip("Максимальная скорость столкновения, при котором оно не будет фатальным")]
    [SerializeField] private float maxCollisionSpeed = 5f;
    [SerializeField] private GameObject explosionModel;

    private Flight playerController;
    private Rigidbody playerRigidbody;

    public delegate void Endgame();
    public static event Endgame GameEnd;

    public delegate void Coin();
    public static event Coin CoinPickup;

    private void Awake()
    {
        CameraFollow.OutOfBounds += Death;

        playerController = gameObject.GetComponent<Flight>();
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    private void Death()
    {
        playerController.enabled = false;
        playerRigidbody.isKinematic = true;
        Instantiate(explosionModel, transform.position, Quaternion.identity);
        StartCoroutine(EndgameRoutine());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > maxCollisionSpeed)
        {
            Death();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        int xOutsideGameArea = -10;
        CoinPickup();
        Transform coinTransform = collider.transform;
        //Я перемещаю монету для избежания ошибок с удалением не тех монет при генерации уровня
        coinTransform.position = new Vector3(xOutsideGameArea, coinTransform.position.y, 0); 
    }
    IEnumerator EndgameRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        GameEnd();
    }
    private void OnDestroy()
    {
        CameraFollow.OutOfBounds -= Death;
    }
}
