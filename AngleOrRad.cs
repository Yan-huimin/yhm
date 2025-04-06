using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Gass
{
    public partial class AngleOrRad : Form
    {
        public AngleOrRad()
        {
            InitializeComponent();
        }

        private void done_Click(object sender, EventArgs e)
        {
            try
            {
                if (angle.Text == "" && rad.Text == "")
                {
                    MessageBox.Show("请输入数据!");
                    return;
                }
                if (angle.Text != "" && rad.Text != "")
                {
                    MessageBox.Show("只能输入角度或弧度其中一个!");
                    return;
                }
                if (angle.Text != "" && rad.Text == "")
                {
                    double ang = double.Parse(angle.Text);
                    double res = ang * Math.PI / 180; // 角度转弧度
                    rad.Text = res.ToString("0.##########"); // 保留10位小数
                }
                if (rad.Text != "" && angle.Text == "")
                {
                    double ra = double.Parse(rad.Text); // 读取弧度值
                    double res = ra * 180 / Math.PI; // 弧度转角度
                    angle.Text = res.ToString("0.##########"); // 保留10位小数
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
            }
        }
    }
}
