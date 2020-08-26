using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Base : Structure
{
    public enum baseType
    {
        Human,
        Elf,
        Dwarf,
        Orc
    }

    [SerializeField]
    protected float buildRange = 8f;
    protected int money = 10000;
    public int towerPrice = 200;
    public int barracksPrice = 200;
    public int pathCost = 5;
    public int unitCost = 10;
    private List<Structure> baseStructures = new List<Structure>();


    protected virtual void Awake()
    {
        strucBase = this;
        placed = true;
        health = new HealthSystem(100);  
    }

    protected virtual void Start()
    {
        //neccesary to be in start (to avoid grid manager being null)
        transform.position += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
    }

    public float getBuildRange()
    {
        return buildRange;
    }

    public int getMoney()
    {
        return money;
    }

    public void LoseMoney(int amount)
    {
        money -= amount;
    }

    public void EarnMoney(int amount)
    {
        money += amount;
    }

    public void AddStrucureToBase(Structure s)
    {
        baseStructures.Add(s);
    }

    public List<Structure> GetBaseStructures()
    {
        return baseStructures;
    }

}
