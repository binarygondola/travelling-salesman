using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;
using System.Diagnostics;
using System.Threading;

namespace Presentation
{
    class MyProgram : SFMLProgram
    {
        RenderWindow window;
        private bool done;
        Stopwatch watch;

        public MyProgram()
        {
            Console.WriteLine("My");
        }

        public void Start()
        {
            Initialize();

            double fps = 60;
            double msForFrame = 1000 / fps;

            double dt = 0.0001;
            double updateTime = msForFrame;

            while (!done)
            {
                window.DispatchEvents();

                while(updateTime > 0)
                {
                    updateTime -= dt;
                    Update(dt);
                }
                updateTime = msForFrame;
                
                window.Clear();
                Render(window);
                window.Display();
            }
        }

        public void Initialize()
        {
            window = new RenderWindow(new VideoMode(500, 600), "Okno", Styles.Close, new ContextSettings() { DepthBits = 24, AntialiasingLevel = 4 });
            done = false;

            window.Closed += OnClosed;
            window.KeyPressed += OnKeyPressed;

            watch = new Stopwatch();
            watch.Start();
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Escape:
                    done = true;
                    break;
                case Keyboard.Key.E:
                    OnEClicked();
                    break;
                case Keyboard.Key.Q:
                    OnQClicked();
                    break;
            }
        }

        private void OnClosed(object sender, EventArgs e)
        {
            done = true;
        }
    }
}
