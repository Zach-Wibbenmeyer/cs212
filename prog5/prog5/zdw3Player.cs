/*
 * CS 212 - Data Structures and Algorithms
 * Professor: Harry Plantinga
 * Author: Zach Wibbenmeyer
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using prog5;

namespace prog5
{
    public class zdw3Player : Player
    {

        //creates a stopwatch to keep track of elapsed time
        Stopwatch stop = new Stopwatch(); 

        public zdw3Player(Position pos, int timeLimit) : base(pos, "Zach Wibbenmeyer", timeLimit) { }

        // method that calls a string that taunts
        public override string gloat()
        {
            return "I win! Suck it!";
        }


        /*
         * Evaluate: return a number saying how much we like this board. 
         * TOP is MAX, so positive scores should be better for TOP.
         * This default just counts the score so far. Override to improve!
         */
        public override int evaluate(Board b)
        {
            return 5;
        }

        /*
         * Provide a photo of yourself (or your avatar) for the
         * tournament. You can return either
         * 1. the url of a photo "http://www.example.com/photo.jpg"
         * 2. the filename of a photo "photo.jpg"
         */
        public override String getImage() { return "BobaFett.jpg"; }

        /*
         * chooseMove()
         * @param: b (type -> Board)
         */
        public override int chooseMove(Board b)
        {
            // starts the stopwatch
            stop.Start();

            Result result = new Result(0, -10000000);
            Result topResult = new Result(0, 10000000);

            // if this is within the time limit
            if (stop.ElapsedMilliseconds < getTimePerMove()) 
            {
                //staged DFS
                for (var i = 1; i < 6; i++) 
                {
                    //calls minimax to choose the best move
                    topResult = MiniMax(b, i);
                    //compare the scores
                    if (topResult.GetScore() > result.GetScore()) 
                    {
                        result = topResult;
                    }
                }
                stop.Stop();
                stop.Reset(); 
                return result.GetMove();
            }

            return 0;
        }

        /*
         * MiniMax()
         * @param: b (type -> Board), depth (type -> int)
         */
        public Result MiniMax(Board b, int depth)
        {
            int bestValue = 0;
            int bestMove = 0;


            //if the depth is 0 or the game is over
            if (depth == 0 || b.gameOver()) 
            {
                Result r = new Result(0, evaluate(b));
                return r;
            }

            //when top player is the one who can move
            if (b.whoseMove() == Position.Top) 
            {
                bestValue = -1000000000;
                for (var i = 7; i <= 12; i++)
                {
                    // execute if this is a legal move
                    if (!b.legalMove(i)) continue;
                    Board b1 = new Board(b);
                    b1.makeMove(i, false);
                    Result result1 = MiniMax(b1, depth - 1);
                    if (result1.score <= bestValue) continue;
                    bestValue = result1.score;
                    bestMove = i;
                }
            }

            // when bottom player is the one who can move
            if (b.whoseMove() == Position.Bottom) 
            {
                bestValue = -1000000000;
                for (var i = 0; i <= 5; i++)
                {
                    // do this if it is a legal move
                    if (b.legalMove(i))
                    {
                        Board b1 = new Board(b);
                        b1.makeMove(i, false);
                        Result result1 = MiniMax(b1, depth - 1);
                        if (result1.score > bestValue)
                        {
                            //sets value as the best value
                            bestValue = result1.score;
                            //sets move as the best move
                            bestMove = i; 
                        }
                    }
                }
            }

            return new Result(bestMove, bestValue);
        }
    }

    
    public class Result
    {
        public int move;
        public int score;

        public Result()
        {
            move = 0;
            score = 0;
        }

        public Result(int a, int b)
        {
            move = a;
            score = b;
        }

        public int GetMove()
        {
            return move;
        }

        public int GetScore()
        {
            return score;
        }

        public void SetScore(int a)
        {
            score = a;
        }

        public void SetMove(int a)
        {
            move = a;
        }
    }
}



