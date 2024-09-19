using Microsoft.Xna.Framework.Content;

namespace FogOfWarChess.MainCore.MainEngine
{
    public class Position
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;  
            Column = column;
        }

        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Row + dir.RowDelta, pos.Column + dir.ColumnDelta);
        }

    }
}