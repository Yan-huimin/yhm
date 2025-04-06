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
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();

            // 初始化帮助页面的Web浏览器控件
            string markdown = @"# 高斯正反算帮助

### 文件格式与功能说明
1. **输入格式**：
   - **正算输入**：`id,B_dd.mmss,L_dd.mmss,central_meridian`  
     示例：`P1,39.5430,116.2845,117.0`（纬度39°54'30""、经度116°28'45""、中央子午线117.0°）
   - **反算输入**：`id,x_coord,y_coord,central_meridian`  
     示例：`P1,4426633.12,39504694.56,117.0`（X/Y坐标单位：米）

2. **核心功能**：
   - **格式转换**：`dd.mmss⇄十进制角度`、`角度⇄弧度`
   - **坐标计算**：
     - 正算（BL→XY）：`P1,39.5430,116.2845 → 4426633.12,39504694.56`
     - 反算（XY→BL）：`P1,4426633.12,39504694.56 → 39.5430,116.2845`
   - **批量处理**：支持txt文件导入导出

3. **技术要求**：
   - 椭球参数：支持四种椭球
     - 克拉索夫斯基椭球体（6378245.0, 6356863.01877304）
     - IUGG 1975 椭球（6378140.0, 6356755.28815752）
     - CGCS2000坐标系椭球（6378137.0, 6356752.31414036）
     - WGS84椭球体（6378137.0, 6356752.31424517）
   - 计算精度：≥4位小数
   - 异常检测：自动识别非法坐标格式

4. **样例代码生成**
    - 高斯正算
    ```python
    import random
    import math

    def generate_ddmmss(min_deg=0, max_deg=90):
        # 生成dd.mmss格式的坐标
        degrees = random.randint(min_deg, max_deg)
        minutes = random.randint(0, 59)
        seconds = random.randint(0, 59)
        return f""{degrees:02d}.{minutes:02d}{seconds:02d}""

    def calculate_6degree_zone(lon_degree):
        # 计算6度带中央子午线
        zone_number = math.floor(lon_degree / 6) + 1
        return 6 * zone_number - 3

    def calculate_3degree_zone(lon_degree):
        # 计算3度带中央子午线
        zone_number = math.floor((lon_degree - 1.5) / 3) + 1
        return 3 * zone_number

    data = []
    for point_num in range(1, 21):
        B = generate_ddmmss(0, 90)  # 纬度
        L_ddmmss = generate_ddmmss(0, 180)  # 经度
    
        # 从dd.mmss提取十进制度数
        deg_part, minsec_part = L_ddmmss.split('.')
        degrees = float(deg_part)
        minutes = float(minsec_part[:2])
        seconds = float(minsec_part[2:])
        L_degree = degrees + minutes/60 + seconds/3600
    
        # 前10条用6度带，后10条用3度带
        if point_num <= 10:
            zone_type = ""6degree""
            central_meridian = calculate_6degree_zone(L_degree)
        else:
            zone_type = ""3degree""
            central_meridian = calculate_3degree_zone(L_degree)
    
        data.append(f""{point_num},{B},{L_ddmmss},{central_meridian}"")

    with open(""gauss_data.txt"", ""w"") as f:
        f.write(""\n"".join(data))

    print(""数据已生成到 gauss_data.txt 文件"")
    ```

    - 高斯反算
    ```python
    import numpy as np

    def generate_gauss_data(num_rows=20):
        """"""
        生成高斯反算计算数据
        参数:
            num_rows: 生成数据的行数
        返回:
            包含行号, x, y, 中央子午线经度的numpy数组
        """"""
        row_numbers = np.arange(1, num_rows + 1).reshape(-1, 1)
    
        x = np.random.uniform(300000, 500000, num_rows).reshape(-1, 1)
    
        y = np.random.uniform(2000000, 4000000, num_rows).reshape(-1, 1)
    
        central_meridians = [75, 102, 105, 108, 111, 114, 117, 120, 123, 126, 129]
        central_meridian = np.random.choice(central_meridians, num_rows).reshape(-1, 1)
    
        data = np.hstack((row_numbers, x, y, central_meridian))
    
        return data

    def save_to_txt(data, filename='data.txt'):
        """"""
        将数据保存到文件
        参数:
            data: 要保存的数据
            filename: 保存的文件名
        """"""
        fmt = ['%d', '%.3f', '%.3f', '%d']  # 中央子午线经度改为整数
    
        np.savetxt(filename, data, fmt=fmt, delimiter=',', header='', comments='')

    data = generate_gauss_data(20)

    save_to_txt(data)

    print(""数据已生成并保存到gauss_data.txt文件中"")

    ```
";

            string html = Markdig.Markdown.ToHtml(markdown);
            webBrowser2.DocumentText = html;
        }
    }
}
