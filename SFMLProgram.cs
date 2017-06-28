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
        double vPocisku = 100;
        List<Drawable> toDraw;

        public SFMLProgram()
        {
            toDraw = new List<Drawable>();
            toDraw.Add(new Circle(20, 10, 10));
        }

        public void ProcessInput()
        {

        }

        public void Update(double dt)
        {
            xPocisk += (dt / 1000) * vPocisku;
            ((Circle)toDraw[0]).Position = new SFML.System.Vector2f((float)xPocisk, 0);
            if (xPocisk >= 500)
            {
                xPocisk = 0;
            }
            if (xPocisk < 0)
            {
                xPocisk = 500;
            }
        }

        public void Render(RenderWindow window)
        {
            for (int i = 0; i < toDraw.Count; i++)
            {
                window.Draw(toDraw[i]);
            }
        }

        public void OnEClicked()
        {
            vPocisku += 200;
            ((Circle)toDraw[0]).OnEClicked();
        }

        public void OnQClicked()
        {
            vPocisku -= 200;
            ((Circle)toDraw[0]).OnQClicked();
        }
    }
}
