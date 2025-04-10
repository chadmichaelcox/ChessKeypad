namespace ChessKeypad.Models
{
    public abstract class ChessPiece
    {
        public abstract string Name { get; }
        public abstract List<(int dr, int dc)> GetMoves();
    }

    public class Knight : ChessPiece
    {
        public override string Name => "Knight";
        public override List<(int dr, int dc)> GetMoves() => new()
        {
            (-2, -1), (-2, 1),
            (-1, -2), (-1, 2),
            (1, -2),  (1, 2),
            (2, -1),  (2, 1)
        };
    }

    public class King : ChessPiece
    {
        public override string Name => "King";
        public override List<(int dr, int dc)> GetMoves() => new()
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1),          (0, 1),
            (1, -1),  (1, 0), (1, 1)
        };
    }

    public class Rook : ChessPiece
    {
        public override string Name => "Rook";
        public override List<(int dr, int dc)> GetMoves() =>
            Enumerable.Range(1, 3)
                .SelectMany(i => new[] { (i, 0), (-i, 0), (0, i), (0, -i) })
                .ToList();
    }

    public class Bishop : ChessPiece
    {
        public override string Name => "Bishop";
        public override List<(int dr, int dc)> GetMoves() =>
            Enumerable.Range(1, 3)
                .SelectMany(i => new[] { (i, i), (-i, -i), (i, -i), (-i, i) })
                .ToList();
    }

    public class Queen : ChessPiece
    {
        public override string Name => "Queen";
        public override List<(int dr, int dc)> GetMoves() =>
            Enumerable.Range(1, 3)
                .SelectMany(i => new[]
                {
                    (i, 0), (-i, 0), (0, i), (0, -i),
                    (i, i), (-i, -i), (i, -i), (-i, i)
                }).ToList();
    }
}
