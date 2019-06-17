using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ScoreText;

    private void Awake()
    {
        ScoreText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void SetScoreText(int score)
    {
        ScoreText.SetText(score.ToString());
    }
}
