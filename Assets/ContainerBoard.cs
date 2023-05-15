using System;
using System.Collections.Generic;
using UnityEngine;


public class CoreBoard
{
    private ChessFigure[,] figures = new ChessFigure[8, 8];
    public int Width { get { return figures.GetLength(1); } }
    public int Height { get { return figures.GetLength(0); } }

    public CoreBoard()
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                figures[i, j] = new();
            }
        }
    }

    public CoreBoard(ChessFigure[,] figures)
    {
        this.figures = figures;
    }

    public bool Borders(Vector2Int cell)
    {
        return (cell.x >= 0) && (cell.y >= 0) && (cell.x < 8) && (cell.y < 8);
    }

    public bool IsWhiteCell(Vector2Int cell)
    {
        return ((cell.x + cell.y) % 2) == 1;
    }

    public virtual ChessFigure this[int i, int j]
    {
        get
        {
            return figures[i, j];
        }
        set
        {
            figures[i, j] = value;
        }
    }

    public ChessFigure this[Vector2Int vec]
    {
        get { return this[vec.x, vec.y]; }
        set { this[vec.x, vec.y] = value; }
    }

}

public class ContainerBoard : CoreBoard
{
    public List<ActionFigure> delta { get; private set; } = new();

    static public ContainerBoard Classic()
    {
        ContainerBoard chess_board = new();

        for (int i = 0; i < 8; ++i)
        {
            chess_board[i, 1] = new Pawn(FigureColor.White);
            chess_board[i, 6] = new Pawn(FigureColor.Black);
        }

        chess_board[0, 0] = new Rook(FigureColor.White);
        chess_board[7, 0] = new Rook(FigureColor.White);
        chess_board[0, 7] = new Rook(FigureColor.Black);
        chess_board[7, 7] = new Rook(FigureColor.Black);

        chess_board[1, 0] = new Knight(FigureColor.White);
        chess_board[6, 0] = new Knight(FigureColor.White);
        chess_board[1, 7] = new Knight(FigureColor.Black);
        chess_board[6, 7] = new Knight(FigureColor.Black);

        chess_board[2, 0] = new Bishop(FigureColor.White);
        chess_board[5, 0] = new Bishop(FigureColor.White);
        chess_board[2, 7] = new Bishop(FigureColor.Black);
        chess_board[5, 7] = new Bishop(FigureColor.Black);

        chess_board[3, 0] = new Queen(FigureColor.White);
        chess_board[4, 0] = new King(FigureColor.White);
        chess_board[3, 7] = new Queen(FigureColor.Black);
        chess_board[4, 7] = new King(FigureColor.Black);
        chess_board.ClearDelta();

        return chess_board;
    }

    public override ChessFigure this[int i, int j]
    {
        get
        {
            return base[i, j];
        }
        set
        {
            delta.Add(new(base[i, j], new Vector2Int(i, j)));
            
            base[i, j].Deactivate();
            base[i, j] = value;
            base[i, j].Activate();
        }
    }

    public void ClearDelta()
    {
        delta.Clear();
    }
}
