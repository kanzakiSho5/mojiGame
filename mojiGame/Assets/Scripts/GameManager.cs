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

    private void Update()
    {

        // ブロックを固定
        if(currentFixTime > blockFixSpeed)
        {
            currentFixTime = 0f;
            currentMovableCube.isMovable = false;
            fieldChar[currentMovableCube.Pos.y-1,currentMovableCube.Pos.x-1] = currentMovableCube.blockChar;
            Debug.Log("FindWord = " + LibraryManager.Instance.FindWordByPos(currentMovableCube.Pos));
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

        
        
        currentMovableCube = Instantiate<GameObject>(blockPrefab).GetComponent<Cube>();
        currentTime = 0;
        currentFixTime = 0;
        currentDownTime = 0;

        PrintField();
    }

    private void PrintField()
    {
        string DebStr = "";
        for (int i = 0; i < fieldChar.GetLength(0); i++)
        {
            for (int j = 0; j < fieldChar.GetLength(1); j++)
            {
                DebStr += fieldChar[i, j].ToString();
            }
            DebStr += "\n";
        }
        Debug.Log(DebStr);
    }
}
