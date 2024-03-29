﻿namespace EvoSim.Sim;

class SimConfiguration
{
    public int GenomeLength { get; set; }
    public int InternalNeuronCount { get; set; }
    public int Size { get; set; }
    public double MutationChance { get; set; }
    public int PopulationSize { get; set; }
    public int Iterations { get; set; }
    public List<IGoal> Goals { get; } = new();
}
