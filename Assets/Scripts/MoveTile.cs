using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Side = GameController.Side;
using State = UnitController.State;

public class MoveTile : MonoBehaviour
{
    public GameObject game;
    GameController gC;
    GameObject unit;    // which unit "owns" this movable tile
    UnitController uC;

    public int x;
    public int y;

    // --- Getters & Setters -------------------
    public GameObject getCurrentlyUnitInUsed() { return unit; }
    public void setUnitOwnMoveTile(GameObject obj)
    {
        unit = obj;
        uC = unit.GetComponent<UnitController>();
    }

    //--------------------------------------
    // Awake is called FIRST
    void Awake()
    {
        game = GameObject.FindWithTag("GameController");
        gC = game.GetComponent<GameController>();
    }

    void Start() { }

    void Update() { }

    //-----------------------------------------

    public void OnMouseUp()
    {
        if (PauseController.isPaused) return;

        // TODO:
        // if (this.isAttack)
        // {
        //     //? Destroy obj currently on pos if move to that pos
        //     GameObject obj = gC.GetUnitAt(x, y);
        //     Destroy(obj);
        // }

        //? move obj to pos
        gC.setPositionEmpty(
            uC.GetX(),
            uC.GetY()
        );

        uC.SetX(x);
        uC.SetY(y);
        uC.UpdatePos();
        uC.destroyTiles("MoveTile");

        gC.SetUnitAt(x, y, this.unit);

        //gC.SetAllState(uC.GetSide(), State.END);
        uC.SetState(State.MOVED);


    }



    public void setWorldPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
