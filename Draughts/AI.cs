using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{

    public interface ICost
    {
        double Cost(Board b, Player p);
    }

    public class AI
    {
        ICost _cost;
        Player _player;
        uint _depth;

        public AI(ICost cost, uint depth, Player player)
        {
            _cost = cost;
            _player = player;
            _depth = depth;
        }
                

        public Move? BestMove(Board board)
        {
            Move? best_move = null;
            double max_v = double.NegativeInfinity;

            List<Move> moves = board.GetAllMoves(_player);
            foreach (var move in moves)
            {
                var s = move.new_board;
                double v = MinValue(s, double.PositiveInfinity, max_v, _depth);
                if (v > max_v)
                {
                    max_v = v;
                    best_move = move;
                }
            }
            return best_move;
        }

        double MaxValue(Board board, double alpha, double beta, uint depth)
        {
            depth--;
            List<Move> moves = board.GetAllMoves(_player);

            if (moves.Count == 0 || depth == 0)
                return _cost.Cost(board, _player);
            double v = double.NegativeInfinity;

            foreach (var move in moves)
            {
                var s = move.new_board;
                v = Math.Max(v, MinValue(s, alpha, beta, depth));
                if (v >= beta)
                    return v;
                alpha = Math.Max(alpha, v);
            }
            return v;
        }

        double MinValue(Board board, double alpha, double beta, uint depth)
        {
            depth--;
            List<Move> moves = board.GetAllMoves(_player);

            if (moves.Count == 0 || depth == 0)
                return _cost.Cost(board, _player);
            double v = double.PositiveInfinity;

            foreach (var move in moves)
            {
                var s = move.new_board;
                v = Math.Min(v, MaxValue(s, alpha, beta, depth));
                if (v <= alpha)
                    return v;
                beta = Math.Min(beta, v);
            }
            return v;
        }
    }
}
