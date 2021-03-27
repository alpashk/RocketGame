using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldRandomizer : MonoBehaviour
{
    [Tooltip("+-����������� ��������� ������ �� ��� � ")]
    [SerializeField] private float xDefault = 4.65f;
    [Tooltip("������������ ���������� �� ������� ����� �������� ����������� �� ��� X")]
    [SerializeField] private float xMaxOffset = 2.6f;
    [Tooltip("����������� ���������� �� ������� ����� �������� ����������� �� ��� X")]
    [SerializeField] private float xMinOffset = 1f; 
    [Tooltip("���������� �� Y �� ������� ��������� ����� � �����")]
    [SerializeField] private float yStep = 1.5f;
    [Tooltip("����������� ���-�� ������ ����� �������������")]
    [SerializeField] private int minBlocksBetweenObstacles = 3; 
    [Tooltip("������������ ���-�� ������ ����� ������������� (�� ������������)")]
    [SerializeField] private int maxBlocksBetweenObstacles = 7;

    [Tooltip("����������� ���-�� ������ ����� ��������")]
    [SerializeField] private int minBlocksBetweenCoins = 6;
    [Tooltip("������������ ���-�� ������ ����� �������� (�� ������������)")]
    [SerializeField] private int maxBlocksBetweenCoins = 10;
    [Tooltip("������������ �������� X ������")]
    [SerializeField] private float xCoinMax = 1.8f;

    [Tooltip("���������� �� Y �� ������, ��� ������� ����� ����������� �������� ��������")]
    [SerializeField] private float yDistanceToDespawn = 10;
    [Tooltip("���������� �� Y �� ������, ��� ������� ����� ����������� �������� ��������")]
    [SerializeField] private float yDistanceToSpawn = 10;


    private Transform cameraTransform;
    private ObjectPool pooler;

    private List<GameObject> spawnedObjects;
    private bool levelIsSpawning = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        pooler = ObjectPool.SharedInstance;
        spawnedObjects = new List<GameObject>();
        SpawnLevelBeginning();
        SpawnLevel();
    }

    void FixedUpdate()
    {
        //��������� ������ � ����� ������������ �������� ������ ����� ����� �������, 
        //������� ����� ��������� ������������� ������ �� ����
        if(spawnedObjects.Count > 0 && spawnedObjects.Last().transform.position.y - yDistanceToSpawn < cameraTransform.position.y && !levelIsSpawning)
        {
            SpawnLevel();
        }

        if(spawnedObjects.Count > 0 && spawnedObjects[0].transform.position.y + yDistanceToDespawn < cameraTransform.position.y)
        {
            DespawnLevel();
        }

        
    }

    void SpawnLevelBeginning()
    {
        float yLevelBeginning = -1.5f; //��������� �������� Y ������
        float xBeginning = 2.15f;      //+-��������� �������� X ������
        int startBricksAmount = 7;

        GameObject tmpBrick;

        //����� ���� ������ ��������
        tmpBrick = pooler.GetObject("Wall");
        tmpBrick.SetActive(true);
        tmpBrick.transform.position = new Vector3(-xBeginning, yLevelBeginning, 0);
        spawnedObjects.Add(tmpBrick);

        tmpBrick = pooler.GetObject("Wall");
        tmpBrick.SetActive(true);
        tmpBrick.transform.position = new Vector3(xBeginning, yLevelBeginning, 0);
        spawnedObjects.Add(tmpBrick);

        for(float currentY = 0; currentY < yLevelBeginning + startBricksAmount* yStep; currentY+=yStep)
        {
            tmpBrick = pooler.GetObject("Wall");
            tmpBrick.SetActive(true);
            tmpBrick.transform.position = new Vector3(-xDefault, currentY, 0);
            spawnedObjects.Add(tmpBrick);

            tmpBrick = pooler.GetObject("Wall");
            tmpBrick.SetActive(true);
            tmpBrick.transform.position = new Vector3(xDefault, currentY, 0);
            spawnedObjects.Add(tmpBrick);
        }

    }


    //� ���� �������� ������ ������� ��� ��������� ������������ ��������� ����������� � ����������� ������������ �������
    void SpawnLevel() 
    {
        levelIsSpawning = true;
        int batchSize = 20;
        float maxY = spawnedObjects.Last().transform.position.y;
        float currentY = maxY + yStep;
        int distanceBetweenCoinsAndObstacles = 3;
        GameObject tmpObject;

        /*
        1 - ������� �������� ������ �� �����������, �� minBlocksBetweenObstacles �� maxBlocksBetweenObstacles 
        2 - �������� ������������ ����������� (� ��� �)
        3 - �������� ������� �����������, ����������� ��������� + M; xMinOffset<M<xMaxOffset;
        => ����� 1 � �.�
        */

        int blocksUntilObstacle = Random.Range(minBlocksBetweenObstacles, maxBlocksBetweenObstacles);
        bool obstacleLeft = Random.Range(0, 100) % 2 == 0;
        float obstacleOffset = Random.Range(xMinOffset, xMaxOffset);
        float xLeftPos = -xDefault;
        float xRightPos = xDefault;

        int blocksUntillCoin = Random.Range(minBlocksBetweenCoins, maxBlocksBetweenCoins);
        float coinPosition = Random.Range(-xCoinMax, xCoinMax);
        for (int i = 0; i < batchSize; i++)
        {
            if (blocksUntilObstacle <= 0)
            {
                if (obstacleLeft)
                {
                    xLeftPos += obstacleOffset;
                }
                else
                {
                    xRightPos -= obstacleOffset;
                }
                if(blocksUntillCoin <=0)
                {
                    blocksUntillCoin+= distanceBetweenCoinsAndObstacles;
                }
            }

            if (blocksUntillCoin <= 0)
            {
                tmpObject = pooler.GetObject("Coin");
                tmpObject.SetActive(true);
                tmpObject.transform.position = new Vector3(coinPosition, currentY, 0);
                spawnedObjects.Add(tmpObject);

                blocksUntillCoin = Random.Range(minBlocksBetweenCoins, maxBlocksBetweenCoins);
                coinPosition = Random.Range(-xCoinMax, xCoinMax);
            }
            else
            {
                blocksUntillCoin--;
            }



            tmpObject = pooler.GetObject("Wall");
            tmpObject.SetActive(true);
            tmpObject.transform.position = new Vector3(xLeftPos, currentY, 0);
            spawnedObjects.Add(tmpObject);

            tmpObject = pooler.GetObject("Wall");
            tmpObject.SetActive(true);
            tmpObject.transform.position = new Vector3(xRightPos, currentY, 0);
            spawnedObjects.Add(tmpObject);
            currentY += yStep;


            //��������� �������� ��� �������� ����������� �������� �������� ����� � ����� ������ � ������� ����� ��������
            if (blocksUntilObstacle <= 0)            
            {
                xLeftPos = -xDefault;
                xRightPos = xDefault;

                blocksUntilObstacle = Random.Range(minBlocksBetweenObstacles, maxBlocksBetweenObstacles);
                obstacleLeft = Random.Range(0, 100) % 2 == 0;
                obstacleOffset = Random.Range(xMinOffset, xMaxOffset);

            }
            else
            {
                blocksUntilObstacle--;
            }
        }
        levelIsSpawning = false;
    }

    void DespawnLevel()
    {
        while (spawnedObjects.Count > 0 && spawnedObjects[0].transform.position.y + yDistanceToDespawn < cameraTransform.position.y)
        {
            spawnedObjects[0].SetActive(false);
            spawnedObjects.RemoveAt(0);
        }
    }


}
