using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Draughts;

namespace TestDraughts
{
    [TestClass]
    public class UnitTestBoard
    {
        [TestMethod]
        public void TestGetMovesManWithoutCapture()
        {
            List<Move> moves;
            List<Coord> new_coords, exp_coords;
            Board board;
            PrivateObject pboard;


            board = Board.Init("d4", "", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithoutCapture", new Coord("d4")) as List<Move>;
            exp_coords = new List<Coord>() { new Coord("c5"), new Coord("e5") };
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("", "d4", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithoutCapture", new Coord("d4")) as List<Move>;
            exp_coords = new List<Coord>() { new Coord("c3"), new Coord("e3") };
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("", "d4 c3", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithoutCapture", new Coord("d4")) as List<Move>;
            exp_coords = new List<Coord>() { new Coord("e3") };
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("d4", "c5", "e5", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithoutCapture", new Coord("d4")) as List<Move>;
            Assert.AreEqual(0, moves.Count);

            board = Board.Init("a1", "", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithoutCapture", new Coord("a1")) as List<Move>;
            exp_coords = new List<Coord>() { new Coord("b2") };
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);
            Assert.AreEqual(BoardField.WHITE, moves[0].new_board[1, 1]);

            board = Board.Init("a7", "", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithoutCapture", new Coord("a7")) as List<Move>;
            exp_coords = new List<Coord>() { new Coord("b8") };
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);
            Assert.AreEqual(BoardField.WHITE_KING, moves[0].new_board[7, 1]);
        }
    }
}
