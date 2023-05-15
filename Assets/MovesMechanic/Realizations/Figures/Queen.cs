using System;
using System.Collections.Generic;
using UnityEngine;


public class Queen : ChessFigure
{
    public override bool IsSolid => true;
    public override MovePolicy Policy(CoreBoard board, Vector2Int From)
    {
        return new QueenPolicy(board, From);
    }

    public Queen(FigureColor color, bool isMoved = false) : base(color, isMoved) { }
    public override ChessFigure copy()
    {
        return new Queen(Color, IsMoved);
    }
}

public class QueenPolicy : DirPolicy
{
    static readonly Vector2Int[] directions = { Vector2Int.left, Vector2Int.down, Vector2Int.right, Vector2Int.up,
    Vector2Int.left + Vector2Int.up, Vector2Int.left + Vector2Int.down, Vector2Int.right + Vector2Int.up,
    Vector2Int.right + Vector2Int.down};

    public QueenPolicy(CoreBoard board, Vector2Int from) : base(board, from)
    {
    }

    public override IEnumerable<Vector2Int> GetDirection()
    {
        return directions;
    }
}
