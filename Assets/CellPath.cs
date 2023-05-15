using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CellPath : IEnumerable<Vector2Int>
{
    public readonly Vector2Int From, To;

    public CellPath(Vector2Int from, Vector2Int to)
    {
        From = from;
        To = to;
    }

    public IEnumerator<Vector2Int> GetEnumerator()
    {
        return new CellPathIterator(From, To);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class CellPathIterator : IEnumerator<Vector2Int>
{
    public readonly Vector2Int From, To;

    private readonly Vector2 step;
    private Vector2 curr;

    public Vector2Int Current => From + new Vector2Int((int)curr.x, (int)curr.y);

    object IEnumerator.Current => Current;

    public CellPathIterator(Vector2Int from, Vector2Int to)
    {
        From = from;
        To = to;

        Vector2Int delta = to - from;
        float norm = Mathf.Max(Mathf.Abs(delta.x), Mathf.Abs(delta.y));
        step = new Vector2(delta.x / norm, delta.y / norm);

        curr = -step;
    }

    public void Dispose()
    {}

    public bool MoveNext()
    {
        if (Current != To)
        {
            curr += step;
        }
        else
        {
            return false;
        }
        return true;
    }

    public void Reset()
    {
        curr = -step;
    }
}
