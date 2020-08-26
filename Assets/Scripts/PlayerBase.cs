using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : Base
{
    public Text moneyAmount;
    public Text healthAmount;

    protected override void Awake()
    {
        base.Awake();  
    }

    protected override void Start()
    {
        base.Start();
        GridManager.instance.AddStructure(transform.position, gameObject.GetComponent<PlayerBase>());
    }

    private void Update()
    {
        moneyAmount.text = "Money: " + money.ToString();
        healthAmount.text = "Health: " + health.GetHealth().ToString();
    }

    public void SpawnTower()
    {
        if (money >= towerPrice)
        {
            Vector3 spawnPosition = Utilities.GetMouseWorldPosition();
            spawnPosition = GridManager.instance.ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
            Instantiate(GameAssets.instance.tower, spawnPosition, Quaternion.identity, transform);


            money -= towerPrice;
        }
        else
        {
            Debug.Log("Not enough money");
        }

    }

    public void SpawnBarracks()
    {
        if (money >= barracksPrice)
        {
            Vector3 spawnPosition = Utilities.GetMouseWorldPosition();
            spawnPosition = GridManager.instance.ValidateWorldGridPosition(spawnPosition);
            spawnPosition += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
            Instantiate(GameAssets.instance.barracks, spawnPosition, Quaternion.identity, transform);

            money -= barracksPrice;
        }
        else
        {
            Debug.Log("Not enough money");
        }

    }

   
}
