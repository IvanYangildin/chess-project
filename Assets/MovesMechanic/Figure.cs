using System;
using System.Collections.Generic;
using UnityEngine;


public enum FigureColor { Black, White, Noone }

public class ChessFigure
{
    public bool IsMoved { get; private set; }
    private readonly FigureColor color;
    public FigureColor Color => color;
    public FigureColor OpponentColor
    {
        get
        {
            if (color == FigureColor.Black) return FigureColor.White;
            if (color == FigureColor.White) return FigureColor.Black;
            return FigureColor.Noone;
        }
    }

    public virtual bool IsSolid => false;

    public virtual MovePolicy Policy(CoreBoard board, Vector2Int From) { return new EmptyPolicy(board, From); }
    public virtual ChessFigure copy() => new(Color, IsMoved);
    public virtual ChessFigure MovedFigure 
    { 
        get 
        {
            ChessFigure mf = copy();
            mf.IsMoved = true;
            return mf; 
        } 
    }

    public ChessFigure(FigureColor color = FigureColor.Noone, bool isMoved = false)
    {
        this.color = color;
        IsMoved = isMoved;
    }

    public virtual void Eaten(ChessFigure eater) { }
    public virtual void Activate() { }
    public virtual void Deactivate() { }

}

public class ActionFigure
{
    public readonly ChessFigure figure;
    public readonly Vector2Int pos;

    public ActionFigure(ChessFigure figure, Vector2Int pos)
    {
        this.figure = figure;
        this.pos = pos;
    }

    public ActionFigure(ActionFigure other)
    {
        figure = other.figure;
        pos = other.pos;
    }
}
