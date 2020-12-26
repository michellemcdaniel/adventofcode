using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day06
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            List<ObjectInSpace> objectsInSpace = new List<ObjectInSpace>();
            
            foreach (string orbit in input)
            {
                string[] pair = orbit.Split(")");
                
                ObjectInSpace orbited = objectsInSpace.FirstOrDefault(o => o.Name == pair[0]);
                if (orbited == null)
                {
                    orbited = new ObjectInSpace(pair[0], null);
                    objectsInSpace.Add(orbited);
                }
                
                ObjectInSpace orbiter = objectsInSpace.FirstOrDefault(o => o.Name == pair[1]);

                if (orbiter == null)
                {
                    orbiter = new ObjectInSpace(pair[1], orbited);
                    objectsInSpace.Add(orbiter);
                }
                else
                {
                    orbiter.DirectlyOrbits = orbited;
                }

                orbited.AddOrbiter(orbiter);
            }

            int totalOrbits = 0;
            foreach (ObjectInSpace objectInSpace in objectsInSpace)
            {
                totalOrbits += objectInSpace.GetTotalOrbits();
            }

            Console.WriteLine($"Total Orbits: {totalOrbits}");

            GetIntersectPoint(objectsInSpace.First(o => o.Name == "YOU"), objectsInSpace.First(o => o.Name == "SAN"));
        }

        public static void GetIntersectPoint(ObjectInSpace a, ObjectInSpace b)
        {
            List<ObjectInSpace> aOrbits = a.GetOrbitList();
            List<ObjectInSpace> bOrbits = b.GetOrbitList();

            List<ObjectInSpace> intersect = aOrbits.Intersect(bOrbits).ToList();

            int aMin = Int32.MaxValue;
            int bMin = Int32.MaxValue;

            foreach (var objectInSpace in intersect)
            {
                aMin = Math.Min(a.GetOrbitsToIndirectOrbit(objectInSpace.Name), aMin);
                bMin = Math.Min(b.GetOrbitsToIndirectOrbit(objectInSpace.Name), bMin);
            }

            Console.WriteLine($"{aMin} + {bMin} = {aMin+bMin}");
        }
    }

    public class ObjectInSpace
    {
        public string Name { get; set; }
        public List<ObjectInSpace> Oribiters { get; }
        public ObjectInSpace DirectlyOrbits { get; set; }

        public ObjectInSpace(string name, ObjectInSpace directlyOrbits)
        {
            Name = name;
            Oribiters = new List<ObjectInSpace>();
            DirectlyOrbits = directlyOrbits;
        }

        public void AddOrbiter(ObjectInSpace orbiter)
        {
            Oribiters.Add(orbiter);
        }

        public int GetTotalOrbits()
        {
            return GetOrbitsToIndirectOrbit("COM");
        }

        public List<ObjectInSpace> GetOrbitList()
        {
            List<ObjectInSpace> orbits = new List<ObjectInSpace>();
            ObjectInSpace orbited = DirectlyOrbits;

            while (orbited != null)
            {
                orbits.Add(orbited);
                orbited = orbited.DirectlyOrbits;
            }

            return orbits;
        }

        public int GetOrbitsToIndirectOrbit(string name)
        {
            ObjectInSpace orbited = DirectlyOrbits;
            int orbits = 0;

            while(orbited != null && orbited.Name != name)
            {
                orbits++;
                orbited = orbited.DirectlyOrbits;
            }

            return orbits;
        }
    }
}
