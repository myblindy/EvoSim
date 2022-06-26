using System.Numerics;

namespace EvoSim.Sim;

class NeuralNet
{
    readonly Neuron[] neurons;
    readonly Cell cell;

    public NeuralNet(Cell cell)
    {
        neurons = new Neuron[Receptor.Count + cell.SimConfiguration.InternalNeuronCount + Effector.Count];
        for (int i = 0; i < neurons.Length; i++)
            neurons[i] = new();
        this.cell = cell;
    }

    public void Load(Genome genome)
    {
        foreach (var neuron in neurons)
            neuron.In.Clear();

        foreach (var gene in genome)
            neurons[gene.SinkId + Receptor.Count].In.Add((neurons[gene.SourceId], gene.Weight));
    }

    public void Step()
    {
        // initialize the receptor values
        neurons[(int)ReceptorType.LocationX].Value = (float)cell.Position.X / cell.SimConfiguration.Size;
        neurons[(int)ReceptorType.LocationY].Value = (float)cell.Position.Y / cell.SimConfiguration.Size;

        // calculate all the internal neuron and effector values
        foreach (var neuron in neurons.Skip(Receptor.Count))
            neuron.Value = null;

        bool any;
        do
        {
            any = false;

            foreach (var neuron in neurons.Skip(Receptor.Count))
                if (neuron.Value is null && neuron.In.Count > 0 && neuron.In.All(w => w.neuron.Value.HasValue))
                {
                    neuron.Value = neuron.In.WeightedAverage(w => (w.neuron.Value!.Value, w.weight));
                    any = true;
                }

        } while (any);

        // calculate the effector actions
        float? getEffectorValue(EffectorType effectorType) =>
            neurons[Receptor.Count + cell.SimConfiguration.InternalNeuronCount + (int)effectorType].Value;

        Vector2 totalMove = default;
        if (getEffectorValue(EffectorType.MoveX) is { } moveXValue)
            totalMove += new Vector2(moveXValue, 0);
        if (getEffectorValue(EffectorType.MoveY) is { } moveYValue)
            totalMove += new Vector2(0, moveYValue);

        var s = new Vector2(MathF.Tanh(totalMove.X), MathF.Tanh(totalMove.Y));
        var movement = new Vector2i(
            (Random.Shared.NextSingle() < MathF.Abs(s.X) ? 1 : 0) * Math.Sign(s.X),
            (Random.Shared.NextSingle() < MathF.Abs(s.Y) ? 1 : 0) * Math.Sign(s.Y));
        if (movement is not { X: 0, Y: 0 })
            cell.Move(movement);
    }
}

class Neuron
{
    public List<(Neuron neuron, float weight)> In { get; } = new();
    public float? Value { get; set; }
}