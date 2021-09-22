using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : MonoBehaviour
{
    
    public GameManagerAI gms;
    private ChessPieceAI kingScript;
    public ChessPieceAI currentChessPiece = null;
    int index = 0;

    [SerializeField]
    private List<ChessPieceAI> toMoveList;
    [SerializeField]
    private List<ChessPieceAI> toMove2List;
    [SerializeField]
    private List<int> xList;
    [SerializeField]
    private List<int> x2List;
    [SerializeField]
    private List<int> yList;
    [SerializeField]
    private List<int> y2List;
    [SerializeField]
    private List<int> scoreList;
    [SerializeField]
    private List<int> indexList;
    private List<GameObject[,]> boards;

    public void GetBotMove()
    {
        StartCoroutine(BotMove());
    }
    public IEnumerator BotMove()
    {
        yield return new WaitForSeconds(0.6f);

        GenerateMoves(null, null, false);
        ValidateMoves();
        EvaluateMoves();
        if (toMoveList[index] == null) GetBotMove();
        else
        {
            DoBotMove();
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < gms.kings.Count; i++)
            {
                gms.IsInCheck(gms.kings[i].GetComponent<ChessPieceAI>().GetXBoard(), gms.kings[i].GetComponent<ChessPieceAI>().GetYBoard(), gms.positions);
            }
            gms.IsCheckMate();
            gms.StartNextTurn();
        }
    }

    #region Move Generation

    public void GenerateMoves(Position position, List<Position> list, bool minimax)
    {
        toMoveList = new List<ChessPieceAI>();
        toMove2List = new List<ChessPieceAI>();
        xList = new List<int>();
        x2List = new List<int>();
        yList = new List<int>();
        y2List = new List<int>();
        scoreList = new List<int>();
        indexList = new List<int>();

        for (int i = 0; i < gms.kings.Count; i++)
        {
            if (gms.kings[i].GetComponent<ChessPieceAI>().player == gms.GetCurrentPlayer()) kingScript = gms.kings[i].GetComponent<ChessPieceAI>();
        }

        if (kingScript.player == "white")
        {
            for (int i = 0; i < gms.playerWhite.Length; i++)
            {
                if (gms.playerWhite[i] != null && gms.playerWhite[i].GetComponent<ChessPieceAI>() != null)
                {
                    int xBoard = currentChessPiece.GetXBoard();
                    int yBoard = currentChessPiece.GetYBoard();
                    bool moved = currentChessPiece.moved;
                    currentChessPiece = gms.playerWhite[i].GetComponent<ChessPieceAI>();
                    for (int ix = 0; ix < 8; ix++)
                    {
                        for (int iy = 0; iy < 8; iy++)
                        {
                            if (board[ix,iy] == currentChessPiece.gameObject)
                            {
                                xBoard = ix;
                                yBoard = iy;
                            }
                        }
                    }

                    switch (gms.playerWhite[i].name)
                    {
                        case "blackQueen":
                        case "whiteQueen":
                            LineAdd(1, 0, xBoard, yBoard, currentChessPiece);
                            LineAdd(0, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(1, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, 0, xBoard, yBoard, currentChessPiece);
                            LineAdd(0, -1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, -1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(1, -1, xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackKnight":
                        case "whiteKnight":
                            LAdd(xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackBishop":
                        case "whiteBishop":
                            LineAdd(1, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(1, -1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, -1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, 1, xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackRook":
                        case "whiteRook":
                            LineAdd(1, 0, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, 0, xBoard, yBoard, currentChessPiece);
                            LineAdd(0, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(0, -1, xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackKing":
                        case "whiteKing":
                            SurroundAdd(xBoard, yBoard, currentChessPiece);
                            CastleAdd(xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackPawn":
                            PawnAdd(xBoard, yBoard - 1, true, currentChessPiece);
                            if (!moved && gms.GetComponent<GameManagerAI>().GetPosition(xBoard, yBoard - 1) == null)
                            {
                                PawnAdd(xBoard, yBoard - 2, false, currentChessPiece);
                            }
                            break;
                        case "whitePawn":
                            PawnAdd(xBoard, yBoard + 1, true, currentChessPiece);
                            if (!moved && gms.GetComponent<GameManagerAI>().GetPosition(xBoard, yBoard + 1) == null)
                            {
                                PawnAdd(xBoard, yBoard + 2, false, currentChessPiece);
                            }
                            break;
                    }
                }
            }
        }
        if (kingScript.player == "black")
        {
            for (int i = 0; i < gms.playerBlack.Length; i++)
            {
                if (gms.playerBlack[i] != null && gms.playerBlack[i].GetComponent<ChessPieceAI>() != null)
                {
                    ChessPieceAI currentChessPiece = gms.playerBlack[i].GetComponent<ChessPieceAI>();
                    int xBoard = currentChessPiece.GetXBoard();
                    int yBoard = currentChessPiece.GetYBoard();
                    bool moved = currentChessPiece.moved;

                    switch (gms.playerBlack[i].name)
                    {
                        case "blackQueen":
                        case "whiteQueen":
                            LineAdd(1, 0, xBoard, yBoard, currentChessPiece);
                            LineAdd(0, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(1, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, 0, xBoard, yBoard, currentChessPiece);
                            LineAdd(0, -1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, -1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(1, -1, xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackKnight":
                        case "whiteKnight":
                            LAdd(xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackBishop":
                        case "whiteBishop":
                            LineAdd(1, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(1, -1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, -1, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, 1, xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackRook":
                        case "whiteRook":
                            LineAdd(1, 0, xBoard, yBoard, currentChessPiece);
                            LineAdd(-1, 0, xBoard, yBoard, currentChessPiece);
                            LineAdd(0, 1, xBoard, yBoard, currentChessPiece);
                            LineAdd(0, -1, xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackKing":
                        case "whiteKing":
                            SurroundAdd(xBoard, yBoard, currentChessPiece);
                            CastleAdd(xBoard, yBoard, currentChessPiece);
                            break;
                        case "blackPawn":
                            PawnAdd(xBoard, yBoard - 1, true, currentChessPiece);
                            if (!moved && gms.GetComponent<GameManagerAI>().GetPosition(xBoard, yBoard - 1) == null)
                            {
                                PawnAdd(xBoard, yBoard - 2, false, currentChessPiece);
                            }
                            break;
                        case "whitePawn":
                            PawnAdd(xBoard, yBoard + 1, true, currentChessPiece);
                            if (!moved && gms.GetComponent<GameManagerAI>().GetPosition(xBoard, yBoard + 1) == null)
                            {
                                PawnAdd(xBoard, yBoard + 2, false, currentChessPiece);
                            }
                            break;
                    }
                }
            }
        }
    }

    private void Add(bool minimax, ChessPieceAI toMove, ChessPieceAI toMove2, int xDest, int xDest2, int yDest, int yDest2, List<Position> list)
    {
        if (!minimax)
        {
            toMoveList.Add(toMove);
            toMove2List.Add(toMove2);
            xList.Add(xDest);
            x2List.Add(xDest2);
            yList.Add(yDest);
            y2List.Add(yDest2);
            scoreList.Add(0);
        }
        else
        {
            list.Add(new Position(toMove, toMove2, xDest, xDest2, yDest, yDest2));
        }
    }

    public void PointAdd(int x, int y, ChessPieceAI currentChessPiece)
    {
        if (gms.PositionOnBoard(x, y))
        {
            if (gms.GetPosition(x, y) == null)
            {
                Add(false, currentChessPiece, null, x, 0, y, 0, null);
            }
            else if (gms.GetPosition(x, y).GetComponent<ChessPieceAI>().player != currentChessPiece.player)
            {
                Add(true, currentChessPiece, null, x, 0, y, 0, null);
            }
        }
    }

    public void LineAdd(int xIncrement, int yIncrement, int xBoard, int yBoard, ChessPieceAI currentChessPiece)
    {

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (gms.PositionOnBoard(x, y) && gms.GetPosition(x, y) == null)
        {
            Add(currentChessPiece, null, x, 0, y, 0);
            x += xIncrement;
            y += yIncrement;
        }

        if (gms.PositionOnBoard(x, y) && gms.GetPosition(x, y).GetComponent<ChessPieceAI>().player != currentChessPiece.player)
        {
            Add(currentChessPiece, null, x, 0, y, 0);
        }
    }

    public void LAdd(int xBoard, int yBoard, ChessPieceAI currentChessPiece)
    {
        if (currentChessPiece != null)
        {
            if (gms.PositionOnBoard(xBoard + 1, yBoard + 2) && gms.GetPosition(xBoard + 1, yBoard + 2) == null) PointAdd(xBoard + 1, yBoard + 2, currentChessPiece);
            else if (gms.PositionOnBoard(xBoard + 1, yBoard + 2) && gms.GetPosition(xBoard + 1, yBoard + 2) != null && gms.GetPosition(xBoard + 1, yBoard + 2).GetComponent<ChessPieceAI>().player != currentChessPiece.player) Add(currentChessPiece, null, xBoard + 1, 0, yBoard + 2, 0);
            PointAdd(xBoard - 1, yBoard + 2, currentChessPiece);
            PointAdd(xBoard + 2, yBoard + 1, currentChessPiece);
            PointAdd(xBoard + 2, yBoard - 1, currentChessPiece);
            PointAdd(xBoard + 1, yBoard - 2, currentChessPiece);
            PointAdd(xBoard - 1, yBoard - 2, currentChessPiece);
            PointAdd(xBoard - 2, yBoard + 1, currentChessPiece);
            PointAdd(xBoard - 2, yBoard - 1, currentChessPiece);
        }
    }

    public void SurroundAdd(int xBoard, int yBoard, ChessPieceAI currentChessPiece)
    {
        PointAdd(xBoard, yBoard + 1, currentChessPiece);
        PointAdd(xBoard, yBoard - 1, currentChessPiece);
        PointAdd(xBoard + 1, yBoard, currentChessPiece);
        PointAdd(xBoard - 1, yBoard, currentChessPiece);
        PointAdd(xBoard + 1, yBoard + 1, currentChessPiece);
        PointAdd(xBoard + 1, yBoard - 1, currentChessPiece);
        PointAdd(xBoard - 1, yBoard - 1, currentChessPiece);
        PointAdd(xBoard - 1, yBoard + 1, currentChessPiece);
    }

    public void CastleAdd(int xBoard, int yBoard, ChessPieceAI currentChessPiece)
    {
        int x = xBoard;
        int y = yBoard;

        //short
        if (!currentChessPiece.moved && !currentChessPiece.inCheck)
        {
            if (gms.GetPosition(x + 1, y) == null && gms.GetPosition(x + 2, y) == null && gms.GetPosition(x + 3, y) != null)
            {
                GameObject rook = gms.GetPosition(x + 3, y);
                if (gms.GetPosition(x + 3, y).name == "blackRook" || gms.GetPosition(x + 3, y).name == "whiteRook")
                {
                    if (rook.GetComponent<ChessPieceAI>().player == currentChessPiece.player && !rook.GetComponent<ChessPieceAI>().moved)
                    {
                        if (gms.IsLegalMove(currentChessPiece, x + 1, y, currentChessPiece, null, 0, 0) &&
                            gms.IsLegalMove(currentChessPiece, x + 2, y, currentChessPiece, null, 0, 0) &&
                            gms.IsLegalMove(currentChessPiece, x + 2, y, currentChessPiece, rook.GetComponent<ChessPieceAI>(), x + 1, y))
                        {
                            Add(currentChessPiece, rook.GetComponent<ChessPieceAI>(), x + 2, x + 1, y, y);
                        }
                    }
                }
            }
        }
        if (!currentChessPiece.moved && !currentChessPiece.inCheck)
        {
            if (gms.GetPosition(x - 1, y) == null && gms.GetPosition(x - 2, y) == null && gms.GetPosition(x - 3, y) == null && gms.GetPosition(x - 4, y) != null)
            {
                GameObject rook = gms.GetPosition(x - 4, y);
                if (gms.GetPosition(x - 4, y).name == "blackRook" || gms.GetPosition(x - 4, y).name == "whiteRook")
                {
                    if (rook.GetComponent<ChessPieceAI>().player == currentChessPiece.player && !rook.GetComponent<ChessPieceAI>().moved)
                    {
                        if (gms.IsLegalMove(currentChessPiece, x - 1, y, currentChessPiece, null, 0, 0) &&
                            gms.IsLegalMove(currentChessPiece, x - 2, y, currentChessPiece, null, 0, 0) &&
                            gms.IsLegalMove(currentChessPiece, x - 2, y, currentChessPiece, rook.GetComponent<ChessPieceAI>(), x - 1, y))
                        {
                            Add(currentChessPiece, rook.GetComponent<ChessPieceAI>(), x - 2, x - 1, y, y);
                        }
                    }
                }
            }
        }

    }

    public void PawnAdd(int x, int y, bool canAttack, ChessPieceAI currentChessPiece)
    {
        if (gms.PositionOnBoard(x, y))
        {
            if (gms.GetPosition(x, y) == null)
            {
                Add(currentChessPiece, null, x, 0, y, 0);
            }

            if (gms.PositionOnBoard(x + 1, y) && gms.GetPosition(x + 1, y) != null &&
                gms.GetPosition(x + 1, y).GetComponent<ChessPieceAI>().player != currentChessPiece.player && canAttack)
            {
                Add(currentChessPiece, null, x + 1, 0, y, 0);
            }

            if (gms.PositionOnBoard(x - 1, y) && gms.GetPosition(x - 1, y) != null &&
                gms.GetPosition(x - 1, y).GetComponent<ChessPieceAI>().player != currentChessPiece.player && canAttack)
            {
                Add(currentChessPiece, null, x - 1, 0, y, 0);
            }
        }
    }

    #endregion

    public void ValidateMoves()
    {
        List<ChessPieceAI> toMoveTemp = new List<ChessPieceAI>(toMoveList);
        List<ChessPieceAI> toMove2Temp = new List<ChessPieceAI>(toMove2List);
        List<int> xTemp = new List<int>(xList);
        List<int> x2Temp = new List<int>(x2List);
        List<int> yTemp = new List<int>(yList);
        List<int> y2Temp = new List<int>(y2List);

        toMoveList = new List<ChessPieceAI>();
        toMove2List = new List<ChessPieceAI>();
        xList = new List<int>();
        x2List = new List<int>();
        yList = new List<int>();
        y2List = new List<int>();

        for (int i = 0; i < toMoveTemp.Count; i++)
        {
            if (gms.IsLegalMove(toMoveTemp[i], xTemp[i], yTemp[i], kingScript, toMove2Temp[i], x2Temp[i], y2Temp[i])) Add(toMoveTemp[i], toMove2Temp[i], xTemp[i], x2Temp[i], yTemp[i], y2Temp[i]);
        }
    }

    private void DoBotMove()
    {
        int rand = 0;
        if (toMoveList.Count > 0) rand = UnityEngine.Random.Range(0, toMoveList.Count);
        if (toMoveList.Count == 0) return;

        if (gms.GetPosition(xList[index], yList[index]) != null)
        {
            GameObject chessPiece = gms.GetPosition(xList[index], yList[index]);
            if (chessPiece.GetComponent<ChessPieceAI>().player == "black")
            {
                gms.blackCount--;
            }
            if (chessPiece.GetComponent<ChessPieceAI>().player == "white")
            {
                gms.whiteCount--;
            }
            Destroy(chessPiece);
        }
        int x = toMoveList[index].GetXBoard();
        int y = toMoveList[index].GetYBoard();
        toMoveList[index].moved = true;
        toMoveList[index].SetXBoard(xList[index]);
        toMoveList[index].SetYBoard(yList[index]);
        toMoveList[index].SetCoords();
        gms.GetComponent<GameManagerAI>().SetPosition(toMoveList[index].gameObject);
        gms.SetPositionEmpty(x, y);

        Debug.Log("Moved " + toMoveList[index] + " to " + xList[index] + ", " + yList[index]);


        if (toMove2List[index] != null)
        {
            x = toMove2List[index].GetXBoard();
            y = toMove2List[index].GetYBoard();

            toMove2List[index].moved = true;
            toMove2List[index].SetXBoard(x2List[index]);
            toMove2List[index].SetYBoard(y2List[index]);
            toMove2List[index].SetCoords();
            gms.GetComponent<GameManagerAI>().SetPosition(toMove2List[index].gameObject);
            gms.SetPositionEmpty(x, y);
        }


    }

    public void EvaluateMoves()
    {

    }

    public int EvaluateMove(Position position)
    {
        int totalEval = 0;

        for (int currX = 0; currX < 8; currX++)
        {
            for (int currY = 0; currY < 8; currY++)
            {
                if (board[currX, currY] != null)
                {
                    switch (board[currX, currY].name)
                    {
                        case "blackQueen":
                            totalEval = totalEval - 90;
                            break;
                        case "whiteQueen":
                            totalEval = totalEval + 90;
                            break;
                        case "blackKnight":
                            totalEval = totalEval - 30;
                            break;
                        case "whiteKnight":
                            totalEval = totalEval + 30;
                            break;
                        case "blackBishop":
                            totalEval = totalEval - 30;
                            break;
                        case "whiteBishop":
                            totalEval = totalEval + 30;
                            break;
                        case "blackRook":
                            totalEval = totalEval - 50;
                            break;
                        case "whiteRook":
                            totalEval = totalEval + 50;
                            break;
                        case "blackKing":
                            totalEval = totalEval - 900;
                            break;
                        case "whiteKing":
                            totalEval = totalEval + 900;
                            break;
                        case "blackPawn":
                            totalEval = totalEval - 10;
                            break;
                        case "whitePawn":
                            totalEval = totalEval + 10;
                            break;
                    }
                }
            }
        }

        return totalEval;

    }

    private int Minimax(Position position, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        if (depth == 0/*|| gameOver*/) return EvaluateMove(position);

        if (maximizingPlayer)
        {
            int maxEval = -999999;
            List<Position> positions = new List<Position>();
            GenerateMoves(position, positions);
            for (int i = 0; i < positions.Count; i++)
            {
                int eval = Minimax(positions[i], depth - 1, false);
                maxEval = Math.Max(maxEval, eval);
            }
            return maxEval;
        }
        else
        {
            int minEval = 999999;
            List<Position> positions = new List<Position>();
            GenerateMoves(position, positions);
            for (int i = 0; i < positions.Count; i++)
            {
                int eval = Minimax(positions[i], depth - 1, true);
                minEval = Math.Min(minEval, eval);
            }
            return minEval;
        }

    }
}
