namespace EvoSim.Support;

static class EnumerableExtensions
{
    public static TValue WeightedAverage<TKey, TValue>(this IEnumerable<TKey> source, Func<TKey, (TValue value, TValue weight)> data)
        where TValue : IFloatingPoint<TValue>
    {
        var totalWeight = TValue.Zero;
        var sum = TValue.Zero;

        foreach (var item in source)
        {
            var (value, weight) = data(item);

            sum += value * weight;
            totalWeight += weight;
        }

        return sum / totalWeight;
    }
}

static class Extensions
{
    public static byte GetByte(this uint src, int byteIndex) =>
        (byte)((src >> (byteIndex * 8)) & 0xFF);

    public static byte GetByte(this int src, int byteIndex) =>
        (byte)((src >> (byteIndex * 8)) & 0xFF);

    public static uint BytesToUInt32(byte b0, byte b1, byte b2, byte b3) =>
        (uint)BytesToInt32(b0, b1, b2, b3);

    public static int BytesToInt32(byte b0, byte b1, byte b2, byte b3) =>
        b0 | (b1 << 8) | (b2 << 16) | (b3 << 24);
}