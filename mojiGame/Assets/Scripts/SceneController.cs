using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CrearType
{
    Score,
    WordLength
}

public class SceneController : MonoBehaviour
{
    [SerializeField]
    ClearTerm[] clearTerm;

    GameManager gameMan;

    public void Start()
    {
        gameMan = GameManager.Instance;
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
                    return true;
                break;
            case CrearType.WordLength:
                if (gameMan.createdWordLength >= clearTerm[stage].value)
                    return true;
                break;
        }

        return false;
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