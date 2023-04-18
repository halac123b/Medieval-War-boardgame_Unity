using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UType = UnitController.UType;

using Unit = MinimaxAI.Unit;

public class CombatController : MonoBehaviour
{
    MapController mapCtrl;

    // damage[attacker, defender] = float
    float[,] damage = new float[4, 4] {
        {  0,  0,  0,  0 },     // Villager
        { 50, 40, 40, 50 },     // Warrior
        { 40, 45, 35, 45 },     // Armor
        { 55, 50, 20, 55 }      // Archer
    };

    // -------------------------------------------

    void Awake()
    {
        mapCtrl = GameObject.FindWithTag("MapController").GetComponent<MapController>();
    }

    // Return int[2] of attacker and defender's HP after simulating combat
    // but don't actually change HP!!!!
    public int[] PredictDamage(UnitController attacker, UnitController defender)
    {
        int[] predictions = new int[2];

        // attack
        int aHP = attacker.GetHP();
        int dHP = defender.GetHP();

        float baseDmg = damage[(int)attacker.GetUType(), (int)defender.GetUType()];
        float HP_modifier = (float)aHP / 100;
        float aDef = mapCtrl.GetTerrainDef(attacker.GetPos());
        float dDef = mapCtrl.GetTerrainDef(defender.GetPos());

        int atkDmg = Mathf.RoundToInt(baseDmg * HP_modifier * dDef);

        predictions[1] = dHP - atkDmg;

        if (predictions[1] <= 0)
        {
            predictions[0] = aHP;

            return predictions;
        }

        else
        {  // counterattack
            if (defender.GetAtkRange() == attacker.GetAtkRange())
            {
                dHP = predictions[1];

                baseDmg = damage[(int)defender.GetUType(), (int)attacker.GetUType()];
                HP_modifier = (float)dHP / 100;

                int defDmg = Mathf.RoundToInt(baseDmg * HP_modifier * aDef);
                predictions[0] = aHP - defDmg;
            }
            else
                predictions[0] = aHP;

            return predictions;
        }
    }

    public void Attack(UnitController attacker, UnitController defender)
    {
        // attack
        int aHP = attacker.GetHP();
        int dHP = defender.GetHP();

        float baseDmg = damage[(int)attacker.GetUType(), (int)defender.GetUType()];
        float HP_modifier = (float)aHP / 100;
        float aDef = mapCtrl.GetTerrainDef(attacker.GetPos());
        float dDef = mapCtrl.GetTerrainDef(defender.GetPos());

        int atkDmg = Mathf.RoundToInt(baseDmg * HP_modifier * dDef);
        defender.SetHP(dHP - atkDmg);

        if (defender.GetHP() <= 0)
            defender.Die();
        else
        {  // counterattack
            if (defender.GetAtkRange() == attacker.GetAtkRange())
            {
                dHP = defender.GetHP();

                baseDmg = damage[(int)defender.GetUType(), (int)attacker.GetUType()];
                HP_modifier = (float)dHP / 100;

                int defDmg = Mathf.RoundToInt(baseDmg * HP_modifier * aDef);
                attacker.SetHP(aHP - defDmg);

                if (attacker.GetHP() <= 0)
                    attacker.Die();
            }
        }
    }

    public void attackStimulate(Unit attacker, Unit defender)
    {
        // attack
        int aHP = attacker.HP;
        int dHP = defender.HP;

        float baseDmg = damage[(int)attacker.unitType, (int)defender.unitType];
        float HP_modifier = (float)aHP / 100;
        float aDef = mapCtrl.GetTerrainDef(new Vector2Int(attacker.x, attacker.y));
        float dDef = mapCtrl.GetTerrainDef(new Vector2Int(defender.x, defender.y));

        int atkDmg = Mathf.RoundToInt(baseDmg * HP_modifier * dDef);
        defender.HP = dHP - atkDmg;

        if (defender.HP <= 0)
        { }
        else
        {  // counterattack
            if (defender.range == attacker.range)
            {
                dHP = defender.HP;

                baseDmg = damage[(int)defender.unitType, (int)attacker.unitType];
                HP_modifier = (float)dHP / 100;

                int defDmg = Mathf.RoundToInt(baseDmg * HP_modifier * aDef);
                attacker.HP = aHP - defDmg;
            }
        }
    }
}
