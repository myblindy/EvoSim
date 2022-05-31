namespace EvoSim.Support;

class Globals
{
    [ThreadStatic]
    static StringBuilder sharedStringBuilder = null!;
    public static StringBuilder SharedStringBuilder =>
        sharedStringBuilder ??= new();
}
