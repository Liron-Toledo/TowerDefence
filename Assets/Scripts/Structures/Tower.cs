using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Tower : Structure
{
    public Vector3 weaponPosition;
    [SerializeField]
    private float range;
    [SerializeField]
    private float fireRate;
    private float shotTimer;
    [SerializeField]
    private int damageOutput;

    private TowerUI towerUI;

    private void Awake()
    {
        strucBase = gameObject.GetComponentInParent<Base>();
        placed = false;
        shotTimer = 0;

        health = new HealthSystem(100);
        towerUI = gameObject.GetComponentInChildren<TowerUI>();
    }

    private void Update()
    {
        if (placed)
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0f)
            {
                shotTimer = fireRate;
                Unit unit = GetClosestUnit();
                if (unit != null)
                {
                    //Enemy is in range
                    Projectile.Create(weaponPosition, unit, Random.Range(damageOutput - 5, damageOutput + 5));
                }
            }
        }
       
    }

    private Unit GetClosestUnit()
    {
        Unit closest = null;
        foreach (Unit unit in Unit.unitList)
        {
            if (unit.IsDead()) continue;

            if (Vector3.Distance(transform.position, unit.GetPosition()) <= range)
            {
                if (closest == null)
                {
                    if (unit.GetBase() != strucBase)
                    {
                        closest = unit;
                    }       
                }
                else
                {
                    if (Vector3.Distance(transform.position, unit.GetPosition()) < Vector3.Distance(transform.position, closest.GetPosition()))
                    {
                        if (unit.GetBase() != strucBase)
                        {
                            closest = unit;
                        }
                    }
                }
            }

        }

        return closest;
    }

    public float GetRange()
    {
        return range;
    }

    public void UpgradeRange()
    {
        range += 10f;
    }

    public void UpgradeDamageOutput()
    {
        damageOutput += 5;
    }

    public void ShowUpgradeUI()
    {
        TowerUI.Show_Static(this);
    }

    public void OnMouseDown()
    {
        if (placed == true)
        {
            towerUI.Show(this);
        }
    }
}
