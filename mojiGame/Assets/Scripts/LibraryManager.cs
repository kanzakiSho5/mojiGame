using System.IO;
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
            Debug.Log(library.words[0].word);
        }
    }

    public Vector2Int[] FindWordByPos(Vector2Int Pos)
    {
        char[,] fieldChar = GameManager.Instance.fieldChar;
        
        char[] Hstr = new char[6];
        char[] Vstr = new char[11];
        for(int i = 0; i < 2; i++)
        {
            for(int j = -5; j <= 5; j++)
            {
                if(i == 0)
                {
                    if(Pos.x + j > 0 && Pos.x + j <= 6)
                    {
                        Hstr[j - 1] = fieldChar[Pos.y -1, Pos.x + j];
                    }
                }
                else
                {
                    if(Pos.y + j > 0 && Pos.y + j <= 20)
                    {
                        Vstr[j - 1] = fieldChar[Pos.y -1 + j, Pos.x - 1];
                    }
                }
            }

            for(int j = 0; j < library.words.Length; j++)
            {
                // TODO: 完成した単語の検索（位置？言葉？完成した単語のブロックの位置がわからないといけない）
            }

        }
        return Vstr;
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
