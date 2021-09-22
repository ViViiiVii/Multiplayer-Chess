using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class GameManagerAI : MonoBehaviour
{

    public GameObject chessPiecePrefab;
    public BotMovement botMovement;
    public Button restartButton;
    public Text timerText;
    public Text pieceCount;
    public Text winnerText;
    public List<GameObject> kings = new List<GameObject>();
    public SpriteRenderer blackTurnBar;
    public SpriteRenderer whiteTrunBar;

    public Button bishopImg;
    public Button rookImg;
    public Button knightImg;
    public Button queenImg;
    
    public Button bbishopImg;
    public Button brookImg;
    public Button bknightImg;
    public Button bqueenImg;
    public SpriteRenderer PawnBG;
    public GameObject chessBoard;

    public GameObject[,] positions = new GameObject[8, 8];
    public GameObject[,] tempPos;
    public GameObject[] playerBlack = new GameObject[16];
    public GameObject[] playerWhite = new GameObject[16];


    public string currentPlayer = "white";
    public string botPlayer = "black";
    public string chosenType;

    private bool gameOver = false;
    public bool counting = true;
    public bool choosingDone = false;
    public bool animReady = false;

    public float timer = 0f;
    public int roundTimer;
    public int timerMinute;
    public int secondsWithoutMinutes;

    public int blackCount = 16;
    public int whiteCount = 16;


    // Start is called before the first frame update
    void Start()
    {


        playerWhite = new GameObject[]
        {
            Create("whiteRook", 0, 0), Create("whiteKnight", 1, 0), Create("whiteBishop", 2, 0), Create("whiteQueen", 3, 0), Create("whiteKing", 4, 0),
            Create("whiteBishop", 5, 0), Create("whiteKnight", 6, 0), Create("whiteRook", 7, 0), Create("whitePawn", 0, 1), Create("whitePawn", 1, 1),
            Create("whitePawn", 2, 1), Create("whitePawn", 3, 1), Create("whitePawn", 4, 1), Create("whitePawn", 5, 1),
            Create("whitePawn", 6, 1), Create("whitePawn", 7, 1)
        };
        playerBlack = new GameObject[]
        {
            Create("blackRook", 0, 7), Create("blackKnight", 1, 7), Create("blackBishop", 2, 7), Create("blackQueen", 3, 7), Create("blackKing", 4, 7),
            Create("blackBishop", 5, 7), Create("blackKnight", 6, 7), Create("blackRook", 7, 7), Create("blackPawn", 0, 6), Create("blackPawn", 1, 6),
            Create("blackPawn", 2, 6), Create("blackPawn", 3, 6), Create("blackPawn", 4, 6), Create("blackPawn", 5, 6),
            Create("blackPawn", 6, 6), Create("blackPawn", 7, 6)
        };

        for (int i = 0; i < playerWhite.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }
    }

    private void FixedUpdate()
    {
        if (counting)
        {
            timer += 0.02f;
            roundTimer = Mathf.RoundToInt(timer);
            timerMinute = roundTimer / 60;
            secondsWithoutMinutes = roundTimer - timerMinute * 60;

            string displayMinutes = null;
            string displaySeconds = null;

            if (timerMinute.ToString().Length == 1)
            {
                displayMinutes = "0" + timerMinute.ToString();
            }
            else
            {
                displayMinutes = timerMinute.ToString();
            }
            if (secondsWithoutMinutes.ToString().Length == 1)
            {
                displaySeconds = "0" + secondsWithoutMinutes.ToString();
            }
            else
            {
                displaySeconds = secondsWithoutMinutes.ToString();
            }

            timerText.text = displayMinutes + ":" + displaySeconds;

            pieceCount.text = whiteCount.ToString() + " - " + blackCount.ToString();
        }

        
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chessPiecePrefab, Vector2.zero, Quaternion.identity);
        ChessPieceAI chessPiece = obj.GetComponent<ChessPieceAI>();

        chessPiece.name = name;
        chessPiece.SetXBoard(x);
        chessPiece.SetYBoard(y);
        chessPiece.Activate();

        return obj;

    }

    public void SetPosition(GameObject obj)
    {
        ChessPieceAI chessPiece = obj.GetComponent<ChessPieceAI>();

        positions[chessPiece.GetXBoard(), chessPiece.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
    public IEnumerator NextTurn()
    {

        // Pawn Trade
        if (GetCurrentPlayer() == "black")
        {

            choosingDone = false;

            ChessPieceAI caller = null;
            for (int i = 0; i < 8; i++)
            {
                if (GetPosition(i, 0) != null && GetPosition(i, 0).name == "blackPawn") caller = GetPosition(i, 0).GetComponent<ChessPieceAI>();
            }

            if (caller != null)
            {
                int x = caller.GetXBoard();
                int y = caller.GetYBoard();

                //choose a captured piece
                chosenType = "blackQueen";

                Destroy(caller.gameObject);
                    
                int index = 20;
                for (int i = 0; i < playerBlack.Length; i++)
                {
                    if (playerBlack[i] == null)
                    {
                        index = i;
                        i = playerBlack.Length;
                    }
                }

                //chosenType = "black" + chosenType;
                if (index != 20)
                {
                    playerBlack[index] = Create(chosenType, x, y);
                    
                }
            }

        }
        if (GetCurrentPlayer() == "white")
        {

            choosingDone = false;

            ChessPieceAI caller = null;
            for (int i = 0; i < 8; i++)
            {
                if (GetPosition(i, 7) != null && GetPosition(i, 7).name == "whitePawn") caller = GetPosition(i, 7).GetComponent<ChessPieceAI>();
            }

            if (caller != null)
            {
                int x = caller.GetXBoard();
                int y = caller.GetYBoard();

                //pull in choosingMenu
                PawnBG.transform.position += new Vector3(20f, 0f);
                bishopImg.gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(5000f, 0f);
                rookImg.gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(5000f, 0f);
                knightImg.gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(5000f, 0f);
                queenImg.gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(5000f, 0f);

                StartCoroutine(SpriteAnim("in", PawnBG));
                StartCoroutine(ImgAnim("in", bishopImg.GetComponent<Image>()));
                StartCoroutine(ImgAnim("in", rookImg.GetComponent<Image>()));
                StartCoroutine(ImgAnim("in", knightImg.GetComponent<Image>()));
                StartCoroutine(ImgAnim("in", queenImg.GetComponent<Image>()));
                yield return new WaitForSeconds(0.5f);
                animReady = false;

                //push out board
                for (int i = 0; i < playerBlack.Length; i++)
                {
                    if (playerBlack[i] != null)
                    {
                        playerBlack[i].transform.position -= new Vector3(20f, 0f);
                    }
                }
                for (int i = 0; i < playerWhite.Length; i++)
                {
                    if (playerWhite[i] != null)
                    {
                        playerWhite[i].transform.position -= new Vector3(20f, 0f);
                    }
                }
                chessBoard.transform.position -= new Vector3(20f, 0f);

                //choose a captured piece
                //chosenType = "blackQueen";

                yield return new WaitUntil(() => choosingDone);

                //pull in board
                for (int i = 0; i < playerBlack.Length; i++)
                {
                    if (playerBlack[i] != null)
                    {
                        playerBlack[i].transform.position += new Vector3(20f, 0f);
                    }
                }
                for (int i = 0; i < playerWhite.Length; i++)
                {
                    if (playerWhite[i] != null)
                    {
                        playerWhite[i].transform.position += new Vector3(20f, 0f);
                    }
                }
                chessBoard.transform.position += new Vector3(20f, 0f);

                animReady = false;
                StartCoroutine(SpriteAnim("out", PawnBG));
                StartCoroutine(ImgAnim("out", bishopImg.GetComponent<Image>()));
                StartCoroutine(ImgAnim("out", rookImg.GetComponent<Image>()));
                StartCoroutine(ImgAnim("out", knightImg.GetComponent<Image>()));
                StartCoroutine(ImgAnim("out", queenImg.GetComponent<Image>()));
                yield return new WaitForSeconds(0.5f);

                //push out choosingMenu
                PawnBG.transform.position -= new Vector3(20f, 0f);
                bishopImg.gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(5000f, 0f);
                rookImg.gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(5000f, 0f);
                knightImg.gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(5000f, 0f);
                queenImg.gameObject.GetComponent<RectTransform>().anchoredPosition -= new Vector2(5000f, 0f);

                int index = 20;

                for (int i = 0; i < playerWhite.Length; i++)
                {
                    if (playerWhite[i] == caller.gameObject) index = i;
                }

                Destroy(caller.gameObject);

                //chosenType = "black" + chosenType;
                if (index != 20)
                {
                    playerWhite[index] = Create(chosenType, x, y);

                }
            }

        }


        if (currentPlayer == "white")
        {
            currentPlayer = "black";
            StartCoroutine(TurnAnim("in", blackTurnBar));
            StartCoroutine(TurnAnim("out", whiteTrunBar));

        } else
        {
            currentPlayer = "white";
            StartCoroutine(TurnAnim("out", blackTurnBar));
            StartCoroutine(TurnAnim("in", whiteTrunBar));
        }

        GameObject[] checkMarks = GameObject.FindGameObjectsWithTag("CheckMark");
        /*for (int i = 0; i < checkMarks.Length; i++)
        {
            Destroy(checkMarks[i]);
        }*/

        for (int i = 0; i < kings.Count; i++)
        {
            kings[i].GetComponent<ChessPieceAI>().inCheck = false;
            if (IsInCheck(kings[i].GetComponent<ChessPieceAI>().GetXBoard(), kings[i].GetComponent<ChessPieceAI>().GetYBoard(), positions)) kings[i].GetComponent<ChessPieceAI>().inCheck = true;
        }

        //if (currentPlayer == botPlayer) botMovement.GetBotMove();

        IsCheckMate();

    }

    public void StartNextTurn()
    {
        StartCoroutine(NextTurn());
    }

    IEnumerator TurnAnim(string type, SpriteRenderer toAnimate)
    {
        float endPoint = 0.7372549f;
        int fragmentCount = 30;
        float duration = 0.25f;

        if (type == "in")
        {
            for (int i = 0; i < fragmentCount; i++)
            {
                toAnimate.color += new Color(0f, 0f, 0f, endPoint / fragmentCount);

                yield return new WaitForSeconds(duration / fragmentCount);
            }
        }
        else if (type == "out")
        {
            for (int i = 0; i < fragmentCount; i++)
            {
                toAnimate.color -= new Color(0f, 0f, 0f, endPoint / fragmentCount);

                if (i == fragmentCount-1)
                {
                    toAnimate.color = new Color(1f, 1f, 1f, 0f);
                }

                yield return new WaitForSeconds(duration / fragmentCount);
            }
        }
        animReady = true;
    }
    IEnumerator ImgAnim(string type, Image toAnimate)
    {
        float endPoint = 1;
        int fragmentCount = 30;
        float duration = 0.25f;

        if (type == "in")
        {
            for (int i = 0; i < fragmentCount; i++)
            {
                toAnimate.color += new Color(0f, 0f, 0f, endPoint / fragmentCount);

                yield return new WaitForSeconds(duration / fragmentCount);
            }
        }
        else if (type == "out")
        {
            for (int i = 0; i < fragmentCount; i++)
            {
                toAnimate.color -= new Color(0f, 0f, 0f, endPoint / fragmentCount);

                if (i == fragmentCount - 1)
                {
                    toAnimate.color = new Color(1f, 1f, 1f, 0f);
                }

                yield return new WaitForSeconds(duration / fragmentCount);
            }
        }
        animReady = true;
    }
    IEnumerator SpriteAnim(string type, SpriteRenderer toAnimate)
    {
        float endPoint = 1f;
        int fragmentCount = 30;
        float duration = 0.25f;

        if (type == "in")
        {
            for (int i = 0; i < fragmentCount; i++)
            {
                toAnimate.color += new Color(0f, 0f, 0f, endPoint / fragmentCount);

                yield return new WaitForSeconds(duration / fragmentCount);
            }
        }
        else if (type == "out")
        {
            for (int i = 0; i < fragmentCount; i++)
            {
                toAnimate.color -= new Color(0f, 0f, 0f, endPoint / fragmentCount);

                if (i == fragmentCount - 1)
                {
                    toAnimate.color = new Color(1f, 1f, 1f, 0f);
                }

                yield return new WaitForSeconds(duration / fragmentCount);
            }
        }
        animReady = true;
    }

    public void Update()
    {
        /*if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }*/
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        counting = false;


        restartButton.gameObject.SetActive(true);
        winnerText.gameObject.SetActive(true);
        winnerText.text = playerWinner + " WON!";
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool CheckCheck(ChessPieceAI caller, int x, int y, string type, GameObject[,] board)
    {
        if (PositionOnBoard(x, y))
        {
            if (board[x, y] != null)
            {
                if (board[x, y].GetComponent<ChessPieceAI>().name == "black" + type || board[x, y].GetComponent<ChessPieceAI>().name == "white" + type)
                {
                    if (board[x, y].GetComponent<ChessPieceAI>().player != caller.player)
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

    public bool IsInCheck(int x, int y, GameObject[,] board)
    {
        ChessPieceAI callerScript;
        if (board[x, y] != null) callerScript = board[x, y].GetComponent<ChessPieceAI>();

        if (LineCheck(0, 1, "Rook", x, y, board)) return true;
        if (LineCheck(1, 0, "Rook", x, y, board)) return true;
        if (LineCheck(-1, 0, "Rook", x, y, board)) return true;
        if (LineCheck(0, -1, "Rook", x, y, board)) return true;

        if (LineCheck(1, 1, "Bishop", x, y, board)) return true;
        if (LineCheck(1, -1, "Bishop", x, y, board)) return true;
        if (LineCheck(-1, -1, "Bishop", x, y, board)) return true;
        if (LineCheck(-1, 1, "Bishop", x, y, board)) return true;

        if (LCheck(x, y, "Knight", board)) return true;

        if (SurroundCheck(x, y, "King", board)) return true;

        if (PawnCheck(x, y, board)) return true;

        if (LineCheck(0, 1, "Queen", x, y, board)) return true;
        if (LineCheck(1, 0, "Queen", x, y, board)) return true;
        if (LineCheck(0, -1, "Queen", x, y, board)) return true;
        if (LineCheck(-1, 0, "Queen", x, y, board)) return true;
        if (LineCheck(1, 1, "Queen", x, y, board)) return true;
        if (LineCheck(1, -1, "Queen", x, y, board)) return true;
        if (LineCheck(-1, -1, "Queen", x, y, board)) return true;
        if (LineCheck(-1, 1, "Queen", x, y, board)) return true;
        if (SurroundCheck(x, y, "Queen", board)) return true;

        return false;

    }

    public bool LineCheck(int xIncrement, int yIncrement, string type, int x, int y, GameObject[,] board)
    {

        ChessPieceAI callerScript = null;
        if (board[x, y] != null) callerScript = board[x, y].GetComponent<ChessPieceAI>();

        x += xIncrement;
        y += yIncrement;

        if (callerScript != null)
        {
            if (CheckCheck(callerScript, x, y, type, board)) return true;
        }

        if (PositionOnBoard(x, y) && board[x, y] == null)
        {
            while (PositionOnBoard(x, y) && board[x, y] == null)
            {
                x += xIncrement;
                y += yIncrement;
                if (CheckCheck(callerScript, x, y, type, board)) return true;
            }
        }

        return false;
    }

    public bool LCheck(int x, int y, string type, GameObject[,] board)
    {

        ChessPieceAI callerScript = null;
        if (board[x, y] != null) callerScript = board[x, y].GetComponent<ChessPieceAI>();

        if (callerScript != null)
        {
            if (CheckCheck(callerScript, x + 1, y + 2, type, board)) return true;
            if (CheckCheck(callerScript, x - 1, y + 2, type, board)) return true;
            if (CheckCheck(callerScript, x + 2, y + 1, type, board)) return true;
            if (CheckCheck(callerScript, x + 2, y - 1, type, board)) return true;
            if (CheckCheck(callerScript, x + 1, y - 2, type, board)) return true;
            if (CheckCheck(callerScript, x - 1, y - 2, type, board)) return true;
            if (CheckCheck(callerScript, x - 2, y + 1, type, board)) return true;
            if (CheckCheck(callerScript, x - 2, y - 1, type, board)) return true;
        }

        return false;
    }

    public bool SurroundCheck(int x, int y, string type, GameObject[,] board)
    {
        ChessPieceAI callerScript = null;
        if (board[x, y] != null) callerScript = board[x, y].GetComponent<ChessPieceAI>();

        if (callerScript != null)
        {
            if (CheckCheck(callerScript, x, y + 1, type, board)) return true;
            if (CheckCheck(callerScript, x, y - 1, type, board)) return true;
            if (CheckCheck(callerScript, x + 1, y, type, board)) return true;
            if (CheckCheck(callerScript, x - 1, y, type, board)) return true;
            if (CheckCheck(callerScript, x + 1, y + 1, type, board)) return true;
            if (CheckCheck(callerScript, x + 1, y - 1, type, board)) return true;
            if (CheckCheck(callerScript, x - 1, y - 1, type, board)) return true;
            if (CheckCheck(callerScript, x - 1, y + 1, type, board)) return true;
        }

        return false;
    }

    public bool PawnCheck(int x, int y, GameObject[,] board)
    {

        ChessPieceAI callerScript = null;
        if (board[x, y] != null) callerScript = board[x, y].GetComponent<ChessPieceAI>();

        if (callerScript != null)
        {
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
        }

        return false;
    }

    public void IsCheckMate()
    {

        

        for (int i = 0; i < kings.Count; i++)
        {
            if (kings[i].GetComponent<ChessPieceAI>().inCheck)
            {
                //GameObject[,] positionsBackup = positions;
                //GameObject[,] tempPositions = new GameObject[8, 8];
                //Array.Copy(positions, tempPositions, positions.Length);
                ChessPieceAI kingScript = kings[i].GetComponent<ChessPieceAI>();
                int x = kingScript.GetXBoard();
                int y = kingScript.GetYBoard();

                bool check1 = false;
                bool check2 = false;
                bool check3 = false;
                bool check4 = false;
                bool check5 = false;
                bool check6 = false;
                bool check7 = false;
                bool check8 = false;
                bool checkOther = true;



                if (PositionOnBoard(x + 1, y + 1) && GetPosition(x + 1, y + 1) != null && GetPosition(x + 1, y + 1).GetComponent<ChessPieceAI>().player == kingScript.player) check1 = true;
                if (PositionOnBoard(x + 1, y + 0) && GetPosition(x + 1, y + 0) != null && GetPosition(x + 1, y + 0).GetComponent<ChessPieceAI>().player == kingScript.player) check2 = true;
                if (PositionOnBoard(x + 1, y - 1) && GetPosition(x + 1, y - 1) != null && GetPosition(x + 1, y - 1).GetComponent<ChessPieceAI>().player == kingScript.player) check3 = true;
                if (PositionOnBoard(x + 0, y - 1) && GetPosition(x + 0, y - 1) != null && GetPosition(x + 0, y - 1).GetComponent<ChessPieceAI>().player == kingScript.player) check4 = true;
                if (PositionOnBoard(x - 1, y - 1) && GetPosition(x - 1, y - 1) != null && GetPosition(x - 1, y - 1).GetComponent<ChessPieceAI>().player == kingScript.player) check5 = true;
                if (PositionOnBoard(x - 1, y + 0) && GetPosition(x - 1, y + 0) != null && GetPosition(x - 1, y + 0).GetComponent<ChessPieceAI>().player == kingScript.player) check6 = true;
                if (PositionOnBoard(x - 1, y + 1) && GetPosition(x - 1, y + 1) != null && GetPosition(x - 1, y + 1).GetComponent<ChessPieceAI>().player == kingScript.player) check7 = true;
                if (PositionOnBoard(x + 0, y + 1) && GetPosition(x + 0, y + 1) != null && GetPosition(x + 0, y + 1).GetComponent<ChessPieceAI>().player == kingScript.player) check8 = true;

                if (!PositionOnBoard(x + 1, y + 1)) check1 = true;
                if (!PositionOnBoard(x + 1, y + 0)) check2 = true;
                if (!PositionOnBoard(x + 1, y - 1)) check3 = true;
                if (!PositionOnBoard(x + 0, y - 1)) check4 = true;
                if (!PositionOnBoard(x - 1, y - 1)) check5 = true;
                if (!PositionOnBoard(x - 1, y + 0)) check6 = true;
                if (!PositionOnBoard(x - 1, y + 1)) check7 = true;
                if (!PositionOnBoard(x + 0, y + 1)) check8 = true;

                if (!check1 && !IsLegalMove(kingScript, x + 1, y + 1, kingScript, null, 0, 0)) check1 = true;
                if (!check2 && !IsLegalMove(kingScript, x + 1, y + 0, kingScript, null, 0, 0)) check2 = true;
                if (!check3 && !IsLegalMove(kingScript, x + 1, y - 1, kingScript, null, 0, 0)) check3 = true;
                if (!check4 && !IsLegalMove(kingScript, x + 0, y - 1, kingScript, null, 0, 0)) check4 = true;
                if (!check5 && !IsLegalMove(kingScript, x - 1, y - 1, kingScript, null, 0, 0)) check5 = true;
                if (!check6 && !IsLegalMove(kingScript, x - 1, y + 0, kingScript, null, 0, 0)) check6 = true;
                if (!check7 && !IsLegalMove(kingScript, x - 1, y + 1, kingScript, null, 0, 0)) check7 = true;
                if (!check8 && !IsLegalMove(kingScript, x + 0, y + 1, kingScript, null, 0, 0)) check8 = true;

                if (kingScript.player == "white")
                {
                    for (int other = 0; other < playerWhite.Length; other++)
                    {


                        if (playerWhite[other] != null && playerWhite[other].name != "whiteKing")
                        {
                            ChessPieceAI caller = playerWhite[other].GetComponent<ChessPieceAI>();
                            int xOther = caller.GetXBoard();
                            int yOther = caller.GetYBoard();

                            switch (playerWhite[other].name)
                            {
                                case "whitePawn":
                                    if (PositionOnBoard(xOther + 1, yOther + 1))
                                    {
                                        if (IsLegalMove(caller, xOther + 1, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    }
                                    if (PositionOnBoard(xOther - 1, yOther + 1))
                                    {
                                        if (IsLegalMove(caller, xOther - 1, yOther + 0, kingScript, null, 0, 0)) checkOther = false;
                                    }
                                    break;
                                case "whiteKnight":
                                    if (PositionOnBoard(xOther + 1, yOther + 2) && IsLegalMove(caller, xOther + 1, yOther + 2, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther - 1, yOther + 2) && IsLegalMove(caller, xOther - 1, yOther + 2, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther + 2, yOther + 1) && IsLegalMove(caller, xOther + 2, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther + 2, yOther - 1) && IsLegalMove(caller, xOther + 2, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther + 1, yOther - 2) && IsLegalMove(caller, xOther + 1, yOther - 2, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther - 1, yOther - 2) && IsLegalMove(caller, xOther - 1, yOther - 2, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther - 2, yOther + 1) && IsLegalMove(caller, xOther - 2, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther - 2, yOther - 1) && IsLegalMove(caller, xOther - 2, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    break;
                                case "whiteRook":
                                    int xRook1 = xOther + 0;
                                    int yRook1 = yOther + 1;
                                    if (PositionOnBoard(xRook1, yRook1) && IsLegalMove(caller, xRook1, yRook1, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xRook1, yRook1) && GetPosition(xRook1, yRook1) == null)
                                    {
                                        //xRook1++;
                                        yRook1++;
                                        if (PositionOnBoard(xRook1, yRook1) && IsLegalMove(caller, xRook1, yRook1, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xRook2 = xOther + 1;
                                    int yRook2 = yOther + 0;
                                    if (PositionOnBoard(xRook2, yRook2) && IsLegalMove(caller, xRook2, yRook2, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xRook2, yRook2) && GetPosition(xRook2, yRook2) == null)
                                    {
                                        xRook2++;
                                        //yRook1++;
                                        if (PositionOnBoard(xRook2, yRook2) && IsLegalMove(caller, xRook2, yRook2, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xRook3 = xOther + 0;
                                    int yRook3 = yOther - 1;
                                    if (PositionOnBoard(xRook3, yRook3) && IsLegalMove(caller, xRook3, yRook3, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xRook3, yRook3) && GetPosition(xRook3, yRook3) == null)
                                    {
                                        //xRook3++;
                                        yRook3--;
                                        if (PositionOnBoard(xRook3, yRook3) && IsLegalMove(caller, xRook3, yRook3, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xRook4 = xOther - 1;
                                    int yRook4 = yOther + 0;
                                    if (PositionOnBoard(xRook4, yRook4) && IsLegalMove(caller, xRook4, yRook4, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xRook4, yRook4) && GetPosition(xRook4, yRook4) == null)
                                    {
                                        xRook4--;
                                        //yRook4++;
                                        if (PositionOnBoard(xRook4, yRook4) && IsLegalMove(caller, xRook4, yRook4, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    break;
                                case "whiteBishop":
                                    int xBishop5 = xOther + 1;
                                    int yBishop5 = yOther + 1;
                                    if (PositionOnBoard(xBishop5, yBishop5) && IsLegalMove(caller, xBishop5, yBishop5, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xBishop5, yBishop5) && GetPosition(xBishop5, yBishop5) == null)
                                    {
                                        xBishop5++;
                                        yBishop5++;
                                        if (PositionOnBoard(xBishop5, yBishop5) && IsLegalMove(caller, xBishop5, yBishop5, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xBishop6 = xOther + 1;
                                    int yBishop6 = yOther - 1;
                                    if (PositionOnBoard(xBishop6, yBishop6) && IsLegalMove(caller, xBishop6, yBishop6, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xBishop6, yBishop6) && GetPosition(xBishop6, yBishop6) == null)
                                    {
                                        xBishop6++;
                                        yBishop6--;
                                        if (PositionOnBoard(xBishop6, yBishop6) && IsLegalMove(caller, xBishop6, yBishop6, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xBishop7 = xOther - 1;
                                    int yBishop7 = yOther - 1;
                                    if (PositionOnBoard(xBishop7, yBishop7) && IsLegalMove(caller, xBishop7, yBishop7, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xBishop7, yBishop7) && GetPosition(xBishop7, yBishop7) == null)
                                    {
                                        xBishop7--;
                                        yBishop7--;
                                        if (PositionOnBoard(xBishop7, yBishop7) && IsLegalMove(caller, xBishop7, yBishop7, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xBishop8 = xOther - 1;
                                    int yBishop8 = yOther + 1;
                                    if (PositionOnBoard(xBishop8, yBishop8) && IsLegalMove(caller, xBishop8, yBishop8, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xBishop8, yBishop8) && GetPosition(xBishop8, yBishop8) == null)
                                    {
                                        xBishop8--;
                                        yBishop8++;
                                        if (PositionOnBoard(xBishop8, yBishop8) && IsLegalMove(caller, xBishop8, yBishop8, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    break;
                                case "whiteQueen":
                                    int xQueen1 = xOther + 0;
                                    int yQueen1 = yOther + 1;
                                    if (PositionOnBoard(xQueen1, yQueen1) && IsLegalMove(caller, xQueen1, yQueen1, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen1, yQueen1) && GetPosition(xQueen1, yQueen1) == null)
                                    {
                                        //xQueen1++;
                                        yQueen1++;
                                        if (PositionOnBoard(xQueen1, yQueen1) && IsLegalMove(caller, xQueen1, yQueen1, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen2 = xOther + 1;
                                    int yQueen2 = yOther + 0;
                                    if (PositionOnBoard(xQueen2, yQueen2) && IsLegalMove(caller, xQueen2, yQueen2, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen2, yQueen2) && GetPosition(xQueen2, yQueen2) == null)
                                    {
                                        xQueen2++;
                                        //yQueen1++;
                                        if (PositionOnBoard(xQueen2, yQueen2) && IsLegalMove(caller, xQueen2, yQueen2, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen3 = xOther + 0;
                                    int yQueen3 = yOther - 1;
                                    if (PositionOnBoard(xQueen3, yQueen3) && IsLegalMove(caller, xQueen3, yQueen3, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen3, yQueen3) && GetPosition(xQueen3, yQueen3) == null)
                                    {
                                        //xQueen3++;
                                        yQueen3--;
                                        if (PositionOnBoard(xQueen3, yQueen3) && IsLegalMove(caller, xQueen3, yQueen3, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen4 = xOther - 1;
                                    int yQueen4 = yOther + 0;
                                    if (PositionOnBoard(xQueen4, yQueen4) && IsLegalMove(caller, xQueen4, yQueen4, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen4, yQueen4) && GetPosition(xQueen4, yQueen4) == null)
                                    {
                                        xQueen4--;
                                        //yQueen4++;
                                        if (PositionOnBoard(xQueen4, yQueen4) && IsLegalMove(caller, xQueen4, yQueen4, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen5 = xOther + 1;
                                    int yQueen5 = yOther + 1;
                                    if (PositionOnBoard(xQueen5, yQueen5) && IsLegalMove(caller, xQueen5, yQueen5, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen5, yQueen5) && GetPosition(xQueen5, yQueen5) == null)
                                    {
                                        xQueen5++;
                                        yQueen5++;
                                        if (PositionOnBoard(xQueen5, yQueen5) && IsLegalMove(caller, xQueen5, yQueen5, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen6 = xOther + 1;
                                    int yQueen6 = yOther - 1;
                                    if (PositionOnBoard(xQueen6, yQueen6) && IsLegalMove(caller, xQueen6, yQueen6, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen6, yQueen6) && GetPosition(xQueen6, yQueen6) == null)
                                    {
                                        xQueen6++;
                                        yQueen6--;
                                        if (PositionOnBoard(xQueen6, yQueen6) && IsLegalMove(caller, xQueen6, yQueen6, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen7 = xOther - 1;
                                    int yQueen7 = yOther - 1;
                                    if (PositionOnBoard(xQueen7, yQueen7) && IsLegalMove(caller, xQueen7, yQueen7, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen7, yQueen7) && GetPosition(xQueen7, yQueen7) == null)
                                    {
                                        xQueen7--;
                                        yQueen7--;
                                        if (PositionOnBoard(xQueen7, yQueen7) && IsLegalMove(caller, xQueen7, yQueen7, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen8 = xOther - 1;
                                    int yQueen8 = yOther + 1;
                                    if (PositionOnBoard(xQueen8, yQueen8) && IsLegalMove(caller, xQueen8, yQueen8, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen8, yQueen8) && GetPosition(xQueen8, yQueen8) == null)
                                    {
                                        xQueen8--;
                                        yQueen8++;
                                        if (PositionOnBoard(xQueen8, yQueen8) && IsLegalMove(caller, xQueen8, yQueen8, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    if (IsLegalMove(caller, xOther + 1, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther + 1, yOther + 0, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther + 1, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther + 0, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther - 1, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther - 1, yOther + 0, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther - 1, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther + 0, yOther + 1, kingScript, null, 0, 0)) checkOther = false;

                                    break;

                            }
                        }
                    }
                }
                else
                {
                    for (int other = 0; other < playerBlack.Length; other++)
                    {


                        if (playerBlack[other] != null && playerBlack[other].name != "blackKing")
                        {
                            ChessPieceAI caller = playerBlack[other].GetComponent<ChessPieceAI>();
                            int xOther = caller.GetXBoard();
                            int yOther = caller.GetYBoard();

                            switch (playerBlack[other].name)
                            {
                                case "blackPawn":
                                    if (PositionOnBoard(xOther + 1, yOther - 1))
                                    {
                                        if (IsLegalMove(caller, xOther + 1, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    }
                                    if (PositionOnBoard(xOther - 1, yOther - 1))
                                    {
                                        if (IsLegalMove(caller, xOther - 1, yOther - 0, kingScript, null, 0, 0)) checkOther = false;
                                    }
                                    break;
                                case "blackKnight":
                                    if (PositionOnBoard(xOther + 1, yOther + 2) && IsLegalMove(caller, xOther + 1, yOther + 2, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther - 1, yOther + 2) && IsLegalMove(caller, xOther - 1, yOther + 2, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther + 2, yOther + 1) && IsLegalMove(caller, xOther + 2, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther + 2, yOther - 1) && IsLegalMove(caller, xOther + 2, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther + 1, yOther - 2) && IsLegalMove(caller, xOther + 1, yOther - 2, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther - 1, yOther - 2) && IsLegalMove(caller, xOther - 1, yOther - 2, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther - 2, yOther + 1) && IsLegalMove(caller, xOther - 2, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (PositionOnBoard(xOther - 2, yOther - 1) && IsLegalMove(caller, xOther - 2, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    break;
                                case "blackRook":
                                    int xRook1 = xOther + 0;
                                    int yRook1 = yOther + 1;
                                    if (PositionOnBoard(xRook1, yRook1) && IsLegalMove(caller, xRook1, yRook1, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xRook1, yRook1) && GetPosition(xRook1, yRook1) == null)
                                    {
                                        //xRook1++;
                                        yRook1++;
                                        if (PositionOnBoard(xRook1, yRook1) && IsLegalMove(caller, xRook1, yRook1, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xRook2 = xOther + 1;
                                    int yRook2 = yOther + 0;
                                    if (PositionOnBoard(xRook2, yRook2) && IsLegalMove(caller, xRook2, yRook2, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xRook2, yRook2) && GetPosition(xRook2, yRook2) == null)
                                    {
                                        xRook2++;
                                        //yRook1++;
                                        if (PositionOnBoard(xRook2, yRook2) && IsLegalMove(caller, xRook2, yRook2, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xRook3 = xOther + 0;
                                    int yRook3 = yOther - 1;
                                    if (PositionOnBoard(xRook3, yRook3) && IsLegalMove(caller, xRook3, yRook3, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xRook3, yRook3) && GetPosition(xRook3, yRook3) == null)
                                    {
                                        //xRook3++;
                                        yRook3--;
                                        if (PositionOnBoard(xRook3, yRook3) && IsLegalMove(caller, xRook3, yRook3, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xRook4 = xOther - 1;
                                    int yRook4 = yOther + 0;
                                    if (PositionOnBoard(xRook4, yRook4) && IsLegalMove(caller, xRook4, yRook4, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xRook4, yRook4) && GetPosition(xRook4, yRook4) == null)
                                    {
                                        xRook4--;
                                        //yRook4++;
                                        if (PositionOnBoard(xRook4, yRook4) && IsLegalMove(caller, xRook4, yRook4, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    break;
                                case "blackBishop":
                                    int xBishop5 = xOther + 1;
                                    int yBishop5 = yOther + 1;
                                    if (PositionOnBoard(xBishop5, yBishop5) && IsLegalMove(caller, xBishop5, yBishop5, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xBishop5, yBishop5) && GetPosition(xBishop5, yBishop5) == null)
                                    {
                                        xBishop5++;
                                        yBishop5++;
                                        if (PositionOnBoard(xBishop5, yBishop5) && IsLegalMove(caller, xBishop5, yBishop5, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xBishop6 = xOther + 1;
                                    int yBishop6 = yOther - 1;
                                    if (PositionOnBoard(xBishop6, yBishop6) && IsLegalMove(caller, xBishop6, yBishop6, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xBishop6, yBishop6) && GetPosition(xBishop6, yBishop6) == null)
                                    {
                                        xBishop6++;
                                        yBishop6--;
                                        if (PositionOnBoard(xBishop6, yBishop6) && IsLegalMove(caller, xBishop6, yBishop6, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xBishop7 = xOther - 1;
                                    int yBishop7 = yOther - 1;
                                    if (PositionOnBoard(xBishop7, yBishop7) && IsLegalMove(caller, xBishop7, yBishop7, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xBishop7, yBishop7) && GetPosition(xBishop7, yBishop7) == null)
                                    {
                                        xBishop7--;
                                        yBishop7--;
                                        if (PositionOnBoard(xBishop7, yBishop7) && IsLegalMove(caller, xBishop7, yBishop7, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xBishop8 = xOther - 1;
                                    int yBishop8 = yOther + 1;
                                    if (PositionOnBoard(xBishop8, yBishop8) && IsLegalMove(caller, xBishop8, yBishop8, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xBishop8, yBishop8) && GetPosition(xBishop8, yBishop8) == null)
                                    {
                                        xBishop8--;
                                        yBishop8++;
                                        if (PositionOnBoard(xBishop8, yBishop8) && IsLegalMove(caller, xBishop8, yBishop8, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    break;
                                case "blackQueen":
                                    int xQueen1 = xOther + 0;
                                    int yQueen1 = yOther + 1;
                                    if (PositionOnBoard(xQueen1, yQueen1) && IsLegalMove(caller, xQueen1, yQueen1, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen1, yQueen1) && GetPosition(xQueen1, yQueen1) == null)
                                    {
                                        //xQueen1++;
                                        yQueen1++;
                                        if (PositionOnBoard(xQueen1, yQueen1) && IsLegalMove(caller, xQueen1, yQueen1, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen2 = xOther + 1;
                                    int yQueen2 = yOther + 0;
                                    if (PositionOnBoard(xQueen2, yQueen2) && IsLegalMove(caller, xQueen2, yQueen2, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen2, yQueen2) && GetPosition(xQueen2, yQueen2) == null)
                                    {
                                        xQueen2++;
                                        //yQueen1++;
                                        if (PositionOnBoard(xQueen2, yQueen2) && IsLegalMove(caller, xQueen2, yQueen2, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen3 = xOther + 0;
                                    int yQueen3 = yOther - 1;
                                    if (PositionOnBoard(xQueen3, yQueen3) && IsLegalMove(caller, xQueen3, yQueen3, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen3, yQueen3) && GetPosition(xQueen3, yQueen3) == null)
                                    {
                                        //xQueen3++;
                                        yQueen3--;
                                        if (PositionOnBoard(xQueen3, yQueen3) && IsLegalMove(caller, xQueen3, yQueen3, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen4 = xOther - 1;
                                    int yQueen4 = yOther + 0;
                                    if (PositionOnBoard(xQueen4, yQueen4) && IsLegalMove(caller, xQueen4, yQueen4, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen4, yQueen4) && GetPosition(xQueen4, yQueen4) == null)
                                    {
                                        xQueen4--;
                                        //yQueen4++;
                                        if (PositionOnBoard(xQueen4, yQueen4) && IsLegalMove(caller, xQueen4, yQueen4, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen5 = xOther + 1;
                                    int yQueen5 = yOther + 1;
                                    if (PositionOnBoard(xQueen5, yQueen5) && IsLegalMove(caller, xQueen5, yQueen5, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen5, yQueen5) && GetPosition(xQueen5, yQueen5) == null)
                                    {
                                        xQueen5++;
                                        yQueen5++;
                                        if (PositionOnBoard(xQueen5, yQueen5) && IsLegalMove(caller, xQueen5, yQueen5, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen6 = xOther + 1;
                                    int yQueen6 = yOther - 1;
                                    if (PositionOnBoard(xQueen6, yQueen6) && IsLegalMove(caller, xQueen6, yQueen6, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen6, yQueen6) && GetPosition(xQueen6, yQueen6) == null)
                                    {
                                        xQueen6++;
                                        yQueen6--;
                                        if (PositionOnBoard(xQueen6, yQueen6) && IsLegalMove(caller, xQueen6, yQueen6, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen7 = xOther - 1;
                                    int yQueen7 = yOther - 1;
                                    if (PositionOnBoard(xQueen7, yQueen7) && IsLegalMove(caller, xQueen7, yQueen7, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen7, yQueen7) && GetPosition(xQueen7, yQueen7) == null)
                                    {
                                        xQueen7--;
                                        yQueen7--;
                                        if (PositionOnBoard(xQueen7, yQueen7) && IsLegalMove(caller, xQueen7, yQueen7, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    int xQueen8 = xOther - 1;
                                    int yQueen8 = yOther + 1;
                                    if (PositionOnBoard(xQueen8, yQueen8) && IsLegalMove(caller, xQueen8, yQueen8, kingScript, null, 0, 0)) checkOther = false;
                                    while (PositionOnBoard(xQueen8, yQueen8) && GetPosition(xQueen8, yQueen8) == null)
                                    {
                                        xQueen8--;
                                        yQueen8++;
                                        if (PositionOnBoard(xQueen8, yQueen8) && IsLegalMove(caller, xQueen8, yQueen8, kingScript, null, 0, 0)) checkOther = false;
                                    }

                                    if (IsLegalMove(caller, xOther + 1, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther + 1, yOther + 0, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther + 1, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther + 0, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther - 1, yOther - 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther - 1, yOther + 0, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther - 1, yOther + 1, kingScript, null, 0, 0)) checkOther = false;
                                    if (IsLegalMove(caller, xOther + 0, yOther + 1, kingScript, null, 0, 0)) checkOther = false;

                                    break;

                            }
                        }
                    }
                }


                Debug.Log("1 " + check1);
                Debug.Log("2 " + check2);
                Debug.Log("3 " + check3);
                Debug.Log("4 " + check4);
                Debug.Log("5 " + check5);
                Debug.Log("6 " + check6);
                Debug.Log("7 " + check7);
                Debug.Log("8 " + check8);
                Debug.Log("oth " + checkOther);
                Debug.Log("-------------");

                if (check1 &&
                    check2 &&
                    check3 &&
                    check4 &&
                    check5 &&
                    check6 &&
                    check7 &&
                    check8 &&
                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    checkOther
                    )
                {
                    if (kings[i].GetComponent<ChessPieceAI>().player == "white")
                    {
                        Winner("BLACK");
                    }
                    if (kings[i].GetComponent<ChessPieceAI>().player == "black")
                    {
                        Winner("WHITE");
                    }
                }
            }
        }


    }

    public bool IsLegalMove(ChessPieceAI toMove, int xDest, int yDest, ChessPieceAI king, ChessPieceAI toMove2, int xDest2, int yDest2)
    {
        GameObject[,] tempPositions = new GameObject[8, 8];
        //positions.CopyTo(tempPositions, 0);
        Array.Copy(positions, tempPositions, positions.Length);
        int x = toMove.GetXBoard();
        int y = toMove.GetYBoard();
        int x2 = 0;
        int y2 = 0;

        if (toMove2 != null)
        {
            x2 = toMove2.GetXBoard();
            y2 = toMove2.GetYBoard();
        }

        if (PositionOnBoard(xDest, yDest)) tempPositions[xDest, yDest] = toMove.gameObject;
        tempPositions[x, y] = null;

        if (toMove2 != null)
        {
            if (PositionOnBoard(xDest2, yDest2)) tempPositions[xDest2, yDest2] = toMove2.gameObject;
            tempPositions[x2, y2] = null;
        }

        if (toMove == king)
        {
            if (IsInCheck(xDest, yDest, tempPositions)) return false;
        }
        else
        {
            if (IsInCheck(king.GetXBoard(), king.GetYBoard(), tempPositions)) return false;
        }
        //Debug.Log(IsInCheck(king, tempPositions));


        return true;
    }

}
