using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

[RequireComponent(typeof(PieceCreator))]
public class PieceSystem : Singleton<PieceSystem>
{
    private BoardLayout _boardLayout;
    private PieceCreator _pieceCreator;

    private void Awake()
    {
        base.Awake();
        _pieceCreator = GetComponent<PieceCreator>();
    }

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<MovePieceGA>(MovePiecePerformer);
        ActionSystem.AttachPerformer<AttackMovePieceGA>(AttackMovePiecePerformer);
        ActionSystem.AttachCoroutinePerformer<WaitForNextMoveGA>(WaitForNextMovePerformer);
        ActionSystem.AttachCoroutinePerformer<SpawnPieceGA>(SpawnPiecePerformer);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<MovePieceGA>();
        ActionSystem.DetachPerformer<AttackMovePieceGA>();
        ActionSystem.DetachPerformer<WaitForNextMoveGA>();
        ActionSystem.DetachPerformer<SpawnPieceGA>();
    }

    public void SetupBoardLayout(BoardLayout boardLayout)
    {
        _boardLayout = boardLayout;
        SpawnPiecesFromBoardLayout(_boardLayout);
    }

    private void SpawnPiecesFromBoardLayout(BoardLayout boardLayout)
    {
        for (int i = 0; i < boardLayout.GetPieceCount(); i++)
        {
            Vector2Int squareCoords = boardLayout.GetSquareCoordsAtIndex(i);
            TeamColor teamColor = boardLayout.GetTeamColorAtIndex(i);
            PieceType pieceType = boardLayout.GetPieceTypeAtIndex(i);

            CreateAndSetupPiece(squareCoords, teamColor, pieceType);
        }
    }

    private Piece CreateAndSetupPiece(Vector2Int squareCoords, TeamColor teamColor, PieceType pieceType)
    {
        Piece piece = _pieceCreator.CreatePiece(pieceType).GetComponent<Piece>();
        Tile toTile = GridSystem.Instance.GetTileAtCoords(squareCoords);

        PieceData pieceData = new PieceData(teamColor, pieceType, toTile);

        piece.Initialize(pieceData);

        var material = _pieceCreator.GetTeamMaterial(teamColor);
        piece.SetMaterial(material);

        return piece;
    }

    // Performers
    private IEnumerator MovePiecePerformer(MovePieceGA movePieceGA)
    {
        var fromTile = movePieceGA.Piece.CurrentTile;
        var toTile = movePieceGA.TargetTile;
        var piece = movePieceGA.Piece;

        if (!ManaSystem.Instance.HasEnoughMana(piece.GetManaCost())) yield break;
        if (!piece.CanMove()) yield break;

        SpendManaGA spendMana = new(piece.GetManaCost());
        yield return ActionSystem.Instance.ExecuteReactionNow(spendMana);

        WaitForNextMoveGA waitForNextMove = new WaitForNextMoveGA(piece);
        ActionSystem.Instance.AddReaction(waitForNextMove);
        fromTile.SetPiece(null);
        piece.SetNewTile(toTile);
        piece.SetCanMove(false);
        toTile.SetPiece(piece);

        yield return piece.MoveToTile(toTile).WaitForCompletion();
    }

    private IEnumerator AttackMovePiecePerformer(AttackMovePieceGA attackMovePiece)
    {
        var fromTile = attackMovePiece.AttackingPiece.CurrentTile;
        var toTile = attackMovePiece.TargetPiece.CurrentTile;
        var attackingPiece = attackMovePiece.AttackingPiece;
        var targetPiece = attackMovePiece.TargetPiece;

        if (!ManaSystem.Instance.HasEnoughMana(attackingPiece.GetManaCost())) yield break;
        if (!attackingPiece.CanMove()) yield break;

        SpendManaGA spendMana = new(attackingPiece.GetManaCost());
        yield return ActionSystem.Instance.ExecuteReactionNow(spendMana);

        WaitForNextMoveGA waitForNextMove = new WaitForNextMoveGA(attackingPiece);
        ActionSystem.Instance.AddReaction(waitForNextMove);

        fromTile.SetPiece(null);
        toTile.SetPiece(attackingPiece);
        attackingPiece.SetNewTile(toTile);
        attackingPiece.SetCanMove(false);

        var tween = targetPiece.transform.DOScale(Vector3.zero, 0.25f);

        attackingPiece.MoveToTile(toTile);

        yield return tween.WaitForCompletion();

        tween.OnComplete(() =>
        {
            Destroy(targetPiece.gameObject);
        });
    }

    private IEnumerator WaitForNextMovePerformer(WaitForNextMoveGA waitForNextMoveGA)
    {
        var piece = waitForNextMoveGA.Piece;

        for (float i = 0; i <= piece.GetTimeDeltaBetweenMoves(); i += 0.01f)
        {
            piece.UpdateTimerBarUI(i / piece.GetTimeDeltaBetweenMoves());
            yield return new WaitForSeconds(0.01f);
        }
        piece.SetCanMove(true);
        piece.HideTimerUI();
    }

    private IEnumerator SpawnPiecePerformer(SpawnPieceGA spawnPieceGA)
    {
        var position = spawnPieceGA.Position;
        var pieceType = spawnPieceGA.PieceType;
        var teamColor = spawnPieceGA.TeamColor;
        var piece = CreateAndSetupPiece(position, teamColor, pieceType);

        piece.transform.localScale = Vector3.zero;
        float pieceSizeScale = .6f;

        Tween tween = piece.transform.DOScale(Vector3.one * pieceSizeScale, 0.1f);

        yield return tween.WaitForCompletion();

        piece.SetCanMove(false);
        WaitForNextMoveGA waitForNextMove = new WaitForNextMoveGA(piece);
        yield return ActionSystem.Instance.ExecuteReactionNow(waitForNextMove);


    }
}
