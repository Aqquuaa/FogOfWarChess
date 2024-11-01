namespace FogOfWarChess.MainCore.MainEngine
{
    public abstract class Move
    {
        public abstract MoveType Type { get; }
        public abstract Position FromPos { get; }
        public abstract Position ToPos { get; }

        public abstract void Execute(ChessBoard board);

        public virtual bool IsMoveSaveForKing(ChessBoard board)
        {
            Color color = board[FromPos].Color;
            ChessBoard boardcopy = board.Copy();
            Execute(boardcopy);
            return !boardcopy.IsInCheck(color);
        }
    }

}

