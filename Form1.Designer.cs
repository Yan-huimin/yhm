namespace Gass
{
    partial class App
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.File = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFIle = new System.Windows.Forms.ToolStripMenuItem();
            this.导入正算数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导入反算数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DownloadFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.kl = new System.Windows.Forms.ToolStripMenuItem();
            this.iu = new System.Windows.Forms.ToolStripMenuItem();
            this.cg = new System.Windows.Forms.ToolStripMenuItem();
            this.wg = new System.Windows.Forms.ToolStripMenuItem();
            this.Toollist = new System.Windows.Forms.ToolStripMenuItem();
            this.度数转度分秒ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.angleRadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.单点高斯正算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.视图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tooltt = new System.Windows.Forms.ToolStripMenuItem();
            this.statt = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更多ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.da = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.da1 = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.status = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.load_file = new System.Windows.Forms.ToolStripDropDownButton();
            this.高斯正算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.高斯反算ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exit = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.da)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.da1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.File,
            this.toolStripMenuItem1,
            this.Toollist,
            this.视图ToolStripMenuItem,
            this.帮助ToolStripMenuItem,
            this.更多ToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // File
            // 
            this.File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadFIle,
            this.DownloadFile});
            this.File.Name = "File";
            resources.ApplyResources(this.File, "File");
            // 
            // LoadFIle
            // 
            this.LoadFIle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导入正算数据ToolStripMenuItem,
            this.导入反算数据ToolStripMenuItem});
            this.LoadFIle.Name = "LoadFIle";
            resources.ApplyResources(this.LoadFIle, "LoadFIle");
            // 
            // 导入正算数据ToolStripMenuItem
            // 
            this.导入正算数据ToolStripMenuItem.Name = "导入正算数据ToolStripMenuItem";
            resources.ApplyResources(this.导入正算数据ToolStripMenuItem, "导入正算数据ToolStripMenuItem");
            this.导入正算数据ToolStripMenuItem.Click += new System.EventHandler(this.导入正算数据ToolStripMenuItem_Click);
            // 
            // 导入反算数据ToolStripMenuItem
            // 
            this.导入反算数据ToolStripMenuItem.Name = "导入反算数据ToolStripMenuItem";
            resources.ApplyResources(this.导入反算数据ToolStripMenuItem, "导入反算数据ToolStripMenuItem");
            this.导入反算数据ToolStripMenuItem.Click += new System.EventHandler(this.导入反算数据ToolStripMenuItem_Click);
            // 
            // DownloadFile
            // 
            this.DownloadFile.Name = "DownloadFile";
            resources.ApplyResources(this.DownloadFile, "DownloadFile");
            this.DownloadFile.Click += new System.EventHandler(this.DownloadFile_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kl,
            this.iu,
            this.cg,
            this.wg});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // kl
            // 
            this.kl.Name = "kl";
            resources.ApplyResources(this.kl, "kl");
            // 
            // iu
            // 
            this.iu.Name = "iu";
            resources.ApplyResources(this.iu, "iu");
            // 
            // cg
            // 
            this.cg.Name = "cg";
            resources.ApplyResources(this.cg, "cg");
            // 
            // wg
            // 
            this.wg.Name = "wg";
            resources.ApplyResources(this.wg, "wg");
            // 
            // Toollist
            // 
            this.Toollist.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.度数转度分秒ToolStripMenuItem,
            this.angleRadToolStripMenuItem,
            this.单点高斯正算ToolStripMenuItem});
            this.Toollist.Name = "Toollist";
            resources.ApplyResources(this.Toollist, "Toollist");
            // 
            // 度数转度分秒ToolStripMenuItem
            // 
            this.度数转度分秒ToolStripMenuItem.Name = "度数转度分秒ToolStripMenuItem";
            resources.ApplyResources(this.度数转度分秒ToolStripMenuItem, "度数转度分秒ToolStripMenuItem");
            this.度数转度分秒ToolStripMenuItem.Click += new System.EventHandler(this.度数转度分秒ToolStripMenuItem_Click);
            // 
            // angleRadToolStripMenuItem
            // 
            this.angleRadToolStripMenuItem.Name = "angleRadToolStripMenuItem";
            resources.ApplyResources(this.angleRadToolStripMenuItem, "angleRadToolStripMenuItem");
            this.angleRadToolStripMenuItem.Click += new System.EventHandler(this.angleRadToolStripMenuItem_Click);
            // 
            // 单点高斯正算ToolStripMenuItem
            // 
            this.单点高斯正算ToolStripMenuItem.Name = "单点高斯正算ToolStripMenuItem";
            resources.ApplyResources(this.单点高斯正算ToolStripMenuItem, "单点高斯正算ToolStripMenuItem");
            this.单点高斯正算ToolStripMenuItem.Click += new System.EventHandler(this.单点高斯正算ToolStripMenuItem_Click);
            // 
            // 视图ToolStripMenuItem
            // 
            this.视图ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tooltt,
            this.statt});
            this.视图ToolStripMenuItem.Name = "视图ToolStripMenuItem";
            resources.ApplyResources(this.视图ToolStripMenuItem, "视图ToolStripMenuItem");
            // 
            // tooltt
            // 
            this.tooltt.Name = "tooltt";
            resources.ApplyResources(this.tooltt, "tooltt");
            this.tooltt.Click += new System.EventHandler(this.工具栏ToolStripMenuItem_Click);
            // 
            // statt
            // 
            this.statt.Name = "statt";
            resources.ApplyResources(this.statt, "statt");
            this.statt.Click += new System.EventHandler(this.状态栏ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            resources.ApplyResources(this.帮助ToolStripMenuItem, "帮助ToolStripMenuItem");
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // 更多ToolStripMenuItem
            // 
            this.更多ToolStripMenuItem.Name = "更多ToolStripMenuItem";
            resources.ApplyResources(this.更多ToolStripMenuItem, "更多ToolStripMenuItem");
            this.更多ToolStripMenuItem.Click += new System.EventHandler(this.更多ToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.da);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // da
            // 
            this.da.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.da, "da");
            this.da.Name = "da";
            this.da.RowTemplate.Height = 27;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.da1);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // da1
            // 
            this.da1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.da1, "da1");
            this.da1.Name = "da1";
            this.da1.RowTemplate.Height = 27;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.status});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // status
            // 
            this.status.Name = "status";
            resources.ApplyResources(this.status, "status");
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.load_file,
            this.exit,
            this.toolStripLabel1});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // load_file
            // 
            this.load_file.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.load_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.高斯正算ToolStripMenuItem,
            this.高斯反算ToolStripMenuItem});
            this.load_file.Image = global::Gass.Properties.Resources.Cal1;
            this.load_file.Name = "load_file";
            resources.ApplyResources(this.load_file, "load_file");
            // 
            // 高斯正算ToolStripMenuItem
            // 
            this.高斯正算ToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.高斯正算ToolStripMenuItem.Name = "高斯正算ToolStripMenuItem";
            resources.ApplyResources(this.高斯正算ToolStripMenuItem, "高斯正算ToolStripMenuItem");
            this.高斯正算ToolStripMenuItem.Click += new System.EventHandler(this.高斯正算ToolStripMenuItem_Click);
            // 
            // 高斯反算ToolStripMenuItem
            // 
            this.高斯反算ToolStripMenuItem.Name = "高斯反算ToolStripMenuItem";
            resources.ApplyResources(this.高斯反算ToolStripMenuItem, "高斯反算ToolStripMenuItem");
            this.高斯反算ToolStripMenuItem.Click += new System.EventHandler(this.高斯反算ToolStripMenuItem_Click);
            // 
            // exit
            // 
            this.exit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.exit.Image = global::Gass.Properties.Resources.exit;
            this.exit.Name = "exit";
            resources.ApplyResources(this.exit, "exit");
            this.exit.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabel1.LinkColor = System.Drawing.Color.DarkCyan;
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chart1);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            resources.ApplyResources(this.chart1, "chart1");
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Change_size);
            // 
            // App
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "App";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.da)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.da1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Chart1_MouseUp1(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void Chart1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem File;
        private System.Windows.Forms.ToolStripMenuItem LoadFIle;
        private System.Windows.Forms.ToolStripMenuItem DownloadFile;
        private System.Windows.Forms.ToolStripMenuItem Toollist;
        private System.Windows.Forms.ToolStripMenuItem 视图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更多ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tooltt;
        private System.Windows.Forms.ToolStripMenuItem statt;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView da;
        private System.Windows.Forms.ToolStripMenuItem 度数转度分秒ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem angleRadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem kl;
        private System.Windows.Forms.ToolStripMenuItem iu;
        private System.Windows.Forms.ToolStripMenuItem cg;
        private System.Windows.Forms.ToolStripMenuItem wg;
        private System.Windows.Forms.ToolStripMenuItem 单点高斯正算ToolStripMenuItem;
        private System.Windows.Forms.DataGridView da1;
        private System.Windows.Forms.ToolStripMenuItem 导入正算数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导入反算数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton load_file;
        private System.Windows.Forms.ToolStripMenuItem 高斯正算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 高斯反算ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton exit;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}

