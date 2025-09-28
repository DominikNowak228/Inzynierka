using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSystem : Singleton<CardSystem>
{
    // This class can be used to manage card-related functionalities
    [SerializeField] private HandView handView;
    [SerializeField] private Transform drawPilePoint;
    [SerializeField] private Transform discardPilePoint;


    private readonly List<Card> drawPile = new();
    private readonly List<Card> discardPile = new();
    private readonly List<Card> hand = new();

    private void OnEnable()
    {
        ActionSystem.AttachCoroutinePerformer<DrawCardsGA>(DrawCardPerformer);
        ActionSystem.AttachCoroutinePerformer<DiscardAllCardsGA>(DiscardAllCardsPerformer);
        ActionSystem.AttachPerformer<PlayCardGA>(PlayCardPerformer);
    }
    private void OnDisable()
    {
        ActionSystem.DetachPerformer<DrawCardsGA>();
        ActionSystem.DetachPerformer<DiscardAllCardsGA>();
        ActionSystem.DetachPerformer<PlayCardGA>();
    }

    // Performeres
    public IEnumerator DrawCardPerformer(DrawCardsGA drawCardsGA)
    {
        int actualAmount = Mathf.Min(drawCardsGA.Amount, drawPile.Count);
        int notDrawnAmount = drawCardsGA.Amount - actualAmount;
        for (int i = 0; i < actualAmount; i++)
        {
            yield return DrawCard();
        }
        if (notDrawnAmount > 0)
        {
            RefillDeck();
            for (int i = 0; i < notDrawnAmount; i++)
            {
                yield return DrawCard();
            }
        }
    }

    public IEnumerator DiscardAllCardsPerformer(DiscardAllCardsGA discardAllCardsGA)
    {
        foreach (var card in hand)
        {
            CardView cardView = handView.RemoveCard(card);
            yield return DiscardCard(cardView);
        }
        hand.Clear();
    }

    public IEnumerator PlayCardPerformer(PlayCardGA playCardGA)
    {
        hand.Remove(playCardGA.Card);
        CardView cardView = handView.RemoveCard(playCardGA.Card);

        StartCoroutine(DiscardCard(cardView));

        SpendManaGA spendManaGA = new SpendManaGA(playCardGA.Card.Mana);
        ActionSystem.Instance.AddReaction(spendManaGA);

        if (playCardGA.Card.ManualTargetEffect != null)
        {
            PerformEffectGA performEffectGA = new(playCardGA.Card.ManualTargetEffect, new() { playCardGA.TileTarget });
            ActionSystem.Instance.AddReaction(performEffectGA);
            //ActionSystem.Instance.AddReaction(performEffectGA);
        }

        foreach (var effectWrapper in playCardGA.Card.OtherEffects)
        {
            List<Tile> targets = effectWrapper.TargetMode.GetTargets();
            Effect effect = effectWrapper.Effect;

            PerformEffectGA performEffectGA = new PerformEffectGA(effect, targets);
            ActionSystem.Instance.AddReaction(performEffectGA);
        }
        yield return null;
    }
    private IEnumerator DiscardCard(CardView cardView)
    {
        discardPile.Add(cardView.Card);

        cardView.transform.DOScale(Vector3.zero, 0.15f);
        Tween tween = cardView.transform.DOMove(discardPilePoint.position, 0.15f);

        yield return tween.WaitForCompletion();
        Destroy(cardView.gameObject);
    }

    private IEnumerator DrawCard()
    {
        Card card = drawPile.Draw();
        hand.Add(card);
        CardView cardView = CardViewCreator.Instance.CreateCardView(card, drawPilePoint.position, drawPilePoint.rotation);
        yield return handView.AddCard(cardView);
    }

    private void RefillDeck()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
    }

    internal void Setup(List<CardData> cardList)
    {
        cardList.ForEach(cd => drawPile.Add(new Card(cd)));
    }
    
    internal bool CanPlayCardWithThisManualEffect(Card card, Tile targetTile)
    {
            
        if (card.TargetManualType == Effect.TargetManualType.Enemy)
            if (targetTile.Piece.TeamColor == TeamColor.Black) return true;
        if (card.TargetManualType == Effect.TargetManualType.Ally)
            if (targetTile.Piece.TeamColor == TeamColor.White) return true;
        if (card.TargetManualType == Effect.TargetManualType.Any)
            if (targetTile.Piece != null) return true;
        if (card.TargetManualType == Effect.TargetManualType.EmptyTile)
            if (targetTile.Piece == null) return true;

        return false;
    }
}
