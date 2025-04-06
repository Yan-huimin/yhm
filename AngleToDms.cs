using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Gass
{
    public partial class AngleToDms : Form
    {
        public AngleToDms()
        {
            InitializeComponent();
        }

        private void AngleToDms_Load(object sender, EventArgs e)
        {
            this.Text = "度分秒与角度转换";
        }

        private void Change_Click(object sender, EventArgs e)
        {
            try
            {
                if (D == null || M == null || S == null)
                {
                    MessageBox.Show("请先输入度、分、秒");
                    return;
                }
                double d = double.Parse(D.Text);
                double m = double.Parse(M.Text);
                double s = double.Parse(S.Text);
                if (d < 0 || m < 0 || s < 0)
                {
                    MessageBox.Show("度、分、秒不能为负数");
                    return;
                }
                if (m >= 60 || s >= 60.0)
                {
                    MessageBox.Show("分不能大于59，秒不能大于59");
                    return;
                }
                double res = d + m / 60 + s / 3600;
                this.Angle.Text = res.ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            try
            {
                if (Angle == null)
                {
                    MessageBox.Show("请输入数据");
                    return;
                }

                fun(double.Parse(this.Angle.Text));
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }

        private void fun(double decimalDegree)
        {
            // 获取度数（整数部分）
            int degree = (int)decimalDegree;

            // 获取分钟数（小数部分 * 60）
            double minuteDecimal = (decimalDegree - degree) * 60;
            int minute = (int)minuteDecimal;

            // 获取秒数（分钟部分的小数 * 60）
            double secondDecimal = (minuteDecimal - minute) * 60;
            int second = (int)Math.Round(secondDecimal);

            this.D.Text = degree.ToString();
            this.M.Text = minute.ToString();
            this.S.Text = second.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.Angle == null)
            {
                MessageBox.Show("内容为空!");
                return;
            }
            Clipboard.SetText(Angle.Text);
        }
    }
}
