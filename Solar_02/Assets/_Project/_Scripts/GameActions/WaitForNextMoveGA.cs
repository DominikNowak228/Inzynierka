public class WaitForNextMoveGA : GameAction
{
    public Piece Piece { get; private set; }
    public WaitForNextMoveGA(Piece piece)
    {
        Piece = piece;
    }
}
