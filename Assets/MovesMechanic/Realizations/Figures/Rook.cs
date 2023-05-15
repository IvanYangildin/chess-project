using System;
using System.Collections.Generic;
using UnityEngine;


public class Rook : ChessFigure
{
    public override bool IsSolid => true;
    public override MovePolicy Policy(CoreBoard board, Vector2Int From)
    {
        return new RookPolicy(board, From);
    }

    public Rook(FigureColor color, bool isMoved = false) : base(color, isMoved) { }

    public override ChessFigure copy()
    {
        return new Rook(Color, IsMoved);
    }
}

public class RookPolicy : DirPolicy
{
    static readonly Vector2Int[] directions = { Vector2Int.left, Vector2Int.down, Vector2Int.right, Vector2Int.up };
    
    public RookPolicy(CoreBoard board, Vector2Int from) : base(board, from)
    {
    }

    public override IEnumerable<Vector2Int> GetDirection()
    {
        return directions;
    }
}
