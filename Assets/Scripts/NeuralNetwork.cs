using System;
using System.Collections;
using System.Collections.Generic;

public class NeuralNetwork
{
    public double[][] neurons;
    public double[][][] weights;
    public double[][] biases;
    private int[] layers;
    private Random r;
    public NeuralNetwork(int[] layers)
    {
        this.layers = layers;
        r = new Random(DateTime.Now.Millisecond);
        biases = new double[layers.Length][];
        weights = new double[layers.Length][][];
        neurons = new double[layers.Length][];
        for(int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new double[layers[i]];
            biases[i] = new double[layers[i]];
            if (i != 0)
            {
                weights[i] = new double[layers[i]][];
                for(int o = 0; o < layers[i]; o++)
                {
                    weights[i][o] = new double[neurons[i - 1].Length];
                }
            }
        }
    }
    private double Sigm(double x) => 1d / (1d + Math.Pow(Math.E, -x));
    public void SetInp(double[] inps) { neurons[0] = inps; }
    public double[] GetOutp() => neurons[neurons.Length - 1];
    public void CalcNeurons()
    {
        for(int i = 1; i < neurons.Length; i++)
        {
            for(int j = 0; j < neurons[i].Length; j++)
            {
                double res = 0.0;
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    res += neurons[i - 1][k] * weights[i][j][k];
                }
                //The neural network works both with bias and without it
                res += biases[i][j];
                neurons[i][j] = Sigm(res);
            }
        }
    }
    public void RandomWeights(double randW, double randB)
    {
        for (int i = 1; i < neurons.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = (r.NextDouble()-0.5d) * 2d * randW;
                }
                biases[i][j] = (r.NextDouble() - 0.5d) * 2d * randB;
            }
        }
    }
    public NeuralNetwork CopyW(double randW, double randB)
    {
        var n = new NeuralNetwork(layers);
        for (int i = 1; i < neurons.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    n.weights[i][j][k] = weights[i][j][k] + (r.NextDouble() - 0.5d) * 2d * randW;
                }
                n.biases[i][j] = biases[i][j] + (r.NextDouble() - 0.5d) * 2d * randB;
            }
        }
        return n;
    }
}