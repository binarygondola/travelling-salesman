using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    class Program
    {
        static void Main(string[] args)
        {
            MySFMLProgram app = new MySFMLProgram();
            app.StartSFMLProgram();
        }
    }

    class MySFMLProgram
    {
        bool dragging = false;
        int dragged = -1;
        double radius = 16;

        Graph G;
        Stack s;

        RenderWindow window;

        public void StartSFMLProgram()
        {
            ContextSettings contextSettings = new ContextSettings();
            contextSettings.DepthBits = 24;
            window = new RenderWindow(new VideoMode(800, 600), "SFML window", Styles.Close, contextSettings);

            window.SetVisible(true);
            //oh boy, what a performance boost
            window.SetFramerateLimit(300);

            window.Closed += OnClosed;
            window.MouseMoved += OnMouseMove;
            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMouseClick;
            window.TextEntered += OnTextEntered;

            window.SetActive();

            G = new Graph(20, 8000);

            Matrix m = new Matrix(G.matrix);

            window.DispatchEvents();
            window.Clear();
            window.Draw(G);
            window.Display();

            s = new Stack(new Triple(G));

            Console.WriteLine(new Triple(G));

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                window.Draw(G);
                window.Display();
            }
        }

        static bool isInside(double x, double y, SFML.System.Vector2f v2f, double radius)
        {
            double distance = Math.Pow(Math.Pow(v2f.X - x, 2) + Math.Pow(v2f.Y - y, 2), 0.5);
            if (distance > radius) return false;
            return true;
        }


        //TODO needs implementation
        private void OnTextEntered(object sender, TextEventArgs e)
        {
            Console.WriteLine(e.Unicode);
        }

        void OnKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            switch (e.Code)
            {
                case Keyboard.Key.Escape:
                    window.Close();
                    break;
                case Keyboard.Key.F5:
                    G.ColorPath(new List<Tuple<int, int>>());
                    G.ColorPathBest(new List<Tuple<int, int>>());
                    s = new Stack(new Triple(G));
                    G.ColorPath(s.Traverse(window, G));
                    break;
                case Keyboard.Key.F6:
                    s.stop = true;
                    G = new Graph(6, 170);
                    Console.Clear();
                    Console.WriteLine(G);
                    s = new Stack(new Triple(G));
                    break;
                case Keyboard.Key.F7:
                    Console.WriteLine("F7");
                    s.stop = true;
                    break;
                default:
                    break;
            }
        }

        void OnClosed(object sender, EventArgs e)
        {
            window.Close();
        }

        void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            for (int i = 0; i < G.Vertices.Count; i++)
            {
                if (isInside(e.X, e.Y, G.Vertices[i].Position, radius))
                {
                    G.Vertices[i].OutlineColor = Color.White;
                    G.ColorEdges(G.Vertices[i].Position, Color.White);
                }
                else
                {
                    G.Vertices[i].OutlineColor = Color.Magenta;
                    G.ColorEdges(G.Vertices[i].Position, Color.Blue);
                }
            }

            if (dragging)
            {
                G.MoveEdges(G.Vertices[dragged].Position, new SFML.System.Vector2f(e.X, e.Y));
                G.Vertices[dragged].Position = new SFML.System.Vector2f(e.X, e.Y);
            }
        }

        void OnMouseClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                for (int i = 0; i < G.Vertices.Count; i++)
                {
                    if (isInside(e.X, e.Y, G.Vertices[i].Position, radius))
                    {
                        dragging = true;
                        dragged = i;
                    }
                }
            }
            else if (e.Button == Mouse.Button.Right)
            {
                if (dragging)
                {
                    G.Vertices[dragged].Position = new SFML.System.Vector2f(e.X, e.Y);
                    G.ColorEdges(new SFML.System.Vector2f(e.X, e.Y), Color.Blue);
                    dragging = false;
                    dragged = -1;
                    Console.Clear();
                    G.RecalculateDistance();
                    Matrix m = new Matrix(G.matrix);
                    Console.WriteLine(m);
                }
            }
        }
    }
}
