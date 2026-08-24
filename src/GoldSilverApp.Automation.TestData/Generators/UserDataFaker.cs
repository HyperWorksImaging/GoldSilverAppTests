using Bogus;
using GoldSilverApp.Automation.TestData.Models;

namespace GoldSilverApp.Automation.TestData.Factories
{
    public static class UserFakerFactory
    {
        public static Faker<UserTestData> Create() =>
            new Faker<UserTestData>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => PasswordGenerator.Generate(f,8))
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past(30, DateTime.Now.AddYears(-18)))
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber());
    }
}