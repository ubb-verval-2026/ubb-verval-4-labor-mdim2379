namespace DatesAndStuff
{
    public interface IPaymentService
    {
        public void StartPayment();

        public double Balance { get; }

        public void SpecifyAmount(double amount);

        public void ConfirmPayment();

        public bool SuccessFul();
    }
}
