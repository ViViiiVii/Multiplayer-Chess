using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGameFree;

public class ChessPieceAI : MonoBehaviour
{
    public GameObject gameManager;
    public GameManagerAI gmScript;
    public GameObject moveIndicator;
    public GameObject checkMark;
    public List<GameObject> attackPlates = new List<GameObject>();
    public BoxCollider2D bc;

    private int xBoard = -1;
    private int yBoard = -1;

    public string player;
    public bool inCheck = false;
    public bool moved = false;
    public bool blackFlip = false;
    public float delayTime = 0.025f;

    // Sprite variables
    public Sprite blackKing, blackQueen, blackBishop, blackKnight, blackRook, blackPawn;
    public Sprite whiteKing, whiteQueen, whiteBishop, whiteKnight, whiteRook, whitePawn;

    public void Activate()
    {
        if (SaveGame.Exists("blackFlip"))
        {
            blackFlip = SaveGame.Load<bool>("blackFlip", false);
        }

        gameManager = GameObject.FindGameObjectWithTag("GameController");
        gmScript = gameManager.GetComponent<GameManagerAI>();
        bc = gameObject.GetComponent<BoxCollider2D>();

        SetCoords();

        switch (this.name)
        {
            case "blackKing": this.GetComponent<SpriteRenderer>().sprite = blackKing; player = "black"; gmScript.kings.Add(gameObject); break;
            case "blackQueen": this.GetComponent<SpriteRenderer>().sprite = blackQueen; player = "black"; break;
            case "blackBishop": this.GetComponent<SpriteRenderer>().sprite = blackBishop; player = "black"; break;
            case "blackKnight": this.GetComponent<SpriteRenderer>().sprite = blackKnight; player = "black"; break;
            case "blackRook": this.GetComponent<SpriteRenderer>().sprite = blackRook; player = "black"; break;
            case "blackPawn": this.GetComponent<SpriteRenderer>().sprite = blackPawn; player = "black"; break;

            case "whiteKing": this.GetComponent<SpriteRenderer>().sprite = whiteKing; player = "white"; gmScript.kings.Add(gameObject); break;
            case "whiteQueen": this.GetComponent<SpriteRenderer>().sprite = whiteQueen; player = "white"; break;
            case "whiteBishop": this.GetComponent<SpriteRenderer>().sprite = whiteBishop; player = "white"; break;
            case "whiteKnight": this.GetComponent<SpriteRenderer>().sprite = whiteKnight; player = "white"; break;
            case "whiteRook": this.GetComponent<SpriteRenderer>().sprite = whiteRook; player = "white"; break;
            case "whitePawn": this.GetComponent<SpriteRenderer>().sprite = whitePawn; player = "white"; break;
        }

        if (player == "black" && blackFlip)
        {
            //gameObject.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y, 180f, transform.rotation.w);
            this.GetComponent<SpriteRenderer>().flipY = true;

        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        float tempX = x - this.transform.position.x;
        float tempY = y - this.transform.position.y;

        //this.transform.position += new Vector3(tempX, tempY);
        StartCoroutine(AnimatedMove());
    }

    IEnumerator AnimatedMove()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        float tempX = x - this.transform.position.x;
        float tempY = y - this.transform.position.y;
        float xMove = tempX / 30;
        float yMove = tempY / 30;

        for (int i = 0; i < 30; i++)
        {
            this.transform.position += new Vector3(xMove, yMove);

            yield return new WaitForSeconds(delayTime);
        }


        //this.transform.position += new Vector3(tempX, tempY);
    }

