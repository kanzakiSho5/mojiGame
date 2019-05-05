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

    [Header("Debug")]
    public float currentTime;
    private int[,] fieldStatus = new int[21,8];     // フィールドにブロックが置かれているかどうか 0:なし 1:あり
    [SerializeField]
    private float currentFixTime;
    [SerializeField]
    private float currentDownTime;
    private Cube currentMovableCube;


    private void Awake()
    {
        init();
    }

    private void Update()
    {
        MoveBlock();

        if(currentFixTime > blockFixSpeed)
        {
            currentFixTime = 0f;
            currentMovableCube.isMovable = false;
            fieldStatus[currentMovableCube.Pos.y, currentMovableCube.Pos.x] = 1;
            PrintField();
            currentMovableCube = Instantiate<GameObject>(blockPrefab).GetComponent<Cube>();
        }

        if(currentDownTime > blockDownSpeed)
        {
            currentDownTime = 0f;
            if(isCanMoveBlock())
                currentMovableCube.DropBlock();
        }

        UpdateTime();
    }

    private void MoveBlock()
    {
        if (Input.GetKeyDown(KeyCode.A) && isCanMoveBlock(-1, 0))
        {
            currentMovableCube.Pos.x--;
            currentFixTime = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.D) && isCanMoveBlock(1, 0))
        {
            currentMovableCube.Pos.x++;
            currentFixTime = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.S) && isCanMoveBlock())
        {
            currentMovableCube.Pos.y++;
            currentDownTime = 0f;
        }
    }

    
    private bool isCanMoveBlock(int Horizontal = 0, int Vertical = 1)
    {
        if(fieldStatus[currentMovableCube.Pos.y + Vertical, currentMovableCube.Pos.x + Horizontal] == 0)
            return true;
        return false;
    }



    private void UpdateTime()
    {
        currentDownTime += Time.deltaTime;
        currentTime     += Time.deltaTime;

        if (!isCanMoveBlock())
            currentFixTime += Time.deltaTime;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void init()
    {
        for (int i = 0; i < fieldStatus.GetLength(0); i++)
        {
            for (int j = 0; j < fieldStatus.GetLength(1); j++)
            {
                fieldStatus[i, j] = 0;
                if (i == 20 || j == 0 || j == 7)
                    fieldStatus[i, j] = 1;
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
        for (int i = 0; i < fieldStatus.GetLength(0); i++)
        {
            for (int j = 0; j < fieldStatus.GetLength(1); j++)
            {
                DebStr += fieldStatus[i, j].ToString();
            }
            DebStr += "\n";
        }
        Debug.Log(DebStr);
    }
}
