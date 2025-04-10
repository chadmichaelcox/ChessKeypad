using ChessKeypad.Models;

namespace ChessKeypad.Services
{
    public class PhoneNumberGeneratorService
    {
        private readonly Dictionary<string, (int row, int col)> _keypad = new()
        {
            ["1"] = (0, 0),
            ["2"] = (0, 1),
            ["3"] = (0, 2),
            ["4"] = (1, 0),
            ["5"] = (1, 1),
            ["6"] = (1, 2),
            ["7"] = (2, 0),
            ["8"] = (2, 1),
            ["9"] = (2, 2),
            ["*"] = (3, 0),
            ["0"] = (3, 1),
            ["#"] = (3, 2),
        };

        private readonly string[] _digits = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "0"];

        private readonly List<ChessPiece> _pieces = new()
        {
            new Knight(),
            new King(),
            new Rook(),
            new Bishop(),
            new Queen()
        };

        public Dictionary<string, int> GenerateAllValidCounts()
        {
            var results = new Dictionary<string, int>();

            foreach (var piece in _pieces)
            {
                int total = 0;

                foreach (var digit in _digits)
                {
                    if (digit == "0" || digit == "1") continue;

                    total += CountValidNumbers(piece, digit, 6);
                }

                results[piece.Name] = total;
            }

            return results;
        }

        private int CountValidNumbers(ChessPiece piece, string current, int stepsLeft)
        {
            if (stepsLeft == 0) return 1;

            var (r, c) = _keypad[current];
            var total = 0;

            foreach (var (dr, dc) in piece.GetMoves())
            {
                var nr = r + dr;
                var nc = c + dc;

                var next = _keypad.FirstOrDefault(kvp => kvp.Value == (nr, nc)).Key;

                if (next != null && next != "*" && next != "#")
                {
                    total += CountValidNumbers(piece, next, stepsLeft - 1);
                }
            }

            return total;
        }
    }
}
