using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using PolynomialApproximation.Models;

namespace PolynomialApproximation.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public void OnPostLeft()
        {
            if (InputPageData.Size > 1)
                InputPageData.Size--;
        }

        public void OnPostRight()
        {
            InputPageData.Size++;
        }

        public void OnPostEnter(List<InputPageData> userInputs)
        {
            double[] x = (from item in userInputs select item.X).ToArray();
            double[] y = (from item in userInputs select item.Y).ToArray();

            List<double> result = new List<double>();
            result = Polynomial.DividedDifferencesCalculation(x, y);

            double p_x = Polynomial.NewtonePolynomialCalculation(InputPageData.Point, x, y, result.ToArray());

            int min = (int)Math.Floor(x[0]);
            int max = (int)Math.Ceiling(x[^1]);

            int length = max - min;

            MyFunc.x = new double[length * 10 + 1];
            MyFunc.y = new double[length * 10 + 1];

            for (int i = 0; i < length * 10 + 1; i++)
            {
                MyFunc.x[min+i] = Math.Round(0.1 * i, 2);
                MyFunc.y[min + i] = Function.pow( MyFunc.x[min + i] );
            }

            MyFunc.ourX = InputPageData.Point;
            MyFunc.ourY = p_x;
        }

        public ActionResult OnGetAjaxAsync()
        {
            double[] x = MyFunc.x;
            double[] y = MyFunc.y;

            return new JsonResult( new
            {
                myX = x,
                myY = y,
                ourX = MyFunc.ourX,
                ourY = MyFunc.ourY
            } );
            
        }
    }
}
