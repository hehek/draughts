using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draughts
{
    public struct Coord
    {
        public int r, c;

        public Coord(int r, int c)
        {
            this.r = r;
            this.c = c;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }

    public struct Move
    {
        public Coord new_pos;
        public Board new_board;
    }
}
