﻿using System;
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

        public MyProgram()
        {
            Console.WriteLine("My");
        }

        public void Start()
        {
            Initialize();

            Stopwatch watch = new Stopwatch();
            watch.Start();

            double fps = 60;
            double msForFrame = 1000 / fps;

            double lag = 0;
            double dt = 0.001;

            double currentTime = watch.ElapsedMilliseconds;
            
            double realTime = 0;
            double frameTime = 0;

            int sleepTime = 0;
            int wtf = 0;
            int countFrames = 0;

            while (!done)
            {
                countFrames++;
                window.DispatchEvents();

                realTime = watch.ElapsedMilliseconds;

                //or this
                frameTime = realTime - currentTime;
                currentTime = realTime;

                //not sure about this
                if (frameTime > msForFrame)
                {
                    Console.Write(frameTime + " ");
                    frameTime = msForFrame;
                    wtf++;
                }

                lag += frameTime;

                while (lag > dt)
                {
                    Update(dt);
                    lag -= dt;
                }


                window.Clear();
                Render(window);
                window.Display();

                sleepTime = (int)frameTime;

                Thread.Sleep(sleepTime);


            }
            Console.Clear();
            Console.WriteLine("WTF: " + wtf + " frames: " + countFrames );
            Console.ReadKey();
        }

        public void Initialize()
        {
            window = new RenderWindow(new VideoMode(500, 600), "Okno", Styles.Close, new ContextSettings() { DepthBits = 24, AntialiasingLevel = 4 });
            done = false;

            window.Closed += OnClosed;
            window.KeyPressed += OnKeyPressed;
        }

        private void OnKeyPressed(object sender, KeyEventArgs e)
        {
            switch(e.Code)
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
