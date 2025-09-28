public class ModifierManaRegenGameAciton : GameAction
{
    public CardOwner owner;
    public float modifierAmount;
    public float modifierTime;

    public ModifierManaRegenGameAciton(float modifierAmount, float modifierTime, CardOwner owner)
    {
        this.modifierAmount = modifierAmount;
        this.modifierTime = modifierTime;
        this.owner = owner;
    }
}