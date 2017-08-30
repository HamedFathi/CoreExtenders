namespace Bogus
{
    public static class FakerExtension
    {
        public static IPersianFaker Persian(this Faker faker) => new PersianFaker(faker);
    }
}
