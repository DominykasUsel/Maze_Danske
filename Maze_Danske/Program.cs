using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Maze_Danske
{
    class Program
    {
        private static int iRows;
        private static int iColumns;
        private static int[,] movements_table;

        //private static bool isValid(int x, int y)
        //{
        //    return (x < iRows && y < iColumns && x >= 0 && y >= 0);
        //}

        private static bool FindPath(int[,] table, int x, int y, List<int> x_List, List<int> y_List)
        {
            movements_table[x, y] = 1;

            // Success
            if (x == iRows-1 || y == iColumns-1)
            {
                return true;
            }

            // go to bottom cell
            if (table[x + 1, y] == 0 && movements_table[x + 1, y] != 1)
            {
                x_List.Add(x);
                y_List.Add(y);
                FindPath(table, x + 1, y, x_List, y_List);
            }
            // go to top cell
            else if (table[x - 1, y] == 0 && movements_table[x - 1, y] != 1)
            {
                x_List.Add(x);
                y_List.Add(y);
                FindPath(table, x - 1, y, x_List, y_List);
            }
            // go to right cell
            else if (table[x, y + 1] == 0 && movements_table[x, y + 1] != 1)
            {
                x_List.Add(x);
                y_List.Add(y);
                FindPath(table, x, y + 1, x_List, y_List);
            }
            // go to left cell
            else if (table[x, y - 1] == 0 && movements_table[x, y - 1] != 1)
            {
                x_List.Add(x);
                y_List.Add(y);
                FindPath(table, x, y - 1, x_List, y_List);
            }
            // Can't move - go back to previous step
            else
            {
                y = y_List[y_List.Count - 1];
                x = x_List[x_List.Count - 1];
                y_List.RemoveAt(y_List.Count - 1);
                x_List.RemoveAt(x_List.Count - 1);

                movements_table[x, y] = 0;
                FindPath(table, x, y, x_List, y_List);
            }

            return true;
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("Maze.txt").ToList();
            iRows = Int32.Parse(lines[0].Split(" ")[0]);
            iColumns = Int32.Parse(lines[0].Split(" ")[1]);
            int[,] table = new int[iRows, iColumns];

            lines.RemoveAt(0);

            int x_Start = 0;
            int y_Start = 0;

            Console.WriteLine("Maze:");

            for (int i = 0; i < lines.Count; i++)
            {
                int j = 0;
                foreach (string number in lines[i].Split(" "))
                {
                    table[i, j] = Int32.Parse(number);

                    if (Int32.Parse(number) > 1)
                    {
                        x_Start = i;
                        y_Start = j;
                    }

                    j = j + 1;

                    Console.Write(number + " ");
                }

                Console.WriteLine();
            }

            movements_table = new int[iRows, iColumns];
            bool completed = FindPath(table, x_Start, y_Start, new List<int>(), new List<int>());

            if (completed == true)
            {
                Console.WriteLine("Solved. Solution:");

                string result = null;

                for (int i = 0; i < iRows; i++)
                {
                    for (int j = 0; j < iColumns; j++)
                    {
                        result = result + movements_table[i, j] + " ";
                        Console.Write(movements_table[i, j] + " ");
                    }
                    result = result + Environment.NewLine;
                    Console.WriteLine();
                }

                File.WriteAllText("Maze_Solved.txt", result);
            }
            else
            {
                Console.WriteLine("Can't solve the maze.");
            }

            Console.ReadLine();
        }
    }
}
