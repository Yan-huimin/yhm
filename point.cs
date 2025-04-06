using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmojiSharp;
using Markdig;

namespace Gass
{
    public partial class point : Form
    {
        private double[] at = new double[4] { 6378245.0, 6378140.0, 6378137.0, 6378137.0 };
        private double[] bt = new double[4] { 6356863.01877304, 6356755.28815752, 6356752.31414036, 6356752.31424517 };
        private string[] name = new string[4] { "克拉索夫斯基椭球体", "IUGG 1975 椭球", "CGCS2000坐标系椭球", "WGS84椭球体" };

        public point()
        {
            InitializeComponent();
            Init_Web();
            this.tabPage1.Text = "高斯正算";
            this.tabPage2.Text = "高斯反算";
            this.tabPage3.Text = "帮助";
            this.Res_A.ReadOnly = true;
            this.Res_B.ReadOnly = true;
        }

        private void Init_Web()
        {
            // 初始化帮助页面的Web浏览器控件
            string markdown = @"# 高斯正反算帮助
## 高斯正算
- B -> 输入值为纬度（度数）可使用app中的工具进行计算
- L -> 输入值为经度（度数）可使用app中的工具进行计算
- Angle -> 中央子午线经度，输入值为度数，范围0~360
- elli -> 选择椭球类型，0~3分别对应不同的椭球体

## 高斯反算
- X -> 输入值为高斯平面坐标X，通常是正算后得到的X（单位：m）
- Y -> 输入值为高斯平面坐标Y，通常是正算后得到的Y（单位：m）
- Angle -> 中央子午线经度，输入值为度数，范围0~360
- elli -> 选择椭球类型，0~3分别对应不同的椭球体

## 注意事项
对于elli参数的选择，请参考以下对应关系：
- 0 : 克拉索夫斯基椭球体
- 1 ： IUGG 1975 椭球
- 2 ： CGCS2000坐标系椭球
- 3 ： WGS84椭球体
";

            string html = Markdown.ToHtml(markdown);
            webBrowser1.DocumentText = html;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                double B = double.Parse(this.B.Text);
                double L = double.Parse(this.L.Text);
                double angle = double.Parse(this.Angle.Text);
                int index = int.Parse(this.elli.Text);
                My_Point p = new My_Point();
                p.B = B;
                p.L = L;
                p.Angle = angle;

                if(index < 0 || index >= 4)
                {
                    throw new FormatException("椭球下标超限，应为0~3");
                }

                Cal_A(ref p, index);

                Res_A.Text = $"X: {p.X.ToString("F3")}, Y: {p.Y_3.ToString("F3")}";
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message + "\n请检查输入数据是否正确，或是否选择了椭球类型。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Cal_A(ref My_Point p, int index)
        {
            double a = at[index];
            double b = bt[index];
            //带号选择
            int num_3 = p.Angle % 3 == 0 ? (int)(p.Angle / 3) : -1;
            int num_6 = (p.Angle + 3) % 6 == 0 ? (int)((p.Angle + 3) / 6) : -1;
            if (num_3 < 0 && num_6 < 0)
            {
                throw new Exception("中央子午线经度不符合3或6的倍数");
            }

            // 计算自然坐标
            double e_2 = (a * a - b * b) / (a * a);
            double e_2_ = e_2 / (1 - e_2);
            double f = (a - b) / a;
            double B = angleToRad(p.B);
            double L = angleToRad(p.L);
            double l = p.L - p.Angle;
            l = angleToRad(l);
            double t = tan(B);
            double eplo_2 = e_2_ * pow(cos(B), 2);
            double N = a / sqrt(1 - e_2 * pow(sin(B), 2));

            double m0 = a * (1 - e_2);
            double m2 = 3.0 / 2.0 * e_2 * m0;
            double m4 = 5.0 / 4.0 * e_2 * m2;
            double m6 = 7.0 / 6.0 * e_2 * m4;
            double m8 = 9.0 / 8.0 * e_2 * m6;

            double a0 = m0 + 0.5 * m2 + 3.0 / 8.0 * m4 + 5.0 / 16.0 * m6 + 35.0 / 128.0 * m8; // a0系数
            double a2 = 1.0 / 2.0 * m2 + 1.0 / 2.0 * m4 + 15.0 / 32.0 * m6 + 7.0 / 16.0 * m8; // a2系数
            double a4 = 1.0 / 8.0 * m4 + 3.0 / 16.0 * m6 + 7.0 / 32.0 * m8; // a4系数
            double a6 = 1.0 / 32.0 * m6 + 1.0 / 16.0 * m8;
            double a8 = 1.0 / 128.0 * m8;

            double M = m0 + m2 * pow(sin(B), 2) + m4 * pow(sin(B), 4) + m6 * pow(sin(B), 6) + m8 * pow(sin(B), 8);
            double X = a0 * B - a2 / 2 * sin(2 * B) + a4 / 4 * sin(4 * B) - a6 / 6 * sin(6 * B) + a8 / 8 * sin(8 * B);

            double x = X + N / 2 * sin(B) * cos(B) * l * l + N / 24 * sin(B) * pow(cos(B), 3) * (5 - t * t + 9 * eplo_2 + 4 * pow(eplo_2, 2)) * pow(l, 4) + N / 720 * sin(B) * pow(cos(B), 5) * (61 - 58 * t * t + pow(t, 4)) * pow(l, 6);
            double y = N * cos(B) * l + N / 6 * pow(cos(B), 3) * (1 - t * t + eplo_2) * pow(l, 3) + N / 120 * pow(cos(B), 5) * (5 - 18 * t * t + pow(t, 4) + 14 * eplo_2 - 58 * eplo_2 * t * t) * pow(l, 5);

            double y_3 = num_3 == -1 ? -1 : num_3 * 1e6 + y + 5e5;
            double y_6 = num_6 == -1 ? -1 : num_6 * 1e6 + y + 5e5;

            p.X = x;
            p.Y_3 = y_3;
            p.Y_6 = y_6;
            p.index = index;
        }

        private double angleToRad(double angle) { return angle * Math.PI / 180.0; }

        private double radToAngle(double rad) { return rad / Math.PI * 180.0; }

        private double abs(double num) { return Math.Abs(num); }
        private double sin(double num) { return Math.Sin(num); }
        private double cos(double num) { return Math.Cos(num); }
        private double tan(double num) { return Math.Tan(num); }
        private double atan(double num) { return Math.Atan(num); }
        private double sqrt(double num) { return Math.Sqrt(num); }
        private double pow(double num, int x) { return Math.Pow(num, x); }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                double x = double.Parse(this.X.Text); // X
                double y = double.Parse(this.Y.Text.Substring(2)); // Y_3
                double angle = double.Parse(this.Angle_B.Text);
                int index = int.Parse(this.elli_B.Text); // 椭球下标

                if (index < 0 || index >= 4)
                {
                    throw new FormatException("椭球下标超限，应为0~3");
                }

                points p = new points(); // 创建一个points对象，用于存储计算结果

                p.x = x; p.y = y;
                p.Angle = angle;

                Cal_B(ref p, index);

                Res_B.Text = $"B: {p.B.ToString("F3")}, L: {p.L.ToString("F3")}";
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message + "\n请检查输入数据是否正确，或是否选择了椭球类型。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Cal_B(ref points p, int index)
        {
            double a = at[index]; double b = bt[index];

            double e_2 = (a * a - b * b) / (a * a);
            double e_2_ = e_2 / (1 - e_2);
            double f = (a - b) / a;

            double x = p.x;
            double y = p.y - 5e5;

            double m0 = a * (1 - e_2);
            double m2 = 3.0 / 2.0 * e_2 * m0;
            double m4 = 5.0 / 4.0 * e_2 * m2;
            double m6 = 7.0 / 6.0 * e_2 * m4;
            double m8 = 9.0 / 8.0 * e_2 * m6;

            double a0 = m0 + 0.5 * m2 + 3.0 / 8.0 * m4 + 5.0 / 16.0 * m6 + 35.0 / 128.0 * m8; // a0系数
            double a2 = 1.0 / 2.0 * m2 + 1.0 / 2.0 * m4 + 15.0 / 32.0 * m6 + 7.0 / 16.0 * m8; // a2系数
            double a4 = 1.0 / 8.0 * m4 + 3.0 / 16.0 * m6 + 7.0 / 32.0 * m8; // a4系数
            double a6 = 1.0 / 32.0 * m6 + 1.0 / 16.0 * m8;
            double a8 = 1.0 / 128.0 * m8;

            double bf = x / a0;
            double bf0 = bf;
            double cur = bf0;

            while (true)
            {
                bf0 = bf;
                double fbf = -a2 / 2.0 * sin(2 * bf0) + a4 / 4.0 * sin(4 * bf0) - a6 / 6.0 * sin(6 * bf0) + a8 / 8.0 * sin(8 * bf0);
                bf = (x - fbf) / a0;
                if (abs(bf - bf0) < 1e-6)
                    break;
            }

            double mf = a * (1 - e_2) / pow(sqrt(1 - e_2 * sin(bf) * sin(bf)), 3);
            double nf = a / sqrt(1 - e_2 * sin(bf) * sin(bf));
            double tf = tan(bf);
            double eplo_2 = e_2_ * cos(bf) * cos(bf);

            double B = bf - tf / (2 * mf * nf) * y * y + tf / (24 * mf * pow(nf, 3)) * (5 + 3 * tf * tf + eplo_2 - 9 * eplo_2 * tf * tf) * pow(y, 4);
            double l = 1.0 / (nf * cos(bf)) * y - 1.0 / (6 * pow(nf, 3) * cos(bf)) * (1 + 2 * tf * tf + eplo_2) * pow(y, 3);

            double L = radToAngle(l) + p.Angle;

            p.B = radToAngle(B);
            p.L = L;
        }
    }
}
