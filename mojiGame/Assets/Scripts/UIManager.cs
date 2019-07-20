using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    GameManager gameMan;

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
        gameMan = GameManager.Instance;
        AllUIHide();
		SceneController.StartStartEvent += SetStartScene;
        SceneController.StartGameEvent += SetGameScene;
        SceneController.StartClearEvent += SetClearScene;
        SceneController.StartNextStageEvent += AllUIHide;
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

	public void OnClickStartButton()
	{
        print("Start");
        gameMan.Init();
        gameMan.ChengeNextScene();
    }
}
