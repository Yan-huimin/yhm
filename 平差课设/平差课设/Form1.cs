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
        private double H1;
        private double H2;

        public Form1()
        {
            InitializeComponent();
            n = 7;  t = 3;  r = n - t;
            Init_tab();
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
            Open_Picture();
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
            Input_Data();
        }

        private void Input_Data()
        {
            try
            {

                Show_data();

                Draw_Grid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Show_data()
        {
            try
            {
                data.Class1 c = new data.Class1();

                string path = c.Get_resource_name();

                var s = path.Trim().Split('\n');

                    for(int i = 1; i<s.Count() - 1; i++)
                    {
                        var r = s[i].Split(',');
                        Points cur = new Points();

                        cur.ID = r[0];
                        cur.S = double.Parse(r[1]);
                        cur.H_dif = double.Parse(r[2]);
                        cur.sepoint = r[3];

                        Section.Add(cur);
                    }
                    var ss = s[s.Count() - 1].Split(',');
                    H1 = double.Parse(ss[0]);
                    H2 = double.Parse(ss[1]);

            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Draw_Grid()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Add("序号", "序号");
            dataGridView1.Columns.Add("测段距离", "测段距离");
            dataGridView1.Columns.Add("高程观测值", "高程观测值");
            dataGridView1.Columns.Add("端点号", "端点号");

            foreach (var p in Section) dataGridView1.Rows.Add(p.ID, p.S, p.H_dif, p.sepoint);
            dataGridView1.Rows.Add("H1 == ", H1, "H2 == ", H2);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Input_Data();
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
            try
            {
                My_data.Class1 c = new My_data.Class1();
                    var s = c.Get_equa().Trim().Split('\n');

                    richTextBox1.AppendText("-------------------------------观测值方程-----------------------------\n");
                    for (int i = 8; i <= 14; i++) richTextBox1.AppendText(s[i] + "\n");
                    tabControl1.SelectedTab = tabPage2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                My_data.Class1 c = new My_data.Class1();
                    var s = c.Get_equa().Trim().Split('\n');

                    richTextBox1.AppendText("-------------------------------误差方程-----------------------------\n");
                    for (int i = 17; i <= 23; i++) richTextBox1.AppendText(s[i] + "\n");
                    tabControl1.SelectedTab = tabPage2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                My_data.Class1 c = new My_data.Class1();
                    var s = c.Get_equa().Trim().Split('\n');

                    richTextBox1.AppendText("-------------------------------法方程-----------------------------\n");
                    for (int i = 26; i <= 35; i++) richTextBox1.AppendText(s[i] + "\n");
                    tabControl1.SelectedTab = tabPage2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;

            Cal_All();

            richTextBox2.AppendText("运算结果为：\n");
            richTextBox2.AppendText("H3的高程为：" + X.Matrix[0][0].ToString("0.000") + "m\n");
            richTextBox2.AppendText("H4的高程为：" + X.Matrix[1][0].ToString("0.000") + "m\n");
            richTextBox2.AppendText("H5的高程为：" + X.Matrix[2][0].ToString("0.000") + "m\n");
        }

        private void Cal_All()
        {
            Get_B();
            Get_l();
            Get_Q();
            Get_P();
            Nbb_1 = Cal_Nbb_1();
            W = Cal_W();

            X = Cal_Mul(Nbb_1, W);

            richTextBox2.AppendText("X\n");
            Print(X);

            Cal_V();
            Print(V);
        }


        private void Cal_V()
        {
            V.Row = Section.Count();
            V.Col = X.Matrix[0].Count();
            Init_Ori_Matrix(ref V);

            richTextBox2.AppendText("V\n");
            V.Matrix[0][0] = X.Matrix[0][0] - H1 - Section[0].H_dif;
            V.Matrix[1][0] = X.Matrix[1][0] - H1 - Section[1].H_dif;
            V.Matrix[2][0] = X.Matrix[0][0] - H2 - Section[2].H_dif;
            V.Matrix[3][0] = X.Matrix[1][0] - H2 - Section[3].H_dif;
            V.Matrix[4][0] = X.Matrix[1][0] - X.Matrix[0][0] - Section[4].H_dif;
            V.Matrix[5][0] = X.Matrix[2][0] - X.Matrix[0][0] - Section[5].H_dif;
            V.Matrix[6][0] = H2 - X.Matrix[2][0] - Section[6].H_dif;
        }

        private Matrixs Cal_Nbb_1()
        {
            Matrixs res = new Matrixs();
            Init_Ori_Matrix(ref res);

            //var cur_1 = Cal_Transpose(B);
            ////Print(cur_1);
            //var cur_2 = Cal_Mul(cur_1, P);
            ////Print(cur_2);
            //var cur_3 = Cal_Mul(cur_2, B);
            ////Print(cur_3);

            //Print(Cal_Inverse(cur_3));

            res = Cal_Inverse(Cal_Mul(Cal_Mul(Cal_Transpose(B), P), B));
            //Print(res);

            return res;
        }


        private Matrixs Cal_W()
        {
            Matrixs res = new Matrixs();
            Init_Ori_Matrix(ref res);

            res = Cal_Mul(Cal_Transpose(B), Cal_Mul(P, l));

            return res;
        }

        private void Get_Q()
        {
            Q.Col = Section.Count();    Q.Row = Section.Count();
            for (int i = 0; i < Section.Count; i++)
            {
                List<double> cur = new List<double>();
                for (int j = 0; j < Section.Count; j++)
                {
                    if (i == j)
                    {
                        cur.Add(Section[i].S);
                        continue;
                    }
                    cur.Add(0);
                }
                Q.Matrix.Add(cur);
            }
        }


        private void Get_P()
        {
            P.Col = Section.Count(); P.Row = Section.Count();
            for (int i = 0; i < Section.Count; i++)
            {
                List<double> cur = new List<double>();
                for (int j = 0; j < Section.Count; j++)
                {
                    if (i == j)
                    {
                        cur.Add(1.0/Section[i].S);
                        continue;
                    }
                    cur.Add(0);
                }
                P.Matrix.Add(cur);
            }
        }

        private void Get_B()
        {
            try
            {
                My_data.Class1 c = new My_data.Class1();
                var s = c.Get_B().Trim().Split('\n');
                B.Row = s.Count();
                B.Col = s[0].Split(',').Count();

                for(int i = 0; i<s.Count(); i++)
                {
                    List<double> cur = new List<double>();
                    var ss = s[i].Split(',');
                    for (int j = 0; j < ss.Count(); j++)
                        cur.Add(double.Parse(ss[j]));
                    B.Matrix.Add(cur);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Get_l()
        {
            try
            {
                My_data.Class1 c = new My_data.Class1();
                    var s = c.Get_l().Trim().Split('\n');
                    l.Row = s.Count();
                    l.Col = s[0].Split(',').Count();

                    for (int i = 0; i < s.Count(); i++)
                    {
                        List<double> cur = new List<double>();
                        cur.Add(double.Parse(s[i]));
                        l.Matrix.Add(cur);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Print(Matrixs a)
        {
            richTextBox2.AppendText("------------------------Matrix------------------------\n");
            for(int i = 0; i<a.Row; i++)
            {
                for (int j = 0; j < a.Col; j++)
                    richTextBox2.AppendText($"{Math.Round(a.Matrix[i][j], 3),-10}");
                richTextBox2.AppendText("\n");
            }
            richTextBox2.AppendText("\n");
        }

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
            //Print(res);
            //MessageBox.Show("计算完成！", "Tips", MessageBoxButtons.OK);
        }

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
            //Print(res);
            //MessageBox.Show("计算完成！", "Tips", MessageBoxButtons.OK);
        }


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
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message, "Tips", MessageBoxButtons.OK);
            }
        }

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
            richTextBox2.AppendText("----------------------------精度评定-----------------------------\n");

            Cal_Sigma0();

            //三号点四号点中误差
            Cal_F_T();

            //3-4高差中误差
            Cal_High_Difference_Of_3to_4();
        }


        private void Cal_Sigma0()
        {
            var cur = Cal_Mul(Cal_Mul(Cal_Transpose(V), P), V);
            double temp = cur.Matrix[0][0];
            Sigma0 = Math.Sqrt(temp / r);
        }

        private void Cal_F_T()
        {
            double res_1 = 0;
            double res_2 = 0;
            Qxx = Nbb_1;

            res_1 = Sigma0 * Math.Sqrt(Qxx.Matrix[0][0]);
            res_2 = Sigma0 * Math.Sqrt(Qxx.Matrix[1][1]);

            Sigma_3 = res_1;
            Sigma_4 = res_2;

            richTextBox2.AppendText("3号点的中误差为：" + (res_1 * 1000).ToString("0.00") + "mm\n");
            richTextBox2.AppendText("4号点的中误差为：" + (res_2 * 1000).ToString("0.00") + "mm\n");
        }

        private void Cal_High_Difference_Of_3to_4()
        {
            double res = 0;
            Fi.Col = 1; Fi.Row = 3;
            List<double> a = new List<double>();
            List<double> b = new List<double>();
            List<double> c = new List<double>();
            a.Add(-1);
            b.Add(1);
            c.Add(0);
            Fi.Matrix.Add(a);
            Fi.Matrix.Add(b);
            Fi.Matrix.Add(c);

            var temp = Cal_Mul(Cal_Mul(Cal_Transpose(Fi), Nbb_1), Fi);
            res = temp.Matrix[0][0];

            Sigma_3To4 = Math.Sqrt(res) * Sigma0;

            richTextBox2.AppendText("3-4高差的中误差为：" + (Sigma_3To4 * 1000).ToString("0.00") + "mm\n");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
            Generate_Report();
        }

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
            rt.AppendText("l\n");
            Print_1(l);
            rt.AppendText("Fi\n");
            Print_1(Fi);
            rt.AppendText("Nbb^-1\n");
            Print_1(Nbb_1);
            rt.AppendText("Q\n");
            Print_1(Q);
            rt.AppendText("Qxx\n");
            Print_1(Qxx);
            rt.AppendText("\n*************************************精度评定*************************************\n");
            rt.AppendText("Sigma0 = " + (Sigma0 * 1000).ToString("0.00") + "mm\n");
            rt.AppendText("3号点的中误差为：\n" + (Sigma_3 * 1000).ToString("0.00") + "mm\n");
            rt.AppendText("4号点的中误差为：\n" + (Sigma_4 * 1000).ToString("0.00") + "mm\n");
            rt.AppendText("3号点-->4号点的高差中误差为：" + (Sigma_3To4 * 1000).ToString("0.00") + "mm\n");
            rt.AppendText("（计算结果保留两位小数）\n");
        }

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
    }
}
