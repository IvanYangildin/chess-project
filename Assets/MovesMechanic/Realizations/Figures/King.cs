using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class King : ChessFigure
{
    public override bool IsSolid => true;
    public override MovePolicy Policy(CoreBoard board, Vector2Int From)
    {
        return new KingPolicy(board, From, CanCastleLeft, CanCastleRight);
    }

    public King(FigureColor color, bool isMoved = false) : base(color, isMoved)
    { }

    override public ChessFigure copy()
    {
        return new King(Color, IsMoved);
    }

    public bool CanCastleLeft(ChessMove move, CoreBoard board)
    {
        ChessJudger judger = new(board);

        if (IsMoved) return false;
        if (judger.KingShah(Color)) return false;
        int y = Color == FigureColor.White ? 0 : 7;

        if (board[0, y].IsMoved) return false;
        if (board[0, y] is Rook)
        {
            for (int i = 2; i <= 3; ++i)
            {
                Vector2Int cell = new Vector2Int(i, y);
                if (board[cell.x, cell.y].IsSolid) return false;
                if (judger.IsAttactedBy(cell, OpponentColor)) return false;
            }
        }
        return true;
    }

    public bool CanCastleRight(ChessMove move, CoreBoard board)
    {
        ChessJudger judger = new(board);

        if (IsMoved) return false;
        if (judger.KingShah(Color)) return false;
        int y = (Color == FigureColor.White ? 0 : 7);

        if (board[7, y].IsMoved) return false;
        if (board[7, y] is Rook)
        {
            for (int i = 5; i <= 6; ++i)
            {
                Vector2Int cell = new Vector2Int(i, y);
                if (board[cell.x, cell.y].IsSolid) return false;
                if (judger.IsAttactedBy(cell, OpponentColor)) return false;
            }
        }
        return true;
    }

}

public class KingPolicy : MovePolicy
{
    List<ChessMove> steps = new List<ChessMove>();

    public KingPolicy(CoreBoard board, Vector2Int from, 
        ChessMove.MoveCondition lcastle, ChessMove.MoveCondition rcastle) : base(board, from)
    {
        steps.Add(ChessMove.left + from);
        steps.Add(ChessMove.down + from);
        steps.Add(ChessMove.right + from);
        steps.Add(ChessMove.up + from);

        steps.Add(ChessMove.lu + from);
        steps.Add(ChessMove.ld + from);
        steps.Add(ChessMove.ru + from); 
        steps.Add(ChessMove.rd + from);

        steps.Add(new LCastleMove(from, 2 * Vector2Int.left + from, lcastle));
        steps.Add(new RCastleMove(from, 2 * Vector2Int.right + from, rcastle));
    }

    public override bool CanBeat(Vector2Int To)
    {
        Vector2Int vec = To - From;
        return (Mathf.Abs(vec.x) <= 1) && (Mathf.Abs(vec.y) <= 1);
    }

    public override IEnumerator<ChessMove> GetEnumerator()
    {
        return steps.GetEnumerator();
    }
}

public class RCastleMove : ChessMove
{
    public RCastleMove(Vector2Int from, Vector2Int to, MoveCondition condition) : base(from, to, condition)
    {
    }

    protected override void make_move(ChessBoard board)
    {
        base.make_move(board);
        board[To.x - 1, To.y] = board[7, To.y].MovedFigure;
        board[7, To.y] = new();
    }
}

public class LCastleMove : ChessMove
{
    public LCastleMove(Vector2Int from, Vector2Int to, MoveCondition condition) : base(from, to, condition)
    {
    }

    protected override void make_move(ChessBoard board)
    {
        base.make_move(board);
        board[To.x + 1, To.y] = board[0, To.y].MovedFigure;
        board[0, To.y] = new();
    }
}
