using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBarracks : Barracks
{
    private List<Vector3> plannedPath = new List<Vector3>();
    private Vector3 target;
    private bool destinationReached = false;
    private bool targetPossible = true;


    public void SpawnPath(Vector3 spawnPosition, float spawnChance)
    {
        float randomChance = Random.Range(0f, 1f);
        if (randomChance < spawnChance)
        {
            if (strucBase.getMoney() >= strucBase.pathCost)
            {
                spawnPosition = GridManager.instance.ValidateWorldGridPosition(spawnPosition);
                spawnPosition += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;

                //Debug.Log("path_pos: " + spawnPosition);
                if (ValidPathSpawn(spawnPosition))
                {
                    pathPositions.Add(spawnPosition);
                    Transform path = Instantiate(GameAssets.instance.path, spawnPosition, Quaternion.identity, transform);

                    pathList.Add(path.gameObject.GetComponent<Path>());
                    GetBase().AddStrucureToBase(path.gameObject.GetComponent<Path>());
                    GridManager.instance.AddStructure(spawnPosition, path.gameObject.GetComponent<Path>());

                    strucBase.LoseMoney(strucBase.pathCost);

                }
            }
        }

       
    }

    public void SpawnUnit(Unit.UnitType unitType, float spawnChance)
    {

        float randomChance = Random.Range(0f, 1f);
        if (randomChance < spawnChance)
        {
            if (strucBase.getMoney() >= strucBase.unitCost)
            {
                if (spawnTimer <= 0f)
                {
                    spawnTimer = timePerSpawn;
                    Vector3 spawnPosition = pathPositions[1];


                    Unit unit = Unit.Create(spawnPosition, gameObject.GetComponent<Barracks>(), unitType, Unit.Behaviour.Rampage);
                    unit.SetPathVectorList(pathPositions);

                    strucBase.LoseMoney(strucBase.unitCost);

                }
            }

        }
    }

    public List<Path> GetPath()
    {
        return pathList;
    }

    public bool DestinationReached()
    {
        return destinationReached;
    }

    public void SetDestinationReached(bool dr)
    {
        destinationReached = dr;
    }
    public bool IsTargetPossible()
    {
        return targetPossible;
    }

    public void SetTargetPossible(bool t)
    {
        targetPossible = t;
    }

    public Vector3 GetTarget()
    {
        return target;
    }
    
    public void SetTarget(Vector3 pos)
    {
        target = pos;
    }

    public List<Vector3> GetPlannedPath()
    {
        return plannedPath;
    }

    public void SetPlannedPath(List<Vector3> p)
    {
        plannedPath = p;
    }

}
