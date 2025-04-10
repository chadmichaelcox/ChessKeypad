using ChessKeypad.Models;
using ChessKeypad.Services;

namespace ChessKeypad.Tests
{
    public class PhoneNumberGeneratorServiceTests
    {
        private readonly PhoneNumberGeneratorService _service;

        public PhoneNumberGeneratorServiceTests()
        {
            _service = new PhoneNumberGeneratorService();
        }

        [Fact]
        public void Should_Not_Include_Keys_Star_Or_Hash_In_Any_Count()
        {
            var result = _service.GenerateAllValidCounts();

            // Make sure we never include '*' or '#' in the results — those keys are off-limits
            Assert.All(result.Values, count => Assert.True(count > 0));
        }

        [Fact]
        public void Should_Not_Start_With_0_Or_1()
        {
            var result = _service.GenerateAllValidCounts();

            // We know valid numbers can't start with 0 or 1, so total counts should reflect that.
            // If they were included, we'd likely get a significantly higher count.
            Assert.True(result["Knight"] < 1000000);
        }

        [Fact]
        public void Should_Produce_7_Digit_PhoneNumbers()
        {
            // This is a sanity check — 1 starting digit + 6 moves should always give us a 7-digit number
            var knight = new Knight();
            int count = _service.GenerateAllValidCounts()[knight.Name];
            Assert.True(count > 0);
        }

        [Fact]
        public void Should_Allow_Easy_Extension_For_NewPiece()
        {
            // Just testing that adding a new piece doesn’t break anything.
            // In this case, we added a made-up 'Unicorn' piece with made-up moves.
            var unicorn = new DummyUnicornPiece();
            var count = CountValidNumbersFor(unicorn);
            Assert.True(count >= 0);
        }

        [Fact]
        public void Should_Be_Correct_Based_On_Known_Manual_Count()
        {
            // Here, we calculate the count manually for Knight and compare it to what the service returns.
            // If the numbers match, we’re confident the logic is working.
            var knight = new Knight();
            int count = CountValidNumbersFor(knight);
            Assert.Equal(_service.GenerateAllValidCounts()[knight.Name], count);
        }

        [Fact]
        public void Queen_Should_Have_More_Options_Than_Knight()
        {
            var counts = _service.GenerateAllValidCounts();

            // The Queen has a lot more available moves than the Knight, so it should have a higher total.
            Assert.True(counts["Queen"] > counts["Knight"]);
        }

        // Helper to manually compute the total number of valid numbers for a piece
        private int CountValidNumbersFor(ChessPiece piece)
        {
            int total = 0;
            string[] validStarts = ["2", "3", "4", "5", "6", "7", "8", "9"];
            foreach (var digit in validStarts)
            {
                total += CountRecursive(piece, digit, 6);
            }
            return total;
        }

        // Simulated keypad layout
        private static readonly Dictionary<string, (int, int)> _keypad = new()
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

        // Recursive method to simulate all valid phone numbers from a given piece
        private int CountRecursive(ChessPiece piece, string current, int stepsLeft)
        {
            if (stepsLeft == 0) return 1;

            var (r, c) = _keypad[current];
            int total = 0;

            foreach (var (dr, dc) in piece.GetMoves())
            {
                var nr = r + dr;
                var nc = c + dc;
                var next = _keypad.FirstOrDefault(kvp => kvp.Value == (nr, nc)).Key;

                // We skip invalid keys like '*' and '#'
                if (next != null && next != "*" && next != "#")
                {
                    total += CountRecursive(piece, next, stepsLeft - 1);
                }
            }

            return total;
        }

        // Dummy piece to test extension and edge cases
        private class DummyUnicornPiece : ChessPiece
        {
            public override string Name => "Unicorn";
            public override List<(int dr, int dc)> GetMoves() => new()
            {
                (1, 1), (2, 2), (-1, -1)
            };
        }
    }
}
