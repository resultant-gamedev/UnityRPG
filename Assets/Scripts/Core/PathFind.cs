﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace UnityRPG
{
    public enum PathDirection{
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }

    public class DirectionCost
    {
        public PathDirection direction;
        public int cost;
        public Point point;

        public DirectionCost(PathDirection dir, int cost, Point point)
        {
            this.direction = dir;
            this.cost = cost;
            this.point = point;
        }
    }

    public class PathFind
    {
        public static List<Point> Pathfind(Tile[,] tileArray, int x, int y, int x2, int y2)
        {

            int Width = tileArray.GetLength(1);
            int Height = tileArray.GetLength(0);
            int[,] cost = new int[Width, Height];

            cost[x, y] = 1; //floor type

            List<Point> active = new List<Point>();
            active.Add(new Point(x, y));
            // pathfind
            while (true)
            {
                // get lowest cost point in active list
                Point point = active[0];
                for (int i = 1; i < active.Count; i++)
                {
                    Point p = active[i];
                    if (cost[p.x, p.y] < cost[point.x, point.y])
                        point = p;
                }

                // if end point
                if (point.x == x2 && point.y == y2)
                    break;

                // move in directions
                int currentCost = cost[point.x, point.y];
                if (point.x - 1 >= 0 && cost[point.x - 1, point.y] == 0)
                {
                    active.Add(new Point(point.x - 1, point.y));

                    cost[point.x - 1, point.y] = currentCost + getCost(tileArray[point.x - 1, point.y]);
                }
                if (point.x + 1 < Width && cost[point.x + 1, point.y] == 0)
                {
                    active.Add(new Point(point.x + 1, point.y));
                    cost[point.x + 1, point.y] = currentCost + getCost(tileArray[point.x + 1, point.y]);
                }
                if (point.y - 1 >= 0 && cost[point.x, point.y - 1] == 0)
                {
                    active.Add(new Point(point.x, point.y - 1));
                    cost[point.x, point.y - 1] = currentCost + getCost(tileArray[point.x, point.y - 1]);
                }
                if (point.y + 1 < Height && cost[point.x, point.y + 1] == 0)
                {
                    active.Add(new Point(point.x, point.y + 1));
                    cost[point.x, point.y + 1] = currentCost + getCost(tileArray[point.x, point.y + 1]);
                }

                active.Remove(point);
            }

            // work backwards and find path
            List<Point> points = new List<Point>();
            Point current = new Point(x2, y2);
            points.Add(current);

            while (current.x != x || current.y != y)
            {
                int highest = cost[current.x, current.y];
                int left = highest, right = highest, up = highest, down = highest;

                // get cost of each direction
                if (current.x - 1 >= 0 && cost[current.x - 1, current.y] != 0)
                {
                    left = cost[current.x - 1, current.y];
                }
                if (current.x + 1 < Width && cost[current.x + 1, current.y] != 0)
                {
                    right = cost[current.x + 1, current.y];
                }
                if (current.y - 1 >= 0 && cost[current.x, current.y - 1] != 0)
                {
                    up = cost[current.x, current.y - 1];
                }
                if (current.y + 1 < Height && cost[current.x, current.y + 1] != 0)
                {
                    down = cost[current.x, current.y + 1];
                }

                // move in the lowest direction
                if (left <= GetMin(up, down, right))
                {
                    points.Add(current = new Point(current.x - 1, current.y));
                }
                else if (right <= GetMin(left, down, up))
                {
                    points.Add(current = new Point(current.x + 1, current.y));
                }
                else if (up <= GetMin(left, right, down))
                {
                    points.Add(current = new Point(current.x, current.y - 1));
                }
                else
                {
                    points.Add(current = new Point(current.x, current.y + 1));
                }
            }

            points.Reverse();

            return points;
        }

        public static List<Point> PathfindDiagonal(Tile[,] tileArray, int x, int y, int x2, int y2)
        {

            int Width = tileArray.GetLength(1);
            int Height = tileArray.GetLength(0);
            int[,] cost = new int[Width, Height];

            cost[x, y] = 1; //floor type

            List<Point> active = new List<Point>();
            active.Add(new Point(x, y));
            // pathfind
            while (true)
            {
                // get lowest cost point in active list
                Point point = active[0];
                for (int i = 1; i < active.Count; i++)
                {
                    Point p = active[i];
                    if (cost[p.x, p.y] < cost[point.x, point.y])
                        point = p;
                }

                // if end point
                if (point.x == x2 && point.y == y2)
                    break;

                // move in directions
                int currentCost = cost[point.x, point.y];
                //left
                if (point.x - 1 >= 0 && cost[point.x - 1, point.y] == 0)
                {
                    active.Add(new Point(point.x - 1, point.y));

                    cost[point.x - 1, point.y] = currentCost + getCost(tileArray[point.x - 1, point.y]);
                }
                //right
                if (point.x + 1 < Width && cost[point.x + 1, point.y] == 0)
                {
                    active.Add(new Point(point.x + 1, point.y));
                    cost[point.x + 1, point.y] = currentCost + getCost(tileArray[point.x + 1, point.y]);
                }
                //up
                if (point.y - 1 >= 0 && cost[point.x, point.y - 1] == 0)
                {
                    active.Add(new Point(point.x, point.y - 1));
                    cost[point.x, point.y - 1] = currentCost + getCost(tileArray[point.x, point.y - 1]);
                }
                //down
                if (point.y + 1 < Height && cost[point.x, point.y + 1] == 0)
                {
                    active.Add(new Point(point.x, point.y + 1));
                    cost[point.x, point.y + 1] = currentCost + getCost(tileArray[point.x, point.y + 1]);
                }
                //upleft
                if (point.x - 1 >= 0 && point.y-1 >=0 && cost[point.x - 1, point.y-1] == 0)
                {
                    active.Add(new Point(point.x - 1, point.y-1));

                    cost[point.x - 1, point.y-1] = currentCost + getCost(tileArray[point.x - 1, point.y-1]);
                }
                //upright
                if (point.x + 1 < Width && point.y - 1 >= 0 && cost[point.x + 1, point.y - 1] == 0)
                {
                    active.Add(new Point(point.x + 1, point.y - 1));

                    cost[point.x + 1, point.y - 1] = currentCost + getCost(tileArray[point.x + 1, point.y - 1]);
                }
                //downleft
                if (point.x - 1 >= 0 && point.y + 1 < Height && cost[point.x - 1, point.y + 1] == 0)
                {
                    active.Add(new Point(point.x - 1, point.y + 1));

                    cost[point.x - 1, point.y + 1] = currentCost + getCost(tileArray[point.x - 1, point.y + 1]);
                }
                //downright
                if (point.x + 1 < Width && point.y + 1 < Height && cost[point.x + 1, point.y + 1] == 0)
                {
                    active.Add(new Point(point.x + 1, point.y + 1));

                    cost[point.x + 1, point.y + 1] = currentCost + getCost(tileArray[point.x + 1, point.y + 1]);
                }

                active.Remove(point);
            }

            // work backwards and find path
            List<Point> points = new List<Point>();
            Point current = new Point(x2, y2);
            points.Add(current);

            while (current.x != x || current.y != y)
            {
                int highest = cost[current.x, current.y];
                List<DirectionCost> directionCostList = new List<DirectionCost>();
                int left = highest, right = highest, up = highest, down = highest, upleft=highest,upright=highest,downleft=highest,downright=highest;

                // get cost of each direction
                if (current.x - 1 >= 0 && cost[current.x - 1, current.y] != 0)
                {
                    directionCostList.Add(new DirectionCost( PathDirection.Left, cost[current.x - 1, current.y],new Point(current.x - 1, current.y)));
                    left = cost[current.x - 1, current.y];
                }
                if (current.x + 1 < Width && cost[current.x + 1, current.y] != 0)
                {
                    directionCostList.Add(new DirectionCost(PathDirection.Right, cost[current.x + 1, current.y], new Point(current.x + 1, current.y)));
                    right = cost[current.x + 1, current.y];
                }
                if (current.y - 1 >= 0 && cost[current.x, current.y - 1] != 0)
                {
                    directionCostList.Add(new DirectionCost(PathDirection.Up, cost[current.x, current.y - 1], new Point(current.x, current.y - 1)));
                    up = cost[current.x, current.y - 1];
                }
                if (current.y + 1 < Height && cost[current.x, current.y + 1] != 0)
                {
                    directionCostList.Add(new DirectionCost(PathDirection.Down, cost[current.x, current.y + 1], new Point(current.x, current.y + 1)));
                    down = cost[current.x, current.y + 1];
                }

                if (current.x - 1 >= 0 && current.y - 1 >= 0 && cost[current.x-1, current.y -1] != 0)
                {
                    directionCostList.Add(new DirectionCost(PathDirection.UpLeft, cost[current.x - 1, current.y - 1], new Point(current.x - 1, current.y - 1)));
                    upleft = cost[current.x-1, current.y - 1];
                }
                if (current.x + 1 < Width && current.y - 1 >= 0 && cost[current.x + 1, current.y - 1] != 0)
                {
                    directionCostList.Add(new DirectionCost(PathDirection.UpRight, cost[current.x + 1, current.y - 1], new Point(current.x + 1, current.y - 1)));
                    upright = cost[current.x + 1, current.y - 1];
                }

                if (current.x - 1 >= 0 && current.y + 1 < Height && cost[current.x - 1, current.y + 1] != 0)
                {
                    directionCostList.Add(new DirectionCost(PathDirection.DownLeft, cost[current.x - 1, current.y + 1], new Point(current.x - 1, current.y + 1)));
                    downleft = cost[current.x - 1, current.y + 1];
                }
                if (current.x + 1 < Width && current.y + 1 < Height && cost[current.x + 1, current.y + 1] != 0)
                {
                    directionCostList.Add(new DirectionCost(PathDirection.DownRight, cost[current.x + 1, current.y + 1], new Point(current.x + 1, current.y + 1)));
                    downright = cost[current.x + 1, current.y + 1];
                }

                // move in the lowest direction
                directionCostList = directionCostList.OrderBy(d => d.cost).ToList();

                points.Add(current = directionCostList[0].point);

              /*
                if (left <= GetMin(up, down, right))
                {
                    points.Add(current = new Point(current.x - 1, current.y));
                }
                else if (right <= GetMin(left, down, up))
                {
                    points.Add(current = new Point(current.x + 1, current.y));
                }
                else if (up <= GetMin(left, right, down))
                {
                    points.Add(current = new Point(current.x, current.y - 1));
                }
                else
                {
                    points.Add(current = new Point(current.x, current.y + 1));
                }
               * */
            }

            points.Reverse();

            return points;
        }

        public static List<Point> Pathfind(Board board, int x, int y, int x2, int y2)
        {

            int Width = board.board.GetLength(1);
            int Height = board.board.GetLength(0);
            int[,] cost = new int[Width, Height];

            cost[x, y] = 1; //floor type

            List<Point> active = new List<Point>();
            active.Add(new Point(x, y));
            // pathfind
            while (true)
            {
                // get lowest cost point in active list
                Point point = active[0];
                for (int i = 1; i < active.Count; i++)
                {
                    Point p = active[i];
                    if (cost[p.x, p.y] < cost[point.x, point.y])
                        point = p;
                }

                // if end point
                if (point.x == x2 && point.y == y2)
                    break;

                // move in directions
                int currentCost = cost[point.x, point.y];
                if (point.x - 1 >= 0 && cost[point.x - 1, point.y] == 0)
                {
                    active.Add(new Point(point.x - 1, point.y));

                    cost[point.x - 1, point.y] = currentCost + getCost(board.board[point.x - 1, point.y]);
                }
                if (point.x + 1 < Width && cost[point.x + 1, point.y] == 0)
                {
                    active.Add(new Point(point.x + 1, point.y));
                    cost[point.x + 1, point.y] = currentCost + getCost(board.board[point.x + 1, point.y]);
                }
                if (point.y - 1 >= 0 && cost[point.x, point.y - 1] == 0)
                {
                    active.Add(new Point(point.x, point.y - 1));
                    cost[point.x, point.y - 1] = currentCost + getCost(board.board[point.x, point.y - 1]);
                }
                if (point.y + 1 < Height && cost[point.x, point.y + 1] == 0)
                {
                    active.Add(new Point(point.x, point.y + 1));
                    cost[point.x, point.y + 1] = currentCost + getCost(board.board[point.x, point.y + 1]);
                }

                active.Remove(point);
            }

            // work backwards and find path
            List<Point> points = new List<Point>();
            Point current = new Point(x2, y2);
            points.Add(current);

            while (current.x != x || current.y != y)
            {
                int highest = cost[current.x, current.y];
                int left = highest, right = highest, up = highest, down = highest;

                // get cost of each direction
                if (current.x - 1 >= 0 && cost[current.x - 1, current.y] != 0)
                {
                    left = cost[current.x - 1, current.y];
                }
                if (current.x + 1 < Width && cost[current.x + 1, current.y] != 0)
                {
                    right = cost[current.x + 1, current.y];
                }
                if (current.y - 1 >= 0 && cost[current.x, current.y - 1] != 0)
                {
                    up = cost[current.x, current.y - 1];
                }
                if (current.y + 1 < Height && cost[current.x, current.y + 1] != 0)
                {
                    down = cost[current.x, current.y + 1];
                }

                // move in the lowest direction
                if (left <= GetMin(up, down, right))
                {
                    points.Add(current = new Point(current.x - 1, current.y));
                }
                else if (right <= GetMin(left, down, up))
                {
                    points.Add(current = new Point(current.x + 1, current.y));
                }
                else if (up <= GetMin(left, right, down))
                {
                    points.Add(current = new Point(current.x, current.y - 1));
                }
                else
                {
                    points.Add(current = new Point(current.x, current.y + 1));
                }
            }

            points.Reverse();

            return points;
        }

        private static int getCost(Tile t)
        {
            if (!t.empty)
            {
                return 99;
            }
            else
            {
                return 1;
            }
        }


        private static int GetMin(int val1, int val2, int val3)
        {
            return Math.Min(val1, Math.Min(val2, val3));
        }
   
    
    
    
    }
}
