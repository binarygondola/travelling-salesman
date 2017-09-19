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

        public bool stop { get; set; }

        public Stack(Triple t)
        {
            stack = new Stack<Triple>();
            stack.Push(t);
            ret = new List<Tuple<int, int>>();
        }

        //Needs implementation
        public bool isCoherent()
        {
            if (stack != null)
            {
                if (stack.Count > 0)
                {
                    Triple t = stack.Peek();
                    bool[] tmp = new bool[t.edges.Count];
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        tmp[i] = false;
                    }
                    for (int i = 0; i < t.edges.Count; i++)
                    {

                    }
                }
            }

            return false;
        }

        public List<Tuple<int, int>> Traverse(RenderWindow w, Graph G)
        {
            int counter = 0,
                substract = 500,
                stackMax = 0,
                wrong = 0;
            double max = 0;
            stop = false;

            Triple tripleFromStack;
            while (stack.Count > 0)
            {
                if (stop)
                    break;

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
                    w.Display();
                    counter -= substract;
                    //Console.WriteLine(stack.Count);
                }
                //TODO smth needs to be done with that mess
                if (tripleFromStack.matrix.matrix.Count == 3)
                {

                    tripleFromStack.distance += tripleFromStack.matrix.Reduce();
                    tripleFromStack.BlockCycles();


                    Tuple<int, int> edge = tripleFromStack.matrix.FindEdge(out max);
                    if (edge.Item1 == 0 && edge.Item2 == 0)
                    {
                        wrong++;
                        continue;
                    }

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
                    //TODO track this down
                    else {
                        wrong++;
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
                    if (tripleFromStack.distance + distanceFromStack + max < min)
                    {
                        Triple Right = new Triple(tripleFromStack);
                        Right.distance += distanceFromStack;
                        Right.distance += max;
                        Right.matrix.BlockEdge(edge);                                           //BLOCKEDGE


                        // You should take the left one first from the top of the stack, so it is in that order
                        stack.Push(Right);
                    }
                    stack.Push(Left);
                }
            }

            Console.WriteLine(this);
            Console.WriteLine("Wrong: " + wrong);
            
            return ret;
        }

        public override string ToString()
        {
            if (ret.Count == 0) return "Solution not found";
            string str = "";
            str+= "Distance = " + min.ToString("000")+"\r\n";
            str+= "Path: " + ret[0].Item1 + " -> " + ret[0].Item2;
            int next = ret[0].Item2;

            for (int i = 0; i < ret.Count; i++)
            {
                if (next == ret[i].Item1)
                {
                    str+=" -> " + ret[i].Item2;
                    next = ret[i].Item2;
                    i = -1;
                }
                if (next == ret[0].Item1) break;
            }
            return str;
        }
    }
}
