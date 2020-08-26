using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour
{
    [SerializeField]
    private int rows = 5;
    [SerializeField]
    private int columns = 8;
    [SerializeField]
    private float tileSize = 1;

    [SerializeField]
    private float redTileSpawnChance;
    [SerializeField]
    private float blueTileSpawnChance;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTerrain();
    }


    private void GenerateTerrain()
    {

        //GameObject referenceTile = Instantiate(RandomizeTile());
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject tile = (GameObject) Instantiate(RandomizeTile(), transform);
                
                float posX = col * tileSize;
                float posY = row * -tileSize;

                tile.transform.position = new Vector2(posX, posY);

            }
        }

        //Destroy(referenceTile);

        // Centres Grid to middle of screen
        float gridW = columns * tileSize;
        float gridH = rows * tileSize;
        transform.position = new Vector2(-gridW / 2 + tileSize / 2, gridH / 2 - tileSize / 2);
    }

    private GameObject RandomizeTile()
    {
        GameObject tile;
        float randNum = Random.Range(0f, 1.0f);


        if ( randNum < redTileSpawnChance)
        { 
            tile = (GameObject) Resources.Load("Red_Square");
            return tile;
        }
        else if(randNum > redTileSpawnChance && randNum < blueTileSpawnChance+redTileSpawnChance)
        {
            tile = (GameObject)Resources.Load("Blue_Square");
            return tile;
        }
        else
        {
            tile = (GameObject)Resources.Load("Green_Square");
            return tile;
        }

    }
}
