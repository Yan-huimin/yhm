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

namespace 大地线长度计算
{
    public partial class Form1 : Form
    {

        private double a, b, f;
        private List<my_point> Points = new List<my_point>();
        private double e2, e_2;

        public Form1()
        {
            InitializeComponent();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 选选择数据文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open_File();
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Calcate();

            Generate_report();
        }

        private void Open_File()
        {
            try
            {
                OpenFileDialog file = new OpenFileDialog();

                file.Filter = "所有文件(*.txt)|*.txt";
                file.Title = "选择文件";

                DialogResult re = file.ShowDialog();
                if(re == DialogResult.OK)
                {
                    string path = file.FileName;

                    Show_data(path);

                    Draw_Grid();
                }
        }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message, "Tips", MessageBoxButtons.OKCancel);
            }
}

        private void Show_data(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    var s = sr.ReadToEnd().Trim().Split('\n');

                    var cur = s[0].Split(',');
                    a = double.Parse(cur[0]);
                    f = 1.0 / double.Parse(cur[1]);

                    for(int i = 2; i<s.Length; i++)
                    {
                        var r = s[i].Split(',');

                        my_point p = new my_point();
                        p.st_id = r[0]; p.end_id = r[3];
                        fun1(ref r[1], ref p);
                        fun2(ref r[2], ref p);
                        fun3(ref r[4], ref p);
                        fun4(ref r[5], ref p);

                        Points.Add(p);
                    }
                }
        }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message, "Tips", MessageBoxButtons.OKCancel);
            }
}

        private void fun1(ref string s, ref my_point p)
        {
            int i = 0;
            for(; s[i] != '.'; i++)     p.wd1 = p.wd1 * 10 + double.Parse(s[i].ToString());

            i++;
            int cnt = 0;
            for (; cnt < 2; i++, cnt++) p.wm1 = p.wm1 * 10 + double.Parse(s[i].ToString());

            //string最后会有一个/r，因此应该循环到s.Length - 1;
            for (; i < s.Length; i++) p.ws1 = p.ws1 * 10 + double.Parse(s[i].ToString());
            p.ws1 = p.ws1 / 10;
        }

        private void fun2(ref string s, ref my_point p)
        {
            int i = 0;
            for (; s[i] != '.'; i++) p.jd1 = p.jd1 * 10 + double.Parse(s[i].ToString());

            i++;
            int cnt = 0;
            for (; cnt < 2; i++, cnt++) p.jm1 = p.jm1 * 10 + double.Parse(s[i].ToString());

            for (; i < s.Length; i++) p.js1 = p.js1 * 10 + double.Parse(s[i].ToString());
            p.js1 = p.js1 / 10;
        }

        private void fun3(ref string s, ref my_point p)
        {
            int i = 0;
            for (; s[i] != '.'; i++) p.wd2 = p.wd2 * 10 + double.Parse(s[i].ToString());

            i++;
            int cnt = 0;
            for (; cnt < 2; i++, cnt++) p.wm2 = p.wm2 * 10 + double.Parse(s[i].ToString());

            for (; i < s.Length; i++) p.ws2 = p.ws2 * 10 + double.Parse(s[i].ToString());
            p.ws2 = p.ws2 / 10;
        }

        private void fun4(ref string s, ref my_point p)
        {
            int i = 0;
            for (; s[i] != '.'; i++) p.jd2 = p.jd2 * 10 + double.Parse(s[i].ToString());

            i++;
            int cnt = 0;
            for (; cnt < 2; i++, cnt++) p.jm2 = p.jm2 * 10 + double.Parse(s[i].ToString());

            cnt = 0;
            for (; cnt<3; i++, cnt++) p.js2 = p.js2 * 10 + double.Parse(s[i].ToString());
            p.js2 = p.js2 / 10;
        }

        private void Draw_Grid()
        {
            d.Columns.Clear();
            d.Rows.Clear();

            d.Columns.Add("起始点ID", "起始点ID");
            d.Columns.Add("经度(°)", "经度(°)");
            d.Columns.Add("经度(′)", "经度(′)");
            d.Columns.Add("经度(\")", "经度(\")");
            d.Columns.Add("纬度(°)", "纬度(°)");
            d.Columns.Add("纬度(′)", "纬度(′)");
            d.Columns.Add("纬度(\")", "纬度(\")");

            d.Columns.Add("终止点ID", "终止点ID");
            d.Columns.Add("经度(°)", "经度(°)");
            d.Columns.Add("经度(′)", "经度(′)");
            d.Columns.Add("经度(\")", "经度(\")");
            d.Columns.Add("纬度(°)", "纬度(°)");
            d.Columns.Add("纬度(′)", "纬度(′)");
            d.Columns.Add("纬度(\")", "纬度(\")");

            foreach(var p in Points)
            {
                d.Rows.Add(p.st_id, p.wd1, p.wm1, p.ws1, p.jd1, p.jm1, p.js1,
                           p.end_id, p.wd2, p.wm2, p.ws2, p.jd2, p.jm2, p.js2);
            }

            d.Rows.Add("椭球长半轴a = ", a, "扁率f = ", f);
        }

        private void 保存计算报告ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sr = new SaveFileDialog();

                sr.Filter = "所有文件(*.txt)|*.txt";
                sr.Title = "选择文件位置";

                DialogResult re = sr.ShowDialog();
                if(re == DialogResult.OK)
                {
                    using (StreamWriter s = new StreamWriter(sr.FileName))
                    {
                        s.WriteLine(r.Text);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误" + ex.Message, "Tips", MessageBoxButtons.OKCancel);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Calcate()
        {
            b = a - a * f;
            e2 = 1 - (b * b) / (a * a);
            e_2 = e2 / (1 - e2);
            double pi = Math.PI;

            foreach(var p in Points)
            {
                double sb1 = (p.wd1 + p.wm1 / 60 + p.ws1 / 3600) / 180 * pi;
                double sl1 = (p.jd1 + p.jm1 / 60 + p.js1 / 3600) / 180 * pi;
                double sb2 = (p.wd2 + p.wm2 / 60 + p.ws2 / 3600) / 180 * pi;
                double sl2 = (p.jd2 + p.jm2 / 60 + p.js2 / 3600) / 180 * pi;

                p.u1 = Math.Atan(Math.Sqrt(1 - e2) * Math.Tan(sb1));
                p.u2 = Math.Atan(Math.Sqrt(1 - e2) * Math.Tan(sb2));

                p.l = sl2 - sl1;

                p.a1 = Math.Sin(p.u1) * Math.Sin(p.u2);
                p.a2 = Math.Cos(p.u1) * Math.Cos(p.u2);
                p.b1 = Math.Cos(p.u1) * Math.Sin(p.u2);
                p.b2 = Math.Sin(p.u1) * Math.Cos(p.u2);

                double fi = 0, fi_1 = 0;
                while(true)
                {
                    p.lamta = fi + p.l;
                    double p1 = Math.Cos(p.u2) * Math.Sin(p.lamta);
                    double q1 = p.b1 - p.b2 * Math.Cos(p.lamta);
                    p.A1 = Math.Atan(p1 / q1);

                    if (p1 > 0 && q1 > 0) p.A1 = Math.Abs(p.A1);
                    else if (p1 > 0 && q1 < 0) p.A1 = pi - Math.Abs(p.A1);
                    else if (p1 < 0 && q1 < 0) p.A1 = pi + Math.Abs(p.A1);
                    else if (p1 < 0 && q1 > 0) p.A1 = 2 * pi - Math.Abs(p.A1);

                    if (p.A1 < 0) p.A1 = p.A1 + 2 * pi;
                    else if (p.A1 > 2 * pi) p.A1 = p.A1 - 2 * pi;

                    double Sin_s = p1 * Math.Sin(p.A1) + q1 * Math.Cos(p.A1);
                    double Cos_s = p.a1 + p.a2 * Math.Cos(p.lamta);
                    p.sigma = Math.Atan(Sin_s / Cos_s);

                    if (Cos_s > 0) p.sigma = Math.Abs(p.sigma);
                    else if (Cos_s < 0) p.sigma = pi - Math.Abs(p.sigma);

                    p.SinA0 = Math.Cos(p.u1) * Math.Sin(p.A1);
                    p.sigma1 = Math.Atan(Math.Tan(p.u1) / Math.Cos(p.A1));

                    double CosA0 = Math.Sqrt(1 - p.SinA0 * p.SinA0);
                    p.aerf = (e2 / 2 + e2 * e2 / 8 + e2 * e2 * e2 / 16) - (e2 * e2 / 16 + e2 * e2 * e2 / 16) * CosA0 * CosA0 + (3 * e2 * e2 * e2 / 128) * Math.Pow(CosA0, 4);
                    p.bet = (e2 * e2 / 16 + e2 * e2 * e2 / 16) * CosA0 * CosA0 - (e2 * e2 * e2 / 32) * Math.Pow(CosA0, 4);
                    p.garma = (e2 * e2 * e2 / 256) * Math.Pow(CosA0, 4);

                    fi = (p.aerf * p.sigma + p.bet * Math.Cos(2 * p.sigma1 + p.sigma) * Math.Sin(p.sigma) + p.garma * Math.Sin(2 * p.sigma) * Math.Cos(4 * p.sigma1 + 2 * p.sigma)) * p.SinA0;

                    if (Math.Abs(fi - fi_1) < 1e-10 || fi == fi_1) break;
                    else   fi_1 = fi;
                }

                double k2 = e_2 * Math.Sqrt(1 - p.SinA0 * p.SinA0)*Math.Sqrt(1 - p.SinA0*p.SinA0);
                p.A = (1 - k2 / 4 + 7 * k2 * k2 / 64 - 15 * k2 * k2 * k2 / 256) / b;
                p.B = (k2 / 4 - k2 * k2 / 8 + 37 * k2 * k2 * k2 / 512);
                p.C = k2 * k2 / 128 - k2 * k2 * k2 / 128;

                p.sigma1 = Math.Atan(Math.Tan(p.u1) / Math.Cos(p.A1));
                double xs = p.C * Math.Sin(2 * p.sigma) * Math.Cos(4 * p.sigma1 + 2 * p.sigma);
                p.S = (p.sigma - p.B * Math.Sin(p.sigma) * Math.Cos(2 * p.sigma1 + p.sigma) - xs) / p.A;
            }
        }

        private void Generate_report()
        {
            var p = Points[0];
            r.Text += "a = " + a.ToString() + "\n";
            r.Text += "1/f = " + Math.Round(1 / f, 3).ToString() + "\n";
            r.Text += "f = " + Math.Round(f, 8) + "\n";
            r.Text += "b = " + Math.Round(b, 3) + "\n";
            r.Text += "e^2 = " + Math.Round(e2, 8) + "\n";
            r.Text += "e'^2 = " + Math.Round(e_2, 8) + "\n";
            r.Text += "u1 = " + Math.Round(p.u1, 8) + "\n";
            r.Text += "u2 = " + Math.Round(p.u2, 8) + "\n";
            r.Text += "l(弧度) = " + Math.Round(p.l, 8) + "\n";
            r.Text += "a1 = " + Math.Round(p.a1, 8) + "\n";
            r.Text += "a2 = " + Math.Round(p.a2, 8) + "\n";
            r.Text += "b1 = " + Math.Round(p.b1, 8) + "\n";
            r.Text += "b2 = " + Math.Round(p.b2, 8) + "\n";
            r.Text += "aerf = " + Math.Round(p.aerf, 8) + "\n";
            r.Text += "β = " + Math.Round(p.bet, 8) + "\n";
            r.Text += "γ = " + Math.Round(p.garma, 8) + "\n";
            r.Text += "A1(弧度) = " + Math.Round(p.A1, 8) + "\n";
            r.Text += "lamta = " + Math.Round(p.lamta, 8) + "\n";
            r.Text += "sigma = " + Math.Round(p.sigma, 8) + "\n";
            r.Text += "SinA0 = " + Math.Round(p.SinA0, 8) + "\n";
            r.Text += "A = " + Math.Round(p.A, 8) + "\n";
            r.Text += "B = " + Math.Round(p.B, 8) + "\n";
            r.Text += "C = " + Math.Round(p.C, 8) + "\n";
            r.Text += "sigma1 = " + Math.Round(p.sigma1, 8) + "\n";

            for (int i = 0; i < Points.Count; i++)
                r.Text += "第 " + i.ToString() + " 条大地线长S" + i.ToString() + " = " + Math.Round(Points[i].S, 3) + "\n";
        }
    }
}
