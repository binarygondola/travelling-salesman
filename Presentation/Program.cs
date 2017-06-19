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

        Graph G;
        Stack s;
        Text text;
        Font font;

        RenderWindow window;

        public void StartSFMLProgram()
        {
            ContextSettings contextSettings = new ContextSettings();
            contextSettings.DepthBits = 24;
            window = new RenderWindow(new VideoMode(800, 600), "SFML window", Styles.Default, contextSettings);

            window.SetVisible(true);
            window.SetFramerateLimit(60);

            window.Closed += OnClosed;
            window.MouseMoved += OnMouseMove;
            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMouseClick;

            window.SetActive();

            try
            {
                font = new Font("FPS.otf");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            text = new Text("Wybrany:", font);
            text.Color = Color.White;

            G = new Graph(7, 15, 204);

            Matrix m = new Matrix(G.matrix);

            window.DispatchEvents();
            window.Clear();
            window.Draw(G);
            window.Draw(text);
            window.Display();

            s = new Stack(new Triple(G));

            Console.WriteLine(new Triple(G));

            G.ColorPath(s.Traverse(window, G, text));

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                window.Draw(G);
                window.Draw(text);
                window.Display();
            }
        }

        static bool isInside(double x, double y, SFML.System.Vector2f v2f)
        {
            double distance = Math.Pow(Math.Pow(v2f.X - x, 2) + Math.Pow(v2f.Y - y, 2), 0.5);
            if (distance > 16) return false;
            return true;
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
                    Stack s = new Stack(new Triple(G));
                    G.ColorPath(s.Traverse(window, G, text));
                    break;
                case Keyboard.Key.F6:
                    G = new Graph(15, 50);
                    Console.Clear();
                    Console.WriteLine(G);
                    s = new Stack(new Triple(G));
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
                if (isInside(e.X, e.Y, G.Vertices[i].Position))
                {
                    G.Vertices[i].OutlineColor = Color.White;
                    G.ColorEdges(G.Vertices[i].Position, Color.White);
                    text = new Text("Wybrany: " + i, font);
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
                    if (isInside(e.X, e.Y, G.Vertices[i].Position))
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
