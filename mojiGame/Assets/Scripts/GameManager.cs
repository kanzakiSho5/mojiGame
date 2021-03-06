﻿using System;
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

        SceneController.StartGameEvent += GameStart;
        SceneController.StartClearEvent += GameClear;
    }

    private void Update()
    {
        switch(SceneController.CurrentScene)
        {
            case SceneType.Clear:
                if (InputManager.Instance.BtnEnterDown)
                {
                    print("NextStage");
                    stage++;
                    cameraCon.StageCameraAllOff();
                    sceneCon.ChengeNextScene();
                    playerCon.ClearStage(stage);
                }
                break;
            case SceneType.Game:
                break;
            case SceneType.GameOver:
                if(InputManager.Instance.BtnEnterDown)
                {
                    sceneCon.ChengeGameScene();
                    CubeManager.Instance.DestroyFieldObj(stage);
                }
                break;
            case SceneType.NextStage:
                break;
            case SceneType.Pause:
                break;
            case SceneType.Start:
                break;
        }
    }

    public void Init()
    {
        print("GameManager init!");
        playerCon.Init();
        stage = 0;
        score = 0;
        isPlaying = false;
    }

    private void GameStart()
    {

        score = 0;
        scoreCon.SetScoreText(0);
        cameraCon.MoveStageCamera(stage);
        isPlaying = true;
    }

    private void GameClear()
    {
        isPlaying = false;

        playerCon.ClearStage(stage);
    }

    private void GameOver()
    {
        isPlaying = false;
    }

    public void ViewWord(Word word)
    {
        createdWordLength = word.word.Length;
        score += createdWordLength * 1000;
        scoreCon.SetScoreText(score);
        UICon.ViewMeanByWord(word);
        sceneCon.CheckClear();
    }

    public void ChengeGameOverScene()
    {
        sceneCon.ChengeGameOverScene();
    }

    public void ChengeStartScene()
    {
        sceneCon.ChengeStartScene();
    }

    public void ChengeNextScene()
    {
        sceneCon.ChengeNextScene();
    }


    public void StageMoveAnimationEnded()
    {
        sceneCon.ChengeGameScene();
    }

}
