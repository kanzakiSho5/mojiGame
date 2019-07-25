using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridAnimation : MonoBehaviour
{
    [SerializeField]
    private Sprite[] GridSprites;
    [SerializeField]
    private Image image;

    private void OnEnable()
    {
        StartCoroutine(AnimateGrid());
    }

    private IEnumerator AnimateGrid()
    {
        for(int i = 0; i < GridSprites.Length; i++)
        {
            image.sprite = GridSprites[i];
            yield return new WaitForSeconds(0.06f);
        }
    }
}
