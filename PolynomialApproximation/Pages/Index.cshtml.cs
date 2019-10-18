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
            InputPageData.PointOfInteresting = Convert.ToDouble(Request.Form["PointOfInteresting"]);

            List<double> result = new List<double>(Polynomial.DividedDifferencesCalculation(x, y));
            Polynomial.result.Clear();
            double p_x = Polynomial.NewtonePolynomialCalculation(InputPageData.PointOfInteresting, x, y, result.ToArray());

            double stepInter = x[1] - x[0];
            //Подберем шаг графика, нужна проверка на направление!
            double stepOfGraph = -1;
            for (int i = 1; i < x.Length; i++)
            {
                if (x[i] > InputPageData.PointOfInteresting)
                {
                    stepOfGraph = InputPageData.PointOfInteresting - x[i - 1];
                    break;
                }
            }

            int length = -1;
            if(stepOfGraph != -1)
                length = (int)Math.Round( (x[^1] - x[0])/stepOfGraph );

            if (length != -1)
            {
                MyFunc.x = new double[length];
                MyFunc.y = new double[length];

                MyFunc.InterX = new double[length];
                MyFunc.InterY = new double[length];
            }
            
            for (int i = 0; i < MyFunc.x.Length; i++)
            {
                MyFunc.x[i] = Math.Round( (stepOfGraph * i) + x[0] , 3);
                MyFunc.y[i] = Function.pow( MyFunc.x[i] );

                MyFunc.InterX[i] = MyFunc.x[i];
                MyFunc.InterY[i] = Polynomial.NewtonePolynomialCalculation(MyFunc.InterX[i], x, y, result.ToArray());
            }
        }

        public ActionResult OnGetAjaxAsync()
        {
            return new JsonResult( new
            {
                myX = MyFunc.x,
                myY = MyFunc.y,
                ourX = MyFunc.InterX,
                ourY = MyFunc.InterY
            } );
            
        }
    }
}
