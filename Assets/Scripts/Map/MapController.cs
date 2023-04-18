using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Terrain = MapTile.Terrain;

public class MapController : MonoBehaviour {
	Tilemap terrainTilemap;

    MapTile[,] mapTiles = new MapTile[8,8];
    bool[,] isVisited = new bool[8,8];

    // Sprites for comparison
    public Sprite plains, forest, fort, blueFort, redFort, base_, BFlag, RFlag;
    List<Sprite> compareSprites = new List<Sprite>();

    // ---------------------------------------

	void Awake() {
        compareSprites.Add(plains);
        compareSprites.Add(forest);
        compareSprites.Add(fort);
        compareSprites.Add(blueFort);
        compareSprites.Add(redFort);
        compareSprites.Add(base_);
        compareSprites.Add(BFlag);
        compareSprites.Add(RFlag);

		GetMapTiles();

        ResetVisit();
	}

    // ---- Functions -----------------------------

	void GetMapTiles() {
        terrainTilemap = GameObject.FindWithTag("MainTilemap").GetComponent<Tilemap>();

        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                Vector3 pos = new Vector3((float)x - 4.0f, (float)y - 4.0f, 0.0f);
                Vector3Int cellPos = Vector3Int.FloorToInt(pos);
                Sprite spriteAt = terrainTilemap.GetSprite(cellPos);
                
                Terrain tileTerrain = GetTerrainFromSprite(spriteAt);

                mapTiles[x,y] = (MapTile)ScriptableObject.CreateInstance("MapTile");
                mapTiles[x,y].Instantiate(tileTerrain);
            }
        }
	}

    Terrain GetTerrainFromSprite(Sprite sprite) {
        for (int i = 0; i < compareSprites.Count; i++)
            if (sprite == compareSprites[i])
                return (Terrain)i;

        return Terrain.Unmovable;       // Doesn't match anything else, is unmovable
    }

    public float GetTerrainDef(Vector2Int pos) { return mapTiles[pos.x, pos.y].GetDef(); }

    public int GetTerrainCost(Vector2Int pos) { return mapTiles[pos.x, pos.y].GetCost(); }

    public Terrain GetTerrain(Vector2Int pos) { return mapTiles[pos.x, pos.y].GetTerrain(); }

    
    // ---- For flood fill -----------------------

    public bool IsVisited(Vector2Int pos) { return isVisited[pos.x, pos.y]; }
    public void Visit(int x, int y) { isVisited[x,y] = true; }
    public void ResetVisit() {
        for (int x = 0; x < 8; x++)
            for (int y = 0; y < 8; y++)
                isVisited[x,y] = false;        
    }
}