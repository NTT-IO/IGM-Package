using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static readonly float cos = Mathf.Cos(Mathf.PI / 2);
    public static readonly float sin = Mathf.Sin(Mathf.PI / 2);
    public static readonly float[] rotateRetrix = new float[] { cos, sin, -sin, cos };
    //形状数据
    public static readonly Dictionary<TetrominoType, Vector2Int[]> cells = new Dictionary<TetrominoType, Vector2Int[]>
    {
        { TetrominoType.I, new Vector2Int[] { new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0),new Vector2Int( 2, 0) } },
        { TetrominoType.O, new Vector2Int[]{ new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) }},
        { TetrominoType.T, new Vector2Int[]{ new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { TetrominoType.S, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0),new Vector2Int( 0, 0) } },
        { TetrominoType.Z, new Vector2Int[]{ new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0),new Vector2Int( 1, 0) } },
        { TetrominoType.J, new Vector2Int[]{ new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { TetrominoType.L,new Vector2Int[]{ new Vector2Int( 1, 1),new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },

    };
    //提墙数据
    public static readonly Vector2Int[,] kickwallI = new Vector2Int[,]
    {
       { new Vector2Int(0,0),new Vector2Int(-2,0),new Vector2Int(1,0),new Vector2Int(-2,-1),new Vector2Int(1,2),} ,
       { new Vector2Int(0,0),new Vector2Int(-1,0),new Vector2Int(2,0),new Vector2Int(2,-1),new Vector2Int(2,-1),} ,
       { new Vector2Int(0,0),new Vector2Int(2,0),new Vector2Int(-1,0),new Vector2Int(-1,-2),new Vector2Int(-1,-2),} ,
       { new Vector2Int(0,0),new Vector2Int(1,0),new Vector2Int(-2,0),new Vector2Int(-2,1),new Vector2Int(-2,1),}
    };
    public static readonly Vector2Int[,] kickwallOTSJZL = new Vector2Int[,]
    {
       { new Vector2Int(0,0),new Vector2Int(-1,0),new Vector2Int(-1,1),new Vector2Int(0,-2),new Vector2Int(-1,-2),} ,
       { new Vector2Int(0,0),new Vector2Int(1,0),new Vector2Int(1,-1),new Vector2Int(0,2),new Vector2Int(1,2),} ,
       { new Vector2Int(0,0),new Vector2Int(1,0),new Vector2Int(1,1),new Vector2Int(0,-2),new Vector2Int(1,-2),} ,
       { new Vector2Int(0,0),new Vector2Int(-1,0),new Vector2Int(-1,-1),new Vector2Int(0,2),new Vector2Int(-1,2),}
    };
    public static readonly Dictionary<TetrominoType, Vector2Int[,]> kickData = new Dictionary<TetrominoType, Vector2Int[,]>
    {
        { TetrominoType.I,kickwallI},
        { TetrominoType.O,kickwallOTSJZL},
        { TetrominoType.S,kickwallOTSJZL},
        { TetrominoType.L,kickwallOTSJZL},
        { TetrominoType.T,kickwallOTSJZL},
        { TetrominoType.Z,kickwallOTSJZL},
        { TetrominoType.J,kickwallOTSJZL},
    };
}