    public int GetXBoard()
    {
        return xBoard;
    }
    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!gameManager.GetComponent<GameManagerAI>().IsGameOver() && gameManager.GetComponent<GameManagerAI>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }

        for (int i = 0; i < gmScript.playerBlack.Length; i++)
        {
            if (gmScript.playerBlack[i] != null)
            {
                gmScript.playerBlack[i].GetComponent<BoxCollider2D>().enabled = true;
            }
            if (gmScript.playerWhite[i] != null)
            {
                gmScript.playerWhite[i].GetComponent<BoxCollider2D>().enabled = true;
            }
        }

    }

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "blackQueen":
            case "whiteQueen":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "blackKnight":
            case "whiteKnight":
                LMovePlate();
                break;
            case "blackBishop":
            case "whiteBishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                break;
            case "blackRook":
            case "whiteRook":
                LineMovePlate(1, 0);
                LineMovePlate(-1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(0, -1);
                break;
            case "blackKing":
            case "whiteKing":
                SurroundMovePlate();
                CastleMovePlate();
                break;
            case "blackPawn":
                PawnMovePlate(xBoard, yBoard - 1, true);
                if (!moved && gameManager.GetComponent<GameManagerAI>().GetPosition(xBoard, yBoard - 1) == null)
                {
                    PawnMovePlate(xBoard, yBoard - 2, false);
                }
                break;
            case "whitePawn":
                PawnMovePlate(xBoard, yBoard + 1, true);
                if (!moved && gameManager.GetComponent<GameManagerAI>().GetPosition(xBoard, yBoard + 1) == null)
                {
                    PawnMovePlate(xBoard, yBoard + 2, false);
                }
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        GameManagerAI gmScript = gameManager.GetComponent<GameManagerAI>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (gmScript.PositionOnBoard(x, y) && gmScript.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y, null, null);
            x += xIncrement;
            y += yIncrement;
        }

        if (gmScript.PositionOnBoard(x, y) && gmScript.GetPosition(x, y).GetComponent<ChessPieceAI>().player != player)
        {
            AttackPlateSpawn(x, y);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
    }

    public void PointMovePlate(int x, int y)
    {
        GameManagerAI gmScript = gameManager.GetComponent<GameManagerAI>();

        if (gmScript.PositionOnBoard(x, y))
        {
            GameObject cp = gmScript.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y, null, null);
            }
            else if (cp.GetComponent<ChessPieceAI>().player != player)
            {
                AttackPlateSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y, bool canAttack)
    {
        GameManagerAI gmScript = gameManager.GetComponent<GameManagerAI>();

        if (gmScript.PositionOnBoard(x, y))
        {
            if (gmScript.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y, null, null);
            }

            if (gmScript.PositionOnBoard(x + 1, y) && gmScript.GetPosition(x + 1, y) != null && 
                gmScript.GetPosition(x + 1, y).GetComponent<ChessPieceAI>().player != player && canAttack)
            {
                AttackPlateSpawn(x + 1, y);
            }

            if (gmScript.PositionOnBoard(x - 1, y) && gmScript.GetPosition(x - 1, y) != null &&
                gmScript.GetPosition(x - 1, y).GetComponent<ChessPieceAI>().player != player && canAttack)
            {
                AttackPlateSpawn(x - 1, y);
            }
        }
    }

    public void CastleMovePlate()
    {
        GameManagerAI gmScript = gameManager.GetComponent<GameManagerAI>();
        int x = xBoard;
        int y = yBoard;

        //short
        if (!moved && !inCheck)
        {
            if (gmScript.GetPosition(x + 1, y) == null && gmScript.GetPosition(x + 2, y) == null && gmScript.GetPosition(x + 3, y) != null)
            {
                GameObject rook = gmScript.GetPosition(x + 3, y);
                if (gmScript.GetPosition(x + 3, y).name == "blackRook" || gmScript.GetPosition(x + 3, y).name == "whiteRook")
                {
                    if (rook.GetComponent<ChessPieceAI>().player == player && !rook.GetComponent<ChessPieceAI>().moved)
                    {
                        if (gmScript.IsLegalMove(this, x + 1, y, this, null, 0, 0) &&
                            gmScript.IsLegalMove(this, x + 2, y, this, null, 0, 0) &&
                            gmScript.IsLegalMove(this, x + 2, y, this, rook.GetComponent<ChessPieceAI>(), x + 1, y))
                        {
                            MovePlateSpawn(x + 3, y, rook, "short");
                        }
                    }
                }
            }
        }
        if (!moved && !inCheck)
        {
            if (gmScript.GetPosition(x - 1, y) == null && gmScript.GetPosition(x - 2, y) == null && gmScript.GetPosition(x - 3, y) == null && gmScript.GetPosition(x - 4, y) != null)
            {
                GameObject rook = gmScript.GetPosition(x - 4, y);
                if (gmScript.GetPosition(x - 4, y).name == "blackRook" || gmScript.GetPosition(x - 4, y).name == "whiteRook")
                {
                    if (rook.GetComponent<ChessPieceAI>().player == player && !rook.GetComponent<ChessPieceAI>().moved)
                    {
                        if (gmScript.IsLegalMove(this, x - 1, y, this, null, 0, 0) &&
                            gmScript.IsLegalMove(this, x - 2, y, this, null, 0, 0) &&
                            gmScript.IsLegalMove(this, x - 2, y, this, rook.GetComponent<ChessPieceAI>(), x - 1, y))
                        {
                            MovePlateSpawn(x - 4, y, rook, "long");
                        }
                    }
                }
            }
        }

    }

    public void MovePlateSpawn(int matrixX, int matrixY, GameObject ref2, string type)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.309f;
        y += -2.309f;

        GameObject mp = Instantiate(moveIndicator, new Vector2(x, y), Quaternion.identity);

        MovePlateAI mpScript = mp.GetComponent<MovePlateAI>();
        mpScript.SetReference(gameObject);
        mpScript.SetReference2(ref2);
        mpScript.SetCoords(matrixX, matrixY);
        mpScript.castleType = type;
    }

    public void AttackPlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        if (gmScript.GetPosition(matrixX, matrixY).name == "blackKing" || gmScript.GetPosition(matrixX, matrixY).name == "whiteKing") return;

        gmScript.GetPosition(matrixX, matrixY).GetComponent<BoxCollider2D>().enabled = false;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.309f;
        y += -2.309f;

        GameObject mp = Instantiate(moveIndicator, new Vector2(x, y), Quaternion.identity);
        attackPlates.Add(mp);

        MovePlateAI mpScript = mp.GetComponent<MovePlateAI>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    private void Update()
    {
        if (inCheck)
        {
            checkMark.SetActive(true);
        }
        if (!inCheck)
        {
            checkMark.SetActive(false);
        }

        //if (!gmScript.choosingDone) bc.enabled = false;
        //if (gmScript.choosingDone) bc.enabled = true;

    }

}
