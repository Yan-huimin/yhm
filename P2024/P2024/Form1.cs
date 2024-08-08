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
using System.Windows.Forms.DataVisualization.Charting;

namespace P2024
{
    public partial class Form1 : Form
    {

        //private double pi = Math.PI;
        private List<my_points> Points = new List<my_points>();//保存所有事件点
        private int cnt_1 = 0, cnt_4 = 0, cnt_6 = 0, Cnt = 0;//保存对应数字区域的事件点数量
        private List<List<my_points>> data = new List<List<my_points>>();//保存每个区域的事件点
        private Matrixs Power_Matrix = new Matrixs();//保存权重矩阵
        private int max_code = -1;//保存分区数量

        public Form1()
        {
            InitializeComponent();

            this.Text = "空间数据分析";
            this.tabPage1.Text = "计算数据";
            this.tabPage2.Text = "计算报告";
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;

            Load_file();
        }

        /// <summary>
        /// 文件导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if(Points.Count() == 0)
            {
                MessageBox.Show("请先导入数据......");
                return;
            }

            tabControl1.SelectedTab = tabPage2;

            Cal_All();

            statues.Text = "计算完成......";
        }


        /// <summary>
        /// 计算函数
        /// </summary>
        private void Cal_All()
        {
            double ave_x = Points.Average(p => p.x);
            double ave_y = Points.Average(p => p.y);

            re.AppendText("1号区域：" + cnt_1.ToString() + "\n");
            re.AppendText("4号区域：" + cnt_4.ToString() + "\n");
            re.AppendText("6号区域：" + cnt_6.ToString() + "\n");
            re.AppendText("全部区域：" + Cnt.ToString() + "\n");
            re.AppendText("X中心坐标为：" + ave_x.ToString("F3") + "\n");
            re.AppendText("Y中心坐标为：" + ave_y.ToString("F3") + "\n");


            Draw_();

            //计算椭圆参数
            Cal_Eli();

            //计算中心点参数
            Cal_cen();

            //计算权重矩阵
            Cal_Power();

            //计算莫兰指数
            Cal_Moran();
        }

        private void Draw_()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();

            chart1.Titles.Add("椭圆图像");
            chart1.Titles.Add("鼠标滑轮可调整图像大小");

            chart1.ChartAreas[0].AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.SharpTriangle;
            chart1.ChartAreas[0].AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.SharpTriangle;


            Series se = new Series
            {
                Name = "点",
                ChartType = SeriesChartType.Point,
                Color = Color.Red
            };
            foreach (var rell in data)
            {
                foreach (var p in rell)
                    se.Points.AddXY(p.x, p.y);
            }

