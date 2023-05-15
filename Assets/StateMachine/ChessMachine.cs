using TMPro;
using UnityEngine;


public class ChessMachine : MonoBehaviour
{
    TurnsManager turns_manager = new();
    
    public ChessBoard Inner = null;
    public UserBoard User;
    public UserPawnTF ChoiceTF;
    public DraggableFigure Drag;
    public TextMeshProUGUI Message;

    public FigureColor CurrSide => turns_manager.CurrentTurn;

    private ChessState curr_state;

    private void Start()
    {
        if (Inner == null)
        {
            Inner = User.chess_board;
        }
        ChoiceTF.Color = turns_manager.CurrentTurn;
        curr_state = new WaitingForFigure(this);
        curr_state.Activate();
    }

    public void NextTurn(ChessTurn turn)
    {
        turns_manager.NextTurn(turn);
        ChoiceTF.Color = turns_manager.CurrentTurn;
        User.UpdateBoard();

        ChessJudger judger = new ChessJudger(Inner.Container);
        FigureColor currSide = turns_manager.CurrentTurn;
        if (judger.IsNoMoves(currSide))
        {
            if (judger.KingShah(currSide))
            {
                Message.text = "Checkmate: " + ((currSide == FigureColor.Black)? "white" : "black") + " win";
            }
            else
            {
                Message.text = "Stalemate";
            }
        }
    }

    public void BackTurn()
    {
        Inner.RejectMove(turns_manager.PrevTurn());
        ChoiceTF.Color = turns_manager.CurrentTurn;
        curr_state.Reject();
        User.UpdateBoard();

        Message.text = "";
    }

    public void Switch(ChessState old_state, ChessState new_state)
    {
        old_state.Deactivate();
        new_state.Activate();
        curr_state = new_state;
    }
}

