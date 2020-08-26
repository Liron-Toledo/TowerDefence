using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TowerUI : MonoBehaviour {

    private static TowerUI instance { get; set; }

    private Tower tower;
    GameObject towerMenu;

    GraphicRaycaster raycaster;

    private void Awake()
    {
        instance = this;
        towerMenu = gameObject.transform.parent.Find("TowerMenu").gameObject;
        this.raycaster = towerMenu.GetComponent<GraphicRaycaster>();
        Hide();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            //Debug.Log(hit.collider.gameObject.name);

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();
            pointerData.position = Input.mousePosition;
            this.raycaster.Raycast(pointerData, results);

            if (tower.GetPlaced() && (gameObject.activeSelf == true && towerMenu.activeSelf == true))
            {
                // if player hasnt clicked on UI
                if (results.Count == 0 || results == null)
                {
                    if (hit.collider != null)
                    {
                        // if player doesnt click on current tower (but can click on another tower)
                        if (hit.collider.gameObject != gameObject.transform.parent.gameObject)
                        {
                            Hide();
                        }
                    }
                    else
                    {
                        Hide();
                    }

                    
                    
                }
            }
           
        }
    }

   
    public static void Show_Static(Tower tower) {
        instance.Show(tower);
    }

    public void Show(Tower tower) {
        this.tower = tower;
        gameObject.SetActive(true);
        towerMenu.SetActive(true);
        transform.position = tower.transform.position;
        RefreshRangeVisual();
    }

    public void Hide() {
        gameObject.SetActive(false);
        towerMenu.SetActive(false);
    }

    public void UpgradeRange() {
        tower.UpgradeRange();
        RefreshRangeVisual();
    }

    public void UpgradeDamage() {
        tower.UpgradeDamageOutput();
    }

    public void Sell()
    {
        tower.Sell(tower.GetBase().towerPrice);
    }

    private void RefreshRangeVisual() {
        transform.Find("Range").localScale = Vector3.one * tower.GetRange() * 2f;
    }

}
