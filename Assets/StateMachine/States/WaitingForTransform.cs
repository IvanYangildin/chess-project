using System.Collections.Generic;
using UnityEngine;


public class WaitingForTransform : ChessState
{
    PawnStep final_step;

    public WaitingForTransform(ChessMachine manager, PawnStep step) : base(manager)
    {
        final_step = step;
    }

    public override void Activate()
    {
        Manager.ChoiceTF.Activate();
        Manager.ChoiceTF.FigureChosen += Handle;
    }

    public override void Deactivate()
    {
        Manager.ChoiceTF.Deactivate();
        Manager.ChoiceTF.FigureChosen -= Handle;
    }

    public void Handle(ChessFigure tf_figure)
    {
        if (tf_figure != null)
        {
            final_step.TransformTo = tf_figure;
            Manager.NextTurn(final_step.Execute(Inner));
        }
        Manager.Switch(this, new WaitingForFigure(Manager));
    }
}