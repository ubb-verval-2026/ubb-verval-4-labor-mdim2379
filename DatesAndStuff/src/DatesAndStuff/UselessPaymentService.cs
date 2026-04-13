using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DatesAndStuff
{
    public class UselessPaymentService : IPaymentService
    {
        public double Balance => Double.PositiveInfinity;

        public void ConfirmPayment()
        {
        }

        public void SpecifyAmount(double amount)
        {
        }

        public void StartPayment()
        {
        }

        public bool SuccessFul()
        {
            return true;
        }
    }
}
