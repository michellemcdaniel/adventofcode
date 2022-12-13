using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day13
    {
        public static void Execute(string filename)
        {
            List<(Packet a, Packet b)> packets = new();
            string[] input = File.ReadAllLines(filename);
            List<Packet> allPackets = new();

            for (int i = 0; i < input.Length; i++)
            {
                if (string.IsNullOrEmpty(input[i]))
                {
                    continue;
                }

                Packet a = ParsePacket(input[i++]);
                Packet b = ParsePacket(input[i]);
                allPackets.Add(a);
                allPackets.Add(b);
                packets.Add((a,b));
            }

            int totalPacketsOk = 0;
            int currentPair = 0;

            foreach(var pair in packets)
            {
                
                if (pair.a.CompareTo(pair.b) <= 0)
                {
                    totalPacketsOk += currentPair;
                }
                currentPair++;
            }

            // now we need to sort this list.

            allPackets.Sort();
            int distressSignal = 1;

            for(int i = 0; i < allPackets.Count(); i++)
            {
                if (allPackets[i].ToString() == "[[2]]")
                {
                    distressSignal *= (i+1);
                }
                if (allPackets[i].ToString() == "[[6]]")
                {
                    distressSignal *= (i+1);
                }
            }

            Console.WriteLine($"Part one: {totalPacketsOk}");
            Console.WriteLine($"Part two: {distressSignal}");
        }

        public static Packet ParsePacket(string line)
        {
            int openBraceCount = 0;
            int i = 0;
            List<Packet> values = new List<Packet>();
            while (i < line.Length)
            {
                if (line[i] == '[')
                {
                    if (openBraceCount == 0)
                        values.Add(new ListPacket());
                    else 
                    {
                        ListPacket packet = (ListPacket) values.Last();
                        for (int j = 1; j < openBraceCount; j++)
                        {
                            // iterate through packet lists
                            packet = (ListPacket) packet.Packets.Last();
                        }

                        packet.Packets.Add(new ListPacket());
                    }
                    openBraceCount++;
                }
                else if (line[i] == ']')
                {
                    openBraceCount--;
                }
                else if (line[i] != ',')
                {
                    string nextValue = "";
                    while (char.IsDigit(line[i]))
                    {
                        nextValue += line[i];
                        i++;
                    }
                    ListPacket packet = (ListPacket) values.Last();
                    for (int j = 1; j < openBraceCount; j++)
                    {
                        packet = (ListPacket) packet.Packets.Last();
                    }

                    packet.Packets.Add(new IntPacket(int.Parse(nextValue)));
                }
                i++;
            }

            return values.First();
        }
    }

    public abstract class Packet : IComparable
    {
        public abstract int CompareTo(object other);
    }

    public class IntPacket : Packet, IComparable
    {
        public int Value;

        public IntPacket (int value)
        {
            Value = value;
        }

        public override int CompareTo(object other)
        {
            switch(other)
            {
                case IntPacket i:
                    return Value.CompareTo(i.Value);
                case ListPacket l:
                    return new ListPacket(new List<Packet>() {this}).CompareTo(l);
                default:
                    throw new NotSupportedException();
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ListPacket : Packet, IComparable
    {
        public List<Packet> Packets;

        public ListPacket(List<Packet> packets)
        {
            Packets = packets;
        }
        public ListPacket()
        {
            Packets = new List<Packet>();
        }

        public override string ToString()
        {
            return "[" + string.Join(",", Packets.Select(p => p.ToString())) + "]";
        }

        public override int CompareTo(object other)
        {
            switch(other)
            {
                case IntPacket i:
                    return this.CompareTo(new ListPacket(new List<Packet>() {i}));
                case ListPacket l:
                    if (Packets.Count() == 0)
                    {
                        if (l.Packets.Count == 0)
                        {
                            return 0;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    
                    for (int i = 0; i < Packets.Count(); i++)
                    {
                        if (i == l.Packets.Count())
                        {
                            // Compare side has fewer values, so it is smaller
                            return 1;
                        }
                        
                        // Compare this packet's score to other packet's score, at the same index
                        int packetCompare = Packets[i].CompareTo(l.Packets[i]);

                        if (packetCompare != 0)
                        {
                            // We only want to continue checking if the score wasn't 0. If it was 0, it means we're still even
                            return packetCompare;
                        }

                        if (i+1 == Packets.Count())
                        {
                            // If this is the last one, we need to go check the length of the other one
                            if (i+1 == l.Packets.Count())
                            {
                                // They are the same length. And we've already checked these values and we don't have a winner, so return 0
                                return 0;
                            }
                            else
                            {
                                return -1; // We have fewer elements, so we are smaller than other
                            }
                        }
                    }
                    return 0;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
