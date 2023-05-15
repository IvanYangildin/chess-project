using UnityEngine;
using UnityEngine.Tilemaps;

public class UserBoard : MonoBehaviour
{ 
    [SerializeField]
    private Tilemap board_tilemap;
    [SerializeField]
    private Tilemap figures_tilemap;

    [SerializeField]
    private TileBase cell_white, cell_black, cell_white_go, cell_black_go;
    [SerializeField]
    private TileBase black_rook, black_knight, black_bishop, black_queen, black_king, black_pawn,
                     white_rook, white_knight, white_bishop, white_queen, white_king, white_pawn;

    public delegate void CellHendler(Vector2Int cell);
    // mouse button released over cell
    public event CellHendler OnUpCell;
    // mouse button presses over cell
    public event CellHendler OnDownCell;

    private bool view_white = true;

    private Vector3Int Center = new(4, 4, 0);

    public ChessBoard chess_board { get; private set; } = new ChessBoard();

    public void ChangeView()
    {
        view_white = !view_white;
        UpdateBoard();
    }

    private Vector2Int RealToChess(Vector3Int cell)
    {
        if (view_white)
        {
            return new Vector2Int(cell.x + Center.x, cell.y + Center.y);
        }
        else
        {
            return new Vector2Int(cell.x + Center.x, -cell.y + Center.y - 1);
        }
    }

    private Vector3Int ChessToReal(Vector2Int pos)
    {
        if (view_white) 
        { 
            return new Vector3Int(pos.x - Center.x, pos.y - Center.y);
        }
        else
        {
            return new Vector3Int(pos.x - Center.x, -pos.y + Center.y - 1);
        }
    }

    public TileBase FigureToTile(ChessFigure figure)
    {
        if (figure == null) return null;
        if (figure is Queen) return (figure.Color == FigureColor.White) ? white_queen : black_queen;
        if (figure is Bishop) return (figure.Color == FigureColor.White) ? white_bishop : black_bishop;
        if (figure is Knight) return (figure.Color == FigureColor.White) ? white_knight : black_knight;
        if (figure is King) return (figure.Color == FigureColor.White) ? white_king : black_king;
        if (figure is Pawn) return (figure.Color == FigureColor.White) ? white_pawn : black_pawn;
        if (figure is Rook) return (figure.Color == FigureColor.White) ? white_rook : black_rook;
        return null;
    }

    public Sprite FigureToSprite(ChessFigure figure)
    {
        TileBase tb = FigureToTile(figure);
        if (tb == null) return null;

        Tile tile = tb as Tile;
        return tile.sprite;
    }

    public void SignCell(Vector2Int cell)
    {
        if (chess_board.Container.Borders(cell))
            board_tilemap.SetTile(ChessToReal(cell),
                chess_board.Container.IsWhiteCell(cell) ? cell_white_go : cell_black_go);
    }

    public void CleanCell(Vector2Int cell)
    {
        if (chess_board.Container.Borders(cell))
            board_tilemap.SetTile(ChessToReal(cell),
                chess_board.Container.IsWhiteCell(cell) ? cell_white : cell_black);
    }

    public void CleanFigure(Vector2Int cell)
    {
        figures_tilemap.SetTile(ChessToReal(cell), null);
    }

    public void PutFigure(Vector2Int cell, ChessFigure figure)
    {
        if (chess_board.Container.Borders(cell))
            figures_tilemap.SetTile(ChessToReal(cell), FigureToTile(figure));
    }

    public void UpdateBoard()
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                ChessFigure figure = chess_board[i, j];
                
                Vector2Int cell = new(i, j);
                Vector3Int pos = ChessToReal(cell);

                board_tilemap.SetTile(pos, chess_board.Container.IsWhiteCell(cell) ? cell_white : cell_black);
                figures_tilemap.SetTile(pos, FigureToTile(figure));
            }
        }
    }

    private void InitBoard()
    {
        for (int i = 0; i < 8; ++i)
        {
            for (int j = 0; j < 8; ++j)
            {
                Vector2Int cell = new(i, j);
                board_tilemap.SetTile(ChessToReal(cell), chess_board.Container.IsWhiteCell(cell) ? cell_white : cell_black);
            }
        }
        UpdateBoard();
    }

    private void Start()
    {
        InitBoard();
    }

    private void Update()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = figures_tilemap.WorldToCell(point);

        if (Input.GetMouseButtonUp(0))
        {
            OnUpCell?.Invoke(RealToChess(cell));
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnDownCell?.Invoke(RealToChess(cell));
        }
    }
}
