using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public static class MoveCheck
{

    static GameManager gms;

    

    public static bool IsLegalMove(GameManager gmScript, ChessPiece toMove, int xDest, int yDest, ChessPiece king)
    {
        gms = gmScript;
        GameObject[,] tempPositions = new GameObject[8, 8];
        Array.Copy(gms.positions, tempPositions, gms.positions.Length);
        int x = toMove.GetXBoard();
        int y = toMove.GetYBoard();

        tempPositions[xDest, yDest] = toMove.gameObject;
        tempPositions[x, y] = null;

        //Debug.Log(IsInCheck(king, tempPositions));
        if (IsInCheck(king, tempPositions)) return false;


        return true;
    }

    public static bool IsInCheck(ChessPiece king, GameObject[,] board)
    {

        if (LineCheck(0, 1, "Rook", king, board)) return true;
        if (LineCheck(1, 0, "Rook", king, board)) return true;
        if (LineCheck(0, -1, "Rook", king, board)) return true;
        if (LineCheck(-1, 0, "Rook", king, board)) return true;

        if (LineCheck(1, 1, "Bishop", king, board)) return true;
        if (LineCheck(1, -1, "Bishop", king, board)) return true;
        if (LineCheck(-1, -1, "Bishop", king, board)) return true;
        if (LineCheck(-1, 1, "Bishop", king, board)) return true;

        if (LCheck(king, "Knight", board)) return true;

        if (SurroundCheck(king, "King", board)) return true;

        if (PawnCheck(king, board)) return true;

        if (LineCheck(0, 1, "Queen", king, board)) return true;
        if (LineCheck(1, 0, "Queen", king, board)) return true;
        if (LineCheck(0, -1, "Queen", king, board)) return true;
        if (LineCheck(-1, 0, "Queen", king, board)) return true;
        if (LineCheck(1, 1, "Queen", king, board)) return true;
        if (LineCheck(1, -1, "Queen", king, board)) return true;
        if (LineCheck(-1, -1, "Queen", king, board)) return true;
        if (LineCheck(-1, 1, "Queen", king, board)) return true;
        if (SurroundCheck(king, "Queen", board)) return true;

        return false;

    }

    public static bool CheckCheck(ChessPiece caller, int x, int y, string type, GameObject[,] board)
    {
        if (gms.PositionOnBoard(x, y))
        {
            if (board[x, y] != null)
            {
                if (board[x, y].GetComponent<ChessPiece>().name == "black" + type || board[x, y].GetComponent<ChessPiece>().name == "white" + type)
                {
                    if (board[x, y].GetComponent<ChessPiece>().player != caller.player)
                    {
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
        else return false;
    }

    public static bool LineCheck(int xIncrement, int yIncrement, string type, ChessPiece callerScript, GameObject[,] board)
    {
        int x = callerScript.GetXBoard();
        int y = callerScript.GetYBoard();

        x += xIncrement;
        y += yIncrement;

        if (CheckCheck(callerScript, x, y, type, board)) return true;

        if (gms.PositionOnBoard(x, y) && board[x, y] == null)
        {
            while (gms.PositionOnBoard(x, y) && board[x, y] == null)
            {
                x += xIncrement;
                y += yIncrement;
                if (CheckCheck(callerScript, x, y, type, board)) return true;
            }
        }

        return false;
    }
    public static bool LCheck(ChessPiece callerScript, string type, GameObject[,] board)
    {
        int x = callerScript.GetXBoard();
        int y = callerScript.GetYBoard();

        if (CheckCheck(callerScript, x + 1, y + 2, type, board)) return true;
        if (CheckCheck(callerScript, x - 1, y + 2, type, board)) return true;
        if (CheckCheck(callerScript, x + 2, y + 1, type, board)) return true;
        if (CheckCheck(callerScript, x + 2, y - 1, type, board)) return true;
        if (CheckCheck(callerScript, x + 1, y - 2, type, board)) return true;
        if (CheckCheck(callerScript, x - 1, y - 2, type, board)) return true;
        if (CheckCheck(callerScript, x - 2, y + 1, type, board)) return true;
        if (CheckCheck(callerScript, x - 2, y - 1, type, board)) return true;

        return false;
    }
    public static bool SurroundCheck(ChessPiece callerScript, string type, GameObject[,] board)
    {
        int x = callerScript.GetXBoard();
        int y = callerScript.GetYBoard();

        if (CheckCheck(callerScript, x, y + 1, type, board)) return true;
        if (CheckCheck(callerScript, x, y - 1, type, board)) return true;
        if (CheckCheck(callerScript, x + 1, y, type, board)) return true;
        if (CheckCheck(callerScript, x - 1, y, type, board)) return true;
        if (CheckCheck(callerScript, x + 1, y + 1, type, board)) return true;
        if (CheckCheck(callerScript, x + 1, y - 1, type, board)) return true;
        if (CheckCheck(callerScript, x - 1, y - 1, type, board)) return true;
        if (CheckCheck(callerScript, x - 1, y + 1, type, board)) return true;

        return false;
    }
    public static bool PawnCheck(ChessPiece callerScript, GameObject[,] board)
    {
        int x = callerScript.GetXBoard();
        int y = callerScript.GetYBoard();

        if (callerScript.name == "blackKing")
        {
            if (CheckCheck(callerScript, x + 1, y - 1, "Pawn", board)) return true;
            if (CheckCheck(callerScript, x - 1, y - 1, "Pawn", board)) return true;
        }
        if (callerScript.name == "whiteKing")
        {
            if (CheckCheck(callerScript, x + 1, y + 1, "Pawn", board)) return true;
            if (CheckCheck(callerScript, x - 1, y + 1, "Pawn", board)) return true;
        }

        return false;
    }
}
