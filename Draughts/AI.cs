using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{

    interface ICost
    {
        double Cost1(Board b, Player p);
    }

    class AI
    {
        ICost _cost;
        public AI(ICost cost)
        {
            _cost = cost;
        }

    }
}
