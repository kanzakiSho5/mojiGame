using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{

    static Library library;


    private void Awake()
    {
        using (var reader = new StreamReader(Application.streamingAssetsPath + "/Library.txt", System.Text.Encoding.UTF8))
        {
            string JsonStr = reader.ReadToEnd();
            library = Library.CreateFromJson(JsonStr);
            Debug.Log(library.words[176].mean);
        }
    }

    /// <summary>
    /// ポジションから、縦横６文字に単語が含まれているかを探す。
    /// </summary>
    /// <returns>見つけた単語と位置</returns>
    /// <param name="Pos">検索する原点</param>
    public static FindedWordAndPos[] FindWordByPos(Vector2Int Pos)
    {
        char[,] fieldChar = CubeManager.Instance.fieldChar;
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
                    str[j] = fieldChar[Pos.y, j + 1];
                }
                else
                {
                    // たて
                    // fieldの範囲外にならないように
                    if(Pos.y + j <= 20)
                    {
                        //Debug.Log("縦 = " + (Pos.y + j) + ", "+ (Pos.x + 1) + ",");
                        str[j] = fieldChar[Pos.y + j, Pos.x + 1];
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
                        ret.Add(new FindedWordAndPos(library.words[j], new Vector2Int(foundIndex, Pos.y), true));
                    else
                        ret.Add(new FindedWordAndPos(library.words[j], new Vector2Int(Pos.x, foundIndex), false));

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
    public bool isHorizontal;

    public FindedWordAndPos(Word word, Vector2Int position,bool isHorizontal)
    {
        this.Word = word;
        this.Position = position;
        this.isHorizontal = isHorizontal;
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
