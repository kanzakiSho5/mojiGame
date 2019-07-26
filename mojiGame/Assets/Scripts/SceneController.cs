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
    GameOver,
	NextStage,
	Pause,
	Clear
}

public class SceneController : MonoBehaviour
{
	[SerializeField]
	ClearTerm[] clearTerm;

	public static SceneType CurrentScene { get; protected set; }

	GameManager gameMan;

	public delegate void ChengeStartEventHandler();
	public static event ChengeStartEventHandler StartStartEvent;

    public delegate void ChengeGameEventHandler();
    public static event ChengeGameEventHandler StartGameEvent;

    public delegate void ChengeClearEventHandler();
    public static event ChengeClearEventHandler StartClearEvent;

    public delegate void ChengeNextStageEventHandler();
    public static event ChengeNextStageEventHandler StartNextStageEvent;

	public delegate void ChengePauseEventHandler();
	public static event ChengePauseEventHandler StartPauseEvent;

    public delegate void ChengeGameOverEventHandler();
    public static event ChengeGameOverEventHandler GameOverEvent;

    private void Start()
	{
		gameMan = GameManager.Instance;
        ChengeStartScene();
	}

	private void Update()
	{
        //print("CurrentScene = " + CurrentScene);
		if(InputManager.Instance.BtnEscapeDown)
		{
			if (CurrentScene == SceneType.Game)
			{
				StartPauseEvent();
				CurrentScene = SceneType.Pause;
			}
			else if(CurrentScene == SceneType.Pause)
			{
				StartGameEvent();
				CurrentScene = SceneType.Game;
			}
		}
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

    public void ChengeStartScene()
    {
        CurrentScene = SceneType.Start;
        StartStartEvent();
    }

    public void ChengeGameOverScene()
    {
        CurrentScene = SceneType.GameOver;
    }

    public void ChengeGameScene()
    {
        print("ChangeGameScene");
        CurrentScene = SceneType.Game;
        StartGameEvent();
    }

    /// <summary>
    /// CurrentSceneをNextStageに変更する
    /// </summary>
    public void ChengeNextScene()
    {
        print("Chenge stage NextScene");
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