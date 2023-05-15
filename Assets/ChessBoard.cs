﻿using System;
using System.Collections.Generic;
using UnityEngine;


public class ChessTurn
{
    public readonly Vector2Int from, to;
    public readonly List<ActionFigure> delta;

    public ChessTurn(Vector2Int from, Vector2Int to, List<ActionFigure> delta)
    {
        this.from = from;
        this.to = to;
        this.delta = new(delta);
    }
}

public class ChessBoard
{
    private ContainerBoard container;
    public ContainerBoard Container { get { return container; } }
    public ChessJudger Judger { get; private set; }

    public event EventHandler OnMoveMade;

    public ChessBoard()
    {
        container = ContainerBoard.Classic();
    }

    public ChessFigure this[int i, int j]
    {
        get { return container[i,j]; }
        set { container[i,j] = value; }
    }

    public ChessFigure this[Vector2Int pos]
    {
        get { return container[pos.x, pos.y]; }
        set { container[pos.x, pos.y] = value; }
    }

    public List<ChessMove> PosibleMoves(Vector2Int from)
    {
        List<ChessMove> moves = new();

        if (!container.Borders(from)) return moves;

        MovePolicy policy = this[from].Policy(Container, from);
        foreach (ChessMove move in policy)
        {
            if (move.CheckCondition(Container))
            { 
                moves.Add(move); 
            }
        }

        return moves;
    }

    public void FinishMove()
    {
        if (OnMoveMade != null)
            OnMoveMade(this, EventArgs.Empty);
    }

    public void RejectMove(ChessTurn turn)
    {
        if (turn == null) return;

        foreach (ActionFigure af in turn.delta)
        {
            Vector2Int pos = af.pos;
            container[pos.x, pos.y] = af.figure;
        }
        container.ClearDelta();
    }
}
