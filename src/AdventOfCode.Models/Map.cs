using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Models
{
    public class DictionaryMap<T>
    {
        Dictionary<(int, int), T> Map { get; }

        public DictionaryMap()
        {
            Map = new Dictionary<(int, int), T>();
        }

        public void Add((int, int) position, T item)
        {
            Map.Add(position, item);
        }

        public bool TryAdd((int, int) position, T item)
        {
            return Map.TryAdd(position, item);
        }

        public T Get((int,int) position)
        {
            if (Map.TryGetValue(position, out T item))
            {
                return item;
            }

            return default;
        }

        public bool TryGet((int, int) position, out T item)
        {
            return Map.TryGetValue(position, out item);
        }

        public void Update((int,int) position, T item)
        {
            Map[position] = item;
        }

        public void Populate((int row, int col) startPosition, (int row, int col) endPosition, T defaultValue)
        {
            for (int row = startPosition.row; row < endPosition.row; row++)
            {
                for (int col = startPosition.col; col < endPosition.col; col++)
                {
                    TryAdd((row,col), defaultValue);
                }
            }
        }

        public string ToString((int row, int col) startPosition, (int row, int col) endPosition, Func<T, char> Transform)
        {
            StringBuilder builder = new();
            for (int row = startPosition.row; row < endPosition.row; row++)
            {
                for (int col = startPosition.col; col < endPosition.col; col++)
                {
                    if (TryGet((row,col), out T item))
                    {
                        builder.Append(Transform(item));
                    }
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }
}