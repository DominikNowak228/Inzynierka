public class AttackMovePieceGA : GameAction
{
    public Piece AttackingPiece { get; private set; }
    public Piece TargetPiece { get; private set; }

    public AttackMovePieceGA(Piece attackingPiece, Piece targetPiece)
    {
        AttackingPiece = attackingPiece;
        TargetPiece = targetPiece;
    }
}