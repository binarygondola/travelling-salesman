using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    struct Triple
    {
        public Matrix matrix;
        public double distance;
        public List<Tuple<int, int>> edges;

        public Triple(Graph G)
        {
            matrix = new Matrix(G.matrix);
            distance = 0;
            edges = new List<Tuple<int, int>>();
        }

        #region ctors
        public Triple(Graph G, double distance, List<Tuple<int, int>> list)
        {
            matrix = new Matrix(G.matrix);
            this.distance = distance;
            edges = new List<Tuple<int, int>>(list);
        }

        public Triple(Matrix M, double distance, List<Tuple<int, int>> list)
        {
            matrix = new Matrix(M.matrix);
            this.distance = distance;
            edges = new List<Tuple<int, int>>(list);
        }

        public Triple(List<List<double>> lista, double distance, List<Tuple<int, int>> list)
        {
            matrix = new Matrix(lista);
            this.distance = distance;
            edges = new List<Tuple<int, int>>(list);
        }

        public Triple(Triple t)
        {
            matrix = new Matrix(t.matrix.matrix);
            distance = t.distance;
            edges = new List<Tuple<int, int>>(t.edges);
        }

        #endregion

        public override string ToString()
        {
            string s = "";
            s += "Distance = " + distance + "\r\n";
            s += "Edges = {" + string.Join(", ", edges) + "}\r\n";
            s += matrix.ToString();
            return s;
        }

        public void BlockCycles()
        {
            List<Tuple<int, int>> tmp = new List<Tuple<int, int>>(edges);

            int x = 0;
            int y = 0;
            while (tmp.Count > 0)
            {
                x = tmp[0].Item1;
                y = tmp[0].Item2;
                tmp.RemoveAt(0);
                for (int i = 0; i < tmp.Count; i++)
                {
                    if (y == tmp[i].Item1)
                    {
                        y = tmp[i].Item2;
                        tmp.RemoveAt(i);
                        i = -1;
                    }
                    if (tmp.Count == 0) break;
                }
                for (int i = 0; i < tmp.Count; i++)
                {
                    if (x == tmp[i].Item2)
                    {
                        x = tmp[i].Item1;
                        tmp.RemoveAt(i);
                        i = -1;
                    }
                    if (tmp.Count == 0) break;
                }
                matrix.BlockEdge(new Tuple<int, int>(y, x));
            }
        }
    }

    class Stack
    {
        Stack<Triple> stack;
        double min = double.MaxValue;
        List<Tuple<int, int>> ret;

        public Stack(Triple t)
        {
            stack = new Stack<Triple>();
            stack.Push(t);
            ret = new List<Tuple<int, int>>();
        }

        public List<Tuple<int, int>> Traverse(RenderWindow w, Graph G, Text t)
        {
            int counter = 0,
                substract = 3000,
                stackMax = 0;
            double max = 0;
            Triple tripleFromStack;
            while (stack.Count > 0)
            {
                counter++;
                stackMax = stack.Count > stackMax ? stack.Count : stackMax;

                tripleFromStack = stack.Pop();
                if (tripleFromStack.distance > min)
                {
                    continue;
                }

                if (counter > substract)
                {
                    G.ColorPath(tripleFromStack.edges);
                    w.DispatchEvents();
                    w.Clear();
                    w.Draw(G);
                    w.Draw(t);
                    w.Display();
                    counter -= substract;
                    Console.WriteLine(stack.Count);
                }

                //TODO coś z tym trzeba zrobić
                if (tripleFromStack.matrix.matrix.Count == 3)
                {
                    tripleFromStack.distance += tripleFromStack.matrix.Reduce();
                    tripleFromStack.BlockCycles();
                    if (tripleFromStack.matrix.matrix[1][1] == 0 && tripleFromStack.matrix.matrix[2][2] == 0 && tripleFromStack.matrix.matrix[0][1] != tripleFromStack.matrix.matrix[1][0] && tripleFromStack.matrix.matrix[0][2] != tripleFromStack.matrix.matrix[2][0])
                    {
                        tripleFromStack.edges.Add(new Tuple<int, int>((int)tripleFromStack.matrix.matrix[1][0], (int)tripleFromStack.matrix.matrix[0][1]));
                        tripleFromStack.edges.Add(new Tuple<int, int>((int)tripleFromStack.matrix.matrix[2][0], (int)tripleFromStack.matrix.matrix[0][2]));


                        if (tripleFromStack.distance < min)
                        {
                            min = tripleFromStack.distance;
                            ret = new List<Tuple<int, int>>(tripleFromStack.edges);
                            G.ColorPathBest(ret);
                        }
                    }
                    else if (tripleFromStack.matrix.matrix[1][2] == 0 && tripleFromStack.matrix.matrix[2][1] == 0 && tripleFromStack.matrix.matrix[0][2] != tripleFromStack.matrix.matrix[1][0] && tripleFromStack.matrix.matrix[0][2] != tripleFromStack.matrix.matrix[1][0])
                    {
                        tripleFromStack.edges.Add(new Tuple<int, int>((int)tripleFromStack.matrix.matrix[1][0], (int)tripleFromStack.matrix.matrix[0][2]));
                        tripleFromStack.edges.Add(new Tuple<int, int>((int)tripleFromStack.matrix.matrix[2][0], (int)tripleFromStack.matrix.matrix[0][1]));


                        if (tripleFromStack.distance < min)
                        {
                            min = tripleFromStack.distance;
                            ret = new List<Tuple<int, int>>(tripleFromStack.edges);
                            G.ColorPathBest(ret);
                        }
                    }
                }
                else
                {
                    max = 0;
                    double distanceFromStack = tripleFromStack.matrix.Reduce();             //REDUCE
                    Tuple<int, int> edge = tripleFromStack.matrix.FindEdge(out max);        //FINDEDGE

                    if (edge.Item1 == 0 && edge.Item2 == 0)
                    {
                        continue;
                    }

                    List<Tuple<int, int>> list = new List<Tuple<int, int>>(tripleFromStack.edges);

                    //left
                    Triple Left = new Triple(tripleFromStack);
                    Left.distance += distanceFromStack;
                    Left.matrix.Erase(edge);                                                //ERASE
                    Left.edges.Add(edge);
                    Left.BlockCycles();                                                     //BLOCKCYCLES

                    //right
                    if (tripleFromStack.distance + max < min)
                    {
                        Triple Right = new Triple(tripleFromStack);
                        Right.distance += distanceFromStack;
                        Right.distance += max;
                        Right.matrix.BlockEdge(edge);                                           //BLOCKEDGE


                        // należy zachować taką kolejność ponieważ to kolejka LIFO
                        stack.Push(Right);
                    }
                    stack.Push(Left);
                }
            }
            Console.WriteLine("Odległość = " + min.ToString("000"));
            Console.WriteLine();
            Console.WriteLine("Ścieżka  = {" + string.Join(",", ret) + "}");

            return ret;
        }
    }
}
