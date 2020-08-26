using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    // Get mouse position in world with z = 0f
    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPosition(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPosition(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

    //transforms vector 3 into euler angle
    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public static Vector3 FindRandomPlotWithinCircle(Vector3 circleCentre, float radius)
    {
        
        Vector3 randomPosition = new Vector3(Mathf.RoundToInt((Random.Range(circleCentre.x - radius, circleCentre.x + radius))), Mathf.RoundToInt(Random.Range(circleCentre.y - radius, circleCentre.y + radius)), 0);
        randomPosition += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
        while (ValidatePosition(randomPosition) != true)
        {
            randomPosition = new Vector3(Mathf.RoundToInt((Random.Range(circleCentre.x - radius, circleCentre.x + radius))), Mathf.RoundToInt(Random.Range(circleCentre.y - radius, circleCentre.y + radius)), 0);
            randomPosition += new Vector3(1, 1, 0) * GridManager.instance.grid.GetCellSize() * .5f;
        }
        
        return randomPosition;
    }

    public static bool ValidatePosition(Vector3 position)
    {
        if (position.x > (GridManager.instance.GetOriginPos().x + GridManager.instance.GetWidth())
                    || position.x < (GridManager.instance.GetOriginPos().x)
                    || position.y > (GridManager.instance.GetOriginPos().y + GridManager.instance.GetHeight() )
                    || position.y < (GridManager.instance.GetOriginPos().y))
        {
            return false;
        }

        if (GridManager.instance.IsOccupied(position) == true)
        {
            return false;
        }

        return true;
    }

    public static Vector3 GetRandomNeighbor(Vector3 position)
    {
        int randomDir = Random.Range(0, 4);
    
        switch (randomDir)
        {
            case 0:
                position += new Vector3(1, 0, 0);
                break;
            case 1:
                position += new Vector3(-1, 0, 0);
                break;
            case 2:
                position += new Vector3(0, 1, 0);
                break;
            case 3:
                position += new Vector3(0, -1, 0);
                break;
        }

        return position;
    }

    public static bool DeternineIfPositionInRange(Vector3 position, Vector3 target, float range)
    {
        if (Vector3.Distance(position, target) <= range)
        {
            return true;
        }

        return false;
    }

    public static bool IsPositionAdjacent(Vector3 pos1, Vector3 pos2)
    {
        if ((System.Math.Abs(pos1.x - pos2.x) == 1 && System.Math.Abs(pos1.y - pos2.y) == 0) ||
               (System.Math.Abs(pos1.x - pos2.x) == 0 && System.Math.Abs(pos1.y - pos2.y) == 1))
        {
            return true;
        }

        return false;
        
    }
}
