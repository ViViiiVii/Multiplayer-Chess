using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{

    public GameObject gameManager;

    GameObject reference = null;
    GameObject reference2 = null;
    ChessPiece kingScript;
    GameManager gms;

    int matrixX;
    int matrixY;

    public bool attack = false;
    public string castleType = null;

    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        gms = gameManager.GetComponent<GameManager>();
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0.0625f, 0.6235294f);
        }

        for (int i = 0; i < gms.kings.Count; i++)
        {
            if (gms.kings[i].GetComponent<ChessPiece>().player == reference.GetComponent<ChessPiece>().player) kingScript = gms.kings[i].GetComponent<ChessPiece>();
        }
        if (!gms.IsLegalMove(reference.GetComponent<ChessPiece>(), matrixX, matrixY, kingScript, null, 0, 0)) Destroy(gameObject);
    }

    public void OnMouseUp()
    {
       

        if (attack)
        {
            GameObject chessPiece = gameManager.GetComponent<GameManager>().GetPosition(matrixX, matrixY);
            if (chessPiece.GetComponent<ChessPiece>().player == "black")
            {
                gameManager.GetComponent<GameManager>().blackCount--;
            }
            if (chessPiece.GetComponent<ChessPiece>().player == "white")
            {
                gameManager.GetComponent<GameManager>().whiteCount--;
            }

            if (chessPiece.name == "whiteKing") gameManager.GetComponent<GameManager>().Winner("BLACK");
            if (chessPiece.name == "blackKing") gameManager.GetComponent<GameManager>().Winner("WHITE");

            Destroy(chessPiece);
        }

        gameManager.GetComponent<GameManager>().SetPositionEmpty(reference.GetComponent<ChessPiece>().GetXBoard(), reference.GetComponent<ChessPiece>().GetYBoard());

        if (reference2 != null && castleType == "short")
        {
            reference2.GetComponent<ChessPiece>().moved = true;
            reference2.GetComponent<ChessPiece>().SetXBoard(matrixX - 2);
            reference2.GetComponent<ChessPiece>().SetYBoard(matrixY);
            reference2.GetComponent<ChessPiece>().SetCoords();
            gameManager.GetComponent<GameManager>().SetPosition(reference2);

            reference.GetComponent<ChessPiece>().moved = true;
            reference.GetComponent<ChessPiece>().SetXBoard(matrixX - 1);
            reference.GetComponent<ChessPiece>().SetYBoard(matrixY);
            reference.GetComponent<ChessPiece>().SetCoords();
            gameManager.GetComponent<GameManager>().SetPosition(reference);
        }
        if (reference2 != null && castleType == "long")
        {
            reference2.GetComponent<ChessPiece>().moved = true;
            reference2.GetComponent<ChessPiece>().SetXBoard(matrixX + 3);
            reference2.GetComponent<ChessPiece>().SetYBoard(matrixY);
            reference2.GetComponent<ChessPiece>().SetCoords();
            gameManager.GetComponent<GameManager>().SetPosition(reference2);

            reference.GetComponent<ChessPiece>().moved = true;
            reference.GetComponent<ChessPiece>().SetXBoard(matrixX + 2);
            reference.GetComponent<ChessPiece>().SetYBoard(matrixY);
            reference.GetComponent<ChessPiece>().SetCoords();
            gameManager.GetComponent<GameManager>().SetPosition(reference);
        }

        if (reference2 == null)
        {
            reference.GetComponent<ChessPiece>().moved = true;
            reference.GetComponent<ChessPiece>().SetXBoard(matrixX);
            reference.GetComponent<ChessPiece>().SetYBoard(matrixY);
            reference.GetComponent<ChessPiece>().SetCoords();
            gameManager.GetComponent<GameManager>().SetPosition(reference);
        }


        reference.GetComponent<ChessPiece>().DestroyMovePlates();

        gameManager.GetComponent<GameManager>().StartNextTurn();

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
