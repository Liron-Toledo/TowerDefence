using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BarracksUI : MonoBehaviour
{

    private static BarracksUI instance { get; set; }

    private Barracks barracks;
    GameObject barracksMenu;

    GraphicRaycaster raycaster;


    private void Awake()
    {
        instance = this;
        barracksMenu = gameObject.transform.parent.Find("BarracksMenu").gameObject;
        this.raycaster = barracksMenu.GetComponent<GraphicRaycaster>();
        Hide();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            List<RaycastResult> results = new List<RaycastResult>();
            pointerData.position = Input.mousePosition;
            this.raycaster.Raycast(pointerData, results);

            if (barracks.GetPlaced() && (gameObject.activeSelf == true && barracksMenu.activeSelf == true))
            {
                // if player hasnt clicked on UI
                if (results.Count == 0 || results == null)
                {
                    if (hit.collider != null)
                    {
                        // if player doesnt click on current barracks (but click on another barracks)
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

    public static void Show_Static(Barracks barracks)
    {
        instance.Show(barracks);
    }

    public void Show(Barracks barracks)
    {
        this.barracks = barracks;
        barracksMenu.SetActive(true);
        gameObject.SetActive(true);
        transform.position = barracks.transform.position;
        //RefreshRangeVisual();
    }

    public void Hide()
    {
        barracksMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SendUnit()
    {
        barracks.SpawnUnit(Unit.UnitType.Yellow);
    }

    public void BuildMode()
    {
        barracks.BuildMode();
    }

    public void Sell()
    {
        barracks.Sell(barracks.GetBase().barracksPrice);
    }

    private void RefreshRangeVisual()
    {
        //transform.Find("Range").localScale = Vector3.one * tower.GetRange() * 2f;
    }
}
