namespace FogOfWarChess.MainCore.MainEngine
{
    internal class Position
    {
        private int Row { get; }
        private int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Color SquareColor()
        {
                if ((Row + Column) % 2 == 0)
                {
                    return Color.White;
                }

                return Color.Black;
        }

    }
}