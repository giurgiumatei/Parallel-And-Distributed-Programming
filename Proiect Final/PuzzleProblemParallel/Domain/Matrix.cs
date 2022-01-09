using System.Text;

namespace PuzzleProblemParallel.Domain;

[Serializable]
public sealed class Matrix
{
    private Matrix? _previousState;
    private static int[] _dx = { 0, -1, 0, 1 };
    private static int[] _dy = { -1, 0, 1, 0 };
    private static string[] _movesStrings = { "left", "up", "right", "down" };
    
    public byte[][] Tiles { get; set; }
    public int FreePositionI { get; set; }
    public int FreePositionJ { get; set; }
    public int NumberOfSteps { get; set; }
    public int MinimumNumberOfSteps { get; set; }
    public int Estimation { get; set; }
    public int Manhattan { get; set; }
    public string Move { get; set; }



    public Matrix(byte[][] tiles, int freePositionI, int freePositionJ, int numberOfSteps, Matrix? previousState, string move)
    {
        Tiles = tiles;
        FreePositionI = freePositionI;
        FreePositionJ = freePositionJ;
        NumberOfSteps = numberOfSteps;
        _previousState = previousState;
        Move = move;
        Manhattan = ManhattanDistance();
        MinimumNumberOfSteps = numberOfSteps + Manhattan;
        Estimation = Manhattan + numberOfSteps;
    }

    public static Matrix FromFile()
    {
        var tiles = new byte[4][];

        for (var i = 0; i < 4; i++)
        {
            tiles[i] = new byte[4];
        }

        tiles = File.ReadAllLines("C:\\Facultate\\Anul 3\\PDP\\Proiect\\Parallel-And-Distributed-Programming\\Proiect\\PuzzleProblemParallel\\PuzzleProblemParallel\\Domain\\Matrix.txt")
            .Select(l => l.Split(' ').Select(byte.Parse).ToArray())
            .ToArray();

        var freeI = -1;
        var freeJ = -1;

        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                if (tiles[i][j] != 0) continue;

                freeI = i;
                freeJ = j;
            }
        }

        return new Matrix(tiles, freeI, freeJ, 0, null, string.Empty);
    }


    public int ManhattanDistance()
    {
        var distance = 0;
        for (var i = 0; i < 4; i++)
        {
            for (var j = 0; j < 4; j++)
            {
                if (Tiles[i][j] == 0) continue;
                
                var targetI = (Tiles[i][j] - 1) / 4;
                var targetJ = (Tiles[i][j] - 1) % 4;
                distance += Math.Abs(i - targetI) + Math.Abs(j - targetJ);
            }
        }

        return distance;
    }

    public List<Matrix> GenerateMoves()
    {
        List<Matrix> moves = new();

        for (var k = 0; k < 4; k++)
        {
            if (FreePositionI + _dx[k] < 0 || FreePositionI + _dx[k] >= 4 || FreePositionJ + _dy[k] < 0 ||
                FreePositionJ + _dy[k] >= 4) continue;

            var movedFreePositionI = FreePositionI + _dx[k];
            var movedFreePositionJ = FreePositionJ + _dy[k];

            if (_previousState != null && movedFreePositionI == _previousState.FreePositionI &&
                movedFreePositionJ == _previousState.FreePositionJ) continue;

            var movedTiles = Tiles.Select(x => x.ToArray()).ToArray();

            movedTiles[FreePositionI][FreePositionJ] = movedTiles[movedFreePositionI][movedFreePositionJ];
            movedTiles[movedFreePositionI][movedFreePositionJ] = 0;

            moves.Add(new Matrix(movedTiles, movedFreePositionI, movedFreePositionJ, NumberOfSteps + 1, this, _movesStrings[k]));
        }

        return moves;
    }


    public override string ToString()
    {
        var current = this;
        var result = new List<string>();
        while (current != null)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Environment.NewLine);
            stringBuilder.Append(current.Move);
            stringBuilder.Append(Environment.NewLine);
        
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    stringBuilder.Append(current.Tiles[i][j]);
                    stringBuilder.Append(" ");
                }
                stringBuilder.Append(Environment.NewLine);
            }
        
            stringBuilder.Append(Environment.NewLine);
            result.Add(stringBuilder.ToString());
            current = current._previousState;
        }
        
        result.Reverse();
        
        return "Moves: " +
               string.Join("", result) +
               "numberOfSteps=" + NumberOfSteps;
    }
}