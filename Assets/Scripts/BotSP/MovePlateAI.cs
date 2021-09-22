using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlateAI : MonoBehaviour
{

    public GameObject gameManager;

    GameObject reference = null;
    GameObject reference2 = null;
    ChessPieceAI kingScript;
    GameManagerAI gms;

    int matrixX;
    int matrixY;

    public bool attack = false;
    public string castleType = null;

    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        gms = gameManager.GetComponent<GameManagerAI>();
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0.0625f, 0.6235294f);
        }

        for (int i = 0; i < gms.kings.Count; i++)
        {
            if (gms.kings[i].GetComponent<ChessPieceAI>().player == reference.GetComponent<ChessPieceAI>().player) kingScript = gms.kings[i].GetComponent<ChessPieceAI>();
        }
        if (!gms.IsLegalMove(reference.GetComponent<ChessPieceAI>(), matrixX, matrixY, kingScript, null, 0, 0)) Destroy(gameObject);
    }

    public void OnMouseUp()
    {
       

        if (attack)
        {
            GameObject chessPiece = gameManager.GetComponent<GameManagerAI>().GetPosition(matrixX, matrixY);
            if (chessPiece.GetComponent<ChessPieceAI>().player == "black")
            {
                gameManager.GetComponent<GameManagerAI>().blackCount--;
            }
            if (chessPiece.GetComponent<ChessPieceAI>().player == "white")
            {
                gameManager.GetComponent<GameManagerAI>().whiteCount--;
            }

            Destroy(chessPiece);
        }

        gameManager.GetComponent<GameManagerAI>().SetPositionEmpty(reference.GetComponent<ChessPieceAI>().GetXBoard(), reference.GetComponent<ChessPieceAI>().GetYBoard());

        if (reference2 != null && castleType == "short")
        {
            reference2.GetComponent<ChessPieceAI>().moved = true;
            reference2.GetComponent<ChessPieceAI>().SetXBoard(matrixX - 2);
            reference2.GetComponent<ChessPieceAI>().SetYBoard(matrixY);
            reference2.GetComponent<ChessPieceAI>().SetCoords();
            gameManager.GetComponent<GameManagerAI>().SetPosition(reference2);

            reference.GetComponent<ChessPieceAI>().moved = true;
            reference.GetComponent<ChessPieceAI>().SetXBoard(matrixX - 1);
            reference.GetComponent<ChessPieceAI>().SetYBoard(matrixY);
            reference.GetComponent<ChessPieceAI>().SetCoords();
            gameManager.GetComponent<GameManagerAI>().SetPosition(reference);
        }
        if (reference2 != null && castleType == "long")
        {
            reference2.GetComponent<ChessPieceAI>().moved = true;
            reference2.GetComponent<ChessPieceAI>().SetXBoard(matrixX + 3);
            reference2.GetComponent<ChessPieceAI>().SetYBoard(matrixY);
            reference2.GetComponent<ChessPieceAI>().SetCoords();
            gameManager.GetComponent<GameManagerAI>().SetPosition(reference2);

            reference.GetComponent<ChessPieceAI>().moved = true;
            reference.GetComponent<ChessPieceAI>().SetXBoard(matrixX + 2);
            reference.GetComponent<ChessPieceAI>().SetYBoard(matrixY);
            reference.GetComponent<ChessPieceAI>().SetCoords();
            gameManager.GetComponent<GameManagerAI>().SetPosition(reference);
        }

        if (reference2 == null)
        {
            reference.GetComponent<ChessPieceAI>().moved = true;
            reference.GetComponent<ChessPieceAI>().SetXBoard(matrixX);
            reference.GetComponent<ChessPieceAI>().SetYBoard(matrixY);
            reference.GetComponent<ChessPieceAI>().SetCoords();
            gameManager.GetComponent<GameManagerAI>().SetPosition(reference);
        }


        reference.GetComponent<ChessPieceAI>().DestroyMovePlates();


        gameManager.GetComponent<GameManagerAI>().StartNextTurn();
        //gms.botMovement.GetBotMove();
    }


    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }
    public void SetReference2(GameObject obj)
    {
        reference2 = obj;
    }


    public GameObject GetReference()
    {
        return reference;
    }

}
