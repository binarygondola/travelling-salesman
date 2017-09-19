using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    class Matrix
    {
        int inf = -1;
        public List<List<double>> matrix;

        // copy m to matrix 0th row and column are the names of vertex
        public Matrix(List<List<double>> m)
        {
            matrix = new List<List<double>>();
            if (m[0][0] == 0)
            {
                for (int i = 0; i < m.Count; i++)
                {
                    matrix.Add(new List<double>());
                    for (int j = 0; j < m[i].Count; j++)
                        matrix[i].Add(m[i][j]);
                }
            }
            else
            {
                matrix.Add(new List<double>());
                matrix[0].Add(0);
                for (int i = 0; i < m.Count; i++)
                {
                    matrix[0].Add(i);
                    matrix.Add(new List<double>());
                    matrix[i + 1].Add(i);
                    for (int j = 0; j < m[i].Count; j++)
                        matrix[i + 1].Add(m[i][j]);
                }
            }

        }

        // returns the length of reduced connections (rows and columns reduced by the common minimum value)
        public double Reduce()
        {
            double ret = 0;
            //rows
            for (int i = 1; i < matrix.Count; i++)
            {
                double min = 10000;
                for (int j = 1; j < matrix[i].Count; j++)
                {
                    min = matrix[i][j] == inf ? min : matrix[i][j] < min ? matrix[i][j] : min;
                }
                if (min > 0 && min != 10000)
                {
                    ret += min;
                    for (int j = 1; j < matrix[i].Count; j++)
                    {
                        if (matrix[i][j] == inf) continue;
                        matrix[i][j] -= min;
                    }
                }
            }
            //cols
            for (int i = 1; i < matrix.Count; i++)
            {
                double min = 10000;
                for (int j = 1; j < matrix[i].Count; j++)
                {
                    min = matrix[j][i] == inf ? min : matrix[j][i] < min ? matrix[j][i] : min;
                }
                if (min > 0 && min != 10000)
                {
                    ret += min;
                    for (int j = 1; j < matrix[i].Count; j++)
                    {
                        if (matrix[j][i] == inf) continue;
                        matrix[j][i] -= min;
                    }
                }
            }
            return ret;
        }

        //deletes row connected with t.item1 vertex and column connected with t.item2 vertex
        public void Erase(Tuple<int, int> t)
        {
            Tuple<int, int> tmp = FindRowAndColumn(t);
            int xth = tmp.Item1;
            int yth = tmp.Item2;

            if (xth == 0 || yth == 0) return;
            for (int i = 0; i < matrix.Count; i++)
                matrix[i].RemoveAt(yth);
            matrix.RemoveAt(xth);
        }

        // distance (u, w) is infinity
        public void BlockEdge(Tuple<int, int> t)
        {
            Tuple<int, int> tmp = FindRowAndColumn(t);
            int xth = tmp.Item1;
            int yth = tmp.Item2;

            if (xth == 0 || yth == 0) return;
            matrix[xth][yth] = inf;
        }

        // returns index in matrix for row t.item1 and column t.item2
        Tuple<int, int> FindRowAndColumn(Tuple<int, int> t)
        {
            int xth = 0;
            int yth = 0;
            for (int i = 1; i < matrix[0].Count; i++)
            {
                if (matrix[0][i] == t.Item2)
                    yth = i;
            }
            for (int i = 1; i < matrix[0].Count; i++)
            {
                if (matrix[i][0] == t.Item1)
                    xth = i;
            }

            return new Tuple<int, int>(xth, yth);
        }

        //gets the edge to erase (i,j) ith row jth column
        public Tuple<int, int> FindEdge(out double x)
        {
            double max = -1,
                    min_r = 0,
                    min_c = 0;
            int ith = 0,
                jth = 0;
            for (int i = 1; i < matrix.Count; i++)
            {
                for (int j = 1; j < matrix[i].Count; j++)
                {
                    if (matrix[i][j] == 0)
                    {
                        min_c = getMinCol(i, j);
                        min_r = getMinRow(i, j);
                        if (min_r + min_c > max)
                        {
                            ith = (int)matrix[i][0];
                            jth = (int)matrix[0][j];
                            max = min_r + min_c;
                        }
                    }
                }
            }
            x = max;
            return new Tuple<int, int>(ith, jth);
        }

        #region auxiliary methods for FindEdge
        double getMinCol(int i, int j)
        {
            List<double> tmp = new List<double>();

            for (int k = 1; k < matrix[0].Count; k++)
                if (matrix[i][k] != inf)
                    tmp.Add(matrix[i][k]);

            return tmp.Min();
        }
        double getMinRow(int i, int j)
        {
            List<double> tmp = new List<double>();

            for (int k = 1; k < matrix[0].Count; k++)
                if (matrix[k][j] != inf)
                    tmp.Add(matrix[k][j]);

            return tmp.Min();
        }
        #endregion

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < matrix.Count; i++)
            {
                str += "{";
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    double x = matrix[i][j];
                    str += x.ToString("000") + (j == matrix[i].Count - 1 ? "" : ", ");
                }
                str += "}\r\n";
            }
            return str;
        }
    }
}
