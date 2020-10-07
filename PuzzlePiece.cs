using JetBrains.Annotations;

namespace Puzzle
{
    public class PuzzlePiece
    {
        public char North { get; private set; }
        public char East { get; private set; }
        public char South { get; private set; }
        public char West { get; private set; }

        public PuzzlePiece(string piece)
        {
            North = piece[0];
            East = piece[1];
            South = piece[2];
            West = piece[3];
        }

        public bool FitsFromNorth([CanBeNull] PuzzlePiece northernPiece)
        {
            return North switch
            {
                'R' when northernPiece == null => true,
                'U' when northernPiece?.South == 'I' => true,
                'I' when northernPiece?.South == 'U' => true,
                _ => false
            };
        }

        public bool FitsFromWest([CanBeNull] PuzzlePiece westernPiece)
        {
            return West switch
            {
                'R' when westernPiece == null => true,
                'U' when westernPiece?.East == 'I' => true,
                'I' when westernPiece?.East == 'U' => true,
                _ => false
            };
        }

        public override string ToString()
        {
            return $"{North}{East}{South}{West}";
        }
    }
}