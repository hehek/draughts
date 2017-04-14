using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public enum Player
    {
        WHITE, BLACK
    }

    public enum BoardField
    {
        EMPTY, WHITE, BLACK, WHITE_KING, BLACK_KING
    }

    public class Board
    {
        private BoardField[,] field;

        /// <summary>
        /// Пустая доска
        /// </summary>
        public Board()
        {
            field = new BoardField[8, 4];
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 4; j++)
                    field[i, j] = BoardField.EMPTY;
        }

        /// <summary>
        /// Возвращает доску с начальной позицией
        /// </summary>
        /// <returns>Доска</returns>
        public static Board Init()
        {
            Board b = new Board();
            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 4; c++)
                    if (r <= 2)
                        b.field[r, c] = BoardField.WHITE;
                    else if (r >= 5)
                        b.field[r, c] = BoardField.BLACK;

            return b;
        }

        /// <summary>
        /// Получение шашки по координатам
        /// </summary>
        /// <param name="r">Строка</param>
        /// <param name="c">Столбец</param>
        /// <returns>Тип шашки</returns>
        public BoardField this[int r, int c]
        {
            get
            {
                if ((r + c) % 2 != 0)
                    return BoardField.EMPTY;
                else
                    return field[r, c / 2];
            }

            set
            {
                if ((r + c) % 2 != 0)
                   field[r,c] = value;
                else
                   field[r, c / 2] = value;
              
            }
        }

        /// <summary>
        /// Владелец шашки на указанном поле
        /// </summary>
        /// <param name="r">Строка</param>
        /// <param name="c">Столбец</param>
        /// <returns>Владелец</returns>
        public Player Owner(int r, int c)
        {
            BoardField t = this[r, c];
            if (t == BoardField.BLACK || t == BoardField.BLACK_KING)
                return Player.BLACK;
            else if (t == BoardField.WHITE || t == BoardField.WHITE_KING)
                return Player.WHITE;
            else
                throw new ArgumentException();
        }
        
        /// <summary>
        /// Ходы шашкой без взятия
        /// </summary>
        /// <param name="init_pos">Шашка</param>
        /// <returns>Список ходов</returns>
        public  List<Move> GetMovesManWithoutCapture(int r, int c, Board b )//программные координаты
        {
            var moves = new List<Move>();
            
            if (b.Owner(r, c) == Player.WHITE)
          
                #region new Boards 
                {
                    if (c<7)
                    if (b.field[r + 1, c] == BoardField.EMPTY )
                    {
                        b.field[r, c] = BoardField.EMPTY;
                        b.field[r + 1, c] = BoardField.WHITE;
                        Coord t = new Coord(r, c);
                        moves.Add(new Move() { new_board = b, new_pos = t });
                    }
                    if (c>0)
                    if (b.field[r + 1, c - 1] == BoardField.EMPTY )
                        {
                            b.field[r, c]= BoardField.EMPTY;
                            b.field[r + 1, c - 1] = BoardField.WHITE;
                            Coord t = new Coord(r, c);
                            moves.Add(new Move() { new_board = b  , new_pos = t });
                        }
                        else throw new ArgumentException();
                 }
                 #endregion
            else
            #region new Boards 
            {
                if (c < 7)
                    if (b.field[r - 1, c] == BoardField.EMPTY)
                    {
                        b.field[r, c] = BoardField.EMPTY;
                        b.field[r - 1, c] = BoardField.WHITE;
                        Coord t = new Coord(r, c);
                        moves.Add(new Move() { new_board = b, new_pos = t });
                    }
                if (c > 0)
                    if (b.field[r - 1, c + 1] == BoardField.EMPTY && c < 7)
                    {
                        b.field[r, c] = BoardField.EMPTY;
                        b.field[r - 1, c + 1] = BoardField.WHITE;
                        Coord t = new Coord(r, c);
                        moves.Add(new Move() { new_board = b, new_pos = t });
                    }
                    else throw new ArgumentException();
            }
            #endregion

            return moves;


            //var moves = new List<Move>();
            //moves.Add(new Move() { new_board=b, new_pos=... })
        }

    }
}
