using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    protected bool placed;
    protected Base strucBase;
    public HealthSystem health;

    public bool IsDead()
    {
        return health.IsDead();
    }
    public void Sell(int price)
    {
        strucBase.EarnMoney(price);
        GridManager.instance.RemoveStructure(transform.position);
        Destroy(gameObject);
    }

    public Base GetBase()
    {
        return strucBase;
    }

    public void SetBase(Base b)
    {
        strucBase = b;
    }

    public bool GetPlaced()
    {
        return placed;
    }

    public void SetPlaced(bool p)
    {
        placed = p;
    }

    public void TakeDamage(int damageAmount)
    {
        health.Damage(damageAmount);
        if (IsDead())
        {
            // play death animation
            GridManager.instance.RemoveStructure(transform.position);
            Destroy(gameObject);
        }
    }
}
