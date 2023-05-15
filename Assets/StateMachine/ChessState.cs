using System.Collections.Generic;
using UnityEngine;


public abstract class ChessState
{
    public readonly ChessMachine Manager;
    public ChessBoard Inner => Manager.Inner;
    public UserBoard User => Manager.User;
    public FigureColor CurrSide => Manager.CurrSide;
    public DraggableFigure Drag => Manager.Drag;

    public ChessState(ChessMachine manager)
    {
        Manager = manager;
    }

    public abstract void Activate();
    public abstract void Deactivate();
    public virtual void Reject()
    {
        Manager.Switch(this, new WaitingForFigure(Manager));
    }

}