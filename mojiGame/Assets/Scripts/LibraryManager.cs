using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{

    static Library library;

    public static LibraryManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        using (var reader = new StreamReader(Application.streamingAssetsPath + "/Library.json", System.Text.Encoding.UTF8))
        {
            string JsonStr = reader.ReadToEnd();
            library = Library.CreateFromJson(JsonStr);
        }
    }

    public static FindedWordAndPos[] FindWordByPos(Vector2Int Pos)
    {
        char[,] fieldChar = GameManager.Instance.fieldChar;
        List<FindedWordAndPos> ret = new List<FindedWordAndPos>();
        for(int i = 0; i < 2; i++)
        {
            char[] str = new char[11];
            // 上下左右文字数制限の６文字分(自身もカウントするので5つ)
            for (int j = -5; j <= 5; j++)
            {
                // たてか横かを判定
                if(i == 0)
                {
                    // よこ
                    // fieldの範囲外にならないように
                    if(Pos.x + j > 0 && Pos.x + j <= 6)
                    {
                        str[Pos.x + j - 1] = fieldChar[Pos.y - 1, Pos.x + j];
                        //Debug.Log("横 = " + (j + 5));
                    }
                }
                else
                {
                    // たて
                    // fieldの範囲外にならないように
                    if(Pos.y + j > 0 && Pos.y + j <= 20)
                    {
                        //Debug.Log("縦 = " + j +", "+ Pos.y + "," + (j + 5));
                        str[j + 5] = fieldChar[Pos.y - 1 + j, Pos.x - 1];
                    }
                }
            }
            Debug.Log(new string(str));
            for(int j = 0; j < library.words.Length; j++)
            {
                //始めの位置を探す
                string searchWord = library.words[j].word;
                string s = new string(str);
                int foundIndex = s.IndexOf(searchWord);

                Debug.Log("foundIndex = " + foundIndex);

                // タテヨコ判定
                if(foundIndex < -1)
                {
                    if (i == 0)
                        ret.Add(new FindedWordAndPos(library.words[j], new Vector2Int(foundIndex, Pos.y)));
                    else
                        ret.Add(new FindedWordAndPos(library.words[j], new Vector2Int(Pos.x, foundIndex)));
                }
                
            }

        }
        return ret.ToArray();
    }
}

/// <summary>
/// Finded word and position.
/// フィールド上で見つけた単語とその開始位置を持ったクラス
/// </summary>
public class FindedWordAndPos
{
    public Word Word;
    public Vector2Int Position;

    public FindedWordAndPos(Word word, Vector2Int position)
    {
        this.Word = word;
        this.Position = position;
    }
}

[System.Serializable]
public class Library
{
    public Word[] words;

    /// <summary>
    /// JsonをLbrary形式で返します。
    /// </summary>
    /// <param name="json">string型のjson文字列</param>
    /// <returns>Library</returns>
    public static Library CreateFromJson(string json)
    {
        return JsonUtility.FromJson<Library>(json);
    }
}

[System.Serializable]
public class Word
{
    public string word;
    public string mean;
}
