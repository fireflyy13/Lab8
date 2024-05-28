using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Lab8TA
{
    public partial class Form1 : Form
    {
        private BalancedTree _BalancedTree;
        private RedBlackTree _RedBlackTree;
        private Bitmap _Balancedbitmap, _RedBlackbitmap;
        private Graphics _Balancedgraphics, _RedBlackgraphics;
        public List<int> nodes = new List<int> {12, 8, 15, 5, 9, 10, 13, 19, 23, 25};


        public Form1()
        {
            InitializeComponent();
            
            _BalancedTree = new BalancedTree(nodes);
            _Balancedbitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            _Balancedgraphics = Graphics.FromImage(_Balancedbitmap);

            _RedBlackTree = new RedBlackTree();
            _RedBlackbitmap = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            _RedBlackgraphics = Graphics.FromImage(_RedBlackbitmap);
            InitialTree();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void InitialTree()
        {
            //Random random = new Random();
            //int numberOfRandomNodes = 90; 

            //for (int i = 0; i < numberOfRandomNodes; i++)
            //{
            //    nodes.Add(random.Next(30, 50));
            //}

            foreach (int value in nodes)
            {
                _BalancedTree.Add(value);
                _RedBlackTree.Add(value);
            }
            DrawTree();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int element))
            {
                MessageBox.Show("Будь ласка, введіть коректне значення " +
                    "кількості злитків!",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (_BalancedTree.Search(element))
            {
                MessageBox.Show("Печера, що містить вказану кількість злитків вже існує!",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            _BalancedTree.Add(element);
            _RedBlackTree.Add(element);
            DrawTree();
            textBox1.Clear();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            //int iterations = 1000;
            if (!int.TryParse(textBox2.Text, out int element))
            {
                MessageBox.Show("Будь ласка, введіть коректне значення " +
                    "кількості злитків!",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!(_BalancedTree.Search(element)))
            {
                MessageBox.Show("Не існує печери, що містить вказану кількість злитків!",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            //for (int i = 0; i < iterations; i++)
            //{
            //    _RedBlackTree.Remove(element);
            //}
            //stopwatch.Stop();
            //long totalTicks = stopwatch.ElapsedTicks;
            //double averageTicks = (double)totalTicks / iterations;
            //double averageMilliseconds = averageTicks / 10000.0;
            //MessageBox.Show($"REMOVE FROM BALANSED {averageMilliseconds} ms");
            
            _RedBlackTree.Remove(element);
            _BalancedTree.Remove(element);
            DrawTree();
            textBox2.Clear();
        }

        private void ShowPath_Click(object sender, EventArgs e)
        {
            int iterations = 1000;
            Stopwatch stopwatch = new Stopwatch();
            if (!int.TryParse(textBox3.Text, out int element))
            {
                MessageBox.Show("Будь ласка, введіть коректне значення елемента" +
                    "кількості злитків!",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!(_BalancedTree.Search(element)))
            {
                MessageBox.Show($"Шуканої печери з кількістю злитків {element} не існує!" +
                    $" Незабаром її буде створено.",
                    "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (!(_RedBlackTree.Search(element)))
            {
                MessageBox.Show($"Шуканої печери з кількістю злитків {element} не існує!" +
                    $" Незабаром її буде створено.",
                    "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //stopwatch.Start();

            //for (int i = 0; i < iterations; i++)
            //{
            //    _BalancedTree.Search(element);
            //}
            //stopwatch.Stop();
            List<int> path1 = _BalancedTree.FindPath(element);
            string pathString1 = string.Join(" -> ", path1);
            List<int> path2 = _RedBlackTree.FindPath(element);
            string pathString2 = string.Join(" -> ", path2);
            //double averageTimePerSearch = stopwatch.Elapsed.TotalMilliseconds / iterations;
            //MessageBox.Show($"Average time per binary search in REDBLACK: {averageTimePerSearch} ms");
            DrawTree();
            label4.Text = ($"Шлях до заданого скарбу: {pathString1}");
            label5.Text = ($"Шлях до заданого скарбу: {pathString2}");
            textBox3.Clear();
        }

        private void DrawTree()
        {
            _Balancedgraphics.Clear(Color.CadetBlue);
            _RedBlackgraphics.Clear(Color.CadetBlue);
            DrawNode(_BalancedTree.root, 300, 20, 150);
            DrawNode(_RedBlackTree.root, 300, 20, 150);
            pictureBox1.Image = _Balancedbitmap;
            pictureBox2.Image = _RedBlackbitmap;
        }

        private void DrawNode(BalancedNode node, float x, float y, float dx)
        {
            if (node == null)
            {
                return;
            }

            float ellipseWidth = 32;
            float ellipseHeight = 32;
            Pen ellipsePen = new Pen(Color.Black, 2);
            _Balancedgraphics.DrawEllipse(ellipsePen, x, y, ellipseWidth, ellipseHeight);
            _Balancedgraphics.FillEllipse(Brushes.White, x, y, ellipseWidth, ellipseHeight);


            SizeF textSize = _Balancedgraphics.MeasureString(node.Key.ToString(),
                new Font("Cascadia Mono", 12));

            float textX = x + (ellipseWidth - textSize.Width) / 2;
            float textY = y + (ellipseHeight - textSize.Height) / 2;

            _Balancedgraphics.DrawString(node.Key.ToString(), new Font("Cascadia Mono", 12),
                Brushes.Black, textX, textY);

            Pen widePen = new Pen(Color.Black, 2); 

            if (node.left != null)
            {
                float childX = x - dx;
                float childY = y + 52; 
                _Balancedgraphics.DrawLine(widePen, x + ellipseWidth / 2, y + ellipseHeight,
                    childX + ellipseWidth / 2, childY);
                DrawNode(node.left, childX, childY, dx / 2);
            }

            if (node.right != null)
            {
                float childX = x + dx;
                float childY = y + 52; 
                _Balancedgraphics.DrawLine(widePen, x + ellipseWidth / 2, y + ellipseHeight,
                    childX + ellipseWidth / 2, childY);
                DrawNode(node.right, childX, childY, dx / 2);
            }
        }

        private void DrawNode(RedBlackNode node, float x, float y, float dx)
        {
            if (node == null)
            {
                return;
            }

            float ellipseWidth = 32;
            float ellipseHeight = 32;
            Pen ellipsePen = new Pen(Color.Black, 2);

            _RedBlackgraphics.DrawEllipse(ellipsePen, x, y, ellipseWidth, ellipseHeight);
            if (node.color == Colors.Red)
            {
                _RedBlackgraphics.FillEllipse(Brushes.Brown, x, y, ellipseWidth, ellipseHeight);
            }

            else if (node.color == Colors.Black)
            {
                _RedBlackgraphics.FillEllipse(Brushes.Black, x, y, ellipseWidth, ellipseHeight);
            }

            SizeF textSize = _RedBlackgraphics.MeasureString(node.Key.ToString(),
                new Font("Cascadia Mono", 12));

            float textX = x + (ellipseWidth - textSize.Width) / 2;
            float textY = y + (ellipseHeight - textSize.Height) / 2;

            _RedBlackgraphics.DrawString(node.Key.ToString(), new Font("Cascadia Mono", 12),
                Brushes.White, textX, textY);

            Pen widePen = new Pen(Color.Black, 2);
            if (node.left != null)
            {
                float childX = x - dx;
                float childY = y + 52; 
                _RedBlackgraphics.DrawLine(widePen, x + ellipseWidth / 2, y + ellipseHeight,
                    childX + ellipseWidth / 2, childY);
                DrawNode(node.left, childX, childY, dx / 2);
            }

            if(node.right != null)
{
                float childX = x + dx;
                float childY = y + 52; 
                _RedBlackgraphics.DrawLine(widePen, x + ellipseWidth / 2, y + ellipseHeight,
                    childX + ellipseWidth / 2, childY);
                DrawNode(node.right, childX, childY, dx / 2);
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }
    }
}