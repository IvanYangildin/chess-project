using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UserPawnTF : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private UserBoard visual;

    public delegate void HandleFigure(ChessFigure figure);
    public event HandleFigure FigureChosen;

    private Button QueenButton, BishopButton, KnightBUtton, RookButton;
    private List<ButtonTF> buttons = new List<ButtonTF>();

    private FigureColor color;
    public FigureColor Color
    {
        get { return color; }
        set
        {
            foreach (var button in buttons)
            {
                button.ChangeColor(value);
            }
            color = value;
        }
    }
    public bool IsInside { get; private set; } = false;

    private List<Func<FigureColor, ChessFigure>> constructors = 
        new List<Func<FigureColor, ChessFigure>> {
            (c) => new Queen(c, true),
            (c) => new Bishop(c, true),
            (c) => new Knight(c, true),
            (c) => new Rook(c, true)
    };

    public void Activate()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < transform.childCount; ++i)
        {
            Button button = transform.GetChild(i).GetComponent<Button>();
            var constr = constructors[i];
            button.onClick.AddListener(() => FigureChosen?.Invoke(constr(Color)));

            ChessFigure w_figure = constructors[i](FigureColor.White);
            ChessFigure b_figure = constructors[i](FigureColor.Black);

            buttons.Add(new(button, visual.FigureToSprite(b_figure), visual.FigureToSprite(w_figure)));
            buttons[i].ChangeColor(Color);
        }
    }

    public void Deactivate()
    {
        buttons.Clear();
        gameObject.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsInside)
                FigureChosen?.Invoke(null);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.fullyExited) IsInside = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsInside = true;
    }
}

class ButtonTF
{
    public readonly Button Trigger;
    public readonly Sprite BlackSprite, WhiteSprite;

    public ButtonTF(Button trigger, Sprite black_sprite, Sprite white_sprite)
    {
        Trigger = trigger;
        BlackSprite = black_sprite;
        WhiteSprite = white_sprite;
    }

    public void ChangeColor(FigureColor color)
    {
        if (color == FigureColor.Black)
        {
            Trigger.image.sprite = BlackSprite;
        }
        else
        {
            Trigger.image.sprite = WhiteSprite;
        }
    }

}