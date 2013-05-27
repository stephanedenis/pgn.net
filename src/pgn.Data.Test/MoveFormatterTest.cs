﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ilf.pgn.Data.Format.Test
{
    [TestClass]
    public class MoveFormatterTest
    {
        [TestMethod]
        public void default_constructor_should_exist()
        {
            new MoveFormatter();
        }

        [TestMethod]
        public void Format_should_format_castling_moves()
        {
            var sut = new MoveFormatter();
            Assert.AreEqual("0-0", sut.Format(new Move { Type = MoveType.CastleKingSide }).ToString());
            Assert.AreEqual("0-0-0", sut.Format(new Move { Type = MoveType.CastleQueenSide }).ToString());
        }

        [TestMethod]
        public void Format_should_format_simple_target_only_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = Piece.Rook
            };

            Assert.AreEqual("Rc5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_simple_pawn_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5)
            };

            Assert.AreEqual("c5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_simple_pawn_move_with_explict_pawn()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = Piece.Pawn
            };

            Assert.AreEqual("c5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_origin_to_target_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = Piece.Knight,
                OriginSquare = new Square(File.B, 7)
            };

            Assert.AreEqual("Nb7c5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_origin_file_to_target_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = Piece.Knight,
                OriginFile = File.B
            };

            Assert.AreEqual("Nbc5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_origin_rank_to_target_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Simple,
                TargetSquare = new Square(File.C, 5),
                Piece = Piece.Knight,
                OriginRank = 7
            };

            Assert.AreEqual("N7c5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_a_capturing_move()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                TargetSquare = new Square(File.C, 5),
                Piece = Piece.Knight,
                TargetPiece = Piece.Bishop
            };

            Assert.AreEqual("NxBc5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_a_capturing_move_with_origin_square()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = Piece.Knight,
                OriginSquare = new Square(File.B, 7),
                TargetPiece = Piece.Bishop,
                TargetSquare = new Square(File.C, 5)
            };

            Assert.AreEqual("Nb7xBc5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_Nb7xc5()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = Piece.Knight,
                OriginSquare = new Square(File.B, 7),
                TargetPiece = Piece.Pawn,
                TargetSquare = new Square(File.C, 5)
            };

            Assert.AreEqual("Nb7xc5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_exd5()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.Capture,
                Piece = Piece.Pawn,
                OriginFile = File.E,
                TargetPiece = Piece.Pawn,
                TargetSquare = new Square(File.D, 5)
            };

            Assert.AreEqual("exd5", sut.Format(move).ToString());
        }

        [TestMethod]
        public void Format_should_format_e4xd5ep()
        {
            var sut = new MoveFormatter();
            var move = new Move
            {
                Type = MoveType.CaptureEnPassant,
                Piece = Piece.Pawn,
                OriginSquare = new Square(File.E, 4),
                TargetPiece = Piece.Pawn,
                TargetSquare = new Square(File.D, 5)
            };

            Assert.AreEqual("e4xd5e.p.", sut.Format(move).ToString());
        }
    }
}
