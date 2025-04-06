using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gass
{
    internal class My_Point
    {
        public int id;
        public double Bd, Bm, Bs, B;// 纬度 其中B存储的是度数，并不是Rad格式
        public double Ld, Lm, Ls, L;// 经度
        public double Angle;// 中央子午线经度

        public double X = 0;
        public double Y_3 = 0;
        public double Y_6 = 0;
        public int index = 0;
    }
}
