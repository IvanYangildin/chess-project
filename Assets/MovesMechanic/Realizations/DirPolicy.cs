using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class DirPolicy : MovePolicy
{
    public abstract IEnumerable<Vector2Int> GetDirection();
    public DirPolicy(CoreBoard board, Vector2Int from) : base(board, from)
    {
    }

    override public bool CanBeat(Vector2Int To)
    {
        Vector2Int vec = To - From;
        bool is_dir = false;
        foreach (Vector2Int dir in GetDirection())
        {
            if ((vec.x * dir.y == vec.y * dir.x) && (vec.x * dir.x + vec.y * dir.y > 0))
            {
                is_dir = true; break;
            }
        }
        if (!is_dir) return false;

        return ChessConditions.PathFree(From, To, Board);
    }

    override public IEnumerator<ChessMove> GetEnumerator()
    {
        return new DirIterator(Board, From, GetDirection().GetEnumerator());
    }

}

public class DirIterator : IEnumerator<ChessMove>
{
    CoreBoard board;
    Vector2Int From;

    IEnumerator<Vector2Int> it_dir;
    int curr_depth;

    CellState curr_state = new CellState(true, false);

    public Vector2Int CurrentTo => it_dir.Current * curr_depth;
    public ChessMove Current => new ChessMove(CurrentTo, ChessConditions.StandardCondition) + From;

    object IEnumerator.Current => Current;

    public DirIterator(CoreBoard board, Vector2Int from, IEnumerator<Vector2Int> it_dir)
    {
        From = from;
        this.it_dir = it_dir;
        this.board = board;
    }

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        bool is_continue = true;
        if (curr_state.is_figure)
        {
            is_continue = it_dir.MoveNext();
            curr_depth = 1;
        }
        else
        {
            ++curr_depth;
        }

        if (is_continue)
        {
            curr_state = Current.Relation(board);
        }

        return is_continue;
    }

    public void Reset()
    {
        it_dir.Reset();
        curr_depth = 1;
    }
}