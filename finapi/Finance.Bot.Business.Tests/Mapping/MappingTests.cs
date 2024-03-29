using AutoMapper;
using Finance.Bot.Business.Mapping;
using Finance.Bot.Business.Models;
using Finance.Bot.Data.Models;
using Newtonsoft.Json;

namespace Finance.Bot.Business.Tests.Mapping
{
    public class MappingTests
    {
        private static IMapper mapper;

        [SetUp]
        public void Setup()
        {
            mapper = new MapperConfiguration(x => x.AddProfile(new DefaultMappingProfile()))
                .CreateMapper();
        }

        [Test]
        public void StateEntity_State()
        {
            //TODO: refactor to test cases
            var source = new StateEntity
            {
                AppName = "Application",
                ChatId = 9223372036854775,
                DataDictionary = JsonConvert.SerializeObject(new Dictionary<string, string> { { "UserId", "123" } }),
                Timestamp = DateTimeOffset.Now,
            };

            State? result = mapper.Map<State>(source);
            
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data, Is.Empty);

            Assert.Pass();
        }
    }
}