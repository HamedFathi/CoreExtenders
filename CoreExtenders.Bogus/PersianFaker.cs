namespace Bogus
{
    public class PersianFaker : IPersianFaker
    {
        public Faker Faker { get; }

        public PersianFaker(Faker faker)
        {
            Faker = faker;
        }
    }
}
