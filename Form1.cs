using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gass
{
    public partial class App : Form
    {
        private List<My_Point> my_points = new List<My_Point>();
        private List<points> points = new List<points>();
        private AngleToDms angleToDmsForm = null;
        private AngleOrRad angleOrRad = null;
        private Help help = null;
        private point pp = null;
        private Aboutme about = null;
        private bool A_Cal = false;
        private bool B_Cal = false;
        private double[] at = new double[4] { 6378245.0, 6378140.0, 6378137.0, 6378137.0 };
        private double[] bt = new double[4] { 6356863.01877304, 6356755.28815752, 6356752.31414036, 6356752.31424517 };
        private string[] name = new string[4] { "克拉索夫斯基椭球体", "IUGG 1975 椭球", "CGCS2000坐标系椭球", "WGS84椭球体" };

        public App()
        {
            InitializeComponent();
            INIT();
        }

        private void INIT()
        {
            da.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // 自动填充列宽
            da1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tabPage1.Text = "原始数据";
            tabPage2.Text = "计算结果";
            tabPage3.Text = "图像";
            chart1.Series.Clear();
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.ArrowStyle = AxisArrowStyle.SharpTriangle;
            chart1.ChartAreas[0].AxisY.ArrowStyle = AxisArrowStyle.SharpTriangle;
            toolStripLabel1.Text = "Designed by yhm";
            this.statt.CheckOnClick = true;
            this.tooltt.CheckOnClick = true;
            this.statt.Checked = true;
            this.tooltt.Checked = true;
            this.wg.CheckOnClick = true;
            this.cg.CheckOnClick = true;
            this.iu.CheckOnClick = true;
            this.kl.CheckOnClick = true;
        }

        private void LoadFile()
        {
            try
            {
                using (OpenFileDialog file = new OpenFileDialog())
                {
                    file.Filter = "文本文件(*.txt)|*.txt";
                    file.Title = "文件选择";

                    var res = file.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        GetData(file.FileName);

                        ShowData();

                        status.Text = "文件导入成功";
                    }
                    else
                    {
                        status.Text = "文件导入失败";
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show("错误信息：" + ex.Message);
            }
        }

        private void LoadFile_B()
        {
            try
            {
                using (OpenFileDialog file = new OpenFileDialog())
                {
                    file.Filter = "文本文件(*.txt)|*.txt";
                    file.Title = "文件选择";

                    var res = file.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        GetData_B(file.FileName);

                        ShowData_B();

                        status.Text = "反算文件导入成功";
                    }
                    else
                    {
                        status.Text = "反算文件导入失败";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message);
            }
        }

        private void GetData_B(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    /*
                     * 文件格式
                     * 横坐标，纵坐标，中央子午线经度
                     */
                    var r = sr.ReadToEnd().Trim().Split('\n');

                    for (int i = 0; i < r.Length; i++)
                    {
                        var s = r[i].Split(',');

                        points p = new points();
                        p.id = int.Parse(s[0]);
                        p.x = double.Parse(s[1]);
                        p.y = double.Parse(s[2].Substring(2));
                        p.Angle = double.Parse(s[3]);
                        points.Add(p);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message);
            }
        }


        private void ShowData_B()
        {
            da.ClearSelection();
            da.Columns.Clear(); da.Rows.Clear();
            da.Columns.Add("ID", "ID");
            da.Columns.Add("X", "X");
            da.Columns.Add("Y", "Y");
            da.Columns.Add("Angle", "Angle");

            foreach(var p in points)
            {
                da.Rows.Add(p.id, p.x, p.y, p.Angle);
            }
        }

        /// <summary>
        /// 读取文件数据
        /// </summary>
        /// <param name="file"></param>
        private void GetData(string file)
        {
            try
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    /*
                     * 文件格式
                     * B 纬度, L 经度, angle 中央子午线经度
                     */
                    var r = sr.ReadToEnd().Trim().Split('\n');

                    for (int i = 0; i < r.Length; i++)
                    {
                        var s = r[i].Split(',');

                        My_Point p = new My_Point();

                        p.id = int.Parse(s[0]);
                        MatchDms(s[1], ref p, true);
                        MatchDms(s[2], ref p, false);
                        p.Angle = double.Parse(s[3]);
                        my_points.Add(p);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("错误信息：" + ex.Message);
            }
        }

        private void ShowData()
        {
            da.Columns.Clear(); da.Rows.Clear();

            da.Columns.Add("ID", "ID");
            da.Columns.Add("B", "B");
            da.Columns.Add("BD", "BD");
            da.Columns.Add("BM", "BM");
            da.Columns.Add("BS", "BS");
            da.Columns.Add("L", "L");
            da.Columns.Add("LD", "LD");
            da.Columns.Add("LM", "LM");
            da.Columns.Add("LS", "LS");
            da.Columns.Add("中央子午线经度", "中央子午线经度");

            for(int i = 0; i < my_points.Count; i++)
            {
                try
                {
                    var p = my_points[i];
                    da.Rows.Add(p.id, p.B, p.Bd, p.Bm, p.Bs, p.L, p.Ld, p.Lm, p.Ls, p.Angle);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("在处理第 " + (i + 1) + " 行数据时发生错误：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MatchDms(string num, ref My_Point p, bool is_B)
        {
            var cur = num.ToString();
            // 定义正则表达式来匹配 dd.mmss 格式
            string pattern = @"^(\d+)\.(\d{2})(\d{2})$";

            // 使用正则表达式匹配输入
            Regex regex = new Regex(pattern);
            Match match = regex.Match(cur);

            // 如果匹配成功，提取度、分、秒
            if (match.Success)
            {
                int degree = int.Parse(match.Groups[1].Value);  // 提取度
                int minute = int.Parse(match.Groups[2].Value);  // 提取分
                int second = int.Parse(match.Groups[3].Value);  // 提取秒

                if (is_B)
                {
                    p.Bd = degree;   // 纬度的度
                    p.Bm = minute;
                    p.Bs = second;   // 纬度的秒
                    p.B = dmsToAngle(degree, minute, second);
                }
                else
                {
                    p.Ld = degree;   // 经度的度
                    p.Lm = minute;   // 经度的分
                    p.Ls = second;   // 经度的秒
                    p.L = dmsToAngle(degree, minute, second);
                }
            }
            else
            {
                throw new FormatException("输入格式不正确，应为 dd.mmss 格式。");
            }
        }

        private double dmsToAngle(int degree, int minute, int second)
        {
            /*
             * 将度分秒转换为弧度
             * 弧度 = (度 + 分/60 + 秒/3600) * π / 180
             */
            double decimalDegrees = degree + (minute / 60.0) + (second / 3600.0);
            return decimalDegrees;
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void 度数转度分秒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(angleToDmsForm == null || angleToDmsForm.IsDisposed) // 如果窗体已经被关闭，重新创建
            {
                angleToDmsForm = new AngleToDms();
                angleToDmsForm.FormBorderStyle = FormBorderStyle.FixedDialog;  // 禁止调整大小
                angleToDmsForm.MaximizeBox = false;  // 禁止最大化按钮
                angleToDmsForm.StartPosition = FormStartPosition.CenterScreen;  // 将窗口居中显示
                angleToDmsForm.Show();
            }
            else
            {
                MessageBox.Show("请勿多次打开","", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Cal_A(ref My_Point p)
        {
            //椭球选择
            int index = 0;
            index = !kl.IsDisposed ? 0 : index; // 克拉索夫斯基椭球
            index = !iu.IsDisposed ? 1 : index; // 国际椭球
            index = !cg.IsDisposed ? 2 : index; // 谢尔顿椭球
            index = !wg.IsDisposed ? 3 : index; // WGS-84椭球
            // 若未选择，则默认使用克拉索夫斯基椭球
            double a = at[index];   double b = bt[index];

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

            double a0 = m0 + 0.5 * m2 + 3.0/8.0* m4 + 5.0/16.0 * m6 + 35.0 / 128.0 * m8; // a0系数
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

        private void 高斯正算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (my_points.Count == 0) 
            {
                MessageBox.Show("请导入数据!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            for (int i = 0; i < my_points.Count; i++)
            {
                try
                {
                    var cur = my_points[i];
                    Cal_A(ref cur);
                    my_points[i] = cur;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Text = "在计算第 " + (i + 1) + " 个点时发生错误：" + ex.Message,
                        "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            tabControl1.SelectedTab = tabPage2;

            Draw_A();

            A_Cal = true;

            status.Text = "正算计算完成......";

            Show_Res();
        }

        private void Draw_A()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();

            Series A = new Series()
            {
                Name = "高斯正算图像",
                ChartType = SeriesChartType.Line,
                Color = Color.Blue,
            };

            Series A_P = new Series()
            {
                Name = "点",
                ChartType = SeriesChartType.Point,
                Color = Color.Red,
            };

            foreach(var p in my_points)
            {
                A.Points.AddXY(p.X, p.Y_3);
                A_P.Points.AddXY(p.X, p.Y_3);
                A_P.Points[A_P.Points.Count - 1].Label = p.id.ToString();
            }

            chart1.Titles.Add("高斯正算坐标图像");
            chart1.Titles.Add("鼠标滑轮可缩放图像");
            chart1.Series.Add(A);
            chart1.Series.Add(A_P);
        }

        private void Show_Res()
        {
            da1.Columns.Clear();
            da1.Rows.Clear();

            da1.Columns.Add("id", "id");
            da1.Columns.Add("X", "X");
            da1.Columns.Add("Y_3", "Y_3");
            da1.Columns.Add("Y_6", "Y_6");
            da1.Columns.Add("index", "index");

            foreach(var p in my_points)
            {
                da1.Rows.Add(p.id, p.X, p.Y_3, p.Y_6, p.index);
            }
        }

        private void 高斯反算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (points.Count == 0)
            {
                MessageBox.Show("请导入数据!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            for (int i = 0; i < points.Count; i++)
            {
                try
                {
                    var cur = points[i];
                    Cal_B(ref cur);
                    points[i] = cur;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(Text = "在计算第 " + (i + 1) + " 个点时发生错误：" + ex.Message,
                        "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            B_Cal = true;

            tabControl1.SelectedTab = tabPage2;

            Draw_B();

            status.Text = "反算计算完成......";

            Show_Res_B();
        }

        private void Draw_B()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();

            Series B = new Series()
            {
                Name = "高斯反算图像",
                ChartType = SeriesChartType.Line,
                Color = Color.Blue,
            };

            Series B_P = new Series()
            {
                Name = "点",
                ChartType = SeriesChartType.Point,
                Color = Color.Red,
            };

            foreach (var p in points)
            {
                B.Points.AddXY(p.x, p.y);
                B_P.Points.AddXY(p.x, p.y);
                B_P.Points[B_P.Points.Count - 1].Label = p.id.ToString();
            }

            chart1.Titles.Add("高斯反算坐标图像");
            chart1.Titles.Add("鼠标滑轮可缩放图像");
            chart1.Series.Add(B);
            chart1.Series.Add(B_P);
        }

        private void Show_Res_B()
        {
            da1.Columns.Clear();
            da1.Rows.Clear();

            da1.Columns.Add("id", "id");
            da1.Columns.Add("B", "B");
            da1.Columns.Add("L", "L");

            foreach (var p in points)
            {
                da1.Rows.Add(p.id, p.B, p.L);
            }
        }

        private void Cal_B(ref points p)
        {
            //椭球选择
            int index = 0;
            index = !kl.IsDisposed ? 0 : index; // 克拉索夫斯基椭球
            index = !iu.IsDisposed ? 1 : index; // 国际椭球
            index = !cg.IsDisposed ? 2 : index; // 谢尔顿椭球
            index = !wg.IsDisposed ? 3 : index; // WGS-84椭球
            // 若未选择，则默认使用克拉索夫斯基椭球
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
                double fbf = -a2 / 2.0 * sin(2 * bf0) + a4 / 4.0 * sin(4 * bf0) - a6 / 6.0 * sin(6 * bf0) +a8 / 8.0 * sin(8 * bf0);
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

        private void 导入正算数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            my_points.Clear();

            A_Cal = false;

            // 反算数据清空
            points.Clear();

            LoadFile();
        }

        private void 导入反算数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            B_Cal = false;

            points.Clear();

            // 正算数据清空
            my_points.Clear();

            LoadFile_B();
        }

        private void DownloadFile_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void SaveFile()
        {
            if (!A_Cal && !B_Cal)
            {
                MessageBox.Show("请进行计算!"); 
                return;
            }
            using (SaveFileDialog sv = new SaveFileDialog())
            {
                try
                {
                    sv.Filter = "文本文件(*.txt)|*.txt";
                    sv.Title = "文件保存";

                    var res = sv.ShowDialog();
                    if (res == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(sv.FileName))
                        {
                            if (A_Cal)
                            {
                                sw.WriteLine("**********************正算计算结果********************");
                                sw.WriteLine($"{name[my_points[0].index]}");
                                sw.WriteLine("点号,x,y");

                                foreach (var p in my_points)
                                {
                                    sw.WriteLine($"{p.id}, {p.X}, {p.Y_3}, {p.Y_6}");
                                }
                            }
                            else
                            {
                                sw.WriteLine("**********************反算计算结果********************");
                                sw.WriteLine($"{name[points[0].index]}");
                                sw.WriteLine("点号,B,L");
                                foreach (var p in points)
                                {
                                    sw.WriteLine($"{p.id}, {p.B}, {p.L}");
                                }
                            }
                        }
                        status.Text = "文件保存成功: " + sv.FileName;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("发生错误："+ ex.Message);
                }
            }
        }

        private void 工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = toolStrip1.Visible ? false : true;
        }

        private void 状态栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Visible = statusStrip1.Visible ? false : true;
        }

        private void angleRadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (angleOrRad == null || angleOrRad.IsDisposed) // 如果窗体已经被关闭，重新创建
            {
                angleOrRad = new AngleOrRad();
                angleOrRad.FormBorderStyle = FormBorderStyle.FixedDialog;  // 禁止调整大小
                angleOrRad.MaximizeBox = false;  // 禁止最大化按钮
                angleOrRad.StartPosition = FormStartPosition.CenterScreen;  // 将窗口居中显示
                angleOrRad.Show();
            }
            else
            {
                MessageBox.Show("请勿多次打开", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void 单点高斯正算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pp == null || pp.IsDisposed) // 如果窗体已经被关闭，重新创建
            {
                pp = new point();
                pp.FormBorderStyle = FormBorderStyle.FixedDialog;  // 禁止调整大小
                pp.MaximizeBox = false;  // 禁止最大化按钮
                pp.StartPosition = FormStartPosition.CenterScreen;  // 将窗口居中显示
                pp.Show();
            }
            else
            {
                MessageBox.Show("请勿多次打开", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (help == null || help.IsDisposed) // 如果窗体已经被关闭，重新创建
            {
                help = new Help();
                help.FormBorderStyle = FormBorderStyle.FixedDialog;  // 禁止调整大小
                help.MaximizeBox = false;  // 禁止最大化按钮
                help.StartPosition = FormStartPosition.CenterScreen;  // 将窗口居中显示
                help.Show();
            }
            else
            {
                MessageBox.Show("请勿多次打开", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void 更多ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (about == null || about.IsDisposed) // 如果窗体已经被关闭，重新创建
            {
                about = new Aboutme();
                about.FormBorderStyle = FormBorderStyle.FixedDialog;  // 禁止调整大小
                about.MaximizeBox = false;  // 禁止最大化按钮
                about.StartPosition = FormStartPosition.CenterScreen;  // 将窗口居中显示
                about.Show();
            }
            else
            {
                MessageBox.Show("请勿多次打开", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        /// <summary>
        /// 图像缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
    }
}
