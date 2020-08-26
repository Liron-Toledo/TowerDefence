using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Structure
{
    protected bool buildMode = false;
    protected List<Vector3> pathPositions = new List<Vector3>();
    protected List<Path> pathList = new List<Path>();

    protected float spawnTimer = 0f;
    [SerializeField]
    protected float timePerSpawn = .6f;

    private BarracksUI barracksUI;

    void Awake()
    {
        if (gameObject.GetComponentInParent<Base>() != null)
        {
            strucBase = gameObject.GetComponentInParent<Base>();
        }
        
        health = new HealthSystem(100);
        barracksUI = gameObject.GetComponentInChildren<BarracksUI>();
    }

    // Update is called once per frame
    void Update()
    {

        spawnTimer -= Time.deltaTime;

        if (buildMode)
        {
            if (Input.GetMouseButton(0))
            {
                SpawnPath(Utilities.GetMouseWorldPosition());
            }
        }

    }

    public void AddPathPosition(Vector3 pos)
    {
        pathPositions.Add(pos);
    }

    public void RemovePath(Vector3 pos)
    {
        //seatch path and remove
        for (int i = 0; i < pathList.Count; i++)
        {
            if (pathList[i].transform.position == pos)
            {
                pathList.RemoveAt(i);
                pathPositions.RemoveAt(i);
            }
        }
    }

    public void SpawnPath(Vector3 spawnPosition)
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
        else
        {
            Debug.Log("not enough money for path");
        }
        

    }

    public List<Vector3> GetPathPositions()
    {
        return pathPositions;
    }

    protected bool ValidPathSpawn(Vector3 spawnPosition)
    {
        if (Utilities.ValidatePosition(spawnPosition) == false)   
        {
            Debug.Log("Can not build path if there is an existing structure at that position");
            return false;
        }

        if (pathPositions != null)
        {
            Vector3 lastPlacedPath = pathPositions[pathPositions.Count - 1];

            if (!((Math.Abs(spawnPosition.x - lastPlacedPath.x) == 1 && Math.Abs(spawnPosition.y - lastPlacedPath.y) == 0) ||
                (Math.Abs(spawnPosition.x - lastPlacedPath.x) == 0 && Math.Abs(spawnPosition.y - lastPlacedPath.y) == 1)))
            {
                Debug.Log("Path needs to be placed adjacent to last placed path");
                return false;
            }

            
            if (pathPositions.Contains(spawnPosition))
            {
                Debug.Log("Path already exists at this position");
                return false;
            }

            
        }

        return true;
    }

    public void BuildMode()
    {
        if (!buildMode)
        {
            Debug.Log("Build Mode Engaged");
            buildMode = true;
        }
        else
        {
            Debug.Log("Build Mode Disengaged");
            buildMode = false;
        }


    }

    public void SpawnUnit(Unit.UnitType unitType)
    {
        if (strucBase.getMoney() >= 10)
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

    public void OnMouseDown()
    {
        if (placed == true)
        {
            barracksUI.Show(this);
            
        }
    }
}
