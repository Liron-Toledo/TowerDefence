using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class AIBase : Base
{
   
    private List<AIBarracks> barracksList = new List<AIBarracks>();
    private List<Tower> towerList = new List<Tower>();
    //[SerializeField]
    private Base[] playerBases;

    public bool aiOn;
    public int numDecisions;

    public HealthBar healthBar;


    protected override void Awake()
    {
        base.Awake();

        RefreshPlayerBases();
        
    }

    protected override void Start()
    {
        base.Start();
        
        if (aiOn)
        {
            StartCoroutine("AIDecisionMaker");
        }

        GridManager.instance.AddStructure(transform.position, gameObject.GetComponent<AIBase>());
    }

    private void Update()
    {
        healthBar.SetSize(health.GetHealth()/100f);
    }

    public void RefreshPlayerBases()
    {
        playerBases = Object.FindObjectsOfType<PlayerBase>();
    }

    public void SpawnTower(Vector3 spawnPosition, float chanceToSpawn)
    {
        if (Utilities.ValidatePosition(spawnPosition))
        {
            float randomChance = Random.Range(0f, 1f);
            if (randomChance < chanceToSpawn)
            {
                if (money >= towerPrice)
                {
                    spawnPosition = GridManager.instance.ValidateWorldGridPosition(spawnPosition);
                    spawnPosition += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
                    Transform tower = Instantiate(GameAssets.instance.towerAI, spawnPosition, Quaternion.identity, transform);

                    towerList.Add(tower.gameObject.GetComponent<Tower>());
                    GridManager.instance.AddStructure(spawnPosition, tower.gameObject.GetComponent<Tower>());

                    money -= towerPrice;
                }
            }
        }
        
    }

    public void SpawnBarracks(Vector3 spawnPosition, float chanceToSpawn)
    {
        if (Utilities.ValidatePosition(spawnPosition))
        {
            float randomChance = Random.Range(0f, 1f);
            if (randomChance < chanceToSpawn)
            {
                if (money >= barracksPrice)
                {
                    spawnPosition = GridManager.instance.ValidateWorldGridPosition(spawnPosition);
                    spawnPosition += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
                    Transform barracks = Instantiate(GameAssets.instance.barracksAI, spawnPosition, Quaternion.identity, transform);
                    barracks.gameObject.GetComponent<AIBarracks>().AddPathPosition(barracks.position);

                    barracksList.Add(barracks.gameObject.GetComponent<AIBarracks>());
                    GridManager.instance.AddStructure(spawnPosition, barracks.gameObject.GetComponent<AIBarracks>());

                    money -= barracksPrice;
                }
            }
        }
        
       
    }

    public List<Vector3> AStarPathfinder(Vector3 start, Vector3 end)
    {
        if (Utilities.ValidatePosition(end) == true)
        {
            return Pathfinding.AStar(start, end);
        }

        Debug.Log("End position is not valid");
        return null;

    }

    //NOTE: NEED TO TAKE INTO ACCOUNT MONEY AS A DECISON FACTOR ELSE THE AI JUST STOPS FUNCTIONING
    IEnumerator AIDecisionMaker()
    {
        Vector3 tempTarget = new Vector3();
        
        //while(health != 0)
        for (int i = 0; i < numDecisions; i++)
        {
            RefreshPlayerBases();
            
            Debug.Log("Decision" + i);
            bool decisionMade = false;

            //SPAWNING
            //for every player base
            foreach (PlayerBase pb in playerBases)
            {
                if (decisionMade) break;
                //for ever structure belonging to that base
                foreach (Structure struc in pb.GetBaseStructures())
                {
                    if (decisionMade) break;
                    // are any of the structures within the build range of the a.i (meaning close)
                    if (Utilities.DeternineIfPositionInRange(transform.position, struc.transform.position, buildRange))
                    {
                        if (struc.gameObject.GetComponent<Path>() != null)
                        {
                            //build tower adjacent to enemy path
                            SpawnTower(Utilities.GetRandomNeighbor(struc.transform.position), 0.8f);
                            //decisionMade = true;
                            //break;
                        }

                    }
                }
            }

            //Spawning Towers
            if (towerList.Count == 0)
            {
                SpawnTower(Utilities.FindRandomPlotWithinCircle(transform.position, buildRange), 0.6f);
            }
            else
            {
                SpawnTower(Utilities.FindRandomPlotWithinCircle(transform.position, buildRange), 0.6f / towerList.Count);
            }

            if (barracksList.Count == 0)
            {
                // high priority to build barracks
                SpawnBarracks(Utilities.FindRandomPlotWithinCircle(transform.position, buildRange), 0.6f);
            }
            else
            {
                //Spawn additional barracks
                SpawnBarracks(Utilities.FindRandomPlotWithinCircle(transform.position, buildRange), 0.6f / barracksList.Count);

                //Spawning Paths
                foreach (AIBarracks b in barracksList)
                {
                    
                    //pick random player base as target (that is different from previous barracks)
                    if (b.GetTarget() == Vector3.zero)
                    {
                        int random = Random.Range(0, playerBases.Length);
                        Vector3 target = Utilities.GetRandomNeighbor(playerBases[random].transform.position);

                        int breakOut = 0;
                        while (target == tempTarget || breakOut == 10)
                        {
                            target = Utilities.GetRandomNeighbor(playerBases[random].transform.position);
                            breakOut++;
                        }
                        
                        b.SetTarget(target);
                        tempTarget = target;
                    }
                    else
                    {
                        if (b.GetPathPositions()[b.GetPathPositions().Count - 1] != b.GetTarget())
                        {
                            
                            if (AStarPathfinder(b.GetPathPositions()[b.GetPathPositions().Count - 1], b.GetTarget()) != null)
                            {
                                b.SetTargetPossible(true);
                                b.SetPlannedPath(AStarPathfinder(b.GetPathPositions()[b.GetPathPositions().Count - 1], b.GetTarget()));
                                b.SpawnPath(b.GetPlannedPath()[1], 0.7f);
                            }
                            else
                            {
                                b.SetTargetPossible(false);
                            }

                            if (b.IsTargetPossible() == false)
                            {
                                if (b.GetPath().Count > 0)
                                {
                                    //delete backwards until a path becomes possible
                                    b.GetPath()[b.GetPath().Count - 1].Sell();
                                }
                                
                                
                                if (b.GetPath().Count == 0)
                                {
                                    barracksList.Remove(b);
                                    b.Sell(b.GetBase().barracksPrice);
                                }


                            }
                            
                        }
                        else
                        {
                            b.SetDestinationReached(true);
                            Debug.Log("Barracks has reached destination");
                        }
                    }

                    
                       //depending on visible security of base a.i can choose to redirect path..but the closer a path is to a base the less likely

                }


            }

            //Spawning Units
            foreach (AIBarracks b in barracksList)
            {
                if (b.DestinationReached())
                {
                    //I can implement more random chance wrt unittype
                    b.SpawnUnit(Unit.UnitType.Yellow, 0.8f);
                }
                else
                {
                    if (b.GetPathPositions().Count > 5)
                    {
                        b.SpawnUnit(Unit.UnitType.Yellow, 0.4f + (b.GetPathPositions().Count - 1) / 100);
                    }
                    
                }
            }

            
   

        yield return new WaitForSeconds(2f);
        }
    }


}