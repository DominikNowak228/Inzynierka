using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _manaText;
    [SerializeField] private SpriteRenderer _imageRenderer;
    [SerializeField] private GameObject _wrapper;
    [SerializeField] private LayerMask _cardInteractZoneMask;

    [SerializeField] private bool _GizmosEnabled = true;

    public Card Card { get; private set; }

    private bool _canBeUsed = false;
    private Vector3 _originalPosition;

    public void Setup(Card card)
    {
        Card = card;
        _titleText.text = card.Title;
        _descriptionText.text = card.Description;
        _manaText.text = card.Mana.ToString();
        _imageRenderer.sprite = card.Image;
    }

    public void SetCanBeUsed(bool canBeUsed) => _canBeUsed = canBeUsed;

    private void OnMouseEnter()
    {
        if (!_canBeUsed) return;
        if (!InteractSystem.Instance.PlayerCanHover()) return;
        _originalPosition = transform.position;
        transform.position += new Vector3(0, .5f, .2f);
        //CardViewHoverSystem.Instance.Show(Card, Vector3.zero);
    }

    private void OnMouseExit()
    {
        if (!_canBeUsed) return;
        if (!InteractSystem.Instance.PlayerCanHover()) return;
        transform.position = _originalPosition;
        //CardViewHoverSystem.Instance.Hide();
    }

    private void OnMouseDown()
    {
        if (!_canBeUsed) return;
        if (!InteractSystem.Instance.PlayerCanInteract()) return;

        if (Card.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargerting(transform.position);
        }
        else
        {
            InteractSystem.Instance.PlayerIsDragging = true;

            transform.position = _originalPosition;
            transform.position = MouseUtil.GetMouseWorldPositionForCard(-1);
        }


    }

    private void OnMouseDrag()
    {
        if (!_canBeUsed) return;
        if (!InteractSystem.Instance.PlayerCanInteract()) return;
        if (Card.ManualTargetEffect != null) return;

        transform.position = MouseUtil.GetMouseWorldPositionForCard(-1);
    }

    private void OnMouseUp()
    {
        if (!_canBeUsed) return;
        if (!InteractSystem.Instance.PlayerCanInteract()) return;

        if (Card.ManualTargetEffect != null)
        {
            Tile targetTile = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetTileUnderCursor());
            if (targetTile != null && ManaSystem.Instance.HasEnoughMana(Card.Mana))
            {
                if (!CardSystem.Instance.CanPlayCardWithThisManualEffect(Card, targetTile)) return;

                PlayCardGA playCardGA = new PlayCardGA(Card, targetTile);
                ActionSystem.Instance.Perform(playCardGA);
            }
            else
            {
                // Return to hand
                HandView.Instance.resetCardPositions();
            }
        }
        else
        {
            if (ManaSystem.Instance.HasEnoughMana(Card.Mana) &&
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f, _cardInteractZoneMask))
            {

                PlayCardGA playCardGA = new PlayCardGA(Card);
                ActionSystem.Instance.Perform(playCardGA);
            }
            else
            {
                // Return to hand
                HandView.Instance.resetCardPositions();
            }
            InteractSystem.Instance.PlayerIsDragging = false;
        }


    }
}
