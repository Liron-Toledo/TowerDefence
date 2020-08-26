using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : Structure
{
    private Barracks barracks;

    private void Awake()
    {
        barracks = gameObject.GetComponentInParent<Barracks>();
        strucBase = barracks.GetBase();
        health = new HealthSystem(50);
    }

    public void Sell()
    {
        strucBase.EarnMoney(strucBase.pathCost);
        GridManager.instance.RemoveStructure(transform.position);
        barracks.RemovePath(transform.position);
        Destroy(gameObject);
    }

}
