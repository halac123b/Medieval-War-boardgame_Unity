using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Side = GameController.Side;
using State = UnitController.State;

public class AtkTile : MonoBehaviour
{
    public GameObject game;
    GameController gC;
    GameObject unit;    // which unit "owns" this atk tile
    UnitController uC;

    public int x;
    public int y;

    // --- Getters & Setters -------------------
    public GameObject getCurrentlyUnitInUsed() { return unit; }
    public void setUnitOwnAtkTile(GameObject obj)
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
        // combat

        // uC.SetX(x);
        // uC.SetY(y);
        // uC.UpdatePos();
        uC.destroyTiles("AtkTile");


        // COMBAT
        CombatController combat = GameObject.FindWithTag("CombatController").GetComponent<CombatController>();
        UnitController attacker = uC;
        UnitController defender = gC.GetUnitAt(this.x, this.y).GetComponent<UnitController>();

        combat.Attack(attacker, defender);

        uC.SetState(State.END);
        //gC.SetAllState(uC.GetSide(), State.END);
    }



    public void setWorldPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
