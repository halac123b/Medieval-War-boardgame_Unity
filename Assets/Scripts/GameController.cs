using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using State = UnitController.State;
using GameMode = Mode.GameMode;

public class GameController : MonoBehaviour
{
    // References
    Text blueText;
    Text redText;
    Button blueBtn;
    Button redBtn;

    // Turn
    public enum Side { BLUE, RED }
    [SerializeField] private Side curPlayer;
    bool isGameOver = false;

    // AI
    GameMode gameMode;
    MinimaxAI ai;

    GameObject[,] boardPos = new GameObject[8, 8];
    List<GameObject> blueUnits = new List<GameObject>();
    List<GameObject> redUnits = new List<GameObject>();
    public GameObject unitPrefab;


    // --- Getters & Setters ------------------
    public GameObject GetUnitAt(int x, int y) { return boardPos[x, y]; }
    public GameObject GetUnitAt(Vector2Int pos) { return boardPos[pos.x, pos.y]; }

    public void SetUnitAt(int x, int y, GameObject unit)
    {
        boardPos[x, y] = unit;

        WinCheck();
    }

    void WinCheck()
    {
        // killing all enemy units
        if (blueUnits.Count == 0)
            endGame(Side.RED);
        else if (redUnits.Count == 0)
            endGame(Side.BLUE);

        // reaching enemy base
        if (curPlayer == Side.BLUE)
        {
            if (boardPos[7, 0] != null && boardPos[7, 0].GetComponent<UnitController>().GetSide() == Side.BLUE)
                endGame(Side.BLUE);
        }
        else
        {
            if (boardPos[0, 7] != null && boardPos[0, 7].GetComponent<UnitController>().GetSide() == Side.RED)
                endGame(Side.RED);
        }
    }

    public void setPositionEmpty(int x, int y) { boardPos[x, y] = null; }

    public Side getCurPlayer() { return this.curPlayer; }

    public bool isAI() { return ai != null; }

    public GameObject[,] getBoardPos()
    {
        return this.boardPos;
    }

    // --- Main --------------------------
    // Awake is called FIRST
    void Awake()
    {
        curPlayer = Side.BLUE;

        gameMode = Mode.mode;

        if (gameMode == GameMode.AI || gameMode == GameMode.AIHard)
        {
            ai = GameObject.FindWithTag("AI").GetComponent<MinimaxAI>();
            GameObject.FindWithTag("RedBtn").SetActive(false);

            if (gameMode == GameMode.AI)
                ai.isHardAI = false;
            else
                ai.isHardAI = true;
        }

        blueText = GameObject.FindWithTag("BlueText").GetComponent<Text>();
        redText = GameObject.FindWithTag("RedText").GetComponent<Text>();
    }

    void Start()
    {
        // show text
        blueText.enabled = true;
        redText.enabled = false;

        // CreateUnit("blue_villager", 2, 5);
        // CreateUnit("blue_warrior", 3, 5);
        // CreateUnit("blue_armor", 4, 5);
        // CreateUnit("blue_archer", 5, 5);
        // CreateUnit("blue_villager", 7, 1);
        // CreateUnit("red_villager", 5, 2);
        // CreateUnit("red_warrior", 4, 2);
        // CreateUnit("red_armor", 3, 2);
        // CreateUnit("red_archer", 2, 2);
        // CreateUnit("red_villager", 1, 7);

        CreateUnit("blue_villager", 1, 2);
        CreateUnit("blue_archer", 0, 4);
        CreateUnit("blue_armor", 2, 3);
        CreateUnit("blue_warrior", 3, 2);
        CreateUnit("red_villager", 6, 5);
        CreateUnit("red_archer", 7, 3);
        CreateUnit("red_armor", 5, 4);
        CreateUnit("red_warrior", 4, 5);
    }

    void Update()
    {
        if (isGameOver && Input.GetMouseButtonDown(0))
        {
            this.isGameOver = false;
            SceneManager.LoadScene("Game");
        }
    }

    // --------------------------------

    void CreateUnit(string uName, int x, int y)
    {
        // world position, NOT board position
        var pos = new Vector3(x, y, -1);

        GameObject unit = Instantiate(unitPrefab, pos, Quaternion.identity);
        UnitController uC = unit.GetComponent<UnitController>();
        uC.name = uName;

        uC.SetX((int)(pos.x));
        uC.SetY((int)(pos.y));

        uC.Activate();

        if (uC.GetSide() == Side.BLUE)
            blueUnits.Add(unit);
        else
            redUnits.Add(unit);

        boardPos[uC.GetX(), uC.GetY()] = unit;
    }

    public void RemoveUnitFromGame(GameObject unit)
    {
        if (unit.GetComponent<UnitController>().GetSide() == Side.BLUE)
        {
            blueUnits.Remove(unit);
        }
        else
        {
            redUnits.Remove(unit);
        }
        boardPos[unit.GetComponent<UnitController>().GetX(), unit.GetComponent<UnitController>().GetY()] = null;

        WinCheck();
    }

    // --- Turns & Game -----------------

    public void SetAllState(Side side, State state)
    {
        if (side == Side.BLUE)
            foreach (GameObject blueUnit in blueUnits)
                blueUnit.GetComponent<UnitController>().SetState(state);
        else
            foreach (GameObject redUnit in redUnits)
                redUnit.GetComponent<UnitController>().SetState(state);

    }

    public void DestroyAllTiles(Side side)
    {
        if (side == Side.BLUE)
            foreach (GameObject blueUnit in blueUnits)
                blueUnit.GetComponent<UnitController>().destroyTiles("Both");
        else
            foreach (GameObject blueUnit in blueUnits)
                blueUnit.GetComponent<UnitController>().destroyTiles("Both");
    }

    public void StartBlueTurn()
    {
        if (curPlayer == Side.RED)
        {
            DestroyAllTiles(Side.RED);
            SetAllState(Side.RED, State.WAIT);

            this.curPlayer = Side.BLUE;
            blueText.enabled = true;
            redText.enabled = false;

            SetAllState(Side.BLUE, State.READY);
        }
    }

    public void StartRedTurn()
    {
        if (curPlayer == Side.BLUE)
        {
            DestroyAllTiles(Side.BLUE);
            SetAllState(Side.BLUE, State.WAIT);

            this.curPlayer = Side.RED;
            redText.enabled = true;
            blueText.enabled = false;

            SetAllState(Side.RED, State.READY);

            if (gameMode == GameMode.AIHard || gameMode == GameMode.AI)
            {
                int numOfUnits = this.redUnits.Count;
                StartCoroutine(playATurn(ai, numOfUnits));
            }
        }
    }

    IEnumerator playATurn(MinimaxAI ai, int numberOfLoops)
    {
        yield return new WaitForSeconds(1);

        ai.playAITurn();
        if (numberOfLoops > 1)
            StartCoroutine(playATurn(ai, numberOfLoops - 1));
        else {
            if (!isGameOver)
                StartBlueTurn();
        }
    }

    public bool isGameEnd() { return this.isGameOver; }

    public void endGame(Side winner)
    {
        this.isGameOver = true;

        if (winner == Side.BLUE)
        {
            blueText.enabled = true;
            blueText.text = $"BLUE Wins!";
        }
        else
        {
            redText.enabled = true;
            redText.text = $"RED Wins!";
        }

    }


    // --- Helpers ---------------
    public bool isValidPos(int x, int y)
    {
        if (x < 0 || y < 0 || x >= 8 || y >= 8)
            return false;
        else return true;
    }

    public bool isValidPos(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= 8 || pos.y >= 8)
            return false;
        else return true;
    }
}
