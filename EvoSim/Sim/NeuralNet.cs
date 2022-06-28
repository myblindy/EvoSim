using System.Numerics;
using System.Windows.Media.Media3D;

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
        for (var idx = Receptor.Count; idx < neurons.Length; ++idx)
            neurons[idx].Value = neurons[idx].In.Count == 0 ? 0 : null;

        bool any;
        do
        {
            any = false;

            for (var idx = Receptor.Count; idx < neurons.Length; ++idx)
            {
                var neuron = neurons[idx];
                if (neuron.Value is null)
                {
                    float a = 0, b = 0;
                    var okay = true;
                    for (int idxIn = 0; idxIn < neuron.In.Count; ++idxIn)
                    {
                        var (otherNeuron, weight) = neuron.In[idxIn];
                        if (otherNeuron.Value is { } otherNeuronValue)
                        {
                            a += otherNeuronValue * weight;
                            b += weight;
                        }
                        else
                        {
                            okay = false;
                            break;
                        }
                    }

                    if (okay)
                    {
                        neuron.Value = b != 0 ? a / b : 0;
                        any = true;
                    }
                }
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