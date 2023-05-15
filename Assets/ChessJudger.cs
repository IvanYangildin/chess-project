using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ChessJudger
{
    private CoreBoard board;
    //positions of kings
    private Vector2Int wk_pos, bk_pos;
    private List<Vector2Int> white_points = new();
    private List<Vector2Int> black_points = new();

    public ChessFigure this[Vector2Int pos]
    {
        get { return board[pos.x, pos.y]; }
    }

    public bool IsAttactedBy(Vector2Int cell, FigureColor att_color)
    {
        List<Vector2Int> fig_points = (att_color == FigureColor.Black) ? black_points : white_points;

        foreach (var pos in fig_points)
        {
            ChessFigure figure = board[pos.x, pos.y];
            MovePolicy policy = figure.Policy(board, pos);

            if (policy.CanBeat(cell))
                return true;
        }
        return false;
    }

    // return true, if king of color king_side is under attack
    public bool KingShah(FigureColor king_side)
    {
        Vector2Int king_pos = (king_side == FigureColor.White) ? wk_pos : bk_pos;
        return IsAttactedBy(king_pos, this[king_pos].OpponentColor);
    }

    // return true, if side don't have any moves
    public bool IsNoMoves(FigureColor side)
    {
        List<Vector2Int> fig_points = (side == FigureColor.Black) ? black_points : white_points;
        foreach (var pos in fig_points)
        {
            ChessFigure figure = board[pos.x, pos.y];
            List<ChessMove> moves = figure.Policy(board, pos).ToList();
            foreach (ChessMove move in moves)
            {
                if (move.CheckCondition(board)) return false;
            }
        }
        return true;
    }

    public bool IsFigure(ChessFigure figure)
    {
        return (figure is Bishop) || (figure is King) || (figure is Knight) || 
            (figure is Pawn) || (figure is Queen) || (figure is Rook);
    }

    public ChessJudger(CoreBoard board, ChessMove move=null)
    {
        ChessFigure[,] field = new ChessFigure[8,8];
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                Vector2Int pos = new(i, j);
                ChessFigure figure = board[i, j];

                if (move != null)
                {
                    if (pos == move.To) figure = board[move.From.x, move.From.y];
                    if (pos == move.From) figure = new();

                }
                field[i, j] = figure;

                if (IsFigure(figure))
                {

                    if (figure is King)
                    {
                        if (figure.Color == FigureColor.White) wk_pos = pos;
                        else bk_pos = pos;
                    }
                    if (figure.Color == FigureColor.White) white_points.Add(pos);
                    else black_points.Add(pos);

                }
            }
        }
        this.board = new CoreBoard(field);
    }
}
