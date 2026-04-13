using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("DatesAndStuff.Tests")]

namespace DatesAndStuff
{
    public class Person
    {
        private bool married = false;

        public string Name { get; private set; }

        public double Salary => Employment.Salary;

        public EmploymentInformation Employment { get; private set; }

        public IPaymentService PreferredPayment { get; private set; }

        public LocalTaxData TaxData { get; private set; }

        public readonly bool CanEatGluten;

        public readonly bool CanEatLactose;

        public readonly bool CanEatEgg;

        public readonly bool CanEatChocolate;

        public const double SubscriptionFee = 500;

        public Person(string name, EmploymentInformation employment, IPaymentService paymentService, LocalTaxData taxData, FoodPreferenceParams foodPreferenceParams)
        {
            this.Name = name;
            this.married = false;
            this.Employment = employment;
            this.PreferredPayment = paymentService;
            this.TaxData = taxData;
            this.CanEatGluten = foodPreferenceParams.CanEatGluten;
            this.CanEatLactose = foodPreferenceParams.CanEatLactose;
            this.CanEatEgg = foodPreferenceParams.CanEatEgg;
            this.CanEatChocolate = foodPreferenceParams.CanEatChocolate;
        }

        public void GotMarried(string newName)
        {
            if (married)
                throw new Exception("Poligamy not yet supported.");

            this.married = true;
            this.Name = newName;
        }

        public void IncreaseSalary(double percentage)
        {
            Employment.IncreaseSalary(percentage);
        }

        public static Person Clone(Person p)
        {
            return new Person(p.Name,
                new EmploymentInformation(p.Employment.Salary, p.Employment.Employer.Clone()),
                p.PreferredPayment,
                p.TaxData,
                new FoodPreferenceParams
                {
                    CanEatGluten = p.CanEatGluten,
                    CanEatEgg = p.CanEatEgg,
                    CanEatChocolate = p.CanEatChocolate,
                    CanEatLactose = p.CanEatLactose
                }
               );
        }

        public bool PerformSubsriptionPayment()
        {
            PreferredPayment.StartPayment();
            PreferredPayment.SpecifyAmount(SubscriptionFee);
            PreferredPayment.ConfirmPayment();
            return true;
        }
    }
}
