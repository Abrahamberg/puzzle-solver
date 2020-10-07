using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;

namespace Puzzle
{
    public class PuzzleSolver
    {
        private readonly IReadOnlyList<PuzzlePiece> _allPieces;
        private readonly List<List<PuzzlePiece>> _solutions;

        public PuzzleSolver(IReadOnlyList<PuzzlePiece> allPieces)
        {
            Validate(allPieces);

            _allPieces = allPieces;

            var width = allPieces.Count(piece => piece.North == 'R');
            var height = allPieces.Count(piece => piece.West == 'R');

            _solutions = new List<List<PuzzlePiece>> {new List<PuzzlePiece>()};

            for (var yPosition = 0; yPosition < height; yPosition++)
            for (var xPosition = 0; xPosition < width; xPosition++)
                foreach (var solution in _solutions.ToList().AsReadOnly())
                    ReplaceCurrentSolutionWithItsBranches(solution, xPosition, yPosition, width);
        }

        public List<List<PuzzlePiece>> Solutions => _solutions;

        public int GetNumberOfSolutions()
        {
            return _solutions.Count();
        }

        private void ReplaceCurrentSolutionWithItsBranches(List<PuzzlePiece> currentSolution, int xPosition,
            int yPosition, int width)
        {
            _solutions.Remove(currentSolution);

            var remainedPieces = _allPieces.Where(x => !currentSolution.Contains(x)).ToList();
            remainedPieces= Optimize(remainedPieces);

            foreach (var piece in remainedPieces)
            {
                var node = new PuzzleMatrixNode(xPosition, yPosition, piece, width);
                if (!FitsSolution(node, currentSolution)) continue;

                var newSolution = currentSolution.ToList();
                newSolution.Add(piece);
                _solutions.Add(newSolution);

#if DEBUG
                newSolution.ForEach(x => Console.Write($"{x} "));
                Console.WriteLine($": {newSolution.Count}");
#endif
            }

            List<PuzzlePiece> Optimize(IEnumerable<PuzzlePiece> unOptimizedRemainedPieces)
            {
                var optimizedRemainedPieces = unOptimizedRemainedPieces.ToList();

                if (yPosition == 0) optimizedRemainedPieces = optimizedRemainedPieces.Where(x => x.North == 'R').ToList();
                if (xPosition == 0) optimizedRemainedPieces = optimizedRemainedPieces.Where(x => x.West == 'R').ToList();
                if (yPosition == width) optimizedRemainedPieces = optimizedRemainedPieces.Where(x => x.East == 'R').ToList();

                return optimizedRemainedPieces;
            }
        }

        private static bool FitsSolution(PuzzleMatrixNode node, IReadOnlyList<PuzzlePiece> currentSolution)
        {
            return node.PuzzlePiece.FitsFromNorth(node.GetNorthNode(currentSolution)) &&
                   node.PuzzlePiece.FitsFromWest(node.GetWestNode(currentSolution));
        }

        private static void Validate(IReadOnlyList<PuzzlePiece> pieces)
        {
            var north = pieces.Where(piece => piece.North == 'R').ToList().AsReadOnly();
            var east = pieces.Where(piece => piece.East == 'R').ToList().AsReadOnly();
            var south = pieces.Where(piece => piece.South == 'R').ToList().AsReadOnly();
            var west = pieces.Where(piece => piece.West == 'R').ToList().AsReadOnly();

            if (north.Count() != south.Count())
                throw new ArgumentException("North and South doe not have same number of pieces");
            if (east.Count() != west.Count())
                throw new ArgumentException("North and South doe not have same number of pieces");

            var northWest = north.First(piece => piece.West == 'R');
            Guard.Against.Null(northWest, "North West side is not provided");
            var northEast = north.First(piece => piece.East == 'R');
            Guard.Against.Null(northEast, "North East side is not provided");

            var southWest = south.First(piece => piece.West == 'R');
            Guard.Against.Null(southWest, "South West side is not provided");

            var southEast = south.First(piece => piece.East == 'R');
            Guard.Against.Null(southEast, "South East side is not provided");
        }
    }
}