using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.TwentyTwo
{
    public class Day07
    {
        public static void Execute(string filename)
        {
            List<string> instructions = new();
            MyDirectory root = new("/", null);
            MyDirectory currentDir = root;

            foreach(var line in File.ReadLines(filename))
            {
                if (line.StartsWith("$ cd"))
                {
                    if (line.Contains(".."))
                    {
                        currentDir = currentDir.GetParent();
                    }
                    else
                    {
                        RegexHelper.Match(line, @"cd ([a-z]+)", out string directoryName);
                        currentDir = currentDir.GetDirectory(directoryName);
                    }
                }
                else if (line.StartsWith("dir"))
                {
                    // Directory
                    RegexHelper.Match(line, @"dir ([a-z]+)", out string directoryName);
                    currentDir.AddDirectory(directoryName, currentDir);
                }
                else
                {
                    // File
                    RegexHelper.Match(line, @"(\d+)", out int size);
                    currentDir.AddFile(size);
                }
            }

            root.CalculateTotalSize();
            int neededSpace = root.Size - 40000000;
            
            Console.WriteLine($"Part one: {root.GetTotalSizeOfDirectoriesUnderCap(100000)}");
            Console.WriteLine($"Part two: {root.FindDirectoryToDelete(neededSpace)}");
        }

        public class MyDirectory
        {
            public int Size;
            int FileSize;
            Dictionary<string, MyDirectory> Directories;
            MyDirectory Parent;
            
            public MyDirectory(string name, MyDirectory parent)
            {
                Directories = new();
                Parent = parent;
                Size = 0;
                FileSize = 0;
            }

            public void AddDirectory(string dir, MyDirectory parent)
            {
                Directories.Add(dir, new MyDirectory(dir, parent));
            }

            public void AddFile(int size)
            {
                FileSize += size;
            }

            public MyDirectory GetDirectory(string name)
            {
                return Directories[name];
            }

            public MyDirectory GetParent()
            {
                return Parent;
            }

            public int FindDirectoryToDelete(int neededSpace)
            {
                int closest = int.MaxValue;
                foreach(var kvp in Directories)
                {
                    if (kvp.Value.Size > neededSpace)
                    {
                        closest = kvp.Value.Size < closest ? kvp.Value.Size : closest;

                        // Only check subdirectories if current directory matches min size requirements.
                        // Subdirectories are guaranteed to be smaller than the current directory.
                        int closestInDir = kvp.Value.FindDirectoryToDelete(neededSpace);
                        closest = closestInDir < closest ? closestInDir : closest;
                    }
                }

                return closest;
            }

            public void CalculateTotalSize()
            {
                Size = FileSize;

                foreach(var kvp in Directories)
                {
                    kvp.Value.CalculateTotalSize();
                    Size += kvp.Value.Size;
                }
            }

            public int GetTotalSizeOfDirectoriesUnderCap(int cap)
            {
                int total = 0;

                if (Size < cap)
                {
                    total += Size;
                }

                foreach(var kvp in Directories)
                {
                    total += kvp.Value.GetTotalSizeOfDirectoriesUnderCap(cap);
                }

                return total;
            }
        }
    }
}
