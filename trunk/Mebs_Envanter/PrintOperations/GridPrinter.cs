using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Mebs_Envanter.PrintOperations
{
    public class GridPrinter
    {

        int _row_count = 0;
        int _col_count = 0;
        float xCoordinate = 0;
        float yCoordinate = 0;
        float width = 0;
        float height = 0;
        
        List<List<PointF>> allPoints = new List<List<PointF>>();
        List<int> xcolperc = new List<int>();
        List<int> ycolperc = new List<int>();
        
        public GridPrinter(RectangleF drawingRectangle,List<int> _xcolperc, List<int> _ycolperc)
        {
            this._col_count = _xcolperc.Count;
            this._row_count = _ycolperc.Count;
            xCoordinate = drawingRectangle.X;
            yCoordinate = drawingRectangle.Y;
            width = drawingRectangle.Width;
            height = drawingRectangle.Height;
            this.xcolperc = _xcolperc;
            this.ycolperc = _ycolperc;
            AssignDrawingPoints();
            getcurrentpercent(xcolperc,60);
        }
        public void PaintGrid(Graphics _gr)
        {
            foreach (List<PointF> item in allPoints)
            {
                int count = item.Count;
                _gr.DrawLine(Pens.Black, item[0], item[count - 1]);
            }

            int verticalCount = allPoints.Count;
            for (int i = 0; i < allPoints[0].Count; i++)
            {
                _gr.DrawLine(Pens.Black, allPoints[0][i], allPoints[verticalCount - 1][i]);
            }

        }

        public float getcurrentpercent(List<int> kriter,int currentperc)
        {
            int sonuc = 0;
            foreach (var item in kriter)
            {                
                sonuc = item + sonuc;                                
            }
            return currentperc /(float)sonuc;
        }

        private void AssignDrawingPoints()
        {
            float currentY = yCoordinate;           
            for (int i = 0; i < _row_count + 1; i++)
            {
                List<PointF> currentHorizontalPoints = new List<PointF>();
                float currentX = xCoordinate;                
                float gapY = 0;
                if (_row_count > i)
                {
                    gapY = (height * getcurrentpercent(ycolperc, ycolperc[i]));
                }
                for (int j = 0; j < _col_count + 1; j++)
                {
                    float gapX=0;
                    if (_col_count  > j) {
                        gapX = (width * getcurrentpercent(xcolperc, xcolperc[j]));
                    }
                     
                    currentHorizontalPoints.Add(new PointF(currentX, currentY));
                    currentX += gapX;
                }
                currentY += gapY;
                allPoints.Add(currentHorizontalPoints);
            }
        }

        public PointF getlocation(int row,int column)
        {

            //PointF p = allPoints[row][column];
            //PointF po = new PointF();
            //po = p;
            //return po;
            return allPoints[row][column];          

        }

        public void printString(Graphics g,int row, int column, Font font, string yazi,bool ortala_dikey,bool ortala_yatay)
        {
            SizeF yaziBoyutu = g.MeasureString(yazi, font);

            PointF row0location = getlocation(row, column);

            PointF row1location = getlocation(row+1, column);

            PointF column1location = getlocation(row, column+1);

            float yukseklikFark = (row1location.Y - row0location.Y);

            float genislikFark = (column1location.X - row0location.X);

            float yaziFarkX = (genislikFark - yaziBoyutu.Width) / 2;

            float yaziFarkiY=(yukseklikFark-yaziBoyutu.Height)/2;


            PointF newYaziLocation = new PointF(row0location.X , row0location.Y);
            if (ortala_dikey) {
                newYaziLocation.Y += yaziFarkiY;                
            }
            if (ortala_yatay) {
                newYaziLocation.X += yaziFarkX;
            }
            g.DrawString(yazi, font, Brushes.Black, newYaziLocation);

            
        }
    }
}
