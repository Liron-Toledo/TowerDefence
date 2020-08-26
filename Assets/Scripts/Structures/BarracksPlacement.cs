using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarracksPlacement : MonoBehaviour
{
    Barracks barracks;

    private void Awake()
    {
        barracks = gameObject.GetComponent<Barracks>();
    }
    void Update()
    {
        if (barracks.GetPlaced() == false)
        {
            transform.position = GridManager.instance.ValidateWorldGridPosition(Utilities.GetMouseWorldPosition());
            transform.position += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (barracks.GetPlaced() == false)
            {
                Destroy(gameObject);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (barracks.GetPlaced() == false)
            {
                if (Vector3.Distance(transform.position, barracks.GetBase().transform.position) <= barracks.GetBase().getBuildRange())
                {
                    if (!GridManager.instance.GetStructureMap().ContainsKey(transform.position))
                    {
                        barracks.SetPlaced(true);
                        barracks.AddPathPosition(transform.position);
                        barracks.GetBase().AddStrucureToBase(barracks);
                        GridManager.instance.AddStructure(transform.position, barracks);

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
                    Debug.Log("barracks not in range");
                }

            }

        }
    }
}
