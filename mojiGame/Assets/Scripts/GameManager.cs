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
    private GameObject ClearObj;

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

        init();
    }

    private void init()
    {
        ClearObj.SetActive(false);
        scoreCon.SetScoreText(0);
        GameStart();
    }

    private void GameStart()
    {
        isPlaying =true;
    }

    private void GameClear()
    {
        isPlaying = false;
        ClearObj.SetActive(true);

        sceneCon.MoveNextStage();
    }

    public void createdWord(Word word)
    {
        createdWordLength = word.word.Length;
        score += createdWordLength * 1000;
        scoreCon.SetScoreText(score);
        libraryUICon.ViewMeanByWord(word);
        if(sceneCon.CheckClear())
            GameClear();
    }
}
