using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessType
{
    None = 0,
    Black = 1,
    White = 2,
}

public class BoardModel {
    public const int WinChessCount = 5;
    ChessType[,] _data = new ChessType[Board.CrossCount, Board.CrossCount];

    public ChessType Get(int x, int y)
    {
        if (x < 0 || x >= Board.CrossCount)
        {
            return ChessType.None;
        }

        if (y < 0 || y >= Board.CrossCount)
        {
            return ChessType.None;
        }

        return _data[x, y];
    }

    public bool Set(int x, int y, ChessType chessType)
    {
        if (x < 0 || x >= Board.CrossCount)
        {
            return false;
        }
        if (y < 0 || y >= Board.CrossCount)
        {
            return false;
        }

        _data[x, y] = chessType;
        return true;
    }

    int CheckVLink (int x, int y, ChessType chessType)
    {
        int linkCount = 1;
        for (int i = y + 1; i < Board.CrossCount; ++ i)
        {
            if (Get(x, i) == chessType)
            {
                ++linkCount;
                if (linkCount >= WinChessCount)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = y - 1; i >= 0; -- i)
        {
            if (Get(x, i) == chessType)
            {
                ++linkCount;
                if (linkCount >= WinChessCount)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        return linkCount;
    }

    int CheckHLink (int x, int y, ChessType chessType)
    {
        int linkCount = 1;
        for (int i = x + 1; i < Board.CrossCount; ++i)
        {
            if (Get(i, y) == chessType)
            {
                ++linkCount;
                if (linkCount >= WinChessCount)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        for (int i = x - 1; i >= 0; --i)
        {
            if (Get(i, y) == chessType)
            {
                ++linkCount;
                if (linkCount >= WinChessCount)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        return linkCount;
    }

    int CheckBLink (int x, int y, ChessType chessType)
    {
        int linkCount = 1;
        for (int i = x - 1, j = y + 1; i >= 0 && j < Board.CrossCount; -- i, ++ j)
        {
            if (Get(i, j) == chessType)
            {
                ++linkCount;
                if (linkCount >= WinChessCount)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }
        
        for (int i = x + 1, j = y - 1; x < Board.CrossCount && y >= 0; ++ i, -- j)
        {
            if (Get(i, j) == chessType)
            {
                ++linkCount;
                if (linkCount >= WinChessCount)
                {
                    return linkCount;
                }
            }
            else
            {
                break;
            }
        }

        int linkCount2 = 1;
        for (int i = x - 1, j = y - 1; i >= 0 &&　j >= 0; --i, -- j)
        {
            if (Get(i, j) == chessType)
            {
                ++linkCount2;
                if (linkCount2 >= WinChessCount)
                {
                    return linkCount2;
                }
            }
            else
            {
                break;
            }
        }
        for (int i = x + 1, j = y + 1; i < Board.CrossCount && j < Board.CrossCount; ++ i , ++ j)
        {
            if (Get(i, j) == chessType)
            {
                ++linkCount2;
                if (linkCount2 >= WinChessCount)
                {
                    return linkCount2;
                }
            }
            else
            {
                break;
            }
        }

        return Mathf.Max(linkCount, linkCount2);
    }

    public int CheckLink (int x, int y, ChessType chessType)
    {
        int linkCount = 0;
        linkCount = CheckBLink(x, y, chessType);
        linkCount = Mathf.Max(CheckVLink(x, y, chessType), linkCount);
        linkCount = Mathf.Max(CheckHLink(x, y, chessType), linkCount);

        return linkCount;
    }
}


