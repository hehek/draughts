using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Draughts;
using System.Diagnostics;

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


        [TestMethod]
        public void TestGetMovesManWithCapture()
        {
            List<Move> moves;
            List<Coord> new_coords, exp_coords;
            Board board;
            PrivateObject pboard;


            board = Board.Init("d4", "", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("d4")) as List<Move>;
            Assert.AreEqual(0, moves.Count);

            board = Board.Init("a1", "", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("a1")) as List<Move>;
            Assert.AreEqual(0, moves.Count);

            board = Board.Init("a7", "", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("a7")) as List<Move>;
            Assert.AreEqual(0, moves.Count);

            board = Board.Init("d4 c5", "", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("d4")) as List<Move>;
            Assert.AreEqual(0, moves.Count);

            board = Board.Init("", "b2", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("b2")) as List<Move>;
            Assert.AreEqual(0, moves.Count);

            board = Board.Init("d4", "c5", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("d4")) as List<Move>;
            exp_coords = new List<Coord>() { new Coord("b6") };
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);
            Assert.AreEqual(BoardField.WHITE, moves[0].new_board[5, 1]);
            Assert.AreEqual(BoardField.EMPTY, moves[0].new_board[4, 2]);

            board = Board.Init("e3", "f2", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("e3")) as List<Move>;
            exp_coords = new List<Coord>() { new Coord("g1") };
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);
            Assert.AreEqual(BoardField.WHITE, moves[0].new_board[0, 6]);
            Assert.AreEqual(BoardField.EMPTY, moves[0].new_board[1, 5]);

            board = Board.Init("d2 f2", "e3", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("e3")) as List<Move>;
            exp_coords = new List<Coord>() { new Coord("g1"), new Coord("c1") };
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("d2", "e3", "", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesManWithCapture", new Coord("e3")) as List<Move>;
            Assert.AreEqual(BoardField.BLACK_KING, moves[0].new_board[0, 2]);
            Assert.AreEqual(BoardField.EMPTY, moves[0].new_board[1, 3]);
        }

        [TestMethod]
        public void TestGetMovesKingWithoutCapture()
        {
            List<Move> moves;
            List<Coord> new_coords, exp_coords;
            Board board;
            PrivateObject pboard;
            string exp_moves;

            board = Board.Init("", "", "g7", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesKingWithoutCapture", new Coord("g7")) as List<Move>;
            exp_moves = "h8 f6 e5 d4 c3 b2 a1 h6 f8";
            exp_coords = (from c in exp_moves.Split(' ') select new Coord(c)).ToList();
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("e5", "", "h8", "");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesKingWithoutCapture", new Coord("h8")) as List<Move>;
            exp_moves = "f6 g7";
            exp_coords = (from c in exp_moves.Split(' ') select new Coord(c)).ToList();
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("b6", "e5", "", "d4");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesKingWithoutCapture", new Coord("d4")) as List<Move>;
            exp_moves = "a1 b2 c3 c5 e3 f2 g1";
            exp_coords = (from c in exp_moves.Split(' ') select new Coord(c)).ToList();
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("b2", "", "", "a1");
            pboard = new PrivateObject(board);
            moves = pboard.Invoke("GetMovesKingWithoutCapture", new Coord("a1")) as List<Move>;
            Assert.AreEqual(0, moves.Count);
        }

        [TestMethod]
        public void TestGetMovesKingWithCapture()
        {
            List<Move> moves;
            List<Coord> new_coords, exp_coords;
            Board board;
            PrivateObject pboard;
            string exp_moves;

            board = Board.Init("", "f6", "c3", "");
            pboard = new PrivateObject(board);
            moves = (pboard.Invoke("GetMovesKingWithCapture", new Coord("c3")) as List<List<Move>>).SelectMany(x => x).ToList();
            exp_moves = "g7 h8";
            exp_coords = (from c in exp_moves.Split(' ') select new Coord(c)).ToList();
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("", "f6 d2", "c3", "");
            pboard = new PrivateObject(board);
            moves = (pboard.Invoke("GetMovesKingWithCapture", new Coord("c3")) as List<List<Move>>).SelectMany(x => x).ToList();
            exp_moves = "g7 h8 e1";
            exp_coords = (from c in exp_moves.Split(' ') select new Coord(c)).ToList();
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("", "h8 d2", "c3", "");
            pboard = new PrivateObject(board);
            moves = (pboard.Invoke("GetMovesKingWithCapture", new Coord("c3")) as List<List<Move>>).SelectMany(x => x).ToList();
            exp_moves = "e1";
            exp_coords = (from c in exp_moves.Split(' ') select new Coord(c)).ToList();
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);

            board = Board.Init("", "", "c3", "");
            pboard = new PrivateObject(board);
            moves = (pboard.Invoke("GetMovesKingWithCapture", new Coord("c3")) as List<List<Move>>).SelectMany(x => x).ToList();
            Assert.AreEqual(0, moves.Count);

            board = Board.Init("", "b2", "c1", "d2");
            pboard = new PrivateObject(board);
            moves = (pboard.Invoke("GetMovesKingWithCapture", new Coord("c1")) as List<List<Move>>).SelectMany(x => x).ToList();
            exp_moves = "a3 e3 f4 g5 h6";
            exp_coords = (from c in exp_moves.Split(' ') select new Coord(c)).ToList();
            new_coords = (from m in moves select m.new_pos).ToList();
            CollectionAssert.AreEquivalent(exp_coords, new_coords);
        }

        }
    }
}
