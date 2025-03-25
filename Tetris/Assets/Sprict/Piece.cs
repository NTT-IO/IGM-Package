using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Piece : MonoBehaviour
{
    public MusicManage musicManage;
    public TextMeshProUGUI speedText;
    public Board board {  get; private set; }
    public Vector3Int position;
    public TerominoData data;
    public Vector3Int[] cells;
    //���δ���
    public int count {  get; private set; }

    //��������
    public int roationIndex {  get; private set; }

    //�����ٶ�
    public static float normalSpeed = 0.4f;

    //��������
    public float fastSpeed = 0.02f;

    //��ǰ�ٶ�
    public float currentSpeed { get; private set; }

    //ʱ���
    public float lastTime {  get; private set; }

    //����������ʱ��
    public float lockDelay = 0.4f; //�ʱ��
    public float lockTime = 0f;    //��ǰʱ��
    //�����ɫ
    public Colors ResoureLibrary;
    public int color;
    public SpriteRenderer[] Block_Image=new SpriteRenderer[5];
    //time
    public float Atime = 0.2f;
    public float Dtime = 0.2f;
    public void Initialize(Board board,Vector3Int position, TerominoData data,int color)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.color = color;
        cells = new Vector3Int[data.cells.Length];
        for(int i=0;i<cells.Length;i++)
        {
            cells[i]=(Vector3Int)data.cells[i];
        }
        count = 0;
        roationIndex = 0;
        currentSpeed = 0;
        lastTime = 0;
        //��������ʱ��
        lockTime = 0f;
    }
    private void Update()
    {
        if(currentSpeed>10)
        {
            currentSpeed = 10;
        }
        float temp = float.Parse( currentSpeed.ToString("#0.000"));
        speedText.text = "" + (temp > 10 ? 10 : temp);
        if(temp<0.8)
        {
            ColorLerpUtility.UpdateTextColor(temp, 0f, 0.8f, Color.red, Color.blue, speedText);
        }
        board.ClearPiece(this);
        //�����±������ı���ɫ   ��Ƭ
        data.tile= ResoureLibrary.tiles[this.color];
        //����Ƿ��������
        bool canMoveDown = CheckIfValidDown();
        if (!canMoveDown)
        {
            lockTime += Time.deltaTime;
            if(lockTime>lockDelay)
            {
                Lock();
                return;
            }
        }
        if(Atime>0f)
        {
            Atime -= Time.deltaTime;
        }
        if(Dtime>0f)
        {
            Dtime -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)&&Atime<=0f)
        {
            Move(Vector2Int.left);
            Atime = 0.2f;
        }
        if (Input.GetKey(KeyCode.D)&&Dtime<=0f)
        {
            Move(Vector2Int.right);
            Dtime = 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            count++;
            musicManage.PlayChange();
            Rotate(count);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            musicManage.PlayChange();

            color++;
            if (color > 6)
                color = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            musicManage.PlayChange();

            color--;
            if (color < 0)
                color = 6;
        }
        Show_Next_Last_Color(color);
        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed =fastSpeed;
        }
        else
        {
            currentSpeed = normalSpeed;
        }
        
        if(Time.time-lastTime>currentSpeed)
        {
            Move(Vector2Int.down);
            lastTime=Time.time;
        }
        board.SetPiece(this);
    }
    //�ƶ�����
    void Show_Next_Last_Color(int index)
    {
        
        int lastIndex = index - 1;
        int nextIndex = index + 1;
        
        if( lastIndex < 0 )
        {
            lastIndex = 6;
        }
        if(nextIndex>6)
        {
            nextIndex = 0;
        }
        int l_lastIndex = lastIndex - 1;
        int n_nextIndex = nextIndex + 1;
        if( l_lastIndex < 0 )
        {
            l_lastIndex = 6;
        }
        if(n_nextIndex>6)
        {
            n_nextIndex = 0;
        }
        Block_Image[0].sprite=board.BlocksColor[l_lastIndex];
        Block_Image[1].sprite =board.BlocksColor[lastIndex];
        Block_Image[2].sprite = board.BlocksColor[index];
        Block_Image[3].sprite = board.BlocksColor[nextIndex];
        Block_Image[4].sprite = board.BlocksColor[n_nextIndex];

    }
    void Move(Vector2Int translate)
    {
        Vector3Int currentPosition = position;
        currentPosition.x += translate.x;
        currentPosition.y += translate.y;
        bool valid = board.IsValidPosition(this,currentPosition);
        if (valid)
        {
            position = currentPosition;
        }
    }
    //��ת
    void Rotate(int count)
    {
        roationIndex = Warp(count,0,4);
        //����ԭ���ı���״̬
        Vector3Int[] originalCells = new Vector3Int[cells.Length];
        Array.Copy(cells, originalCells, cells.Length);
        if(data.type==TetrominoType.O)
        {
            return;
        }
        else
        {
            for (int i = 0; i < data.cells.Length; i++)
            {
                Vector3Int cell = cells[i];
                int x = Mathf.RoundToInt(cell.x * Data.rotateRetrix[0] + cell.y * Data.rotateRetrix[1]);
                int y = Mathf.RoundToInt(cell.x * Data.rotateRetrix[2] + cell.y * Data.rotateRetrix[3]);
                cells[i] = new Vector3Int(x, y, 0);
            }
        }
        //�ж��Ƿ���ǽ���Լ��ع�
        bool valid = TryKick(roationIndex);
        if (!valid)
        {
            Array.Copy(originalCells, cells, cells.Length);
        }
    }
    //��ǽ�б�ѭ������
    public int Warp(int count,int min,int max)
    {
        if (count < min)
        {
            return max - (min - count) % (max - min);
        }
        else
        {
            return min + (count - min) % (max - min);

        }
    }
    //�����Ƿ���ǽ�ɹ�
    public bool TryKick(int rotationIndex)
    {
        Vector2Int[,] kickData = Data.kickData[this.data.type];
        for (int i = 0; i < kickData.Length; i++)
        {
            Vector2Int kick = kickData[roationIndex, i];
            Vector3Int newPosition = position + new Vector3Int(kick.x, kick.y, 0);
            if(board.IsValidPosition(this,newPosition))
            {
                this.position = newPosition;
                return true;
            }
        }
        return false;
    }
    //����ǿ��Ƿ��ܼ�������
    private bool CheckIfValidDown()
    {
        Vector3Int newPosition = position;
        newPosition.y += Vector2Int.down.y;
        return board.IsValidPosition(this,newPosition);
    }
    //������
    private void Lock()
    {
        board.LockPiece(this, color);
    }
}
