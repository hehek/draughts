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
        public AI(ICost cost)
        {
            _cost = cost;
        }

        public Move? BestMove(Player player, Board board, uint depth)
        {
            Move? best_move = null;
            double max_v = double.NegativeInfinity;

            List<Move> moves = board.GetAllMoves(player);
            foreach (var move in moves)
            {
                var s = move.new_board;
                double v = MinValue(player, s, double.PositiveInfinity, max_v, depth);
                if (v > max_v)
                {
                    max_v = v;
                    best_move = move;
                }
            }
            return best_move;
        }

        double MaxValue(Player player, Board board, double alpha, double beta, uint depth)
        {
            depth--;
            List<Move> moves = board.GetAllMoves(player);

            if (moves.Count == 0 || depth == 0)
                return _cost.Cost(board, player);
            double v = double.NegativeInfinity;

            foreach (var move in moves)
            {
                var s = move.new_board;
                v = Math.Max(v, MinValue(player, s, alpha, beta, depth));
                if (v >= beta)
                    return v;
                alpha = Math.Max(alpha, v);
            }
            return v;
        }

        double MinValue(Player player, Board board, double alpha, double beta, uint depth)
        {
            depth--;
            List<Move> moves = board.GetAllMoves(player);

            if (moves.Count == 0 || depth == 0)
                return _cost.Cost(board, player);
            double v = double.PositiveInfinity;

            foreach (var move in moves)
            {
                var s = move.new_board;
                v = Math.Min(v, MaxValue(player, s, alpha, beta, depth));
                if (v <= alpha)
                    return v;
                beta = Math.Min(beta, v);
            }
            return v;
        }
    }
}
