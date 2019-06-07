﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{

    Library library;

    public static LibraryManager Instance;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;

        using (var reader = new StreamReader(Application.streamingAssetsPath + "/Library.json", System.Text.Encoding.UTF8))
        {
            string JsonStr = reader.ReadToEnd();
            library = Library.CreateFromJson(JsonStr);
            //Debug.Log(library.words[0].word);
        }
    }

    /// <summary>
    /// ポジションから、縦横６文字に単語が含まれているかを探す。
    /// </summary>
    /// <returns>見つけた単語と位置</returns>
    /// <param name="Pos">検索する原点</param>
    public FindedWordAndPos[] FindWordByPos(Vector2Int Pos)
    {
        // TODO: Cubeの原点を直した際にpositionのオフセットを直す。
        char[,] fieldChar = GameManager.Instance.fieldChar;
        List<FindedWordAndPos> ret = new List<FindedWordAndPos>();
        for(int i = 0; i < 2; i++)
        {
            char[] str = new char[6];
            // 下左右文字数制限の６文字
            for (int j = 0; j < 6; j++)
            {
                // たてか横かを判定
                if(i == 0)
                {
                    // 横
                    //Debug.Log("横 = " + (j));
                    str[j] = fieldChar[Pos.y - 1, j + 1];
                }
                else
                {
                    // たて
                    // fieldの範囲外にならないように
                    if(Pos.y + j <= 20)
                    {
                        Debug.Log("縦 = " + (Pos.y + j - 1) + ", "+ (Pos.x - 1) + ",");
                        str[j] = fieldChar[Pos.y + j - 1, Pos.x - 1];
                    }
                }
            }
            //Debug.Log(new string(str));
            for(int j = 0; j < library.words.Length; j++)
            {
                //始めの位置を探す
                string searchWord = library.words[j].word;
                string s = new string(str);
                int foundIndex = s.IndexOf(searchWord);
                while (0 <= foundIndex)
                {
                    // タテヨコ判定
                    if (i == 0)
                        ret.Add(new FindedWordAndPos(library.words[j], new Vector2Int(foundIndex, Pos.y)));
                    else
                        ret.Add(new FindedWordAndPos(library.words[j], new Vector2Int(Pos.x, foundIndex)));

                    //次の検索開始位置
                    int nextIndex = foundIndex + searchWord.Length;
                    if (nextIndex < s.Length)
                    {
                        //次の位置を探す
                        foundIndex = s.IndexOf(searchWord, nextIndex);
                    }
                    else
                    {
                        //最後まで検索したときは終わる
                        break;
                    }
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
