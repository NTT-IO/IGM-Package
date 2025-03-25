using UnityEngine;
using UnityEngine.Tilemaps;

public enum TetrominoType
{
    I,O,T,S,Z,J,L
};
[System.Serializable]
public struct TerominoData
{
    public TetrominoType type;
    public Tile tile;
    public Vector2Int[] cells;
    public void Initial()
    {
        cells = Data.cells[this.type];
    }
}
