using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitType
    {
        Yellow,
        Orange,
        Red
    }

    public enum State
    {
        Normal,
        Attacking,
        Busy
    }

    public enum Behaviour
    {
        Win,
        Destroy,
        Rampage
    }
    private const float MAX_SPEED = 3f;
    private float speed;
    private HealthSystem health;
    private State state;
    private Behaviour behaviour;
    private List<Vector3> pathVectorList;

    public static List<Unit> unitList = new List<Unit>();

    private Barracks barracks;
    private Base barracksBase;

    private float attackRange = 2f;
    private float attackRate = 1f;
    private float attackTimer = 0f;

    private float timer = 0f;
    private float timeMax = 1f;

    private int currentTargetIndex;

    private List<Structure> enemyStructuresInRange = new List<Structure>();
    private Structure attackTarget;

    private void Awake()
    {  
        unitList.Add(this);
        health = new HealthSystem(100);
    }
    private void Start()
    {
        currentTargetIndex = 2;
        speed = MAX_SPEED;

        //StartCoroutine("RefreshStructuresInRange");
    }

    private void Update()
    {

        if (speed == MAX_SPEED)
        {
            state = State.Normal;
        }

        GetEnemyStructuresInRange();

        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            attackTimer = attackRate;
            HandleAttack();
        }

        HandleMovement();

        RefreshRangeVisual();
    }

    public static Unit Create(Vector3 position, Barracks barracks)
    {
        Transform unitTransform = Instantiate(GameAssets.instance.unit, position, Quaternion.identity, barracks.transform);
        Unit unitHandler = unitTransform.GetComponent<Unit>();
        unitHandler.SetBarracks(barracks);
        unitHandler.SetBase(barracks.GetBase());
        return unitHandler;
    }

    public static Unit Create(Vector3 position, Barracks barracks, UnitType unitType, Behaviour behaviour)
    {
        Transform unitTransform = Instantiate(GameAssets.instance.unit, position, Quaternion.identity, barracks.transform);
        Unit unitHandler = unitTransform.GetComponent<Unit>();
        unitHandler.SetUnitType(unitType);
        unitHandler.SetBehaviour(behaviour);
        unitHandler.SetBarracks(barracks);
        unitHandler.SetBase(barracks.GetBase());
        return unitHandler;
    }

    public void SetPathVectorList(List<Vector3> pathVectorList)
    {
        this.pathVectorList = pathVectorList;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public bool IsDead()
    {
        return health.IsDead();
    }

    public Barracks GetBarracks()
    {
        return barracks;
    }

    public void SetBarracks(Barracks barracks)
    {
        this.barracks = barracks;
    }

    public Base GetBase()
    {
        return barracksBase;
    }

    public void SetBase(Base barracksBase)
    {
        this.barracksBase = barracksBase;
    }

    public void SetTargetPosition(Vector3 targetPosition)
    {
        pathVectorList = new List<Vector3> { targetPosition };
        if (pathVectorList != null && pathVectorList.Count > 1)
        {
            pathVectorList.RemoveAt(0);
        }
    }

    private void SetBehaviour(Behaviour b)
    {
        behaviour = b;
    }

    private void SetUnitType(UnitType unitType)
    {
        switch (unitType)
        {
            default:
            case UnitType.Orange:
                health.SetHealthMax(80, true);
                break;
            case UnitType.Red:
                health.SetHealthMax(130, true);
                break;
            case UnitType.Yellow:
                health.SetHealthMax(50, true);
                break;
        }
    }

    private void GetEnemyStructuresInRange()
    {
        List<Structure> structures = new List<Structure>();
        foreach (KeyValuePair<Vector3, Structure> struc in GridManager.instance.GetStructureMap())
        {
            
            if (struc.Value != null &&
                struc.Value.GetBase() != barracksBase &&
                Vector3.Distance(transform.position, struc.Value.transform.position) < attackRange &&
                struc.Value.gameObject.GetComponent<Path>() == null)
            {
                //Debug.Log("Added struc");
                structures.Add(struc.Value);
            }
            
        }

        enemyStructuresInRange = structures;
    }

    private void RefreshRangeVisual()
    {
        transform.Find("AttackRange").localScale = Vector3.one * attackRange * 2f;
    }

    private void HandleAttack()
    {
        
        foreach (Structure struc in enemyStructuresInRange)
        {
            if (struc != null)
            {
                if (behaviour == Behaviour.Win)
                {
                    if (struc.gameObject.GetComponent<Base>() != null)
                    {
                        state = State.Attacking;
                        
                        DamagePopUp.Create(struc.transform.position, 20, false);
                        struc.TakeDamage(20);
                    }
                   
                }

                if (behaviour == Behaviour.Destroy)
                {
                    state = State.Attacking;
                    if (attackTarget == null)
                    {
                        attackTarget = struc;
                    }
                    else
                    { 
                        DamagePopUp.Create(attackTarget.transform.position, 20, false);
                        attackTarget.TakeDamage(20);
                    }
                    
                    if (attackTarget != null)
                    {
                        speed = 0;
                        if (attackTarget.IsDead())
                        {
                            speed = MAX_SPEED;
                        }
                    }
                    
                }

                if (behaviour == Behaviour.Rampage)
                {
                    state = State.Attacking;
                    speed = MAX_SPEED / 2;
                    DamagePopUp.Create(struc.transform.position, 20, false);
                    struc.TakeDamage(20);
                    
                }
               
            }
            
        }
    }

    public void TakeDamage(int damageAmount)
    {
        //Vector3 dir = GetRandomDir();

        // Could create a damage pop up here

        health.Damage(damageAmount);
        if (IsDead())
        {
            // play death animation
            Destroy(gameObject);
        }
        else
        {
            // Knockback
            //transform.position += dir * 2.5f;
        }
    }

    private void HandleMovement()
    {
        if (pathVectorList != null)
        {
            if (currentTargetIndex < pathVectorList.Count)
            {
                transform.position = Vector3.MoveTowards(transform.position, pathVectorList[currentTargetIndex], speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, pathVectorList[currentTargetIndex]) < 0.01f)
                {
                    currentTargetIndex++;
                }     
            }
            
        }
    }



}
