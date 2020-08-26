using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets instance
    {
        get 
        {
            if (_i == null) 
                _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>(); ;
                return _i;
        }
    }

    //Player:
    public Transform projectile;
    public Transform unit;
    public Transform tower;
    public Transform path;
    public Transform barracks;

    //AI:
    public Transform barracksAI;
    public Transform towerAI;

    //UI:
    public Transform DamagePopUp;
}
