using UnityEngine;

public class CardViewHoverSystem : Singleton<CardViewHoverSystem>
{
    [SerializeField] private CardView cardViewHower;

    public void Show(Card card, Vector3 position)
    {
        cardViewHower.gameObject.SetActive(true);
        cardViewHower.Setup(card);
        //cardViewHower.transform.position = position;
    }

    public void Hide()
    {
        cardViewHower.gameObject.SetActive(false);
    }
}
