namespace EvoSim.Sim;

struct Gene
{
    public readonly byte SourceId, SinkId;
    public readonly ushort RawWeight;
    public float Weight => RawWeight / 8192f;

    public Gene(int sourceId, int sinkId, float weight) =>
        (SourceId, SinkId, RawWeight) = ((byte)sourceId, (byte)sinkId, (ushort)(weight * 8192));

    public Gene(int sourceId, int sinkId, int rawWeight) =>
        (SourceId, SinkId, RawWeight) = ((byte)sourceId, (byte)sinkId, (ushort)rawWeight);
}
