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
            List<(PacketValue a, PacketValue b)> originalPacketPairs = new();

            string[] input = File.ReadAllLines(filename);
            List<Packet> allPackets = new();
            int twoPosition = 1;
            int sixPosition = 1;

            int originalTwoPosition = 1;
            int originalSixPosition = 1;

            for (int i = 0; i < input.Length; i++)
            {
                if (string.IsNullOrEmpty(input[i]))
                {
                    continue;
                }

                (Packet a, bool aLessThanTwo, bool aLessThanSix) = ParsePacket(input[i]);
                (PacketValue originalA, bool oaLessThanTwo, bool oaLessThanSix) = ParsePacketOriginal(input[i++]);
                
                (Packet b, bool bLessThanTwo, bool bLessThanSix) = ParsePacket(input[i]);
                (PacketValue originalB, bool obLessThanTwo, bool obLessThanSix) = ParsePacketOriginal(input[i]);
                
                twoPosition = aLessThanTwo ? twoPosition+1 : twoPosition;
                twoPosition = bLessThanTwo ? twoPosition+1 : twoPosition;
                sixPosition = aLessThanSix ? sixPosition+1 : sixPosition;
                sixPosition = bLessThanSix ? sixPosition+1 : sixPosition;

                originalTwoPosition = oaLessThanTwo ? originalTwoPosition+1 : originalTwoPosition;
                originalTwoPosition = obLessThanTwo ? originalTwoPosition+1 : originalTwoPosition;
                originalSixPosition = aLessThanSix ? originalSixPosition+1 : originalSixPosition;
                originalSixPosition = obLessThanSix ? originalSixPosition+1 : originalSixPosition;

                packets.Add((a,b));
                originalPacketPairs.Add((originalA, originalB));
            }

            int totalPacketsOk = 0;
            int totalPacketsOkOriginal = 0;
            int totalPacketsOkStatic = 0;

            for (int i = 0; i < originalPacketPairs.Count(); i++)
            {
                if (packets[i].a.CompareTo(packets[i].b) <= 0)
                {
                    totalPacketsOk += i+1;
                }
                if (originalPacketPairs[i].a.Compare(originalPacketPairs[i].b) <= 0)
                {
                    totalPacketsOkOriginal += i+1;
                }
                if (Compare(originalPacketPairs[i].a, originalPacketPairs[i].b) <= 0)
                {
                    totalPacketsOkStatic += i+1;
                }
            }

            Console.WriteLine($"Part one: {totalPacketsOk} {totalPacketsOkOriginal} {totalPacketsOkStatic}");
            Console.WriteLine($"Part two: {twoPosition*sixPosition} {originalTwoPosition*originalSixPosition}");
        }

        public static ListPacket IteratePacket(ListPacket packet, int openBraceCount)
        {
            ListPacket currentPacket = packet;
            for (int j = 1; j < openBraceCount; j++)
            {
                // iterate through packet lists
                currentPacket = (ListPacket)currentPacket.Packets.Last();
            }
            return currentPacket;
        }

        public static PacketValue IteratePacket(PacketValue packet, int openBraceCount)
        {
            PacketValue currentPacket = packet;
            for (int j = 1; j < openBraceCount; j++)
            {
                // iterate through packet lists
                currentPacket = currentPacket.Values.Last();
            }
            return currentPacket;
        }

        public static (Packet, bool, bool) ParsePacket(string line)
        {
            int openBraceCount = 0;
            int i = 0;
            List<Packet> values = new List<Packet>();

            bool firstValue = true;
            bool LessThanTwo = false;
            bool LessThanSix = false;

            while (i < line.Length)
            {
                if (line[i] == '[')
                {
                    if (openBraceCount == 0)
                        values.Add(new ListPacket());
                    else 
                    {
                        ListPacket packet = IteratePacket((ListPacket) values.Last(), openBraceCount);
                        packet.Packets.Add(new ListPacket());
                    }
                    openBraceCount++;
                }
                else if (line[i] == ']')
                {
                    ListPacket packet = IteratePacket((ListPacket) values.Last(), openBraceCount);
                    if (firstValue)
                    {
                        LessThanSix = true;
                        LessThanTwo = true;
                        firstValue = false;
                    }
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
                    if (firstValue)
                    {
                        LessThanSix = int.Parse(nextValue) < 6;
                        LessThanTwo = int.Parse(nextValue) < 2;
                        firstValue = false;
                    }
                    ListPacket packet = IteratePacket((ListPacket) values.Last(), openBraceCount);
                    packet.Packets.Add(new IntPacket(int.Parse(nextValue)));
                }
                i++;
            }

            return (values.First(), LessThanTwo, LessThanSix);
        }

        public static (PacketValue, bool, bool) ParsePacketOriginal(string line)
        {
            int openBraceCount = 0;
            int i = 0;
            PacketValue values = new PacketValue();

            bool firstValue = true;
            bool LessThanTwo = false;
            bool LessThanSix = false;

            while (i < line.Length)
            {
                if (line[i] == '[')
                {
                    if (openBraceCount == 0)
                    {
                        values = new PacketValue();
                    }
                    else 
                    {
                        PacketValue packet = IteratePacket(values, openBraceCount);
                        packet.Values.Add(new PacketValue());
                    }
                    openBraceCount++;
                }
                else if (line[i] == ']')
                {
                    PacketValue packet = IteratePacket(values, openBraceCount);

                    if (firstValue && packet.Values.Count() == 0)
                    {
                        LessThanSix = true;
                        LessThanTwo = true;
                        firstValue = false;
                    }
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
                    if (firstValue)
                    {
                        LessThanSix = int.Parse(nextValue) < 6;
                        LessThanTwo = int.Parse(nextValue) < 2;
                        firstValue = false;
                    }

                    PacketValue packet = IteratePacket(values, openBraceCount);
                    packet.Values.Add(new PacketValue(int.Parse(nextValue)));
                }
                i++;
            }

            return (values, LessThanTwo, LessThanSix);
        }

        public static int Compare(PacketValue first, PacketValue second)
        {
            if (first.Value == -1)
            {
                if (second.Value != -1)
                {
                    // List vs Value
                    return Compare(first, new PacketValue(second.Value, true));
                }
                else
                {
                    // List vs List
                    for (int i = 0; ; i++)
                    {
                        if (i == first.Values.Count())
                        {
                            return i.CompareTo(second.Values.Count());
                        }

                        if (i == second.Values.Count())
                        {
                            return 1;
                        }

                        int subPacketCompare = Compare(first.Values[i], second.Values[i]);
                        if (subPacketCompare != 0)
                        {
                            return subPacketCompare;
                        }
                    }
                }
            }
            else
            {
                if (second.Value != -1)
                {
                    // Value vs Value
                    return first.Value.CompareTo(second.Value);
                }
                else
                {
                    // Value vs List
                    return Compare(new PacketValue(first.Value, true), second);
                }
            }
        }
    }

    public class PacketValue
    {
        public int Value;
        public List<PacketValue> Values;

        public PacketValue()
        {
            Values = new List<PacketValue>();
            Value = -1;
        }

        public PacketValue(int value, bool asList = false)
        {
            if (asList)
            {
                Values = new List<PacketValue>() 
                {
                    new PacketValue(value)
                };
                Value = -1;
            }
            else
            {
                Value = value;
            }
        }

        public int Compare(PacketValue other)
        {
            if (Value == -1)
            {
                // This is a List one. Other should be a List one too
                if (other.Value != -1)
                {
                    // Make it a list one and compare that
                    return this.Compare(new PacketValue(other.Value, true));
                }
                else
                {
                    // Two lists ones. Need to compare subparts.
                    for (int i = 0; ; i++)
                    {
                        if (i == Values.Count())
                        {
                            return i.CompareTo(other.Values.Count());
                        }

                        if (i == other.Values.Count())
                        {
                            return 1;
                        }

                        int subPacketCompare = Values[i].Compare(other.Values[i]);
                        if (subPacketCompare != 0)
                        {
                            return subPacketCompare;
                        }
                    }
                }
            }
            else
            {
                // This is a value one
                if (other.Value != -1)
                {
                    return Value.CompareTo(other.Value);
                }
                else
                {
                    return new PacketValue(Value, true).Compare(other);
                }
            }
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
                        return 0.CompareTo(l.Packets.Count);
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
                            return packetCompare;
                        }

                        if (i+1 == Packets.Count())
                        {
                            // If this is the last one, we need to go check the length of the other one
                            return (i+1).CompareTo(l.Packets.Count);
                        }
                    }
                    return 0;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
