using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using System.Threading;

namespace Presentation
{
    class SFMLProgram
    {
        double xPocisk = 0;
        double vPocisku = 5731;
        List<Drawable> toDraw;

        public SFMLProgram()
        {
            toDraw = new List<Drawable>();
            toDraw.Add(new CircleShape(20, 10));
            Console.WriteLine("SFML");
        }

        public void ProcessInput()
        {

        }

        public void Update(double dt)
        {
            xPocisk += (dt / 1000) * vPocisku;
            ((CircleShape)toDraw[0]).Position = new SFML.System.Vector2f((float)xPocisk, 0);
            if (xPocisk >= 500)
            {
                xPocisk = 0;
            }
        }

        public void Render(RenderWindow window)
        {
            for (int i = 0; i < toDraw.Count; i++)
            {
                window.Draw(toDraw[i]);
            }
        }
    }
}
