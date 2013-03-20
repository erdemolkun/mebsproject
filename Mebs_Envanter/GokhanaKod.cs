using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mebs_Envanter
{
    class GokhanaKod
    {
        PointF pageSize;
        public  float getXByPercantage(float x)
        {
            return (pageSize.X * x) / 100f;
        }
        public  float getYByPercantage(float y)
        {
            return (pageSize.Y * y) / 100f;
        }

        public  RectangleF getPrintingRect(float xPerc, float yPerc, float widthPerc, float heightPerc)
        {
            float h = Math.Abs(getYByPercantage(heightPerc) - getYByPercantage(0));
            float w = Math.Abs(getXByPercantage(widthPerc) - getXByPercantage(0));
            float x = getXByPercantage(xPerc);
            float y = getYByPercantage(yPerc);
            return new RectangleF(x, y, w, h);
        }
    }
}
