using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    Tower tower;

    private void Awake()
    {
        tower = gameObject.GetComponent<Tower>();
    }

    void Update()
    {
      
        if (tower.GetPlaced() != true)
        {
            transform.position = GridManager.instance.ValidateWorldGridPosition(Utilities.GetMouseWorldPosition());
            transform.position += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (tower.GetPlaced() == false)
            {
                Destroy(gameObject);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (tower.GetPlaced() == false)
            {
                if (Vector3.Distance(transform.position, tower.GetBase().transform.position) <= tower.GetBase().getBuildRange())
                {
                    if (!GridManager.instance.GetStructureMap().ContainsKey(transform.position))
                    {
                        tower.SetPlaced(true);
                        tower.weaponPosition = transform.Find("Weapon").position;
                        tower.GetBase().AddStrucureToBase(tower);
                        GridManager.instance.AddStructure(transform.position, tower);
                    }
                    else
                    {
                        // can higlight red if over an existing structure
                        Debug.Log("There already exists a structure at that location");
                    }
                        
                }
                else
                {
                    //later down the line. Can highlight red when out of range
                    Debug.Log("tower not in range");
                }
            }
           
        }
        
    }

    
}
