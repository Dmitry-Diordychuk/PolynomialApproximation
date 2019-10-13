using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Identity.UI.Pages.Internal.Account.Manage;

namespace PolynomialApproximation
{
    public static class Polynomial
    {
        private static List<double> result = new List<double>();
        public static List<double> DividedDifferencesCalculation(double[] x, double[] y)
        {
            
            if (y.Length == 2)
            {
                result.Add(Yi(x[0], y[0], x[x.Length - 1], y[1]));
                return result;
            }
            else
            {
                double[] next_y = new double[y.Length-1];
                for( int i = 0; i < next_y.Length; i++ )
                    next_y[i] = Yi( x[i], y[i], x[( x.Length - y.Length ) + i + 1], y[i + 1] );
                result.Add(next_y[0]);
                DividedDifferencesCalculation( x, next_y );
                return result;
            }
        }

        private static double Yi(double x0, double y0, double x1, double y1)
        {
            return (y0 - y1) / (x0 - x1);
        }

        public static double NewtonePolynomialCalculation(double pointx, double[] x, double[] y, double[] divDiff)
        {
            //P_n(x) += y_0;
            double poly_result_x = y[0];

            double [] xsubxn = new double[x.Length-1];
            //(x-x0), (x-x0)(x-x1), ..., (x-x0)(x-x1)*..*(x-xn)
            for (int i = 0; i < x.Length-1; i++)           
            {
                if (i > 0)
                {
                    xsubxn[i] = xsubxn[i-1] * (pointx - x[i]);
                }
                else
                    xsubxn[i] = pointx - x[i];
            }
            //P_n (x)=y_0+(x-x_0)y(x_0,x_1)+(x-x_0)(x-x_1)y(x_0,x_1,x_2)+...(x-x_0)(x-x_1)...(x-x_(n-1))y(x_0,x_1,...,x_n)
            for (int i = 0; i < xsubxn.Length; i++)
            {
                poly_result_x += xsubxn[i] * divDiff[i];
            }

            return poly_result_x;
        }
    }
}
