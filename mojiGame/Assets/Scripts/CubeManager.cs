using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeManager : MonoBehaviour
{
    #region Parameters
    [Header("Blocks Stetus")]
    [SerializeField]
    GameObject blockPrefab;
    [SerializeField]
    float blockDownSpeed = 2f;                      // ブロックの落ちるスピード
    [SerializeField]
    float blockFixSpeed = .5f;                      // ブロックが固定される時間
    [Header("StageObj")]
    [SerializeField]
    private Transform[] StagePoint;
    public char[,] fieldChar {get; protected set;} = new char[21,8];     // フィールドにブロックが置かれているかどうか 0:なし その他:あり
    private Cube[,] fieldCube = new Cube[20, 6];

    private GameManager gameMan;


    /* TODO: fieldCharいる？？
     *    
     * fieldChar[19,1]が左下
     * fieldCube[20,0]が左下
     * Pos[0,0] →　fieldChar[0, 1]   
     *          →　fieldCube[0, 0]
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

    public static CubeManager Instance;
    #endregion

    #region private Method
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        gameMan = GameManager.Instance;
        SceneController.StartGameEvent += new SceneController.ChengeGameEventHandler(init);
    }

    private void Update()
    {
        // 時間を更新
        UpdateTime();

        // ブロックを固定
        if (currentFixTime > blockFixSpeed)
        {
            FixedCurrentCube();
        }

        // ブロックを自由落下
        if(currentDownTime > blockDownSpeed)
        {
            currentDownTime = 0f;
            if(isBlockByPos())
                currentMovableCube.DropBlock();
        }

    }

    private void UpdateTime()
    {
        if (gameMan.isPlaying)
        {
            currentDownTime += Time.deltaTime;
            currentTime += Time.deltaTime;

            if (!isBlockByPos())
                currentFixTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// 引数に指定された座標から作られた言葉を集計する。
    /// </summary>
    /// <param name="findedWords">Finded words.</param>
    private void CreatedWordSetScore(FindedWordAndPos[] findedWords)
    {
        for (int i = 0; i < findedWords.Length; i++)
        {
            bool isCreatWord = false;
            // TODO: Cubeの原点を直した際にpositionのオフセットを直す。
            var Pos = findedWords[i].Position;
            for (int j = 0; j < findedWords[i].Word.word.Length; j++)
            {
                Debug.Log("currentCube.x = " + currentMovableCube.Pos.x + ", Pos.x = " + Pos.x +
                    "\ncurrentCube.y = " + currentMovableCube.Pos.y + ", Pos.y = " + Pos.y +
                    "\nisHorizontal = "+ findedWords[i].isHorizontal);

                // タテヨコを判定
                if (findedWords[i].isHorizontal)
                {
                    // 横 
                    Cube cube = fieldCube[Pos.y, Pos.x + j];
                    if (cube != null)
                    {
                        if (!cube.isMovable)
                        {
                            isCreatWord = cube.CreatedWord();
                        }
                    }
                }
                else
                {
                    // たて
                    Debug.Log("fieldCube = " + Pos.x + ", " + (currentMovableCube.Pos.y + Pos.y + j));

                    Cube cube = fieldCube[currentMovableCube.Pos.y + Pos.y + j, Pos.x];
                    if (cube != null)
                    {
                        if (!cube.isMovable)
                        {
                            isCreatWord = cube.CreatedWord();
                        }
                    }
                }
                if(isCreatWord)
                {
                    GameManager.Instance.ViewWord(findedWords[i].Word);
                }
            }
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    private void init()
    {
        print("CubeManager inited!");
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

        //前のステージのCubeを全削除
        print("reset Stage = " + ((gameMan.stage + 2) % 3));
        foreach (Transform n in StagePoint[(gameMan.stage + 2) % 3])
        {
            Destroy(n.gameObject);
        }
        fieldCube = new Cube[20, 6];

        currentTime = 0;
        currentFixTime = 0;
        currentDownTime = 0;

        currentMovableCube = Instantiate<GameObject>(blockPrefab, StagePoint[gameMan.stage]).GetComponent<Cube>();

        //PrintField();
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
    /// 指定された位置のブロックを消す
    /// </summary>
    /// <param name="Position">消すブロックの座標</param>
    public void DestroyBlockByPos(Vector2Int Position)
    {
        // TODO: Cubeの原点を直した際にpositionのオフセットを直す。
        print("Position = [" + (Position.x) + ", " + (Position.y) + "]");
        Destroy(fieldCube[Position.y, Position.x].gameObject);
        // 消したブロックの上のブロックたちを一つ下にずらす。
        for (int i = Position.y; i > 0; i--)
        {
            print("Chenge FieldChar [" + i + ", " + (Position.x + 1) + "] = " + fieldChar[i, (Position.x + 1)]);
            fieldChar[i, Position.x + 1] = fieldChar[i - 1, Position.x + 1];

            Debug.Log("fieldCube[" + i + ", " + Position.x + "] = " +
                fieldCube[i, Position.x].blockChar);

            fieldCube[i, Position.x] = fieldCube[i - 1, Position.x];

            //もし上のブロックがなくなったら終わる
            if (fieldChar[i, Position.x + 1] == '0')
                break;

            fieldCube[i, Position.x].DropBlock();
        }

        //PrintField();
    }

    /// <summary>
    /// 座標にブロックがあるかどうかを調べる
    /// </summary>
    public bool isBlockByPos(int Horizontal = 0, int Vertical = 1)
    {
        if (fieldChar[(currentMovableCube.Pos.y) + Vertical, (currentMovableCube.Pos.x + 1) + Horizontal] == '0')
            return true;
        return false;
    }

    /// <summary>
    /// currentCubeを固定して次のCubeを生成する。
    /// </summary>
    public void FixedCurrentCube()
    {
        currentFixTime = 0f;
        currentMovableCube.OnFixedEnter();
        // TODO: Cubeの原点を直した際にpositionのオフセットを直す。
        fieldChar[currentMovableCube.Pos.y, currentMovableCube.Pos.x + 1] = currentMovableCube.blockChar;
        fieldCube[currentMovableCube.Pos.y, currentMovableCube.Pos.x] = currentMovableCube;
        FindedWordAndPos[] findedWords = LibraryManager.FindWordByPos(currentMovableCube.Pos);
        Debug.Log("findedWords = " + findedWords.Length);
        // 完成した単語を検索して文字を赤くする
        CreatedWordSetScore(findedWords);
        //PrintField();
        currentMovableCube = Instantiate(blockPrefab, StagePoint[gameMan.stage]).GetComponent<Cube>();
    }
    #endregion
}
