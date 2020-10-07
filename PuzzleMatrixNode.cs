using System.Collections.Generic;

namespace Puzzle
{
    public class PuzzleMatrixNode
    {
        private readonly int _puzzleWith;
        private readonly int _x;
        private readonly int _y;

        public PuzzleMatrixNode(int x, int y, PuzzlePiece puzzlePiece, int puzzleWith)
        {
            _x = x;
            _y = y;
            _puzzleWith = puzzleWith;

            PuzzlePiece = puzzlePiece;
        }

        public PuzzlePiece PuzzlePiece { get; }

        public PuzzlePiece GetWestNode(IReadOnlyList<PuzzlePiece> currentSolution)
        {
            if (_x == 0) return null;
            var index = _y * _puzzleWith + _x - 1;
            return currentSolution[index];
        }

        public PuzzlePiece GetNorthNode(IReadOnlyList<PuzzlePiece> currentSolution)
        {
            if (_y == 0) return null;
            var index = (_y - 1) * _puzzleWith + _x;
            return currentSolution[index];
        }
    }
}