using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 文件操作_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            text_date_locate.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "仅TXT文件 (*.txt)|*.txt";
            openFileDialog.Title = "获取文件位置";
            openFileDialog.Multiselect = true;

            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                text_date_locate.Text = openFileDialog.FileName.ToString();
            else
            {
                MessageBox.Show("出现错误：", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string file_path = text_date_locate.Text.ToString();

            txt_date.Clear();
            try
            {
                ASCIIEncoding ae = new ASCIIEncoding();
                FileStream date = new FileStream(file_path, FileMode.Open);
                int length = (int)date.Length;

                Byte[] buffer = new Byte[length];
                date.Read(buffer, 0, length);
                for(int i = 0; i < length; i++)
                {
                    txt_date.Text += (char)buffer[i];
                }
            }
            catch(FileNotFoundException ex)
            {
                MessageBox.Show("未找到相关文件：" + ex.Message);
                return;
            }
            catch(IOException ex)
            {
                MessageBox.Show("接口错误：" + ex.Message);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("发生错误：" + ex.Message);
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FileStream fileStream = new FileStream(text_date_locate.Text, FileMode.Append);

            ASCIIEncoding ar = new ASCIIEncoding();
            byte[]writebyte = ar.GetBytes(write_date.Text);
            for(int i = 0; i<writebyte.Length; i++)
            {
                fileStream.WriteByte(writebyte[i]);
            }
            fileStream.Close();
            write_date.Clear();
        }

        private void write_date_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
