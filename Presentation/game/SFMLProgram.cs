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
        List<Drawable> toDraw;
        int count = 0;
        int times = 4000;

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
            for (int i = 0; i < toDraw.Count; i++)
            {
                ((Updatable)toDraw[i]).Update(dt);
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
            ((Circle)toDraw[0]).OnEClicked();
        }

        public void OnQClicked()
        {
            ((Circle)toDraw[0]).OnQClicked();
        }
    }
}
