using Microsoft.Xna.Framework.Content;

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

        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
        }

    }
}