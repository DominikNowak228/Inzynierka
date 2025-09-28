using System.Collections.Generic;

public class NoTargetTM : TargetMode
{
    public override List<Tile> GetTargets()
    {
        return null;
    }
}