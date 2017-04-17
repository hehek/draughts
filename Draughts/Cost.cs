using System;



namespace Draughts
{
    public class Cost1 : ICost
    {
        public double Cost(Board b, Player p)
        {
            int num, king, end, s, k; BoardField t;
            num = 0; king = 0; end = 0;
            if (p == Player.BLACK)
            {
                t = BoardField.BLACK_KING;
                s = 0;
                k = 1;
            }
            else
            {
                t = BoardField.WHITE_KING;
                s = 8;
                k = -1;
            }

            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 4; c++)
                {
                    if (b.Owner(r, c) == p)
                        num++;

                    if (b[r, c] == t)
                        king++;
                    end = end + s + k * r;
                }
            return (num * 2 + king * 10) * num / end;
        }

      
    }

    public class Cost2 : ICost
    {

        public double Cost(Board b, Player p)
        {
            int num, king, end, oppnum, oppking, oppend, s, k; BoardField t, opt;
            num = 0; king = 0; end = 0; oppnum = 0; oppking = 0; oppend = 0;
            if (p == Player.BLACK)
            {
                t = BoardField.BLACK_KING;
                opt = BoardField.WHITE_KING;
                s = 0;
                k = 1;
            }
            else
            {
                t = BoardField.WHITE_KING;
                opt = BoardField.BLACK_KING;
                s = 8;
                k = -1;
            }

            for (int r = 0; r < 8; r++)
                for (int c = 0; c < 4; c++)
                {
                    if (b.Owner(r, c) == p)
                    {
                        num++;
                        end = end + s + k * r;
                    }
                    else if (b[r,c] != BoardField.EMPTY)
                    {
                        oppnum++;
                        s =Math.Abs( s - 8);
                        oppend = oppend + s + k * r * -1;
                    }
                   
                    if (b[r, c] == t)
                        king++;
                    if (b[r, c] == opt)
                        oppking++;
                    
                }
            end = end / num;
            oppend = oppend / oppnum;
            return ((num - oppnum)*2 + (king - oppking)*10) * oppend / end;
        }
    }
}

