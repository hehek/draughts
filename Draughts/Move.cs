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

        public Coord(string pos)
        {
            c = pos[0] - 'a';
            r = pos[1] - '1';
        }

        public override string ToString()
        {
            return $"{c + 'a'}{r + '1'}";
        }
    }

    public struct Move
    {
        public Coord new_pos;
        public Board new_board;
    }
}
