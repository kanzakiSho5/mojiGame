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

    
}
