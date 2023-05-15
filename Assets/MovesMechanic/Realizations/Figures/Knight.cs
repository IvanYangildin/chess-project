using System;
using System.Collections.Generic;
using UnityEngine;


public class Knight : ChessFigure
{
    public override bool IsSolid => true;
    public override MovePolicy Policy(CoreBoard board, Vector2Int From)
    {
        return new KnightPolicy(board, From);
    }

    public Knight(FigureColor color, bool isMoved = false) : base(color, isMoved) { }

    public override ChessFigure copy()
    {
        return new Knight(Color);
    }
}

public class KnightPolicy : MovePolicy
{
    List<ChessMove> steps = new List<ChessMove>();

    public KnightPolicy(CoreBoard board, Vector2Int from) : base(board, from)
    {
        steps.Add(new ChessMove(new Vector2Int(-1, 2), ChessConditions.StandardCondition) + from);
        steps.Add(new ChessMove(new Vector2Int(1, 2), ChessConditions.StandardCondition) + from);
        steps.Add(new ChessMove(new Vector2Int(-1, -2), ChessConditions.StandardCondition) + from);
        steps.Add(new ChessMove(new Vector2Int(1, -2), ChessConditions.StandardCondition) + from);

        steps.Add(new ChessMove(new Vector2Int(2, -1), ChessConditions.StandardCondition) + from);
        steps.Add(new ChessMove(new Vector2Int(2, 1), ChessConditions.StandardCondition) + from);
        steps.Add(new ChessMove(new Vector2Int(-2, -1), ChessConditions.StandardCondition) + from);
        steps.Add(new ChessMove(new Vector2Int(-2, 1), ChessConditions.StandardCondition) + from);
    }

    public override bool CanBeat(Vector2Int To)
    {
        ChessMove move = steps.Find(move => move.To == To);
        return move != null;
    }

    public override IEnumerator<ChessMove> GetEnumerator()
    {
        return steps.GetEnumerator();
    }
}