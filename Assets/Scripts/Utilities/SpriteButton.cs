using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpriteButton : MonoBehaviour
{
    private TowerUI towerUI;
    private BarracksUI barracksUI;
    private GameObject parent;

    private void Awake()
    {
        parent = gameObject.transform.parent.gameObject;
        if (parent.name == "TowerUI")
        {
            towerUI = transform.GetComponentInParent<TowerUI>();
        }
        else
        {
            barracksUI = transform.GetComponentInParent<BarracksUI>();
        }
        
    }

    void OnMouseDown()
    {
        if (parent.name == "UpgradeOverlay")
        {
            if (gameObject.name == "RangeUpBtn")
            {
                towerUI.UpgradeRange();
            }

            if (gameObject.name == "DamageUpBtn")
            {
                towerUI.UpgradeDamage();
            }

            if (gameObject.name == "CloseBtn")
            {
                towerUI.Hide();
            }
        }

        if (parent.name == "BarracksOverlay")
        {
            if (gameObject.name == "SendUnitBtn")
            {

            }

            if (gameObject.name == "BuildModeBtn")
            {
                barracksUI.BuildMode();
            }

            if (gameObject.name == "CloseBtn")
            {
                barracksUI.Hide();
            }
        }

         
    }

}
