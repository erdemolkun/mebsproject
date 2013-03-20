using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Mebs_Envanter.PrintOperations;

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


        ////hasta  bilğilerinin etrafını çiz
        //    GraphicsPath path1 = RoundedRectangle.Create(15, 113, 770, (int)yPozisyonTemp - 97);
        //    e.Graphics.DrawPath(Pens.Black, path1);

        //    //hastane bilğilerinin etrafını çiz
        //    GraphicsPath path = RoundedRectangle.Create(15, 15, 770, 80);
        //    e.Graphics.DrawPath(Pens.Black, path);
    }
}
