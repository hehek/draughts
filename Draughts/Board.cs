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
        /// Заполнить указанные клетки
        /// </summary>
        /// <param name="positions">Строка с координатами, разделёнными пробелом</param>
        /// <param name="bf">Тип поля</param>
        void Fill(string positions, BoardField bf)
        {
            if (positions == null || positions == "")
                return;
            var coords = from c in positions.Split(new[] { ' ' }) select new { C = c[0] - 'a', R = int.Parse(c[1].ToString()) - 1 };
            foreach (var pos in coords)
                this[pos.R, pos.C] = bf;
        }


        /// <summary>
        /// Создать начальную позицию
        /// </summary>
        /// <param name="white_men">Координаты белых шашек через пробел</param>
        /// <param name="black_men">Координаты чёрных шашек через пробел</param>
        /// <param name="white_kings">Координаты белых дамок через пробел</param>
        /// <param name="black_kings">Координаты чёрных дамок через пробел</param>
        /// <returns>Доска с шашками на указанных позициях</returns>
        public static Board Init(string white_men, string black_men, string white_kings, string black_kings)
        {
            var board = new Board();

            board.Fill(white_men, BoardField.WHITE);
            board.Fill(black_men, BoardField.BLACK);
            board.Fill(white_kings, BoardField.WHITE_KING);
            board.Fill(black_kings, BoardField.BLACK_KING);

            return board;
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
                    throw new ArgumentException();
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
        /// <param name="p">Шашка</param>
        /// <returns>Список ходов</returns>
        List<Move> GetMovesManWithoutCapture(Coord p)//программные координаты
        {
            if (this[p.r, p.c] != BoardField.BLACK && this[p.r, p.c] != BoardField.WHITE)
                throw new ArgumentException();
            
            var moves = new List<Move>();

            int dir;
            if (this.Owner(p.r, p.c) == Player.WHITE)
                dir = 1;
            else 
                dir = -1;

            if (p.c != 7 && this[p.r + dir, p.c + 1] == BoardField.EMPTY)
            {
                var b = new Board(this);
                b[p.r + dir, p.c + 1] = b[p.r, p.c];
                b[p.r, p.c] = BoardField.EMPTY;
                Coord pos = new Coord(p.r + dir, p.c + 1);
                b.MenToKings();
                moves.Add(new Move() { new_board = b, new_pos = pos });
            }
            if (p.c != 0 && this[p.r + dir, p.c - 1] == BoardField.EMPTY)
            {
                var b = new Board(this);
                b[p.r + dir, p.c - 1] = b[p.r, p.c];
                b[p.r, p.c] = BoardField.EMPTY;
                Coord pos = new Coord(p.r + dir, p.c - 1);
                b.MenToKings();
                moves.Add(new Move() { new_board = b, new_pos = pos });
            }

            return moves;
        }

        /// <summary>
        /// Ходы шашкой со взятием
        /// </summary>
        /// <param name="p">Шашка</param>
        /// <returns>Список ходов</returns>
        List<Move> GetMovesManWithCapture(Coord p)//программные координаты
        {
            if (this[p.r, p.c] != BoardField.BLACK && this[p.r, p.c] != BoardField.WHITE)
                throw new ArgumentException();

            var moves = new List<Move>();
            Player opponent;
            if (Owner(p.r, p.c) == Player.WHITE)
                opponent = Player.BLACK;
            else
                opponent = Player.WHITE;

            foreach (var dir in new[] { -1, 1 })
            {
                if (p.c < 6 && p.r + dir * 2 >= 0 && p.r + dir * 2 <= 7
                    && this[p.r + dir * 2, p.c + 2] == BoardField.EMPTY && this[p.r + dir, p.c + 1] != BoardField.EMPTY
                    && Owner(p.r + dir, p.c + 1) == opponent)
                {
                    var b = new Board(this);
                    b[p.r + dir * 2, p.c + 2] = b[p.r, p.c];
                    b[p.r, p.c] = BoardField.EMPTY;
                    b[p.r + dir, p.c + 1] = BoardField.EMPTY;
                    Coord pos = new Coord(p.r + dir * 2, p.c + 2);
                    b.MenToKings();
                    moves.Add(new Move() { new_board = b, new_pos = pos });
                }
                if (p.c > 1 && p.r + dir * 2 >= 0 && p.r + dir * 2 <= 7
                    && this[p.r + dir * 2, p.c - 2] == BoardField.EMPTY && this[p.r + dir, p.c - 1] != BoardField.EMPTY
                    && Owner(p.r + dir, p.c - 1) == opponent)
                {
                    var b = new Board(this);
                    b[p.r + dir * 2, p.c - 2] = b[p.r, p.c];
                    b[p.r, p.c] = BoardField.EMPTY;
                    b[p.r + dir, p.c - 1] = BoardField.EMPTY;
                    Coord pos = new Coord(p.r + dir * 2, p.c - 2);
                    b.MenToKings();
                    moves.Add(new Move() { new_board = b, new_pos = pos });
                }
            }

            return moves;
        }

        /// <summary>
        /// Ходы дамкой без взятия
        /// </summary>
        /// <param name="p">Шашка</param>
        /// <returns>Список ходов</returns>
        List<Move> GetMovesKingWithoutCapture(Coord p)//программные координаты
        {
            var moves = new List<Move>();
            var r0 = p.r;
            var c0 = p.c;

            if (this[r0, c0] != BoardField.BLACK_KING && this[r0, c0] != BoardField.WHITE_KING)
                throw new ArgumentException();

            for (var dr = -1; dr <= 1; dr += 2)
                for (var dc= -1; dc <=1; dc += 2)
                {
                    int r = r0; int c = c0;
                    while ((r + dr) >= 0 && (r + dr) <= 7
                       && (c + dc) >= 0 && (c + dc) <= 7
                       && this[r + dr, c + dc] == BoardField.EMPTY)
                    {
                        var b = new Board(this);
                        b[r + dr, c + dc] = b[r0, c0];
                        b[r0, c0] = BoardField.EMPTY;
                        Coord pos = new Coord(r + dr, c + dc);
                        moves.Add(new Move() { new_board = b, new_pos = pos });
                        r += dr;
                        c += dc;
                    }
                }
            return moves;
        }
        
        /// <summary>
        /// Ходы дамкой со взятием
        /// </summary>
        /// <param name="p">Шашка</param>
        /// <returns>Список ходов</returns>
        List<Move> GetMovesKingWithCapture(Coord p)//программные координаты
        {
            var moves = new List<Move>();
            var r0 = p.r;
            var c0 = p.c;

            if (this[r0, c0] != BoardField.BLACK_KING && this[r0, c0] != BoardField.WHITE_KING)
                throw new ArgumentException();

            Player opponent;
            if (Owner(p.r, p.c) == Player.WHITE)
                opponent = Player.BLACK;
            else
                opponent = Player.WHITE;

            for (var dr = -1; dr <= 1; dr += 2)
                for (var dc = -1; dc <= 1; dc += 2)
                {
                    int r = r0; int c = c0;
                    while ((r + dr) >= 0 && (r + dr) <= 7
                        && (c + dc) >= 0 && (c + dc) <= 7
                        && this[r + dr, c + dc] == BoardField.EMPTY)
                    {
                        r += dr;
                        c += dc;
                    }

                    r += dr;
                    c += dc;

                    if (!((r + dr) >= 0 && (r + dr) <= 7
                       && (c + dc) >= 0 && (c + dc) <= 7)
                       || Owner(r,c)!=opponent )
                        continue;

                    int ro = r;
                    int co = c;

                    while ((r + dr) >= 0 && (r + dr) <= 7
                       && (c + dc) >= 0 && (c + dc) <= 7
                       && this[r + dr, c + dc] == BoardField.EMPTY)
                    {
                        var b = new Board(this);
                        b[r + dr, c + dc] = b[r0, c0];
                        b[r0, c0] = BoardField.EMPTY;
                        b[ro, co] = BoardField.EMPTY;
                        Coord pos = new Coord(r + dr, c + dc);
                        moves.Add(new Move() { new_board = b, new_pos = pos });
                        r += dr;
                        c += dc;
                    }
                }
            return moves;
        }

        /// <summary>
        /// Поиск дамок
        /// </summary>
        private void MenToKings()
        {
            for (int c = 0; c < 8; c++)
            {
                if (this[7, c] == BoardField.WHITE)
                    this[7, c] = BoardField.WHITE_KING;
                if (this[0, c] == BoardField.BLACK)
                    this[0, c] = BoardField.BLACK_KING;
            }
        }

        /// <summary>
        /// Копирующий конструктор
        /// </summary>
        /// <param name="b">Исходная доска</param>
        private Board(Board b)
        {
            field = b.field.Clone() as BoardField[,];
        }
    }
}
