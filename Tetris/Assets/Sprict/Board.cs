using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public MusicManage musicManage;
    public GameObject IGM;
    public TerominoData[] terominoDatas = new TerominoData[7];
    public Piece activePiece { get; private set; }
    public Vector3Int position;
    public Tilemap tilemap { get; private set; }
    public Vector2Int boardSize = new Vector2Int(14, 22);
    //计分事件
    public event System.Action<int> onLinesCleared;
    //游戏结束事件
    public event System.Action onGameOver;
    public bool gameOver { get; private set; }
    public Colors ResourceLibrary;
    public ParticleSystem blockbroken;
    public bool DestoryMonster;
    public TerominoData Next_tile;

    //bonus
    public static int nextPiece = 0;
    public static int whichPiece = 0;

    public MosterData mosterData;
    //next
    public SpriteRenderer nextImage;
    public Sprite[] BlocksSharp = new Sprite[7];
    public Sprite[] BlocksColor = new Sprite[7];

    public bool ifFinal = false;
    public RectInt Bounds
    {
        get
        {
            Vector2Int originalPosition = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(originalPosition, boardSize);
        }
    }
    //*****怪物
    public GameObject mosterPostion;
    public MosterData moster;
    //消除的行号
    public static int clearing_row;
    public void Awake()
    {
        Next_tile.tile = null;
        activePiece = GetComponent<Piece>();
        tilemap = GetComponentInChildren<Tilemap>();
        for (int i = 0; i < terominoDatas.Length; i++)
        {
            terominoDatas[i].Initial();
        }
        gameOver = false;
    }
    private void Start()
    {
        GeneratePiece();
    }
    public static void setBonusPiece(int sharp, int count)
    {
        nextPiece = count;
        whichPiece = sharp;
    }
    public void GeneratePiece()
    {
        int sharp = Random.Range(0, terominoDatas.Length);
        int color = Random.Range(0, ResourceLibrary.tiles.Length);
        TerominoData data;
        if (Next_tile.tile == null)
        {
            Next_tile = terominoDatas[Random.Range(0, terominoDatas.Length)];
            Next_tile.tile = ResourceLibrary.tiles[Random.Range(0, ResourceLibrary.tiles.Length)];
            data = terominoDatas[sharp];
            data.tile = ResourceLibrary.tiles[color];
        }
        else
        {
            data = Next_tile;
            data.tile = Next_tile.tile;
            if (nextPiece == 0)
            {
                Next_tile = terominoDatas[sharp];
            }
            else
            {
                switch (whichPiece)
                {
                    case 0:
                        Next_tile = terominoDatas[0];
                        break;
                    case 1:
                        Next_tile = terominoDatas[1];
                        break;
                    case 2:
                        Next_tile = terominoDatas[2];
                        break;
                    case 3:
                        Next_tile = terominoDatas[3];
                        break;
                    case 4:
                        Next_tile = terominoDatas[4];
                        break;
                    case 5:
                        Next_tile = terominoDatas[5];
                        break;
                    case 6:
                        Next_tile = terominoDatas[5];
                        break;


                }
                nextPiece--;
            }
            Next_tile.tile = ResourceLibrary.tiles[color];
        }
        int index = FindIndex(data.tile);
        setNextImag(Next_tile);
        activePiece.Initialize(this, position, data, index);
        if (!IsValidPosition(activePiece, position))
        {
            Time.timeScale = 0f;
            gameOver = true;
            onGameOver?.Invoke();
        }
        if (gameOver || ifFinal)
        {
            return;
        }
        mosterPostion.GetComponent<SpriteRenderer>().sprite = moster.MosterDir[(int)moster.MosterList[0]].image;
        SetPiece(activePiece);
    }
    void setNextImag(TerominoData td)
    {
        nextImage.sprite = BlocksSharp[(int)td.type];
    }
    int FindIndex(Tile t)
    {
        for (int i = 0; i < ResourceLibrary.tiles.Length; i++)
        {
            if (t == ResourceLibrary.tiles[i])
            {
                return i;
            }
        }
        return -1;
    }
    public void ClearBoard()
    {
        RectInt bounds = Bounds;
        for (int row = bounds.yMin; row < bounds.yMax; row++)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 00);
                tilemap.SetTile(position, null);
            }
        }
        gameOver = false;
    }
    public void SetPiece(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }
    public void ClearPiece(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }
    //移动超界检查
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }
            if (tilemap.HasTile(tilePosition))
            {
                return false;
            }

        }
        return true;
    }
    //消除行
    public void ClearLine()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;
        int rowsCleared = 0;
        while (row < bounds.yMax)
        {
            if (IsLineFull(row) && !ifFinal)
            {
                rowsCleared++;
                ClearRow(row);
                DropRow(row + 1);
                clearing_row = row;
            }
            else if (ifFinal)
            {
                ClearRow(row);
                row++;
            }
            else
            {
                row++;
            }
        }
        if (rowsCleared > 0 && !ifFinal)
        {
            onLinesCleared?.Invoke(rowsCleared);
        }
        if (row == bounds.yMax && ifFinal)
        {
            vetory();
        }
    }
    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            if (!tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }
    public void ClearRow(int row)
    {
        RectInt bounds = Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            if (tilemap.GetTile(position) != null)
            {
                var shapeModule = blockbroken.shape;
                Tile tile = tilemap.GetTile(position) as Tile;
                shapeModule.texture = tile.sprite.texture;
                Instantiate(blockbroken, position, UnityEngine.Quaternion.identity);
            }
            tilemap.SetTile(position, null);
        }
        DestoryMonster = true;
        musicManage.PlayClear();
    }
    //块下落
    public void DropRow(int startRow)
    {
        RectInt bounds = Bounds;
        for (int row = startRow; row < bounds.yMax; row++)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int above = new Vector3Int(col, row, 0);
                Vector3Int below = new Vector3Int(col, row - 1, 0);
                TileBase tile = tilemap.GetTile(above);
                tilemap.SetTile(below, tile);
                tilemap.SetTile(above, null);
            }
        }
    }
    //块触底锁
    public void LockPiece(Piece piece, int color)
    {
        SetPiece(piece);
        musicManage.PlayFall();
        ClearLine();
        MosterJudge(color);
        GeneratePiece();
    }
    //怪物逻辑判定
    public void MosterJudge(int color)
    {
        if (!moster.SameColor(color))
        {
            Hp.hurt();
            musicManage.PlayHurt();
            if(GameOverJudge())
            {
                return;
            }
        }
        moster.decrease_moster(ref DestoryMonster);
    }

    public void vetory()
    {
        IGM.SetActive(true);
    }
    public void resetboard()
    {
        ClearBoard();
        GeneratePiece();
    }
    public bool GameOverJudge()
    {
        if (Hp.currHp <= 0)
        {
            ClearBoard();
            Time.timeScale = 0f;
            gameOver = true;
            onGameOver?.Invoke();
        }
        if (gameOver)
        {
            return true;
        }
        return false;
    }
}
