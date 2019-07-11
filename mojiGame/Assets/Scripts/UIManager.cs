using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField]
    private GameObject gameScene;
    [SerializeField]
    private GameObject StartScene;
    [SerializeField]
    private GameObject ClearScene;

    [Header("GameScene UI")]
    [SerializeField]
    private GameObject ClearText;
    [SerializeField]
    private TextMeshProUGUI MeanText;

    private void Start()
    {
        AllUIHide();
        SceneController.StartGameEvent += new SceneController.ChengeGameEventHandler(SetGameScene);
        SceneController.StartClearEvent += new SceneController.ChengeClearEventHandler(SetClearScene);
        SceneController.StartNextStageEvent += new SceneController.ChengeNextStageEventHandler(AllUIHide);
    }

    private void AllUIHide()
    {
        gameScene.SetActive(false);
        StartScene.SetActive(false);
        ClearScene.SetActive(false);
    }

    private void SetGameScene()
    {
        AllUIHide();
        gameScene.SetActive(true);
        MeanText.SetText("");
    }

    private void SetStartScene()
    {
        AllUIHide();
        StartScene.SetActive(true);
    }

    private void SetClearScene()
    {
        AllUIHide();
        ClearScene.SetActive(true);
    }

    public void ViewMeanByWord(Word word)
    {
        print(word.word + "\n\n" + word.mean);
        MeanText.SetText(word.word + "\n\n" + word.mean);
    }
}
