using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrearType
{
    Score,
    WordLength
}

public enum SceneType
{
    Start,
    Game,
    Pause,
    Clear
}

public class SceneController : MonoBehaviour
{
    [SerializeField]
    ClearTerm[] clearTerm;

    public SceneType CurrentScene { get; protected set; } 

    GameManager gameMan;

    public void Start()
    {
        gameMan = GameManager.Instance;
        CurrentScene = SceneType.Start;
    }

    public ClearTerm CurrentClearTermByStage(int Stage)
    {
        return clearTerm[Stage];
    }

    public bool CheckClear()
    {

        int stage = gameMan.stage;
        switch (clearTerm[stage].type)
        {
            case CrearType.Score:
                if (gameMan.score >= clearTerm[stage].value)
                {
                    CurrentScene = SceneType.Clear;
                    return true;
                }
                break;
            case CrearType.WordLength:
                if (gameMan.createdWordLength >= clearTerm[stage].value)
                {
                    CurrentScene = SceneType.Clear;
                    return true;
                }
                break;
        }

        return false;
    }

    public void MoveNextStage()
    {
        /*
         * TODO: ステージの切り替えのアニメーションをChinemashineをつくる
         * 
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", -transform.position.z,
            "y", transform.position.y + 15f,
            "z", transform.position.x,
            "time", 2f,
            "delay", .5f
            ));

        iTween.RotateAdd(gameObject, iTween.Hash(
            "y", -90f,
            "time", 2f,
            "delay", .5f
            ));
        */           
    }

}

[System.Serializable]
public class ClearTerm
{
    [SerializeField]
    public CrearType type;
    [SerializeField]
    public int value;

}