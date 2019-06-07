using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    #region Parameters
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

    /* TODO: CubeのPositionをfieldCube基準に直す.
     * TODO: fieldCharいる？？   
     * fieldChar[19,1]が左下
     * fieldCube[20,0]が左下
     * Pos[0,0] →　fieldChar[0, 0]   
     *          →　fieldCube[0,-1] //OutOfIndexになるので注意！！
     * 
     * 
     * fieldChar    
     * [ 0, 0][ 0, 1][ 0, 2][ 0, 3][ 0, 4][ 0, 5][ 0, 6][ 0, 7][ 0, 8]
     * [ 1, 0][ 1, 1][ 1, 2][ 1, 3][ 1, 4][ 1, 5][ 1, 6][ 1, 7][ 1, 8]
     * [ 2, 0][ 2, 1][ 2, 2][ 2, 3][ 2, 4][ 2, 5][ 2, 6][ 2, 7][ 2, 8]
     * ・ ・   
     * ・ ・
     * ・ ・
     * [20, 0][20, 1][20, 2][20, 3][20, 4][20, 5][20, 6][20, 7][20, 8]
     * 
     * Field系,Pos系は左上が原点なので注意！
     * 
     */

    [Header("Debug")]
    public float currentTime;//{ get; protected set; }
    [SerializeField]
    private float currentFixTime;
    [SerializeField]
    private float currentDownTime;
    private Cube currentMovableCube;

    public static GameManager Instance;
    #endregion

    #region private Method
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
            currentMovableCube.FixedBlock();
            // TODO: Cubeの原点を直した際にpositionのオフセットを直す。
            fieldChar[currentMovableCube.Pos.y - 1, currentMovableCube.Pos.x - 1] = currentMovableCube.blockChar;
            fieldCube[currentMovableCube.Pos.y - 1, currentMovableCube.Pos.x - 2] = currentMovableCube;
            FindedWordAndPos[] findedWords = LibraryManager.Instance.FindWordByPos(currentMovableCube.Pos);
            //Debug.Log("findedWords = "+ findedWords.Length);
            // 完成した単語を検索して文字を赤くする
            CreatedWordSetScore(findedWords);
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

    private void UpdateTime()
    {
        currentDownTime += Time.deltaTime;
        currentTime     += Time.deltaTime;

        if (!isBlockByPos())
            currentFixTime += Time.deltaTime;
    }

    /// <summary>
    /// 指定された位置のブロックを消す
    /// </summary>
    /// <param name="Position">消すブロックの座標</param>
    public void DestroyBlockByPos(Vector2Int Position)
    {
        // TODO: Cubeの原点を直した際にpositionのオフセットを直す。
        print("Position = [" + (Position.x - 2) + ", " + (Position.y - 1) + "]" );
        Destroy(fieldCube[Position.y - 1, Position.x - 2].gameObject);
        // 消したブロックの上のブロックたちを一つ下にずらす。
        for (int i = Position.y - 1; i > 0; i--)
        {
            //もし上のブロックがなくなったら終わる
            if (fieldChar[i - 1, Position.x - 1] == '0')
                break;
            print("Chenge FieldChar ["+ (i - 1) +", "+ (Position.x - 1) +"] = " + fieldChar[i, (Position.x - 1)]);
            fieldChar[i - 1, Position.x - 1] = fieldChar[i - 2, Position.x - 1];

            Debug.Log("fieldCube[" + (Position.x - 2) + ", " + (i - 1) + "] = " +
                fieldCube[i - 1, Position.x - 2].blockChar);

            fieldCube[i - 1, Position.x - 2] = fieldCube[i - 2, Position.x - 2];
            fieldCube[i - 1, Position.x - 2].DropBlock();
        }

        PrintField();
    }

    /// <summary>
    /// 引数に指定された座標から作られた言葉を集計する。
    /// </summary>
    /// <param name="findedWords">Finded words.</param>
    private void CreatedWordSetScore(FindedWordAndPos[] findedWords)
    {
        for (int i = 0; i < findedWords.Length; i++)
        {
            // TODO: Cubeの原点を直した際にpositionのオフセットを直す。
            var Pos = findedWords[i].Position;
            for (int j = 0; j < findedWords[i].Word.word.Length; j++)
            {
                Debug.Log("currentCube.x = " + currentMovableCube.Pos.x + ", Pos.x = " + Pos.x +
                    "\ncurrentCube.y = " + currentMovableCube.Pos.y + ", Pos.y = " + Pos.y);
                // タテヨコを判定
                if (currentMovableCube.Pos.x == Pos.x)
                {
                    // 横
                    //Debug.Log("fieldCube = " + (Pos.x - 1) + ", " + (currentMovableCube.Pos.y + Pos.y + j - 2));

                    Cube cube = fieldCube[currentMovableCube.Pos.y + Pos.y + j - 1, Pos.x - 2];
                    if (cube != null)
                        if (!cube.isFixed)
                            fieldCube[currentMovableCube.Pos.y + Pos.y + j - 1, Pos.x - 2].CreatedWord();
                }
                else
                {
                    // たて                       
                    //Debug.Log("fieldCube = " + (Pos.x + j) + ", " + (Pos.y - 1));

                    Cube cube = fieldCube[Pos.y - 1, Pos.x + j];
                    if (cube != null)
                        if (!cube.isFixed)
                            fieldCube[Pos.y - 1, Pos.x + j].CreatedWord();
                }
            }
        }
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

    /// <summary>
    /// デバッグ用
    /// </summary>
    private void PrintField()
    {
        string DebStr = "fieldChar = \n";


        for (int i = 0; i < fieldChar.GetLength(0); i++)
        {
            for (int j = 0; j < fieldChar.GetLength(1); j++)
            {
                DebStr += fieldChar[i, j].ToString();
            }
            DebStr += " "+ i.ToString() +"\n";
        }
        print(DebStr);

        DebStr = "fieldCube = \n";
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
    #endregion

    #region public Method

    /// <summary>
    /// 座標にブロックがあるかどうかを調べる
    /// </summary>
    public bool isBlockByPos(int Horizontal = 0, int Vertical = 1)
    {
        if (fieldChar[(currentMovableCube.Pos.y - 1) + Vertical, (currentMovableCube.Pos.x - 1) + Horizontal] == '0')
            return true;
        return false;
    }

    #endregion
}
