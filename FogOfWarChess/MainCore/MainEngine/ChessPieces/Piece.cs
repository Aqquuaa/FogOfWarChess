using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FogOfWarChess.MainCore.MainEngine
{
    public abstract class Piece
    {
        public string PieceName { get; protected set; }
        private Texture2D pieceTexture2D;
        public abstract PieceType Type { get; }
        public abstract Color Color { get; }
        public bool HasMoved { get; set; } = false;
        public abstract Piece Copy();
        public abstract IEnumerable<Move> GetMoves(Position from, ChessBoard board);

        //This method finds all possibles moves for rook, bishop and queen. They share similar pattern: they can move as many tiles as possible. 
        protected IEnumerable<Position> MovePositionInDir(Position from, ChessBoard board, Direction dir)
        {
            
            for (Position pos = from + dir; board.IsInside(pos); pos += dir)
            {
                if(board.IsEmpty(pos))
                {
                    yield return pos;
                    continue;
                }

                Piece piece = board[pos];
                if (piece.Color != Color)
                {
                    yield return pos;
                }
                
                yield break;
            }
        }

        protected IEnumerable<Position> MovePositionInDirs(Position from, ChessBoard board, Direction[] dirs)
        {
            return dirs.SelectMany(dir => MovePositionInDir(from,board,dir));
        }

        public virtual bool EnemiesKingCanBeCaptured(Position from, ChessBoard board) 
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.King;
            });
        }

        public void LoadTexture(ContentManager content)
        {
            pieceTexture2D = content.Load<Texture2D>($"CoreTextures/{PieceName + Color}");
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 positionVector2D)
        {
            spriteBatch.Draw(pieceTexture2D, positionVector2D, Microsoft.Xna.Framework.Color.White);
        }
    }

}