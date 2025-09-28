public class ModifierEnemyManaRegenGameAciton : GameAction
{
    public CardOwner owner;
    public float modifierAmount;
    public float modifierTime;

    public ModifierEnemyManaRegenGameAciton(float modifierAmount, float modifierTime, CardOwner owner)
    {
        this.modifierAmount = modifierAmount;
        this.modifierTime = modifierTime;
        this.owner = owner;
    }
}
