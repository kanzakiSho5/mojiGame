using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int Score;
    [Header("Blocks Stetus")]
    [SerializeField]
    GameObject blockPrefab;
    [SerializeField]
    float blockDownSpeed = 2f;                      // ブロックの落ちるスピード
    [SerializeField]
    float blockFixSpeed = .5f;                      // ブロックが固定される時間

    public char[,] fieldChar {get; protected set;} = new char[21,8];     // フィールドにブロックが置かれているかどうか 0:なし その他:あり
    private Cube[,] fieldCube = new Cube[20, 6];

    [Header("Debug")]
    public float currentTime;
    [SerializeField]
    private float currentFixTime;
    [SerializeField]
    private float currentDownTime;
    private Cube currentMovableCube;

    public static GameManager Instance;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        currentMovableCube = Instantiate<GameObject>(blockPrefab).GetComponent<Cube>();
    }

    private void Update()
    {

        // ブロックを固定
        if(currentFixTime > blockFixSpeed)
        {
            currentFixTime = 0f;
            currentMovableCube.isMovable = false;
            fieldChar[currentMovableCube.Pos.y - 1, currentMovableCube.Pos.x - 1] = currentMovableCube.blockChar;
            fieldCube[currentMovableCube.Pos.y - 1, currentMovableCube.Pos.x - 2] = currentMovableCube;
            FindedWordAndPos[] findedWords = LibraryManager.Instance.FindWordByPos(currentMovableCube.Pos);
            Debug.Log("findedWords = "+ findedWords.Length);
            // 完成した単語を検索して文字を赤くする
            for (int i = 0; i < findedWords.Length; i++)
            {
                var Pos = findedWords[i].Position;
                for (int j = 0; j < findedWords[i].Word.word.Length; j++)
                {
                    Debug.Log("currentCube.x = " + currentMovableCube.Pos.x + ", Pos.x = " + Pos.x +
                        "\ncurrentCube.y = " + currentMovableCube.Pos.y + ", Pos.y = " + Pos.y);
                    // タテヨコを判定
                    if(currentMovableCube.Pos.x == Pos.x)
                    {
                        // 縦
                        Debug.Log("fieldCube = " + (Pos.x - 2) + ", " + (currentMovableCube.Pos.y - j));
                        fieldCube[currentMovableCube.Pos.y - j, Pos.x - 2].SetColor();
                    }
                    else
                    {
                        // 横                       
                        Debug.Log("fieldCube = " + (Pos.x + j) + ", " + (Pos.y - 1));
                        fieldCube[Pos.y - 1, Pos.x + j].SetColor();
                    }
                }
            }
            PrintField();
            currentMovableCube = Instantiate<GameObject>(blockPrefab).GetComponent<Cube>();
        }

        if(currentDownTime > blockDownSpeed)
        {
            currentDownTime = 0f;
            if(isBlockByPos())
                currentMovableCube.DropBlock();
        }

        UpdateTime();
    }

    /// <summary>
    /// 座標にブロックがあるかどうかを調べる
    /// </summary>
    public bool isBlockByPos(int Horizontal = 0, int Vertical = 1)
    {
        if(fieldChar[(currentMovableCube.Pos.y - 1) + Vertical, (currentMovableCube.Pos.x - 1) + Horizontal] == '0')
            return true;
        return false;
    }



    private void UpdateTime()
    {
        currentDownTime += Time.deltaTime;
        currentTime     += Time.deltaTime;

        if (!isBlockByPos())
            currentFixTime += Time.deltaTime;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void init()
    {
        if(Instance == null)
            Instance = this;


        // fieldCharの初期化
        for (int i = 0; i < fieldChar.GetLength(0); i++)
        {
            for (int j = 0; j < fieldChar.GetLength(1); j++)
            {
                fieldChar[i, j] = '0';
                if (i == 20 || j == 0 || j == 7)
                    fieldChar[i, j] = '1';
            }
        }

        
        
        currentTime = 0;
        currentFixTime = 0;
        currentDownTime = 0;

        PrintField();
    }

    private void PrintField()
    {
        string DebStr = "";
        /*
        for (int i = 0; i < fieldChar.GetLength(0); i++)
        {
            for (int j = 0; j < fieldChar.GetLength(1); j++)
            {
                DebStr += fieldChar[i, j].ToString();
            }
            DebStr += "\n";
        }
        */

        for (int i = 0; i < fieldCube.GetLength(0); i++)
        {
            for(int j = 0; j < fieldCube.GetLength(1); j++)
            {
                if (fieldCube[i, j] != null)
                    DebStr += fieldCube[i, j].blockChar.ToString();
                else
                    DebStr += "●";
            }
            DebStr += (i.ToString() +"\n");
        }
        Debug.Log(DebStr);
    }
}
