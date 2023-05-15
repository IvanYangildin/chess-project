using System.Collections.Generic;
using UnityEngine;


public class WaitingForFigure : ChessState
{
    public WaitingForFigure(ChessMachine manager) : base(manager)
    {
    }

    public override void Activate()
    {
        User.OnDownCell += Handle;
    }

    public override void Deactivate()
    {
        User.OnDownCell -= Handle;
    }

    public void Handle(Vector2Int pos)
    {
        if (Inner.Container.Borders(pos) && Inner[pos].IsSolid && (Inner[pos].Color == CurrSide))
        {
            List<ChessMove> moves = Inner.PosibleMoves(pos);
            if (moves.Count > 0)
                Manager.Switch(this, new DraggingFigure(Manager, moves, pos, Inner[pos]));
        }
    }
}
