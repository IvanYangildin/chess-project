using System;
using System.Collections.Generic;
using UnityEngine;


// Ghost is beging created by pawn after double-step
public class Ghost : ChessFigure
{
    public override bool IsSolid => false;

    readonly ChessBoard board;

    readonly Vector2Int ghost_point;
    Vector2Int pawn_point
    {
        get
        {
            return new Vector2Int(ghost_point.x, ghost_point.y + (Color == FigureColor.White ? 1 : -1));
        }
    }

    public Ghost(ChessBoard board, FigureColor color, Vector2Int ghost_point) : base(color)
    {
        this.ghost_point = ghost_point;
        this.board = board;
    }

    public override ChessFigure copy()
    {
        return new Ghost(board, Color, ghost_point);
    }

    public override void Eaten(ChessFigure eater)
    {
        if (eater is Pawn)
        {
            board[pawn_point] = new();
        }
    }

    public override void Activate()
    {
        board.OnMoveMade += GhostKill;
    }

    public override void Deactivate()
    {
        board.OnMoveMade -= GhostKill;
    }

    public void GhostKill(object sender, EventArgs e)
    {
        if (board[ghost_point] == this)
        {
            board[ghost_point] = new();
        }
    }
}

public class Pawn : ChessFigure
{
    public override bool IsSolid => true;
    public override MovePolicy Policy(CoreBoard board, Vector2Int From)
    {
        return new PawnPolicy(board, From);
    }

    public Pawn(FigureColor color, bool isMoved = false) : base(color, isMoved)
    { }

    public override ChessFigure copy()
    {
        return new Pawn(Color, IsMoved);
    }

}

public class PawnPolicy : MovePolicy
{
    Vector2Int move_vec;
    List<ChessMove> moves = new();

    public PawnPolicy(CoreBoard board, Vector2Int from) : base(board, from)
    {
        FigureColor side = board[from].Color;
        bool is_start = !board[from].IsMoved;

        move_vec = (side == FigureColor.White) ? Vector2Int.up : Vector2Int.down;

        moves.Add(new PawnStep(from, from + move_vec, ChessConditions.UnsolidOnly, side));
        moves.Add(new PawnStep(from, from + move_vec + Vector2Int.right, ChessConditions.EatableOnly, side));
        moves.Add(new PawnStep(from, from + move_vec + Vector2Int.left, ChessConditions.EatableOnly, side));

        if (is_start)
            moves.Add(new PawnJump(from, from + 2*move_vec, ChessConditions.FreeOnlyPass));

    }

    public override bool CanBeat(Vector2Int cell)
    {
        Vector2Int vec = cell - From;
        return (vec == (move_vec + Vector2Int.left)) || (vec == (move_vec + Vector2Int.right));
    }

    public override IEnumerator<ChessMove> GetEnumerator()
    {
        return moves.GetEnumerator();
    }
}


class PawnJump : ChessMove
{
    public PawnJump(Vector2Int from, Vector2Int to, MoveCondition condition) : base(from, to, condition)
    {
    }

    protected override void make_move(ChessBoard board)
    {
        base.make_move(board);

        Vector2Int ghost_point = new Vector2Int(From.x, (From.y + To.y) / 2);
        board[ghost_point.x, ghost_point.y] = new Ghost(board, board[To].Color, ghost_point);
    }
}

public class PawnStep : ChessMove
{
    public FigureColor Color;
    public ChessFigure TransformTo;

    public bool IsTransform => (Color == FigureColor.Black) ? (To.y == 0) : (To.y == 7);

    public PawnStep(Vector2Int from, Vector2Int to, MoveCondition condition, FigureColor color) : base(from, to, condition)
    {
        Color = color;
        TransformTo = new Pawn(Color, true);
    }

    protected override void make_move(ChessBoard board)
    {
        ChessFigure actor = board[From.x, From.y];
        ChessFigure victim = board[To.x, To.y];

        board[To] = TransformTo.MovedFigure;
        board[From] = new();
        victim.Eaten(actor);
        board.FinishMove();

    }

}