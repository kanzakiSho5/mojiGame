using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cube : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private GameObject Effect;
    
    [SerializeField]
    public bool isMovable;
    public bool isCreatedWord { get; protected set; }
    [SerializeField]
    private GameObject createdParticle;

    [SerializeField]
    public Vector2Int Pos;
    
    [System.NonSerialized]
    public char blockChar;
    

    private InputManager inputMan;
    private CubeManager cubeMan;
    private const float DeleteTime = 20.0f;
    private float FixedTime = 0;
    private GameObject hardDropParticle;

    private bool isHardDrop = false;

    public delegate void FixedEnter();
    public FixedEnter OnFixedEnter;

    private void OnEnable()
    {
        Pos = new Vector2Int(3, 4);
        inputMan = InputManager.Instance;
        cubeMan = CubeManager.Instance;
        gameObject.transform.localPosition = new Vector3(Pos.x, (20 - Pos.y), Random.Range(0f, .7f));
        isMovable = true;
        isCreatedWord = false;
        SetChar(true);
        OnFixedEnter += FixedBlock;
    }

    private void Update()
    {
        Move();
        if (!isMovable && !isCreatedWord)
            DeleteBlock();
    }

    private void DeleteBlock()
    {
        if (cubeMan.currentTime - FixedTime >= DeleteTime)
            cubeMan.DestroyBlockByPos(Pos);
    }

    private void UpdatePos()
    {
        gameObject.transform.localPosition = new Vector3(Pos.x, (20 - Pos.y), this.transform.localPosition.z);
    }

    /// <summary>
    /// ブロックの文字を設定
    /// </summary>
    /// <param name="DropItem">trueで"メメタァクパパン"のみ選ばれる</param>
    public void SetChar(bool DropItem = false)
    {
        if(DropItem)
        {
            int rand = Random.Range(0, 6);
            char ret;
            switch(rand)
            {
                case 0:
                    ret = 'め';
                    break;
                case 1:
                    ret = 'た';
                    break;
                case 2:
                    ret = 'あ';
                    break;
                case 3:
                    ret = 'く';
                    break;
                case 4:
                    ret = 'ぱ';
                    break;
                case 5:
                    ret = 'ん';
                    break;
                default:
                    ret = 'め';
                    break;
            }

            blockChar = ret;
        }
        else
        {
            int rand = Random.Range(12353, 12436);
            blockChar = (char)rand;
        }

        if (text != null)
            text.SetText(blockChar.ToString());
    }
    
    private void Move()
    {
        if(isMovable)
        {
            if (inputMan.BtnLeftDown && cubeMan.isBlockByPos(-1, 0))
            {
                Pos.x--;
                UpdatePos();
                cubeMan.PlayCubeSound(SoundType.move);
            }
            else if (inputMan.BtnRightDown && cubeMan.isBlockByPos(1, 0))
            {
                Pos.x++;
                UpdatePos();
                cubeMan.PlayCubeSound(SoundType.move);
            }
            else if (inputMan.BtnDownDown && cubeMan.isBlockByPos())
            {
                Pos.y++;
                UpdatePos();
                cubeMan.PlayCubeSound(SoundType.move);
            }
            else if (inputMan.BtnUpDown && cubeMan.isBlockByPos())
            {
                if (isHardDrop)
                    return;
                print("Harddrop");
                cubeMan.PlayCubeSound(SoundType.hardDrop);
                isHardDrop = true;
                StartCoroutine(HardDrop());
            }
        }
    }

    /// <summary>
    /// 動かないようにする。
    /// </summary>
    private void FixedBlock()
    {
        print("FixedBlock");
        if (hardDropParticle != null)
            StartCoroutine(WaitHardDropParticle());
        isMovable = false;
        FixedTime = cubeMan.currentTime;
    }

    IEnumerator WaitHardDropParticle()
    {
        yield return new WaitForSeconds(.2f);
        Destroy(hardDropParticle);
    }
    
    IEnumerator HardDrop()
    {
        hardDropParticle = Instantiate(Effect, transform);
        while (cubeMan.isBlockByPos())
        {
            Pos.y++;
            UpdatePos();
            yield return new WaitForEndOfFrame();
        }
        print("EndHardDrop");
        cubeMan.FixedCurrentCube();
    }

    /// <summary>
    /// 1ブロック分下げる
    /// </summary>
    public void DropBlock()
    {
        Pos.y++;
        UpdatePos();
    }

    /// <summary>
    /// ブロックが文字ができた時
    /// </summary>
    /// <returns><c>true</c>, if word was createded, <c>false</c> otherwise.</returns>
    public bool CreatedWord()
    {
        
        if (isCreatedWord)
            return false;

        isCreatedWord = true;

        var particle = Instantiate(createdParticle, transform);
        particle.transform.localPosition = -Vector3.forward;
        text.color = Color.red;

        return true;
    }
}
