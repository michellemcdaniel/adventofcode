using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day08
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            string inputString = string.Join("", input).Trim();

            int width = 25;
            int height = 6;
            int stringPointer = 0;

            Dictionary<int, Layer> layers = new Dictionary<int, Layer>();

            int layer = 0;

            while (stringPointer < inputString.Length)
            {
                for (int j = 0; j < height; j++)
                {
                    if (!layers.ContainsKey(layer))
                    {
                        layers[layer] = new Layer();
                    }
                    layers[layer].AddRow(j, inputString.Substring(stringPointer, width));
                    stringPointer+=width;
                }
                layer++;
            }

            int minZeros = Int32.MaxValue;
            int layerWithMinZeros = 0;
            int answer = 0;
            foreach(var kvp in layers)
            {
                int zerosInLayer = kvp.Value.CountCharacter('0');
            
                if (zerosInLayer < minZeros)
                {
                    minZeros = zerosInLayer;
                    layerWithMinZeros = kvp.Key;
                    answer = kvp.Value.CountCharacter('1') * kvp.Value.CountCharacter('2');
                }
            }

            Layer mergedLayers = layers[0];

            foreach(var kvp in layers)
            {
                foreach (var row in mergedLayers.Rows)
                {
                    char[] newRow = row.Value.ToCharArray();
                    for (int i = 0; i < newRow.Length; i++)
                    {
                        if (newRow[i] == '2')
                        {
                            newRow[i] = kvp.Value.Rows[row.Key][i];
                        }
                    }
                    mergedLayers.Rows[row.Key] = string.Join("", newRow);
                }
            }

            foreach (var row in mergedLayers.Rows)
            {
                Console.WriteLine(row.Value.Replace('0',' ').Replace('1','#'));
            }

            Console.WriteLine($"Found {minZeros} zeros in layer {layerWithMinZeros}.");
            Console.WriteLine($"Answer: {answer}");
        }
    }

    public class Layer
    {
        public Dictionary<int, string> Rows;

        public Layer()
        {
            Rows = new Dictionary<int, string>();
        }

        public void AddRow(int row, string data)
        {
            Rows[row] = data;
        }

        public int CountCharacter(char c)
        {
            int charInLayer = 0;
            foreach(var row in Rows)
            {
                charInLayer += row.Value.Count(l => l == c);
            }
            return charInLayer;
        }
    }
}