            chart1.Series.Add(se);
        }


        /// <summary>
        /// 计算莫兰指数
        /// </summary>
        private void Cal_Moran()
        {
            double n = (double)Points.Count();
            double ave_x = n / 7;

            re.AppendText("研究区域犯罪事件的平均值 Xba == " + ave_x.ToString("F6") + "\n");

            //全局莫兰指数计算
            double l = 0, r = 0, S0 = 0;
            for (int i = 0; i < max_code; i++)
            {
                r += Math.Pow(data[i].Count() - ave_x, 2);
                for (int j = 0; j < max_code; j++)
                {
                    S0 += Power_Matrix.Matrix[i][j];
                    int xi = data[i].Count(); int xj = data[j].Count;
                    l += Power_Matrix.Matrix[i][j] * (xi - ave_x) * (xj - ave_x);
                }
            }

            double Global_Moran = (max_code / S0) * (l / r);

            re.AppendText("全局莫兰指数为：" + Global_Moran.ToString("F6") + "\n");
            re.AppendText("S0 == " + S0.ToString("0.000000") + "\n");

            //初始化控件
            chart2.Series.Clear();
            chart2.Titles.Clear();

            chart2.Titles.Add("莫兰指数图像");
            chart2.Titles.Add("鼠标滑轮可调整图像大小");

            chart2.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Transparent;
            chart2.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Transparent;

            chart2.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.SharpTriangle;
            chart2.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.SharpTriangle;

            Series glo = new Series
            {
                Name = "Global_Moran",
                ChartType = SeriesChartType.Spline,
                Color = Color.Red
            };


            Series pam = new Series
            {
                Name = "Part_Moran",
                ChartType = SeriesChartType.Spline,
                Color = Color.Blue
            };

            double sum_moran = 0;
            List<double> Sum = new List<double>();
            //局部莫兰指数
            for(int i = 0; i<max_code; i++)
            {
                var rell = data[i];
                double res = 0;
                double l1 = 0, r1 = 0;
                for(int j = 0; j<max_code; j++)
                {
                    if (i == j) continue;
                    l1 += Power_Matrix.Matrix[i][j] * (data[j].Count() - ave_x);
                    r1 += Math.Pow(data[j].Count() - ave_x, 2);
                }

                r1 = r1 / (max_code - 1);

                res = (data[i].Count() - ave_x) / r1 * l1;

                Sum.Add(res);
                sum_moran += res;

                glo.Points.AddXY(i, Global_Moran);
                pam.Points.AddXY(i, res);
                re.AppendText("局部区域" + (i + 1).ToString() + "的局部莫兰指数为：" + res.ToString("F6") + "\n");
            }

            chart2.Series.Add(glo);
            chart2.Series.Add(pam);

            //计算Z得分

            double ave_m = sum_moran / data.Count();

            double sigma = 0;
            foreach(var p in Sum)
            {
                sigma += Math.Pow(p - ave_m, 2);
            }

            sigma = Math.Sqrt(sigma / (max_code - 1));

            re.AppendText("局部莫兰指数平均值：" + ave_m.ToString("F6") + "\n");
            re.AppendText("标准差为：" + sigma.ToString("F6") + "\n");
            //re.AppendText("局部莫兰指数平均值：" + ave_m.ToString("F3") + "\n");

            int cur = 1;
            foreach(var p in Sum)
            {
                double rell = 0;
                rell = (p - ave_m) / sigma;
                re.AppendText("区域" + cur++.ToString() + "的Z得分为：" + rell.ToString("F6") + "\n");
            }

        }

        

        /// <summary>
        /// 计算权重矩阵
        /// </summary>
        private void Cal_Power()
        {
            int n = data.Count();

            Power_Matrix.Init_(n, n);

            for(int i = 0; i<n; i++)
            {
                for(int j = 0; j<n; j++)
                {
                    if (i == j) continue;

                    double xi = data[i].Average(p => p.x);
                    double yi = data[i].Average(p => p.y);
                    double xj = data[j].Average(p => p.x);
                    double yj = data[j].Average(p => p.y);

                    double r = Math.Sqrt((xi - xj) * (xi - xj) + (yi - yj) * (yi - yj));

                    double d = 1000.0 / r;
                    Power_Matrix.Matrix[i][j] = d;
                }
            }

            re.AppendText("Power_1-4 == " + Power_Matrix.Matrix[0][3].ToString("F6") + "\n");
            re.AppendText("Power_6-7 == " + Power_Matrix.Matrix[5][6].ToString("F6") + "\n");
            re.AppendText("\n");
        }

        /// <summary>
        /// 计算中心点参数
        /// </summary>
        private void Cal_cen()
        {
            foreach(var rell in data)
            {
                re.AppendText("区域" + rell[0].code + "\n");

                double ave_x = rell.Average(p => p.x);
                double ave_y = rell.Average(p => p.y);

                re.AppendText("ave_x == " + ave_x.ToString("F3") + "\n");
                re.AppendText("ave_y == " + ave_y.ToString("F3") + "\n");
                re.AppendText("\n");
            }
        }


        /// <summary>
        /// 计算椭圆参数
        /// </summary>
        private void Cal_Eli()
        {
            int n = Points.Count();

            double ave_x = Points.Average(p => p.x);
            double ave_y = Points.Average(p => p.y);

            double ai = 0, bi = 0, ab = 0;
            foreach(var p in Points)
            {
                ai += Math.Pow((p.x - ave_x), 2);
                bi += Math.Pow((p.y - ave_y), 2);
                ab += (p.x - ave_x) * (p.y - ave_y);
            }

            double A = ai - bi;
            double B = Math.Sqrt((ai - bi) * (ai - bi) + 4 * ab * ab);
            double C = 2 * ab;

            double angle = Math.Atan((A + B)/ C);

            double Sx = 0, Sy = 0;
            foreach(var p in Points)
            {
                Sx += Math.Pow((p.x - ave_x) * Math.Cos(angle) + (p.y - ave_y) * Math.Sin(angle), 2);
                Sy += Math.Pow((p.x - ave_x) * Math.Sin(angle) - (p.y - ave_y) * Math.Cos(angle), 2);
            }
            Sx = Math.Sqrt(Sx / n * 2);
            Sy = Math.Sqrt(Sy / n * 2);

            re.AppendText("A = " + A.ToString("F3") + "\n");
            re.AppendText("B = " + B.ToString("F3") + "\n");
            re.AppendText("C = " + C.ToString("F3") + "\n");

            re.AppendText("角度为：" + angle.ToString("F3") + "\n");
            re.AppendText("Sx = " + Sx.ToString("F3") + "\n");
            re.AppendText("Sy = " + Sy.ToString("F3") + "\n");

            foreach(var p in Points)
            {
                if(p.ID == "P6")
                {
                    re.AppendText(p.ID + "\n");
                    re.AppendText("X分量：" + (p.x - ave_x).ToString("F3") + "\n");
                    re.AppendText("Y分量：" + (p.y - ave_y).ToString("F3") + "\n");
                    break;
                }
            }

            //re.AppendText("1号区域的x中心坐标")


            Series eli = new Series
            {
                Name = "椭圆",
                ChartType = SeriesChartType.Line,
                Color = Color.Red
            };
            for(int i = 0; i<1000; i++)
            {
                double angle_1 = 2 * Math.PI / 1000 * i;
                double x1 = ave_x + Sx * Math.Cos(angle) * Math.Cos(angle_1) + Sy * Math.Sin(angle) * Math.Sin(angle_1);
                double y1 = ave_y + Sy * Math.Cos(angle) * Math.Sin(angle_1) - Sy * Math.Sin(angle) * Math.Cos(angle_1);
                eli.Points.AddXY(x1, y1);
            }

            chart1.Series.Add(eli);
        }


        private void Change_size(object sender, MouseEventArgs e)
        {
            double rell = e.Delta > 0 ? 0.1 : -0.1;

            double len_x = chart1.ChartAreas[0].AxisX.Maximum - chart1.ChartAreas[0].AxisX.Minimum;
            double len_y = chart1.ChartAreas[0].AxisY.Maximum - chart1.ChartAreas[0].AxisY.Minimum;

            chart1.ChartAreas[0].AxisX.Minimum -= rell * len_x;
            chart1.ChartAreas[0].AxisX.Maximum += rell * len_x;
            chart1.ChartAreas[0].AxisY.Minimum -= rell * len_y;
            chart1.ChartAreas[0].AxisY.Maximum += rell * len_y;
        }

        private void Change_size_2(object sender, MouseEventArgs e)
        {
            double rell = e.Delta > 0 ? 0.1 : -0.1;

            double len_x = chart2.ChartAreas[0].AxisX.Maximum - chart2.ChartAreas[0].AxisX.Minimum;
            double len_y = chart2.ChartAreas[0].AxisY.Maximum - chart2.ChartAreas[0].AxisY.Minimum;

            chart2.ChartAreas[0].AxisX.Minimum -= rell * len_x;
            chart2.ChartAreas[0].AxisX.Maximum += rell * len_x;
            chart2.ChartAreas[0].AxisY.Minimum -= rell * len_y;
            chart2.ChartAreas[0].AxisY.Maximum += rell * len_y;
        }

        private void 导出文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(re.Text == "")
            {
                MessageBox.Show("请先计算");
                return;
            }
            using (SaveFileDialog sv = new SaveFileDialog())
            {
                sv.Filter = "文本文件(*.txt)|*.txt";
                sv.Title = "文件";

                var res = sv.ShowDialog();
                if(res == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sv.FileName))
                        sw.Write(re.Text);

                    statues.Text = "文件保存成功......";
                }
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = toolStrip1.Visible ? false : true;
        }

        private void 状态栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Visible = statusStrip1.Visible ? false : true;
        }

        private void 计算数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void 计算报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("请查看帮助文档......");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Points.Clear();
            data.Clear();
            re.Clear();

            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart2.Series.Clear();
            chart2.Titles.Clear();
        }

        private void 导出椭圆图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sv = new SaveFileDialog())
            {
                sv.Filter = "(*.jpeg)|*.jpeg";
                sv.Title = "文件";

                var res = sv.ShowDialog();
                if (res == DialogResult.OK)
                {
                    chart1.SaveImage(sv.FileName, ChartImageFormat.Jpeg);
                    statues.Text = "椭圆图像保存成功......";
                }
            }
        }

        private void 导出莫兰指数图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sv = new SaveFileDialog())
            {
                sv.Filter = "(*.jpeg)|*.jpeg";
                sv.Title = "文件";

                var res = sv.ShowDialog();
                if (res == DialogResult.OK)
                {
                    chart2.SaveImage(sv.FileName, ChartImageFormat.Jpeg);
                    statues.Text = "莫兰指数图像保存成功......";
                }
            }
        }

        private void 莫兰指数图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void 椭圆图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        /// <summary>
        /// 导入文件
        /// </summary>
        private void Load_file()
        {
            try
            {
                using (OpenFileDialog file = new OpenFileDialog())
            {
                file.Filter = "文本文件(*.txt)|*.txt";
                    file.Title = "文件";

                    var res = file.ShowDialog();
                    if(res == DialogResult.OK)
                    {
                        Get_data(file.FileName);

                        Show_data();

                        statues.Text = "数据导入成功......";
                    }
                }
        }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
}


        /// <summary>
        /// 文件数据获取函数
        /// </summary>
        /// <param name="path"></param>
        private void Get_data(string path)
        {

            using (StreamReader sr = new StreamReader(path))
            {
                var r = sr.ReadToEnd().Trim().Split('\n');

                for(int i = 1; i<r.Count(); i++)
                {
                    var s = r[i].Split(',');
                    my_points p = new my_points();

                    p.ID = s[0];
                    p.x = double.Parse(s[1]);
                    p.y = double.Parse(s[2]);
                    p.code = int.Parse(s[3]);

                    if(p.ID == "P6")
                    {
                        re.AppendText("-----------------------计算报告------------------------\n");
                        re.AppendText("P6的x坐标为：" + p.x.ToString("F3") + "\n");
                        re.AppendText("P6的y坐标为：" + p.y.ToString("F3") + "\n");
                        re.AppendText("P6的区号为：" + p.code.ToString("F3") + "\n");
                    }

                    if (p.code == 1) cnt_1++;
                    if (p.code == 4) cnt_4++;
                    if (p.code == 6) cnt_6++;
                    max_code = Math.Max(p.code, max_code);
                    Cnt++;


                    Points.Add(p);
                }

                for(int i = 0; i<max_code; i++)
                {
                    var cur = new List<my_points>();
                    data.Add(cur);
                }

                for(int i = 0; i<Points.Count(); i++)
                {
                    var p = Points[i];
                    data[p.code - 1].Add(p);
                }
            }
        }

        /// <summary>
        /// 数据显示函数
        /// </summary>
        private void Show_data()
        {
            da.Columns.Clear(); da.Rows.Clear();

            da.Columns.Add("ID", "ID");
            da.Columns.Add("X", "X");
            da.Columns.Add("Y", "Y");
            da.Columns.Add("Code", "Code");

            foreach (var p in Points)
                da.Rows.Add(p.ID, p.x, p.y, p.code);
        }



    }
}
