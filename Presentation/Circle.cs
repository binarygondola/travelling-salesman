using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    class Circle : CircleShape, Drawable, IMoveable, Updatable
    {
        private double number;
        public double Number
        {
            get { return number; }
            set {
                number = value;
                t = new Text(number.ToString(), f, 30);
            }
        }

        double v = 100;
        double x = 0;
        Font f;
        Text t;

        public Circle(double number)
        {
            f = new Font(Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + "\\arial.ttf");
            t = new Text(number.ToString(), f, 30);
            t.Color = Color.Black;
        }

        public Circle(float r, uint pointCount, double n) : this(n)
        {
            number = n;
            Radius = r;
            SetPointCount(pointCount);
            t.Origin = new SFML.System.Vector2f(-r + t.GetLocalBounds().Width / 2 + t.GetLocalBounds().Left, -r + t.GetLocalBounds().Height / 2  + t.GetLocalBounds().Top);
        }

        public new void Draw(RenderTarget target, RenderStates states)
        {
            base.Draw(target, states);
            target.Draw(t);
        }

        public new SFML.System.Vector2f Origin
        {
            get { return base.Origin; }
            set
            {
                base.Origin = value;
                t.Origin += value;
            }
        }

        public new SFML.System.Vector2f Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;
                t.Position = value;
            }
        }


        public void OnEClicked()
        {
            FillColor = Color.Green;
            v += 100;
        }


        public void OnQClicked()
        {
            FillColor = Color.Blue;
            v -= 100;
        }

        public void Update(double dt)
        {
            x += v * dt / 1000;
            Position = new SFML.System.Vector2f((float)x, 0);
        }
    }
}
