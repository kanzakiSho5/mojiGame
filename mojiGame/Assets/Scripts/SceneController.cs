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
	NextStage,
	Pause,
	Clear
}

public class SceneController : MonoBehaviour
{
	[SerializeField]
	ClearTerm[] clearTerm;

	public SceneType CurrentScene { get; protected set; }

	GameManager gameMan;

    public delegate void ChengeGameEventHandler();
    public static event ChengeGameEventHandler StartGameEvent;

    public delegate void ChengeClearEventHandler();
    public static event ChengeClearEventHandler StartClearEvent;

    public delegate void ChengeNextStageEventHandler();
    public static event ChengeNextStageEventHandler StartNextStageEvent;

    private void Start()
	{
		gameMan = GameManager.Instance;
		CurrentScene = SceneType.Start;
	}

	public ClearTerm CurrentClearTermByStage(int Stage)
	{
		return clearTerm[Stage];
	}

	public void CheckClear()
	{

		int stage = gameMan.stage;
		switch (clearTerm[stage].type)
		{
			case CrearType.Score:
				if (gameMan.score >= clearTerm[stage].value)
				{
					CurrentScene = SceneType.Clear;
                    StartClearEvent();
				}
				break;
			case CrearType.WordLength:
				if (gameMan.createdWordLength >= clearTerm[stage].value)
				{
					CurrentScene = SceneType.Clear;
                    StartClearEvent();
				}
				break;
		}
        
	}

    public void StartGame()
    {
        StartGameEvent();
    }

    /// <summary>
    /// CurrentSceneをNextStageに変更する
    /// (Animationを再生する)
    /// </summary>
    public void MoveNextScene()
    {
        CurrentScene = SceneType.NextStage;
        StartNextStageEvent();
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