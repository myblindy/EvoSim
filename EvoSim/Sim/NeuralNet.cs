namespace EvoSim.Sim;

class NeuralNet
{
    readonly Neuron[] neurons;

    public NeuralNet(SimConfiguration simConfiguration)
    {
        neurons = new Neuron[Receptor.Count + simConfiguration.InternalNeuronCount + Effector.Count];
        for (int i = 0; i < neurons.Length; i++)
            neurons[i] = new();
    }

    public void Load(Genome genome)
    {
        foreach (var neuron in neurons)
            neuron.In.Clear();

        foreach (var gene in genome)
            neurons[gene.SinkId + Receptor.Count].In.Add((neurons[gene.SourceId], gene.Weight));
    }
}

class Neuron
{
    public List<(Neuron neuron, float weight)> In { get; } = new();
    public float? Value { get; set; }
}