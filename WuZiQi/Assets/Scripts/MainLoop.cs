using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoop : MonoBehaviour {

    public GameObject whitePrefab;
    public GameObject blackPrefab;

    public ResultWindow resultWindow;

    enum State
    {
        BlackGo,
        WhiteGo,
        Over
    }

    State _state;
    Board _board;
    BoardModel _model;
    AI _ai;

    bool CanPlace (int x, int y)
    {
        return _model.Get(x, y) == ChessType.None;
    }

    bool PlaceChess(Cross cross, bool isBlack)
    {
        if (cross == null)
        {
            return false;
        }

        GameObject newChess = GameObject.Instantiate<GameObject>(isBlack ? blackPrefab : whitePrefab);
        newChess.transform.SetParent(cross.gameObject.transform, false);

        ChessType chessType = isBlack ? ChessType.Black : ChessType.White;
        _model.Set(cross.GridX, cross.GridY, chessType);

        int linkCount = _model.CheckLink(cross.GridX, cross.GridY, chessType);
        return linkCount >= BoardModel.WinChessCount;
    }

    public void Restart ()
    {
        _state = State.BlackGo;
        _model = new BoardModel();
        _ai = new AI();
        _board.Reset();
    }

    int lastPlayerX, lastPlayerY;
    public void OnClick (Cross cross)
    {
        if (_state != State.BlackGo)
        {
            return;
        }
        if (CanPlace(cross.GridX, cross.GridY))
        {
            lastPlayerX = cross.GridX;
            lastPlayerY = cross.GridY;

            if (PlaceChess(cross, true))
            {
                Debug.Log("you win!");
                _state = State.Over;
                ShowResult(ChessType.Black);
            } else
            {
                _state = State.WhiteGo;
            }
        }
    }

	// Use this for initialization
	void Start () {
        _board = GetComponent<Board>();
        Restart();
	}

    void ShowResult (ChessType winType)
    {
        resultWindow.gameObject.SetActive(true);
        resultWindow.Show(winType);
    }
	
	// Update is called once per frame
	void Update () {
		switch (_state)
        {
            case State.WhiteGo:
                {
                    int x, y;
                    _ai.ComputerDo(lastPlayerX, lastPlayerY, out x, out y);

                    if (PlaceChess(_board.GetCross(x, y), false))
                    {
                        _state = State.Over;
                        ShowResult(ChessType.White);
                        Debug.Log("computer win!");
                    }
                    else
                    {
                        _state = State.BlackGo;
                    }

                }
                break;
        }
	}
}
