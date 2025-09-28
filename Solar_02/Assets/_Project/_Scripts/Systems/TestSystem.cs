using System.Collections.Generic;
using UnityEngine;

public class TestSystem : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    [SerializeField] private HandView handView;//moje
    [SerializeField] private CardData cardData;//moje

    [SerializeField] private BoardLayout _startBoardLayout;

    [SerializeField] private List<CardData> testCards;

    [Header("PlayerStats")]
    [SerializeField] private int maxMana = 10;

    private PlayerStats playerStats;
    private void Start()
    {
        GridSystem.Instance.Setup(width, height);
        PieceSystem.Instance.SetupBoardLayout(_startBoardLayout);
        PiecePlayerSystem.Instance.Setup(TeamColor.White);
        playerStats = new PlayerStats(maxMana, 3f);
        ManaSystem.Instance.Setup(playerStats);
        CardSystem.Instance.Setup(testCards);

        DrawCardsGA drawCardsGA = new DrawCardsGA(5);
        ActionSystem.Instance.Perform(drawCardsGA);
    }

   
    
}
