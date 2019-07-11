using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public int score { get; protected set; } = 0;
	public int stage { get; protected set; } = 0;
	public int createdWordLength { get; protected set; } = 0;

	public bool isPlaying { get; protected set; } = false;

	[Header("Classes")]
	[SerializeField]
	private ScoreController scoreCon;
	[SerializeField]
	private LibraryUIController libraryUICon;
	[SerializeField]
	private SceneController sceneCon;
	[SerializeField]
	private CameraManager cameraCon;
	[SerializeField]
	private PlayerController playerCon;
    [SerializeField]
    private UIManager UICon;

    
	

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
	}

	private void Start()
	{
		if (!scoreCon)
			Debug.LogError("ScoreController is NotFound");

		if (!libraryUICon)
			Debug.LogError("LibraryUIController is NotFound");
        
        SceneController.StartGameEvent += new SceneController.ChengeGameEventHandler(GameStart);
        SceneController.StartClearEvent += new SceneController.ChengeClearEventHandler(GameClear);

		init();
	}

	private void Update()
	{
		if(sceneCon.CurrentScene == SceneType.Clear)
		{
			if(InputManager.Instance.BtnEnterDown)
			{
                print("NextStage");
				stage++;
				cameraCon.StageCameraAllOff();
				sceneCon.MoveNextScene();
				playerCon.ClearStage(stage);
			}
		}
	}

	private void init()
	{
	}

	private void GameStart()
	{
        scoreCon.SetScoreText(0);
        cameraCon.MoveStageCamera(stage);
        isPlaying = true;
	}

	private void GameClear()
	{
		isPlaying = false;

		playerCon.ClearStage(stage);
	}

	public void ViewWord(Word word)
	{
		createdWordLength = word.word.Length;
		score += createdWordLength * 1000;
		scoreCon.SetScoreText(score);
		UICon.ViewMeanByWord(word);
        sceneCon.CheckClear();

    }
    


	public void StageMoveAnimationEnded()
	{
        sceneCon.StartGame();
	}

}
