namespace EvoSim.Sim;

struct Gene
{
    public readonly byte SourceId, SinkId;
    public readonly ushort RawWeight;
    public float Weight => (float)RawWeight / (ushort.MaxValue / 2) - 1;

    public Gene(int sourceId, int sinkId, float weight) =>
        (SourceId, SinkId, RawWeight) = ((byte)sourceId, (byte)sinkId, (ushort)(weight * ushort.MaxValue));

    public Gene(int sourceId, int sinkId, int rawWeight) =>
        (SourceId, SinkId, RawWeight) = ((byte)sourceId, (byte)sinkId, (ushort)rawWeight);

    public override int GetHashCode() =>
        HashCode.Combine(SourceId, SinkId, RawWeight);
}
