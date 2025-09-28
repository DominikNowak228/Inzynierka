public class SpendManaGA : GameAction
{
    public int Amount { get; private set; }
    public SpendManaGA(int amount)
    {
        Amount = amount;
    }
}
