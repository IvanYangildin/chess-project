using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class MovePolicy : IEnumerable<ChessMove>
{
    public readonly Vector2Int From;
    public readonly CoreBoard Board;

    public MovePolicy(CoreBoard board, Vector2Int from)
    {
        Board = board;
        From = from;
    }

    public abstract bool CanBeat(Vector2Int cell);

    abstract public IEnumerator<ChessMove> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class EmptyPolicy : MovePolicy
{
    public EmptyPolicy(CoreBoard board, Vector2Int from) : base(board, from)
    {
    }

    public override bool CanBeat(Vector2Int cell)
    {
        return false;
    }

    public override IEnumerator<ChessMove> GetEnumerator()
    {
        return new List<ChessMove>().GetEnumerator();
    }
}