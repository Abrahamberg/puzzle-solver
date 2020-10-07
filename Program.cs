using System;

namespace Puzzle
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till Puzzle solver");
            Console.WriteLine("Ange excel-filadressen");
            Console.WriteLine("ex: C:\\folder\\test.xlsx");
            Console.WriteLine("");

            var path = Console.ReadLine();

            try
            {
                var pieces = ExcelReader.Read(path);
                var puzzleSolver = new PuzzleSolver(pieces);
                Console.WriteLine("################################");
                Console.WriteLine($"# Det finns {puzzleSolver.GetNumberOfSolutions()} lösningar. ");
                Console.WriteLine("################################");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Press Enter to continue");
                Console.ReadLine();
            }
        }
    }
}