using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldRandomizer : MonoBehaviour
{
    [Tooltip("+-—тандартное положение стенки по оси ’ ")]
    [SerializeField] private float xDefault = 4.65f;
    [Tooltip("ћаксимальное рассто€ние на которое можно сдвинуть преп€тствие по оси X")]
    [SerializeField] private float xMaxOffset = 2.6f;
    [Tooltip("ћинимальное рассто€ние на которое можно сдвинуть преп€тствие по оси X")]
    [SerializeField] private float xMinOffset = 1f; 
    [Tooltip("–ассто€ние по Y на которое смещаютс€ блоки в р€дах")]
    [SerializeField] private float yStep = 1.5f;
    [Tooltip("ћинимальное кол-во блоков между преп€тстви€ми")]
    [SerializeField] private int minBlocksBetweenObstacles = 3; 
    [Tooltip("ћаксимальное кол-во блоков между преп€тстви€ми (не включительно)")]
    [SerializeField] private int maxBlocksBetweenObstacles = 7;

    [Tooltip("ћинимальное кол-во блоков между монетами")]
    [SerializeField] private int minBlocksBetweenCoins = 6;
    [Tooltip("ћаксимальное кол-во блоков между монетами (не включительно)")]
    [SerializeField] private int maxBlocksBetweenCoins = 10;
    [Tooltip("ћаксимальное значение X монеты")]
    [SerializeField] private float xCoinMax = 1.8f;

    [Tooltip("–ассто€ние по Y от камеры, при котором будет происходить удаление объектов")]
    [SerializeField] private float yDistanceToDespawn = 10;
    [Tooltip("–ассто€ние по Y от камеры, при котором будет происходить создание объектов")]
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
        //последний объект в листе заспауненных объектов всегда будет самым верхним, 
        //поэтому можно провер€ть наполненность уровн€ по нему
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
        float yLevelBeginning = -1.5f; //начальное значение Y уровн€
        float xBeginning = 2.15f;      //+-начальное значение X уровн€
        int startBricksAmount = 7;

        GameObject tmpBrick;

        //—паун двух нижних кирпичей
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


    //€ буду спаунить уровни пачками дл€ упрощени€ отслеживани€ состо€ний преп€тствий и недопущени€ непроходимых уровней
    void SpawnLevel() 
    {
        levelIsSpawning = true;
        int batchSize = 20;
        float maxY = spawnedObjects.Last().transform.position.y;
        float currentY = maxY + yStep;
        int distanceBetweenCoinsAndObstacles = 3;
        GameObject tmpObject;

        /*
        1 - —начала рандомлю отступ до преп€тстви€, от minBlocksBetweenObstacles до maxBlocksBetweenObstacles 
        2 - –андомлю расположение преп€тстви€ (Ћ или ѕ)
        3 - –андомлю позицию преп€тстви€, стандартное положение + M; xMinOffset<M<xMaxOffset;
        => пункт 1 и т.д
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


            //повторна€ проверка дл€ возврата стандартных значений позици€м левых и правх блоков и рандома новых значений
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
