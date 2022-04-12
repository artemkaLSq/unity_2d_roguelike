using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Board : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;             
        public int maximum;             
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;                                         
    public int rows = 8;                                            
    public Count boxCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;                                            
    public GameObject floorTile;
    public GameObject foodTile;
    public GameObject wallTile;
    public GameObject boxTile;
    public GameObject enemyTile;                                                                   

    private Transform boardHolder;                                    
    private List<Vector3> gridPositions = new List<Vector3>();

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns-1; x++)
        {
            for (int y = 1; y < rows-1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns+1; x++)
        {
            for (int y = -1; y < rows+1; y++)
            {
                GameObject toInstantiate = floorTile;
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = wallTile;
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            Instantiate(tile, randomPosition, Quaternion.identity);
        }
    }


    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(boxTile, boxCount.minimum, boxCount.maximum);
        int enemyCount = level/2+1;
        LayoutObjectAtRandom(enemyTile, enemyCount, enemyCount+1);
        LayoutObjectAtRandom(foodTile, foodCount.minimum, foodCount.maximum);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
