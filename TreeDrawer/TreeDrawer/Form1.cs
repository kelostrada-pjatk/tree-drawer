using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeDrawer
{
    public partial class TreeDrawer : Form
    {
        public TreeDrawer()
        {
            InitializeComponent();
            GenerateTree();
        }

        private class node<T>
        {
            public node<T> left, right; // node prawy i lewy
            public T val; // Wartość do przechowywania
            public Color Color = Color.Blue; // Kolor do rysowania (może się przydać przy drzewach czerwono czarnych
            public double i = 0; // Indeks node'a - potrzebny do rysowania, każdy ma unikatowy. Przydzielany przez funkcję void Order(node<double> p); Jak i==0 to znaczy że nie ustawione

            public node(T _val)
            {
                val = _val;
            }
        }

        private int TreeH(node<double> p)
        {
            if (p == null) return 0;
            int l, r;
            l = ((p.left != null) ? TreeH(p.left) : 0);
            r = ((p.right != null) ? TreeH(p.right) : 0);
            return ((l > r) ? l : r) + 1;
        }

        private void Order(node<double> p,int i = 1)
        {
            if (p == null) return;
            p.i = i;
            Order(p.left, 2 * i);
            Order(p.right, 2 * i + 1);
        }

        private Point _DrawTree(node<double> p,double R,double H,Graphics g)
        {
            if (p == null) return Point.Empty;
            double level = Math.Floor(Math.Log(p.i,2));
            double X = pictureBox1.Width * (p.i - Math.Pow(2, level) + 1) / (Math.Pow(2, level) + 1);
            double Y = (2 * pictureBox1.Height * level) / (2 * H - 1) + R;
            //MessageBox.Show("Punkt: ("+X+","+Y+")\nval: "+p.val+"\nLevel: "+level+"\np.i:"+p.i);
            DrawNode(X, Y, R, p, g);
            
            Point A = _DrawTree(p.left, R, H, g);
            if (A != Point.Empty)
                DrawLine(A,new Point((int)X,(int)Y), R, p.left, g);
            A = _DrawTree(p.right, R, H, g);
            if (A != Point.Empty)
                DrawLine(A,new Point((int)X, (int)Y), R, p.right, g);
            return new Point((int)X, (int)Y);
        }

        private void DrawNode(double X, double Y, double R, node<double> p, Graphics g)
        {
            g.DrawEllipse(new Pen(p.Color), (int)(X-R), (int)(Y-R), (int)(2*R), (int)(2*R));
            Font font = new Font("Arial", (float)(R/1.5));
            // TODO: Zrobić ładne przesunięcie tekstu na środek.
            double i = R/1.5;
            g.DrawString(p.val.ToString(), font, new SolidBrush(p.Color), new PointF((int)(X-i), (int)(Y-(R/2))));
        }

        private void DrawLine(Point A, Point B, double R, node<double> p, Graphics g)
        {
            double r = Math.Sqrt(Math.Pow(A.X - B.X, 2) + Math.Pow(A.Y - B.Y, 2));
            double Ax, Ay, Bx, By;
            Ax = R / r * (B.X - A.X) + A.X;
            Ay = R / r * (B.Y - A.Y) + A.Y;
            Bx = R / r * (A.X - B.X) + B.X;
            By = R / r * (A.Y - B.Y) + B.Y;

            A.X = (int)Ax;
            A.Y = (int)Ay;
            B.X = (int)Bx;
            B.Y = (int)By;
            g.DrawLine(new Pen(p.Color), A, B);
        }

        private void DrawTree(node<double> p)
        {
            double R1, R2, Radius;
            int H = TreeH(p);
            R1 = pictureBox1.Height / (4 * H);
            R2 = pictureBox1.Width / Math.Pow(2,H - 1);
            Radius = (R1 < R2) ? R1 : R2;
            if (Math.Pow(2,H)*Radius > pictureBox1.Width)
                Radius = pictureBox1.Width / (Math.Pow(2, H)+1);

            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Image img = bmp;
            Graphics g = Graphics.FromImage(img);

            Order(p);
            //MessageBox.Show("Radius: " + Radius);
            _DrawTree(p,Radius,H,g);

            pictureBox1.Image = img;
        }

        private void GenerateTree()
        {
            node<double> A = new node<double>(15);
            A.left = new node<double>(1);
            A.right = new node<double>(21);
            A.right.right = new node<double>(28);
            A.right.right.left = new node<double>(25);
            A.right.right.left.right = new node<double>(26);
            A.right.right.left.right.right = new node<double>(27);
            A.right.right.left.left = new node<double>(24);
            A.right.right.right = new node<double>(30);
            A.right.right.right.right = new node<double>(31);
            A.right.right.right.left = new node<double>(29);
            A.right.left = new node<double>(20);
            DrawTree(A);
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            GenerateTree();
        }

    }
}
