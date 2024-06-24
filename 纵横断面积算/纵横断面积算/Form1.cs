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

namespace 纵横断面积算
{
    public partial class Form1 : Form
    {

        private double H0;
        private my_points A = new my_points();
        private my_points B = new my_points();
        private List<my_points> All_Points = new List<my_points>();
        private List<my_points> points_k = new List<my_points>();
        private List<my_points> points = new List<my_points>();
        private List<my_points> P = new List<my_points>();
        private List<M> rell = new List<M>();



        public Form1()
        {
            InitializeComponent();

            tabPage1.Text = "初始数据";
            tabPage2.Text = "基础计算数据";
            tabPage3.Text = "横断面计算数据";
            tabPage4.Text = "纵断面计算数据";
            tabPage5.Text = "纵断面图像";
            tabPage6.Text = "横断面图像";
            tabPage7.Text = "计算报告";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            double res = Cal_Area(points_k[0], points_k[1]);

            double sum = 0;
            for(int i = 0; i< points_k.Count() - 1; i++)
            {
                var p0 = points_k[i];
                var p1 = points_k[i+1];
                sum += Cal_Area(p0, p1);
            }

            basic.AppendText("纵断面总面积为：" + Math.Round(sum, 3).ToString() + "\n");
            basic.AppendText("K0·K1的断面面积为：" + Math.Round(res, 3).ToString() + "\n");
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input_File();
        }


