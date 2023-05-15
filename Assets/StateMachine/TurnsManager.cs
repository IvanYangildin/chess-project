using System.Collections.Generic;
using UnityEngine;


public class TurnsManager
{
    public FigureColor CurrentTurn { get; private set; }
    public List<ChessTurn> TurnsHistory { get; private set; } 

    public TurnsManager()
    {
        CurrentTurn = FigureColor.White;
        TurnsHistory = new();
    }
    
    public void NextTurn(ChessTurn prev_turn)
    {
        TurnsHistory.Add(prev_turn);
        CurrentTurn = (CurrentTurn == FigureColor.White) ? FigureColor.Black : FigureColor.White;
    }

    public ChessTurn PrevTurn()
    {
        if (TurnsHistory.Count == 0) return null;

        ChessTurn last = TurnsHistory[TurnsHistory.Count - 1];
        TurnsHistory.RemoveAt(TurnsHistory.Count - 1);
        CurrentTurn = (CurrentTurn == FigureColor.White) ? FigureColor.Black : FigureColor.White;
        return last;
    }

}
