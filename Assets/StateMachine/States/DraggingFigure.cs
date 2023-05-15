using System.Collections.Generic;
using UnityEngine;


public class DraggingFigure : ChessState
{
    readonly List<ChessMove> moves;
    readonly ChessFigure figure;
    readonly Vector2Int from;

    public DraggingFigure(ChessMachine manager, List<ChessMove> moves, Vector2Int from, ChessFigure figure) : base(manager)
    {
        this.moves = moves;
        this.figure = figure;
        this.from = from;
    }

    public override void Activate()
    {
        Drag.Activate(User.FigureToSprite(figure));
        User.CleanFigure(from);

        User.OnUpCell += Handle;
        foreach (ChessMove move in moves)
        {
            User.SignCell(move.To);
        }
    }

    public override void Deactivate()
    {
        Drag.Deactivate();
        User.UpdateBoard();

        User.OnUpCell -= Handle;
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
            Manager.Switch(this, new WaitingForMove(Manager, moves));
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