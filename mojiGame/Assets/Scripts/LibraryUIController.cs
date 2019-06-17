using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LibraryUIController : MonoBehaviour
{
    private TextMeshProUGUI MeanText;

    private void Awake()
    {
        MeanText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void ViewMeanByWord(Word word)
    {
        print(word.word + "\n\n" + word.mean);
        MeanText.SetText(word.word + "\n\n" + word.mean);
    }
}
