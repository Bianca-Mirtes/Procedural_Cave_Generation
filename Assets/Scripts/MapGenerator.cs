using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;
    public int SmoothNumber;
    public string seed;
    public bool useRandomSeed;

    [Range(0, 100)]
    public int randomFillPercent;

    int[,] map;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
            GenerateMap();
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for(int ii=0; ii < SmoothNumber; ii++)
            SmoothMap();

        int borderSize = 5;
        int[,] borderMap = new int[width + borderSize * 2, height + borderSize * 2];

        for (int ii = 0; ii < borderMap.GetLength(0); ii++)
            for (int jj = 0; jj < borderMap.GetLength(1); jj++)
                if (ii >= borderSize && ii < width + borderSize && jj >= borderSize && jj < height + borderSize)
                    borderMap[ii, jj] = map[ii - borderSize, jj - borderSize];
                else
                    borderMap[ii, jj] = 1;

        GetComponent<MeshGenerator>().GenerateMesh(borderMap, 1);
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
            seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
            }
        }
    }

    void SmoothMap()
    {
        for (int ii = 0; ii < width; ii++)
            for (int jj = 0; jj < height; jj++)
            {
                int neighboutWallTiles = GetSurroundWallCount(ii, jj);

                if(neighboutWallTiles > 4)
                    map[ii, jj] = 1;
                else if(neighboutWallTiles < 4)
                    map[ii, jj] = 0;
            }
    }

    int GetSurroundWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }
}