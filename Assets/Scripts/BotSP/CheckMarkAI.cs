using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMarkAI : MonoBehaviour
{

    public GameObject reference;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (reference.GetComponent<ChessPieceAI>().gmScript.IsGameOver())
        {
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        reference = GetComponentInParent<ChessPieceAI>().gameObject;
    }
}
