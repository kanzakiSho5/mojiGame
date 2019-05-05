using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour
{

    Library library;

    private void Awake()
    {
        using (var reader = new StreamReader(Application.streamingAssetsPath + "/Library.json", System.Text.Encoding.UTF8))
        {
            string JsonStr = reader.ReadToEnd();
            Debug.Log(JsonStr);
        }
    }
}

[System.Serializable]
public class Library
{
    public string word;
    public string mean;

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
