using UnityEngine;

public static class ChessConditions
{
    public delegate bool FigureRule(ChessFigure actor, ChessFigure sub);

    public static bool BinaryCondition(Vector2Int from, Vector2Int to, CoreBoard board, FigureRule rule)
    {
        if (board.Borders(from) && board.Borders(to))
        {
            ChessFigure actor = board[from.x, from.y];
            ChessFigure sub = board[to.x, to.y];

            return rule(actor, sub);
        }
        return false;
    }

    public static bool EatableOnly(ChessMove move, CoreBoard board)
    {
        return BinaryCondition(move.From, move.To, board, (actor, sub) => sub.Color == actor.OpponentColor);
    }

    public static bool EatableSolidOnly(ChessMove move, CoreBoard board)
    {
        return BinaryCondition(move.From, move.To, board, (actor, sub) => (sub.Color == actor.OpponentColor) && sub.IsSolid);
    }

    public static bool UnsolidOnly(ChessMove move, CoreBoard board)
    {
        return BinaryCondition(move.From, move.To, board, (actor, sub) => !sub.IsSolid);
    }

    public static bool StandardCondition(ChessMove move, CoreBoard board)
    {
        return BinaryCondition(move.From, move.To, board, (actor, sub) => (!sub.IsSolid) || (sub.Color == actor.OpponentColor));
    }

    public static bool PathFree(Vector2Int from, Vector2Int to, CoreBoard board)
    {
        foreach (Vector2Int cell in new CellPath(from, to))
        {
            if ((cell == from) || (cell == to)) 
                continue;
            if (!BinaryCondition(from, cell, board, (actor, sub) => !sub.IsSolid)) 
                return false;
        }
        return true;
    }

    public static bool PathCondition(Vector2Int from, Vector2Int to, CoreBoard board, FigureRule rule)
    {
        if (PathFree(from, to, board))
        {
            return BinaryCondition(from, to, board, rule);
        }
        return false;
    }

    public static bool FreeOnlyPass(ChessMove move, CoreBoard board)
    {
        return PathCondition(move.From, move.To, board, (actor, sub) => !sub.IsSolid);
    }

}