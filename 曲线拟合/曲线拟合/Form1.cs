using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows;

namespace 曲线拟合
{
    public partial class Form1 : Form
    {
        //基本数据
        private List<my_point> Points = new List<my_point>();
        private List<my_point> data = new List<my_point>();
        private List<double> Sin = new List<double>();
        private List<double> Cos = new List<double>();
        private List<E> e = new List<E>();
        private List<F> f = new List<F>();

        private double x_min, x_max;
        private double y_min, y_max;

        public Form1()
        {
            InitializeComponent();
            this.Text = "曲线拟合";
            tabPage1.Text = "数据文件";
            tabPage2.Text = "图形显示";
            tabPage3.Text = "报告显示";
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();

                file.Filter = "所有文件（.TXT）|*.txt";
                file.Title = "选择文件";

                DialogResult re = file.ShowDialog();
                if(re == DialogResult.OK)
                {
                    string path = file.FileName;
                    Show_Data(path);
                    Draw_chart();
                } 
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message,null, MessageBoxButtons.OK);
            }
        }

        //显示数据文件
        private void Show_Data(string path)
        {
            try
            {
                Points.Clear();
                using (StreamReader sr = new StreamReader(path))
                {
                    string s = sr.ReadToEnd();
                    var a = s.Trim().Split('\n');

                    foreach(var p in a)
                    {
                        var cur = p.Split(',');
                        string id = cur[0];
                        double x = double.Parse(cur[1]);
                        double y = double.Parse(cur[2]);

                        my_point i = new my_point();
                        i.ID = id;  i.x = x;    i.y = y;
                        Points.Add(i);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message, null, MessageBoxButtons.OK);
            }
        }

        //绘制表格
        private void Draw_chart()
        {
            try
            {
                d.Dock = DockStyle.Fill;
                d.Columns.Clear();
                d.Rows.Clear();

                d.Columns.Add("ID", "ID");
                d.Columns.Add("X", "X");
                d.Columns.Add("Y", "Y");

                foreach(var p in Points)
                {
                    d.Rows.Add(p.ID, p.x, p.y);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message, null, MessageBoxButtons.OK);
            }
        }

        private void 闭曲线拟合计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calcate(false);

            Generate_report(0);
        }

        private void 开曲线拟合计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calcate(true);

            Generate_report(1);
        }

        private void Calcate(bool st)
        {
            Init_Chart();

            Draw_Points();

            Add_Points(st);

            Calcate_Sin_Cos();

            Draw_Lines(st);
        }

        private void Init_Chart()
        {
            ch.Series.Clear();

            ch.ChartAreas[0].AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            ch.ChartAreas[0].AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            

            ch.Series.Add("Points");
            ch.Series.Add("Lines");

            ch.Series["Lines"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            ch.Series["Points"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

            ch.Series["Lines"].Color = Color.Black;
            ch.Series["Points"].Color = Color.Red;

            x_min = ch.ChartAreas[0].AxisX.Minimum;
            x_max = ch.ChartAreas[0].AxisX.Maximum;
            y_min = ch.ChartAreas[0].AxisY.Minimum;
            y_max = ch.ChartAreas[0].AxisY.Maximum;
        }

        private void Draw_Points()
        {
            for (int i = 0; i < Points.Count; i++)
            {
                ch.Series["Points"].Points.AddXY(Points[i].x, Points[i].y);
                ch.Series["Points"].Points[i].Label = i.ToString();
            }
        }

        private void Add_Points(bool st)
        {
            data.Clear();
            if(st)
            {
                my_point a = new my_point();
                my_point b = new my_point();

                a.x = Points[2].x - 3 * Points[1].x + 3 * Points[0].x;
                a.y = Points[2].y - 3 * Points[1].y + 3 * Points[0].y;

                b.x = Points[1].x - 3 * Points[0].x + 3 * a.x;
                b.y = Points[1].y - 3 * Points[0].y + 3 * a.y;

                data.Add(b);
                data.Add(a);

                foreach (var p in Points) data.Add(p);

                my_point c = new my_point();
                my_point d = new my_point();

                int n = Points.Count;
                c.x = Points[n - 3].x - 3 * Points[n - 2].x + 3 * Points[n - 1].x;
                c.y = Points[n - 3].y - 3 * Points[n - 2].y + 3 * Points[n - 1].y;

                d.x = Points[n - 2].x - 3 * Points[n - 1].x + 3 * c.x;
                d.y = Points[n - 2].y - 3 * Points[n - 1].y + 3 * c.y;
                data.Add(c);
                data.Add(d);
                return;
            }
            else
            {
                my_point a = Points[Points.Count - 2];
                my_point b = Points[Points.Count - 1];

                data.Add(a);
                data.Add(b);

                foreach (var p in Points) data.Add(p);

                my_point c = Points[0];
                my_point d = Points[1];

                data.Add(c);
                data.Add(d);
                return;
            }
        }

        private void Calcate_Sin_Cos()
        {
            Sin.Clear();
            Cos.Clear();

            for(int i = 2; i<=data.Count - 3; i++)
            {
                double a0, a1, a2, a3, a4;
                double b0, b1, b2, b3, b4;
                double w2, w3;
                double x1, x2, x, y1, y2, y;
                double x_1, x_2, y_1, y_2;

                x = data[i].x;    y = data[i].y;
                x1 = data[i + 1].x;   x2 = data[i + 2].x;
                y1 = data[i + 1].y;    y2 = data[i + 2].y;
                x_1 = data[i - 1].x; x_2 = data[i - 2].x;
                y_1 = data[i - 1].y; y_2 = data[i - 2].y;

                a1 = x_1 - x_2; b1 = y_1 - y_2;
                a2 = x - x_1;   b2 = y - y_1;
                a3 = x1 - x;    b3 = y1 - y;
                a4 = x2 - x1;   b4 = y2 - y1;

                w2 = Math.Abs(a3 * b4 - a4 * b3);
                w3 = Math.Abs(a1 * b2 - a2 * b1);

                a0 = w2 * a2 + w3 * a3;
                b0 = w2 * b2 + w3 * b3;

                double r = Math.Sqrt(a0 * a0 + b0 * b0);
                Sin.Add(b0 / r);
                Cos.Add(a0 / r);
            }

            Sin.Add(Sin[0]);
            Cos.Add(Cos[0]);
        }

        private void Draw_Lines(bool st)
        {
            int c = 0;
            if (st) c = 1;

            e.Clear();  f.Clear();

            for(int i = 2; i<=data.Count - 3 - c; i++)
            {
                double e0, e1, e2, e3;
                double f0, f1, f2, f3;
                double dx = data[i + 1].x - data[i].x;
                double dy = data[i + 1].y - data[i].y;
                double r = Math.Sqrt(dx * dx + dy * dy);

                e0 = data[i].x; f0 = data[i].y;
                e1 = r * Cos[i - 2];    f1 = r * Sin[i - 2];
                e2 = 3 * (data[i + 1].x - data[i].x) - r * (Cos[i + 1 - 2] + 2 * Cos[i - 2]);
                f2 = 3 * (data[i + 1].y - data[i].y) - r * (Sin[i + 1 - 2] + 2 * Sin[i - 2]);
                e3 = -2 * (data[i + 1].x - data[i].x) + r * (Cos[i + 1 - 2] + Cos[i - 2]);
                f3 = -2 * (data[i + 1].y - data[i].y) + r * (Sin[i + 1 - 2] + Sin[i - 2]);

                E ei = new E(); F fi = new F();
                ei.e0 = e0; ei.e1 = e1; ei.e2 = e2; ei.e3 = e3;
                fi.f0 = f0; fi.f1 = f1; fi.f2 = f2; fi.f3 = f3;
                e.Add(ei);  f.Add(fi);

                double z = 0;
                while(z<1)
                {
                    double x = e0 + e1 * z + e2 * z * z + e3 * z * z * z;
                    double y = f0 + f1 * z + f2 * z * z + f3 * z * z * z;

                    ch.Series["Lines"].Points.AddXY(x, y);
                    z += 0.01;
                }
            }
        }

        private void Generate_report(int st)
        {
            try
            {
                // 清空 RichTextBox
                r.Clear();

                // 添加标题和基本信息
                r.AppendText("数据报告\n");
                r.AppendText("-------------------------------基本信息----------------------------------\n");
                r.AppendText((st == 1 ? "开曲线拟合" : "闭曲线拟合") + "\n");
                r.AppendText("总点数 ： " + Points.Count.ToString() + "\n");
                r.AppendText("-------------------------------计算结果----------------------------------\n");
                r.AppendText("说明 : \n");
                r.AppendText("计算公式 : x = e0 + e1 * z + e2 * z * z + e3 * z * z * z\t\t" + "y = f0 + f1 * z + f2 * z * z + f3 * z * z * z\n");
                r.AppendText("r为两点之间的弦长变量(从0循环至1)\n");

                // 添加表头
                r.AppendText($"{"起点ID",-10}{"x",-15}{"y",-15}{"终点ID",-10}{"x",-15}{"y",-15}{"p0",-10}{"p1",-10}{"p2",-10}{"p3",-10}{"q0",-10}{"q1",-10}{"q2",-10}{"q3",-10}\n");

                // 添加数据
                for (int i = 2; i <= data.Count - st - 3; i++)
                {
                    my_point a = data[i];
                    my_point b = data[i + 1];
                    E m = e[i - 2];
                    F n = f[i - 2];

                    r.AppendText(
                        $"{a.ID,-10}{Math.Round(a.x, 3),-15}{Math.Round(a.y, 3),-15}{b.ID,-10}{Math.Round(b.x, 3),-15}{Math.Round(b.y, 3),-15}" +
                        $"{Math.Round(m.e0, 3),-10}{Math.Round(m.e1, 3),-10}{Math.Round(m.e2, 3),-10}{Math.Round(m.e3, 3),-10}" +
                        $"{Math.Round(n.f0, 3),-10}{Math.Round(n.f1, 3),-10}{Math.Round(n.f2, 3),-10}{Math.Round(n.f3, 3),-10}\n"
                    );
                }

                r.Text += "保留三位小数";
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message);
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control_Size(0.8);
        }

        private void 缩小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Control_Size(1.25);
        }

        private void 图像复原ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reinit_chart();
        }

        private void Control_Size(double st)
        {
            try
            {
                double width = ch.ChartAreas[0].AxisX.Maximum - ch.ChartAreas[0].AxisX.Minimum;
                double height = ch.ChartAreas[0].AxisY.Maximum - ch.ChartAreas[0].AxisY.Minimum;

                double x_center = ch.ChartAreas[0].AxisX.Minimum + width / 2;
                double y_center = ch.ChartAreas[0].AxisY.Minimum + height / 2;

                ch.ChartAreas[0].AxisX.Minimum = x_center - width / 2 * st;
                ch.ChartAreas[0].AxisX.Maximum = x_center + width / 2 * st;

                ch.ChartAreas[0].AxisY.Maximum = y_center + height * st / 2;
                ch.ChartAreas[0].AxisY.Minimum = y_center - height * st / 2;
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 保存报告文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog s = new SaveFileDialog();

                s.Filter = "文本文件|*.txt|所有文件|*.*";
                s.Title = "选择文件保存位置";

                DialogResult re = s.ShowDialog();
                if(re == DialogResult.OK)
                {
                    string path = s.FileName;

                    using (StreamWriter w = new StreamWriter(path))
                    {
                        w.WriteLine(r.Text);
                    }

                    MessageBox.Show("文件保存成功", "Tips", MessageBoxButtons.OK);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("保存文件发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Reinit_chart()
        {
            try
            {
                ch.ChartAreas[0].AxisX.Minimum = x_min;
                ch.ChartAreas[0].AxisX.Maximum = x_max;
                ch.ChartAreas[0].AxisY.Minimum = y_min;
                ch.ChartAreas[0].AxisY.Maximum = y_max;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message);
            }
        }
    }
}
