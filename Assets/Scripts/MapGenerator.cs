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
        GetComponent<MeshGenerator>().GenerateMesh(map, 1);
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            GenerateMap();
            GetComponent<MeshGenerator>().GenerateMesh(map, 1);
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();

        for(int ii=0; ii < SmoothNumber; ii++)
            SmoothMap();
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
            seed = Time.time.ToString();

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        for (int ii = 0; ii < width; ii++)
            for (int jj = 0; jj < height; jj++)
            {
                if (ii == 0 || jj == 0 || ii == width - 1 || jj == height - 1)
                    map[ii, jj] = 1;
                else
                    map[ii, jj] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;

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
                else
                    map[ii, jj] = 0;
            }
    }

    int GetSurroundWallCount(int x, int y)
    {
        int wallCount = 0;
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != x || neighbourY != y) // passando pelos vizinhos ele passa por ele mesmo
                        wallCount += map[neighbourX, neighbourY];
                }
                else
                {
                    wallCount++;
                }
        return wallCount;
    }
    
    /*private void OnDrawGizmos()
    {
        if(map!= null) 
            for(int ii=0; ii< width;ii++)
                for( int jj=0; jj< height; jj++)
                {
                    Gizmos.color = (map[ii, jj] == 1) ? Color.red : Color.white;
                    Vector3 pos = new Vector3(-width / 2 + ii + 0.5f, 0, -height / 2 + jj +0.5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
    }*/
}