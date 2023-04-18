using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapTile : Tile {

    public enum Terrain { Plains, Forest, Fort, BlueFort, RedFort, Base, BFlag, RFlag, Unmovable }
    Terrain terrain;
    
    public bool isVisited = false;

    float def;
    int gold;
    int mvCost;
    Sprite _sprite;

    // ---- Getters & Setters -----------------------

    public Terrain GetTerrain() { return terrain; }
    public float GetDef() { return def; }
    public int GetCost() { return mvCost; }

    // ---- Main ----------------------
    public void Instantiate(Terrain _terrain) {
        terrain = _terrain;

        switch(terrain) {
            case Terrain.Plains:
                mvCost = 1;
                def = 1;
                break;
            case Terrain.Forest:
                mvCost = 2;
                def = 0.9f;
                break;
            case Terrain.Unmovable:
                mvCost = 100;
                def = 0;
                break;
            case Terrain.Fort:
            case Terrain.BlueFort:
            case Terrain.RedFort:
                mvCost = 1;
                def = 0.8f;
                break;
            case Terrain.Base:
                mvCost = 1;
                def = 0.7f;
                break;
            case Terrain.BFlag:
            case Terrain.RFlag:
                mvCost = 1;
                def = 1;
                break;
        }

        // TODO: gold for building
        if (terrain == Terrain.Base) {

        }
    }

}
