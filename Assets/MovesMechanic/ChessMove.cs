using UnityEngine;

public class ChessMove
{
    public readonly Vector2Int From;
    public readonly Vector2Int To;
    public delegate bool MoveCondition(ChessMove move, CoreBoard board);
    public readonly MoveCondition condition;

    static public ChessMove up {  get { return new ChessMove(Vector2Int.up, ChessConditions.StandardCondition); } }
    static public ChessMove down { get { return new ChessMove(Vector2Int.down, ChessConditions.StandardCondition); } }
    static public ChessMove left { get { return new ChessMove(Vector2Int.left, ChessConditions.StandardCondition); } }
    static public ChessMove right { get { return new ChessMove(Vector2Int.right, ChessConditions.StandardCondition); } }
    static public ChessMove lu { get { return new ChessMove(Vector2Int.up + Vector2Int.left, ChessConditions.StandardCondition); } }
    static public ChessMove ld { get { return new ChessMove(Vector2Int.down + Vector2Int.left, ChessConditions.StandardCondition); } }
    static public ChessMove ru { get { return new ChessMove(Vector2Int.right + Vector2Int.up, ChessConditions.StandardCondition); } }
    static public ChessMove rd { get { return new ChessMove(Vector2Int.right + Vector2Int.down, ChessConditions.StandardCondition); } }

    public bool CheckCondition(CoreBoard board)
    {
        if (condition != null)
            if (!condition(this, board))
                return false;

        FigureColor side = board[From.x, From.y].Color;
        ChessJudger judger = new(board, this);

        return !judger.KingShah(side);
    }

    static public ChessMove operator+(ChessMove move, Vector2Int point)
    {
        return new ChessMove(point + move.From, point + move.To, move.condition);
    }

    public ChessMove(Vector2Int from, Vector2Int to, MoveCondition condition)
    {
        From = from;
        To = to;
        this.condition = condition;
    }

    public ChessMove(Vector2Int to, MoveCondition condition)
    {
        From = Vector2Int.zero;
        To = to;
        this.condition = condition;
    }

    protected virtual void make_move(ChessBoard board)
    {
        board[To] = board[From.x, From.y].MovedFigure;
        board[From] = new();
    }

    public virtual ChessTurn Execute(ChessBoard board)
    {
        ChessFigure actor = board[From.x, From.y];
        ChessFigure victim = board[To.x, To.y];
        FigureColor side = actor.Color;

        make_move(board);
        
        victim.Eaten(actor);
        ChessTurn turn = new ChessTurn(From, To, board.Container.delta, side);
        board.FinishMove(turn);
        turn = new ChessTurn(From, To, board.Container.delta, side);

        board.Container.ClearDelta();

        return turn;
    }
}


