using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnButton : MonoBehaviour
{

    public GameManager gms;
    public string type = "blackQueen";

    // Start is called before the first frame update
    void Start()
    {
        gms = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendData()
    {
        gms.chosenType = type;
        gms.choosingDone = true;
    }
}
