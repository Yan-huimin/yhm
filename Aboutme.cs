using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gass
{
    public partial class Aboutme : Form
    {
        public Aboutme()
        {
            InitializeComponent();
            string markdown = @"
# 🥰关于我🐼
- QQ: 2234489774
- WeChat: yhm20031225
- Email: yanhuimin434@gmail.com
- Github: https://github.com/Yan-huimin
- MyWeb: https://blog.yhmyo.cn
";

            string html = Markdig.Markdown.ToHtml(markdown);
            webBrowser1.DocumentText = html; // 显示在webBrowser控件中
        }

        private void Aboutme_Load(object sender, EventArgs e)
        {

        }
    }
}
