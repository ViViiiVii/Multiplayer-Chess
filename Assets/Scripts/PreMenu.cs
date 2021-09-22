using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class PreMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Settings()
    {
        if (SaveGame.Load<bool>("blackFlip", false))
        {
            SaveGame.Save<bool>("blackFlip", false);
        } 
        else
        {
            SaveGame.Save<bool>("blackFlip", true);
        }
    }
}
