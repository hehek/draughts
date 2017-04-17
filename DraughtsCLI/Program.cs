using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Draughts;

namespace DraughtsCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            // На сколько ходов вперёд смотрим
            const uint N = 5;

            // Создаём искусственные интеллекты для белых и чёрных
            var ai_white = new AI(new Cost1(), Player.WHITE);
            var ai_black = new AI(new Cost1(), Player.BLACK);

            // На доске начальная позиция
            Board board = Board.Init();

            // Ход или null
            Move? m;

            while (true)
            {
                // Белые выполняют ход
                Console.WriteLine(board);
                Console.WriteLine("Ход белых");

                m = ai_white.BestMove(board, N);
                if (m == null)
                {
                    Console.WriteLine("Белые проиграли!");
                    return;
                }
                board = m?.new_board;   // Заменяем доску на новую

                // Теперь очередь чёрных
                Console.WriteLine(board);
                Console.WriteLine("Ход чёрных");

                m = ai_black.BestMove(board, N);
                if (m == null)
                {
                    Console.WriteLine("Чёрные проиграли!");
                    return;
                }
                board = m?.new_board;   // Заменяем доску на новую
            }
        }
    }
}
