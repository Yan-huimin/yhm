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
using System.Reflection;
using Adjustment_course_design;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace 平差课设
{
    public partial class Form1 : Form
    {

        private int n;
        private int t;
        private int r;
        private double Sigma0;
        private double Sigma_3;
        private double Sigma_4;
        private double Sigma_3To4;
        private List<Points> Section = new List<Points>();
        private Matrixs B = new Matrixs();
        private Matrixs l = new Matrixs();
        private Matrixs W = new Matrixs();
        private Matrixs Q = new Matrixs();
        private Matrixs X = new Matrixs();
        private Matrixs P = new Matrixs();
        private Matrixs V = new Matrixs();
        private Matrixs Fi = new Matrixs();
        private Matrixs Qxx = new Matrixs();
        private Matrixs Qvv = new Matrixs();
        private Matrixs Nbb_1 = new Matrixs();
        private bool []st = new bool [1000];
        private double[] High = new double[1000];
        private bool[,] st_line = new bool[1000, 1000];
        private string[] Name_Points = new string[1000];
        private int Unknown_Points;
        private int []index_of_Unknown_Points = new int[1000];
        private int Cnt_Points;
        private List<Site> Sites = new List<Site> ();

        public Form1()
        {
            InitializeComponent();
            this.Text = "水准网简介平差计算";
            Init_tab();
            st_label.Text = "等待操作...";
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;

            Open_Picture();

            st_label.Text = "图像导入成功";
        }

        private void Open_Picture()
        {
            try
            {
                Pic.Class1 c = new Pic.Class1();

                pictureBox1.Image = c.Get_Pic();

                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 导入文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            try
            {
                using (OpenFileDialog file = new OpenFileDialog())
                {
                    file.Filter = "文本文件(*.txt)|*.txt";
                    file.Title = "文件";

                    DialogResult re = file.ShowDialog();
                    if (re == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(file.FileName))
                        {
                            var s = sr.ReadToEnd().Trim().Split('\n');

                            //读取n, t, r, 总点数数值
                            var cur = s[1].Split(',');
                            n = int.Parse(cur[0]);
                            t = int.Parse(cur[1]);
                            r = int.Parse(cur[2]);  
                            Cnt_Points = int.Parse(cur[3]);

                            //初始化每个点的状态为False

                            //读取已知点的点号和高程
                            var ss = s[2].Split(';');
                            for (int i = 0; i < ss.Length; i++)
                            {
                                var r = ss[i].Split(',');
                                st[int.Parse(r[0])] = true;
                                Name_Points[int.Parse(r[0])] = "H" + r[0];
                                High[int.Parse(r[0])] = double.Parse(r[1]);
                            }

                            int cnt = 1;//未知量计数器
                            for (int i = 1; i<= Cnt_Points; i++)
                            {
                                if (st[i] == true) continue;
                                Name_Points[i] = "X" + cnt.ToString();
                                Unknown_Points++;
                                index_of_Unknown_Points[i] = cnt++;
                            }

                            //读取每一条边的数据
                            for (int i = 3; i < s.Length; i++)
                            {
                                Get_All_data(s[i]);
                            }

                            Draw_Grid();
                        }
                    }
                }
                st_label.Text = "文件读取成功...";
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK);
            }
        }

        //处理每一条边并更新每一条边的状态
        private void Get_All_data(string s_1)
        {
            var s = s_1.Split(',');

            var ss = s[0].Split('-');
            Site r = new Site();
            int P1 = int.Parse(ss[0]);  int P2 = int.Parse(ss[1]);//起点与终点的ID
            r.begin = P1;   r.end = P2;//每一条边的起点与终点
            st_line[P1, P2] = false;//将这条边的状态更新为false
            r.Length = double.Parse(s[2]);//测站路线长度，用于计算权阵
            r.site = s[0];//测站号
            r.ID = s[3];//序号
            r.Dif_H = double.Parse(s[1]);//高差
            Sites.Add(r);//将测站加入集合进行后续观测方程的列立
        }


        //列立观测方程以及误差方程
        private void Do_equ()
        {
            test_Box.AppendText("---------------------------设未知数----------------------------\n");
            //设置未知变量X1, X2....
            for (int i = 1; i <= Cnt_Points; i++)
            {
                if (st[i] == true) continue;
                richTextBox1.AppendText(i.ToString() + "号点设为：" + Name_Points[i] + "\n");
            }

            //观测方程
            richTextBox1.AppendText("----------------------------观测值方程---------------------------\n");
            int cnt = 1;//Equ计数器
            foreach (var p in Sites)
            {
                //通式,按读取数据的序号进行列立方程
                //Li + Vi = P_ed - P_st
                int P1 = p.begin; int P2 = p.end;
                richTextBox1.AppendText($"{"L" + cnt.ToString(),-5} + {"V" + cnt++.ToString(),-5} = {Name_Points[P2], -5} - {Name_Points[P1], -5}\n");
            }
        }


        //列立误差方程
        private void Do_wc()
        {
            richTextBox1.AppendText("-------------------------------误差方程----------------------------------\n");
            int cnt = 1;//Equ计数器
            foreach (var p in Sites)
            {
                //通式,按读取数据的序号进行列立方程
                //Li + Vi = P_ed - P_st
                int P1 = p.begin; int P2 = p.end;
                richTextBox1.AppendText($"{"V" + cnt.ToString(),-5} = {Name_Points[P2],-5} - {Name_Points[P1],-5}{((p.Dif_H > 0) ? " + " + p.Dif_H.ToString() + "( L" + cnt++.ToString() + ")" : " - " + Math.Abs(p.Dif_H).ToString() + "( L" + cnt++.ToString() + ")")}\n");
            }
        }

        //列立法方程并初始化B矩阵，l矩阵，权阵
        private void Do_Nor_equ()
        {
            //对B, l, P矩阵进行最初初始化(内部值全为零)
            B.Row = n;  B.Col = Unknown_Points;
            Init_Ori_Matrix(ref B);
            l.Row = n; l.Col = 1;//l的列数一定为零
            Init_Ori_Matrix(ref l);
            P.Col = n;  P.Row = n;
            Init_Ori_Matrix(ref P);
            Q.Col = n; Q.Row = n;
            Init_Ori_Matrix(ref Q);

            //进行B,l矩阵的初始化
            int cnt = 0;//计数器
            foreach (var p in Sites)
            {
                double sum = p.Dif_H;
                List<double> cur = new List<double>();
                int P1 = p.begin; int P2 = p.end;

                //处理l矩阵
                if (st[P1]) sum += High[P1];
                if (st[P2]) sum -= High[P2];
                l.Matrix[cnt][0] = sum;

                //处理P矩阵
                P.Matrix[cnt][cnt] = p.Length;
                Q.Matrix[cnt][cnt] = 1.0 / p.Length;

                //处理B矩阵
                for (int i = 1; i <= Cnt_Points; i++)
                {
                    if (st[i]) continue;
                    int temp = index_of_Unknown_Points[i];
                    if (i == P1) B.Matrix[cnt][temp - 1] = -1;
                    else if (i == P2) B.Matrix[cnt][temp - 1] = 1;
                }
                cnt++;
            }

            //var cur_1 = P;
            //Q = Cal_Inverse(cur_1);

            //输出B矩阵
            richTextBox1.AppendText("B\n");
            Print(B);
            //输出l矩阵
            richTextBox1.AppendText("l\n");
            Print(l);
            //输出Q矩阵
            richTextBox1.AppendText("Q\n");
            Print(P);
            //输出P矩阵
            richTextBox1.AppendText("P\n");
            Print(Q);
        }



        private void Draw_Grid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("序号", "序号");
            dataGridView1.Columns.Add("测段距离", "测段距离");
            dataGridView1.Columns.Add("高程观测值", "高程观测值");
            dataGridView1.Columns.Add("端点号", "端点号");

            dataGridView1.Rows.Add("n = ", n, "t = ", t);

            foreach (var p in Sites)
                dataGridView1.Rows.Add(p.ID, p.Length, p.Dif_H, p.site);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void Init_tab()
        {
            tabPage1.Text = "基础数据&图像";
            tabPage2.Text = "方程";
            tabPage3.Text = "计算数据";
            tabPage4.Text = "计算报告";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;

            Do_equ();

            st_label.Text = "观测值方程列立成功";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;

            Do_wc();

            st_label.Text = "误差方程列立成功";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;

            richTextBox1.AppendText("----------------------------------法方程------------------------------------\n");
            richTextBox1.AppendText("BX - l = 0\n");

            Do_Nor_equ();

            st_label.Text = "法方程列立成功";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;

            Nbb_1 = Cal_Inverse(Cal_Mul(Cal_Mul(Cal_Transpose(B), Q), B));
            Qxx = Nbb_1;
            W = Cal_Mul(Cal_Mul(Cal_Transpose(B), Q), l);
            X = Cal_Mul(Nbb_1, W);

            for(int i = 0; i<Cnt_Points; i++)
            {
                if (st[i]) continue;
                richTextBox2.AppendText("X" + (index_of_Unknown_Points[i]).ToString() + "(H" + i.ToString() + ")" + "高程为：" + (X.Matrix[index_of_Unknown_Points[i]][0] * 1000).ToString("0.000") + "m\n");
            }

            st_label.Text = "数据计算完成...";
        }


        /// <summary>
        /// 这是一个输出矩阵的函数
        /// </summary>
        /// <param name="a">传入参数为Matrix类型</param>
        /// <returns>返回值为一个Matrix</returns>
        private void Print(Matrixs a)
        {
            richTextBox1.AppendText("------------------------Matrix------------------------\n");
            for(int i = 0; i<a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                    richTextBox1.AppendText($"{Math.Round(a.Matrix[i][j], 3),-10}");
                richTextBox1.AppendText("\n");
            }
            richTextBox1.AppendText("\n");
        }

        /// <summary>
        /// 这是一个计算矩阵加法的函数
        /// </summary>
        /// <param name="a">Left</param>
        /// <param name="b">Right</param>
        /// <returns>返回所传入的两个矩阵的和矩阵</returns>
        private Matrixs Cal_Add(Matrixs a, Matrixs b)
        {
            Matrixs res = new Matrixs();
            if (a.Col != b.Col || a.Row != b.Row || a.Row == 0 ||
                b.Row == 0 || a.Col == 0 || b.Col == 0)
            {
                MessageBox.Show("两个矩阵无法进行加法运算，请重新输入矩阵", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return res;
            }

            res.Col = a.Col; res.Row = a.Row;
            Init_Ori_Matrix(ref res);

            for (int i = 0; i < a.Row; i++)
                for (int j = 0; j < a.Col; j++)
                    res.Matrix[i][j] = b.Matrix[i][j] + a.Matrix[i][j];

            return res;
        }

        /// <summary>
        /// 矩阵减法运算
        /// </summary>
        /// <param name="a">Left</param>
        /// <param name="b">Right</param>
        /// <returns>两个矩阵的减法操作后得到的结果</returns>
        private Matrixs Cal_Sub(Matrixs a, Matrixs b)
        {
            Matrixs res = new Matrixs();
            if (a.Col != b.Col || a.Row != b.Row || a.Row == 0 ||
                b.Row == 0 || a.Col == 0 || b.Col == 0)
            {
                MessageBox.Show("两个矩阵无法进行减法运算，请重新输入矩阵", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return res;
            }

            res.Col = a.Col; res.Row = a.Row;
            Init_Ori_Matrix(ref res);

            for (int i = 0; i < a.Row; i++)
                for (int j = 0; j < a.Col; j++)
                    res.Matrix[i][j] = a.Matrix[i][j] - b.Matrix[i][j];

            return res;
        }


        /// <summary>
        /// 矩阵乘法操作
        /// </summary>
        /// <param name="a">Left</param>
        /// <param name="b">Right</param>
        /// <returns>a*b矩阵的结果(注意矩阵左右区别)</returns>
        private Matrixs Cal_Mul(Matrixs a, Matrixs b)
        {
            Matrixs res = new Matrixs();
            if (a.Col != b.Row)
            {
                MessageBox.Show("两个矩阵无法进行乘法运算，请重新输入矩阵", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return res;
            }

            res.Col = b.Col; res.Row = a.Row;
            Init_Ori_Matrix(ref res);

            for (int i = 0; i < res.Row; i++)
                for (int j = 0; j < res.Col; j++)
                {
                    double cur = 0;
                    for (int k = 0; k < a.Col; k++)
                        cur += a.Matrix[i][k] * b.Matrix[k][j];
                    res.Matrix[i][j] = cur;
                }

            return res;
        }

        /// <summary>
        /// 矩阵的转置计算函数
        /// </summary>
        /// <param name="a"></param>
        /// <returns>传入矩阵的转置矩阵</returns>
        private Matrixs Cal_Transpose(Matrixs a)
        {
            Matrixs res = new Matrixs();

            if (a.Row == 0 || a.Col == 0)
            {
                MessageBox.Show("矩阵不合法，请重新输入矩阵", "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            res.Col = a.Row; res.Row = a.Col;
            Init_Ori_Matrix(ref res);

            for (int i = 0; i < res.Row; i++)
                for (int j = 0; j < res.Col; j++)
                    res.Matrix[i][j] = a.Matrix[j][i];

            return res;
        }

        /// <summary>
        /// 逆矩阵运算函数
        /// </summary>
        /// <param name="a"></param>
        /// <returns>传入参数的逆矩阵 (本函数采用高斯约旦消元法计算逆矩阵) </returns>
        private Matrixs Cal_Inverse(Matrixs a)
        {
            Matrixs res = new Matrixs();
            res.Col = a.Col; res.Row = a.Row;

            if (a.Col != a.Row)
            {
                MessageBox.Show("This Matrix is not a square!", "Tips", MessageBoxButtons.OK);
            }

            int n = res.Row;
            //初始化单位矩阵
            for (int i = 0; i < a.Row; i++)
            {
                List<double> cur = new List<double>();
                cur.Clear();
                for (int j = 0; j < a.Col; j++)
                {
                    if (i != j) cur.Add(0);
                    else cur.Add(1);
                }
                res.Matrix.Add(cur);
            }

            for (int i = 0; i < n; i++)
            {
                //枢轴化
                double pivot = a.Matrix[i][i];
                if (pivot == 0)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (a.Matrix[j][i] != 0)
                        {
                            swap(ref a, i, j);
                            swap(ref res, i, j);
                            pivot = a.Matrix[i][i];
                            break;
                        }

                    }
                }

                if (pivot == 0)
                {
                    MessageBox.Show("该矩阵为奇异矩阵，无法进行逆矩阵的运算", "Tips", MessageBoxButtons.OK);
                }

                //归一化枢轴行
                for (int j = 0; j < n; j++)
                {
                    a.Matrix[i][j] /= pivot;
                    res.Matrix[i][j] /= pivot;
                }

                //消元其他行
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        double factror = a.Matrix[j][i];
                        for (int k = 0; k < n; k++)
                        {
                            a.Matrix[j][k] -= factror * a.Matrix[i][k];
                            res.Matrix[j][k] -= factror * res.Matrix[i][k];
                        }
                    }
                }
            }

            return res;
        }

        /// <summary>
        /// 进行矩阵的初等行变换
        /// </summary>
        /// <param name="a"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void swap(ref Matrixs a, int x, int y)//进行初等行变换
        {
            for (int i = 0; i < a.Col; i++)
            {
                double cur = a.Matrix[x][i];
                a.Matrix[x][i] = a.Matrix[y][i];
                a.Matrix[y][i] = cur;
            }
            return;
        }


        /// <summary>
        /// 初始化传入的矩阵，内部全部初始化为0
        /// </summary>
        /// <param name="res"></param>
        private void Init_Ori_Matrix(ref Matrixs res)
        {
            for (int i = 0; i < res.Row; i++)
            {
                List<double> cur = new List<double>();
                cur.Clear();
                for (int j = 0; j < res.Col; j++)
                    cur.Add(0);
                res.Matrix.Add(cur);
            }
        }

        private void 保存报告文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog sv = new SaveFileDialog())
                {
                    sv.Filter = "文本文件*.txt|*.txt";
                    sv.Title = "选择文件位置";

                    DialogResult re = sv.ShowDialog();
                    if(re == DialogResult.OK)
                    {
                        string path = sv.FileName;
                        Save_Calcate_Report(path);
                    }
                }

                st_label.Text = "文件保存成功...";
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK);
            }
        }


        /// <summary>
        /// 保存计算报告文件
        /// </summary>
        /// <param name="path"></param>
        private void Save_Calcate_Report(string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.Write(rt.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;

            //初始化V矩阵
            V.Col = 1;  V.Row = n;
            Init_Ori_Matrix(ref V);

            int cnt = 0;
            foreach(var p in Sites)
            {
                int P1 = p.begin;   int P2 = p.end;
                double res = 0;
                if (st[P1]) 
                    res -= High[P1];
                else
                {
                    double value = X.Matrix[index_of_Unknown_Points[P1] - 1][0];
                    res -= value;
                }
                if (st[P2]) 
                    res += High[P2];
                else
                {
                    double value = X.Matrix[index_of_Unknown_Points[P2] - 1][0];
                    res += value;
                }
                res -= p.Dif_H;
                V.Matrix[cnt++][0] = res;
            }
            richTextBox2.AppendText("\n*************************************精度评定*************************************\n");

            var cur = Cal_Mul(Cal_Mul(Cal_Transpose(V), Q), V);
            double temp = cur.Matrix[0][0];
            Sigma0 = Math.Sqrt(temp / r);
            richTextBox2.AppendText("Sigma0 = " + (Sigma0*1000).ToString("0.000") + "mm\n");

            //未知点精度评定
            for (int i = 1; i<=Cnt_Points; i++)
            {
                if (st[i]) continue;
                int index = index_of_Unknown_Points[i];
                double res = 0;
                res = Sigma0 * Math.Sqrt(Qxx.Matrix[index - 1][index - 1]);
                richTextBox2.AppendText(i.ToString() + "号点精度为：" + (res*1000).ToString("0.000") + "mm\n");
            }

            //测边精度评定
            int rell = 1;//点号计数器
            foreach(var p in Sites)
            {
                int P1 = p.begin;   int P2 = p.end;
                Matrixs F = new Matrixs();

                //初始化F矩阵
                F.Col = 1;  F.Row = Unknown_Points;
                Init_Ori_Matrix(ref F);

                if (!st[P1]) F.Matrix[index_of_Unknown_Points[P1] - 1][0] = -1;//起点为-1
                if (!st[P2]) F.Matrix[index_of_Unknown_Points[P2] - 1][0] = 1;//终点为1

                double res = 0;
                var temp_1 = Cal_Mul(Cal_Mul(Cal_Transpose(F), Qxx), F);
                res = Math.Sqrt(temp_1.Matrix[0][0]) * Sigma0;

                richTextBox2.AppendText(rell++.ToString() + "号测站的高差精度为：" + (res * 1000).ToString("0.000") + "mm\n");
            }
            richTextBox2.AppendText("精度评定结果均保留三位小数.\n");

            st_label.Text = "精度评定完成...";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
            Generate_Report();

            st_label.Text = "计算报告生成完成...";
        }

        /// <summary>
        /// 生成计算报告
        /// </summary>
        private void Generate_Report()
        {
            rt.AppendText("##################################################计算报告##################################################\n");
            rt.AppendText("\nn = " + n.ToString() + "\n" + "t = " + t.ToString() + "\n" + "r = " + r.ToString() + "\n");
            rt.AppendText("\n*************************************使用公式*************************************\n");
            rt.AppendText("r = n - t;\nBX - l = V;\nW = Transport(B)Pl;\nNbb = Transpose(B)PB;\nQxx = Nbb^-1;\nφ = -1*X1 + 1*X2 + 0*X3\nQfifi = Transpose(Fi)QxxFi");
            rt.AppendText("\n*************************************计算结果*************************************\n");
            rt.AppendText("X\n");
            Print_1(X);
            rt.AppendText("V\n");
            Print_1(V);
            rt.AppendText("\n*************************************中间变量*************************************\n");
            rt.AppendText("B\n");
            Print_1(B);
            rt.AppendText("l\n");
            Print_1(l);
            rt.AppendText("W\n");
            Print_1(W);
            rt.AppendText("Nbb^-1\n");
            Print_1(Nbb_1);
            rt.AppendText("Q\n");
            Print_1(P);
            rt.AppendText("Qxx\n");
            Print_1(Qxx);
            rt.AppendText(richTextBox2.Text);
        }


        /// <summary>
        /// 打印矩阵
        /// </summary>
        /// <param name="a"></param>
        private void Print_1(Matrixs a)
        {
            rt.AppendText("\n------------------------Matrix------------------------\n");
            for (int i = 0; i < a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                    rt.AppendText($"{Math.Round(a.Matrix[i][j], 3),-10}");
                rt.AppendText("\n");
            }
            rt.AppendText("\n");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("先点击左侧的File或者OpenFile按键\n随后自顶向下依次点击左侧按键。\n", "Help", MessageBoxButtons.OK);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void 计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
