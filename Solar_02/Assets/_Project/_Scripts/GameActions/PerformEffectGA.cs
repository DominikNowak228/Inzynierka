using System.Collections.Generic;

public class PerformEffectGA : GameAction
{
    public Effect Effect { get; private set; }
    public List<Tile> Targets { get; private set; }
    public PerformEffectGA(Effect effect, List<Tile> targets)
    {
        Effect = effect;
        Targets = targets == null ? null : new(targets);
    }
}