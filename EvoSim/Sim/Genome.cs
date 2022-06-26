using System.Runtime.CompilerServices;

namespace EvoSim.Sim;

struct Genome : IEnumerable<Gene>
{
    readonly Gene[] genes;

    public Genome(SimConfiguration simConfiguration, bool random = true)
    {
        genes = new Gene[simConfiguration.GenomeLength];

        if (random)
            for (int i = 0; i < simConfiguration.GenomeLength; i++)
                genes[i] = new(
                    Random.Shared.Next(Receptor.Count + simConfiguration.InternalNeuronCount),
                    Random.Shared.Next(Effector.Count + simConfiguration.InternalNeuronCount),
                    Random.Shared.NextSingle());
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        foreach (var gene in genes)
            hash.Add(gene);
        return hash.ToHashCode();
    }

    public static Genome MakeChild(SimConfiguration simConfiguration, IList<Genome> parentGenomes)
    {
        if (parentGenomes.Count == 0)
            throw new InvalidOperationException("Can't generate children with no parents");
        else if (parentGenomes.Count == 1)
            return parentGenomes[0];

        var parent1Idx = Random.Shared.Next(parentGenomes.Count);
        var parent2Idx = Random.Shared.Next(parentGenomes.Count);
        while (parent1Idx == parent2Idx)
            parent2Idx = Random.Shared.Next(parentGenomes.Count);

        var child = new Genome(simConfiguration, false);
        for (int i = 0; i < simConfiguration.GenomeLength; i++)
        {
            ref var selectedGene = ref parentGenomes[Random.Shared.Next(2) == 0 ? parent1Idx : parent2Idx].genes[i];
            Gene newGene;

            if (Random.Shared.NextDouble() < simConfiguration.MutationChance)
            {
                var chance = Random.Shared.NextDouble();
                var bitIndex = 1 << Random.Shared.Next(8);
                if (chance < 1.0 / 3)
                {
                    // mutate the source
                    var sourceIsReceptor = selectedGene.SourceId < Receptor.Count;
                    newGene = new((selectedGene.SourceId ^ bitIndex)
                            % (sourceIsReceptor ? Receptor.Count : simConfiguration.InternalNeuronCount)
                            + (sourceIsReceptor ? 0 : Receptor.Count),
                        selectedGene.SinkId, selectedGene.Weight);
                }
                else if (chance < 2.0 / 3)
                {
                    // mutate the sink
                    var sinkIsEffector = selectedGene.SinkId >= Receptor.Count + simConfiguration.InternalNeuronCount;
                    newGene = new(selectedGene.SourceId,
                        (selectedGene.SinkId ^ bitIndex)
                            % (sinkIsEffector ? Effector.Count : simConfiguration.InternalNeuronCount)
                            + (sinkIsEffector ? Receptor.Count + simConfiguration.InternalNeuronCount : Receptor.Count),
                        selectedGene.Weight);
                }
                else
                {
                    // mutate the weight
                    newGene = new(selectedGene.SourceId, selectedGene.SinkId,
                        selectedGene.RawWeight ^ (1 << Random.Shared.Next(16)));
                }
            }
            else
                newGene = selectedGene;

            child.genes[i] = newGene;
        }
        return child;
    }

    public IEnumerator<Gene> GetEnumerator() => ((IEnumerable<Gene>)genes).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => genes.GetEnumerator();

    public ref Gene this[int idx] => ref genes[idx];

    public override string ToString()
    {
        Globals.SharedStringBuilder.Clear();

        foreach (var b in MemoryMarshal.AsBytes(genes.AsSpan()))
            Globals.SharedStringBuilder.AppendFormat("{0:X2}", b);

        return Globals.SharedStringBuilder.ToString();
    }
}
