using System.Collections.Generic;
using UnityEngine;


public class WaitingForMove : ChessState
{
    readonly List<ChessMove> moves;

    public WaitingForMove(ChessMachine manager, List<ChessMove> moves) : base(manager)
    {
        this.moves = moves;
    }

    public override void Activate()
    {
        User.OnDownCell += Handle;
        foreach (ChessMove move in moves)
        {
            User.SignCell(move.To);
        }
    }

    public override void Deactivate()
    {
        User.OnDownCell -= Handle;
        foreach (ChessMove move in moves)
        {
            User.CleanCell(move.To);
        }
    }

    public void Handle(Vector2Int to)
    {
        ChessMove move = moves.Find(m => m.To == to);
        if (move == null)
        {
            Manager.Switch(this, new WaitingForFigure(Manager));
        }
        else
        {
            PawnStep step = move as PawnStep;
            if ((step != null) && step.IsTransform)
            {
                Manager.Switch(this, new WaitingForTransform(Manager, step));
            }
            else
            {
                Manager.NextTurn(move.Execute(Inner));
                Manager.Switch(this, new WaitingForFigure(Manager));
            }
        }
    }
}