        private void Input_File()
        {
            try
            {
                using(OpenFileDialog file = new OpenFileDialog())
                {
                    file.Filter = "文本文件*.txt|*.txt";
                    file.Title = "选择文件";

                    DialogResult re = file.ShowDialog();
                    if(re == DialogResult.OK)
                    {
                        var path = file.FileName;
                        Show_data(path);

                        Draw_Grid();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Show_data(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    var s = sr.ReadToEnd().Trim().Split('\n');

                    var fi = s[0].Split(',');
                    H0 = double.Parse(fi[1]);

                    var se = s[2].Split(',');
                    A.x = double.Parse(se[1]);
                    A.y = double.Parse(se[2]);
                    A.ID = se[0];

                    var th = s[3].Split(',');
                    B.x = double.Parse(th[1]);
                    B.y = double.Parse(th[2]);
                    B.ID = th[0];

                    for(int i = 5; i<s.Count(); i++)
                    {
                        my_points p = new my_points();
                        var rell = s[i].Split(',');
                        p.ID = rell[0];
                        p.x = double.Parse(rell[1]);
                        p.y = double.Parse(rell[2]);
                        p.z = double.Parse(rell[3]); 
                        All_Points.Add(p);
                        if (p.ID[0] == 'K') points_k.Add(p);
                        else points.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Draw_Grid()
        {
            try
            {
                da.Columns.Clear(); da.Rows.Clear();

                da.Columns.Add("ID", "ID");
                da.Columns.Add("X", "X");
                da.Columns.Add("Y", "Y");
                da.Columns.Add("Z", "Z");

                da.Rows.Add("H0 = ", H0);
                da.Rows.Add("K点集数据");
                foreach (var p in points_k) da.Rows.Add(p.ID, p.x, p.y, p.z);

                da.Rows.Add("离散点集数据");
                foreach (var p in points) da.Rows.Add(p.ID, p.x, p.y, p.z);
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            tabControl1.SelectedTab = tabPage2;

            double angle = Cal_Angle(A, B);

            int d = (int)angle;

            double fri = (angle - d) * 60;
            int m = (int)fri;

            double sec = (fri - m) * 60;
            double s = sec;

            if(s == 60)
            {
                s = 0;
                m += 1;
            }
            if(m == 60)
            {
                m = 0;
                d += 1;
            }

            basic.AppendText("A与B的坐标方位角为：" + d.ToString() + "°" + m.ToString() + "\"" + s.ToString() + "'\n");
        }

        private double Cal_Angle(my_points a, my_points b)
        {
            double angle = 0;
            double dx = b.x - a.x; double dy = b.y - a.y;
            angle = Math.Atan(dy / dx) / Math.PI * 180;

            if (dx < 0 && dy > 0 && angle < 0 || dx < 0 && dy < 0 && angle > 0) angle = angle + 180;
            else if (dx < 0 && dy > 0 && angle < 0) angle = angle + 360;
            else if (dx == 0 && dy > 0) angle = 90;
            else if (dx == 0 && dy < 0) angle = 270;

            return angle;
            
        }

        private double Cal_H(my_points a)
        {
            var cop = points;
            foreach (var p in cop) p.d = Cal_dis(a, p);

            var cur = cop.OrderBy(p => p.d).ToList();

            double l = 0, r = 0;
            for(int i = 0; i<5; i++)
            {
                l += (cur[i].z / cur[i].d);
                r += (1.0 / cur[i].d);
            }

            if (a.ID == "K1")
            {
                for (int i = 0; i < 5; i++)
                    points_k[1].Near_Points.Add(cur[i].ID);
            }

            return l / r;
        }

        private double Cal_H_2(my_points a)
        {
            var cop = All_Points;
            foreach (var p in cop) p.d = Cal_dis(a, p);

            var cur = cop.OrderBy(p => p.d).ToList();

            double l = 0, r = 0;
            for (int i = 0; i < 5; i++)
            {
                l += (cur[i].z / cur[i].d);
                r += (1.0 / cur[i].d);
            }

            if (a.ID == "K1")
            {
                for (int i = 0; i < 5; i++)
                    points_k[1].Near_Points.Add(cur[i].ID);
            }

            return l / r;
        }

        private double Cal_dis(my_points a, my_points b)
        {
            double dx = a.x - b.x; double dy = a.y - b.y;
            double res = Math.Sqrt(dx * dx + dy * dy);
            return res;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
            double res = Cal_Z_H();
            z.AppendText("纵断面总长度为：" + Math.Round(res, 3).ToString() + "\n");
        }

        private double Cal_Z_H()
        {
            double res = 0;
            for(int i = 0; i<points_k.Count - 1; i++)
            {
                res += Cal_dis(points_k[i], points_k[i + 1]);
            }
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            for (int i = 0; i < points_k.Count; i++)
            {
                double res = Cal_H(points_k[i]);
                points_k[i].h = res;
                if(i == 1)
                {
                    basic.AppendText($"{"K1点的高程为 : "}{Math.Round(res, 3),-10}\n");
                    basic.AppendText("距离K1最近点的五个点的坐标为：");
                    for (int j = 0; j < points_k[i].Near_Points.Count(); j++)
                        basic.AppendText($"{points_k[i].Near_Points[j],-10},");
                    basic.AppendText("\n");
                }
            }
        }

        private double Cal_Area(my_points a, my_points b)
        {
            a.h = Cal_H(a); b.h = Cal_H(b);
            double ds = Cal_dis(a, b);
            double res = (a.h + b.h - 2 * H0) * ds / 2;
            return res;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Input_File();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;

            Cal_Site();

            Show_Site();
        }

        private void Cal_Site()
        {
            int cnt = 1;
            int sum = 10;
            for(int i = 0; i<points_k.Count() - 1; i++)
            {
                var P0 = points_k[i];
                var P1 = points_k[i+1];

                double dis = Cal_dis(points_k[0], points_k[1]);

                P.Add(P0);

                if (sum < dis)
                {
                    while (sum < dis)
                    {
                        my_points p = new my_points();
                        p.x = P0.x + sum * Math.Cos(Cal_Angle(P0, P1) / 180 * Math.PI);
                        p.y = P0.y + sum * Math.Sin(Cal_Angle(P0, P1) / 180 * Math.PI);
                        p.h = Cal_H(p);
                        p.ID = "Insert-" + (cnt++).ToString();
                        p.d = sum;
                        P.Add(p);
                        sum += 10;
                    }
                    continue;
                }
                else if (sum > dis && sum <= Cal_dis(points_k[0], points_k[points_k.Count - 1]))
                {
                    while (sum > dis && sum < Cal_dis(points_k[0], P1))
                    {
                        double len = sum - Cal_dis(points_k[0], P0);
                        my_points p = new my_points();
                        p.x = P0.x + len * Math.Cos(Cal_Angle(P0, P1) / 180 * Math.PI);
                        p.y = P0.y + len * Math.Sin(Cal_Angle(P0, P1) / 180 * Math.PI);
                        p.h = Cal_H_2(p);
                        p.ID = "Insert-" + (cnt++).ToString();
                        p.d = sum;
                        P.Add(p);
                        sum += 10;
                    }
                }
            }
            P.Add(points_k[points_k.Count() - 1]);
        }

        private void Show_Site()
        {
            basic.AppendText("\n");
            foreach(var p in P)
            {
                z.AppendText(p.ID + ",\t" + Math.Round(p.d, 3).ToString() + "\t," + Math.Round(p.x, 3).ToString() + "\t," +  Math.Round(p.y, 3).ToString() + "\t," + Math.Round(p.h, 3).ToString() + "\n");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;

            Cal_center();
        }

        private void Cal_center()
        {
            h.AppendText($"{"ID",-10}{"X",-10}{"Y",-10}\n");
            for (int i = 0; i < points_k.Count - 1; i++)
            {
                double xm = points_k[i].x + points_k[i + 1].x;
                double ym = points_k[i].y + points_k[i + 1].y;
                h.AppendText($"{"M" + i.ToString(), -10}{Math.Round(xm / 2, 3), -10} {Math.Round(ym / 2, 3), -10}\n");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;

            for (int i = 0; i<points_k.Count - 1; i++)
            {
                var a = points_k[i];    var b = points_k[i + 1];
                double angle = (Cal_Angle(a, b) + 90) / 180 * Math.PI;
                double xm, ym;
                xm = (a.x + b.x) / 2; ym = (a.y + b.y) / 2;

                M cur = new M();
                int cnt = 0;
                for(int j = -5; j<=5; j++)
                {
                    my_points p = new my_points();
                    p.x = xm + j * 5 * Math.Cos(angle);
                    p.y = ym + j * 5 *  Math.Sin(angle);
                    p.ID = "M-" + (cnt++).ToString();
                    p.h = Cal_H_2(p);
                    p.d = j * 5;
                    cur.Arr.Add(p);
                }

                rell.Add(cur);
            }

            for(int i = 0; i<rell.Count; i++)
            {
                h.AppendText("以M" + i.ToString() + "为中心点的内插点的坐标数据为：\n");
                //h.AppendText("横断面面积为：\n");
                h.AppendText("-------------------------------------------------------------\n");
                h.AppendText("ID"+ ",\t" + "X" + ",\t" + "Y" + "\n");
                foreach (var p in rell[i].Arr)
                    h.AppendText(p.ID + ",\t" + Math.Round(p.x, 3).ToString() + ",\t" + Math.Round(p.y, 3).ToString() + ",\t" + Math.Round(p.h, 3) + "\n");
                h.AppendText("-------------------------------------------------------------\n\n");
            }

            for(int i = 0; i<rell.Count; i++)
            {
                h.AppendText($"{"-----------------------"}M{i.ToString()}{"-------------------------"}\n");
                for(int j = 0; j < rell[i].Arr.Count; j++)
                {
                    var cop = All_Points;

                    foreach(var p in cop)
                    {
                        p.d = Cal_dis(p, rell[i].Arr[j]);
                    }

                    var cur = cop.OrderBy(p => p.d).ToList();

                    h.AppendText($"{"以"}{rell[i].Arr[j].ID}{"为中心周围最近的五个离散点ID为 : "}");
                    for(int r = 0; r<5; r++)
                    {
                        h.AppendText($"{cur[r].ID,-5},");
                    }
                    h.AppendText("\n");
                }
                h.AppendText("\n");
            }
        }

        private void butt_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;

            for(int i = 0; i<rell.Count; i++)
            {
                double res = 0;

                for(int j = 0; j<rell[i].Arr.Count - 1; j++)
                {
                    res += Cal_Area(rell[i].Arr[j], rell[i].Arr[j + 1]);
                }
                h.AppendText($"{"横断面面积为："}{"Area M-" + i.ToString() + " = "}{res}\n");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage5;
            chart1.Series.Clear();

            chart1.ChartAreas[0].Name = "纵断面图像";
            chart1.Series.Add("K点");
            chart1.Series.Add("点位");
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            chart1.ChartAreas[0].AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.SharpTriangle;
            chart1.ChartAreas[0].AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.SharpTriangle;

            int cnt = 0;
            foreach(var p in P)
            {
                double x = Cal_dis(p, points_k[0]);
                double y = p.h;
                chart1.Series[0].Points.AddXY(x, y);
                chart1.Series[1].Points.AddXY(x, y);
                chart1.Series[1].Points[cnt++].Label = p.ID;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
            chart2.Series.Clear();
            chart2.ChartAreas[0].AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.SharpTriangle;
            chart2.ChartAreas[0].AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.SharpTriangle;


            int cnt = 0;
            int temp = 0;
            for (int i = 0; i < rell.Count; i++)
            {
                chart2.Series.Add("M-" + cnt.ToString());
                chart2.Series[(temp++)].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chart2.Series.Add("Points-" + cnt.ToString());
                chart2.Series[(temp++)].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;

                for (int j = 0; j < rell[i].Arr.Count; j++)
                {
                    var p = rell[i].Arr[j];
                    chart2.Series["M-" + cnt.ToString()].Points.AddXY(p.d, p.h);
                    chart2.Series["Points-" + cnt.ToString()].Points.AddXY(p.d, p.h);
                    chart2.Series["Points-" + cnt.ToString()].Points[j].Label = p.ID;
                }
                cnt++;
            }
        }

        private void exitEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void helpHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("第一步：导入数据文件。\n" +
                            "第二步：左侧按键从上到下依次点击，否则程序会产生错误。\n" +
                            "第三步：点击右上方生成计算报告按键生成报告。\n",
                            "Help", MessageBoxButtons.OK);
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Generate_Calculate_Report();
        }

        private void Generate_Calculate_Report()
        {
            report.AppendText("---------------------------------计算报告------------------------------------\n");
            report.AppendText("*********************************基础数据************************************\n");
            report.AppendText(basic.Text + "\n");
            report.AppendText("*********************************纵断面计算数据*************************************\n");
            report.AppendText(z.Text);
            report.AppendText("*********************************横断面计算数据*************************************\n");
            report.AppendText (h.Text);
        }
    }
}
