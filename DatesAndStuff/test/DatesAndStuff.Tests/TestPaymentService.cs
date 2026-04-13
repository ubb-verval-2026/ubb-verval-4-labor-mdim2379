namespace DatesAndStuff.Tests
{
    internal class TestPaymentService : IPaymentService
    {
        uint startCallCount = 0;
        uint specifyCallCount = 0;
        uint confirmCallCount = 0;

        public void StartPayment()
        {
            if (startCallCount != 0 || specifyCallCount > 0 || confirmCallCount > 0)
                throw new Exception();

            startCallCount++;
        }

        public double Balance
        {
            get { return 750; }
        }

        public void SpecifyAmount(double amount)
        {
            if (startCallCount != 1 || specifyCallCount > 0 || confirmCallCount > 0)
                throw new Exception();

            specifyCallCount++;
        }

        public void ConfirmPayment()
        {
            if (startCallCount != 1 || specifyCallCount != 1 || confirmCallCount > 0)
                throw new Exception();

            confirmCallCount++;
        }

        public bool SuccessFul()
        {
            return startCallCount == 1 && specifyCallCount == 1 && confirmCallCount == 1;
        }
    }
}
