using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{

    [SerializeField]
    private string _gameVersion = "0.8";
    public string GameVersion { get { return _gameVersion; } }
    private string _nickname = "kakaoscsiga";
    public string Nickname
    {
        get
        {
            int value = Random.Range(0, 9999);
            return _nickname + value.ToString();
        }
    }

}
