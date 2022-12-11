using ipstack_lib;
using ipstack_lib.interfaces;
using ipstack_lib.exceptions;

namespace TestRunner
{
    public class Tests
    {
        private IIPInfoProvider _ipInfoProvider;

        [SetUp]
        public void Setup()
        {
            _ipInfoProvider = new IPInfoProvider();
        }

        [Test]
        public async Task ShouldReturnCorrectIPDetails()
        {
            var result = await _ipInfoProvider.GetDetails("134.201.250.155");

            Assert.IsNotNull(result);

            Assert.AreEqual("Los Angeles", result.City);
            Assert.AreEqual("United States", result.Country);
            Assert.AreEqual("North America", result.Continent);
            Assert.AreEqual(34.0655517578125, result.Latitude);
            Assert.AreEqual(-118.24053955078125, result.Longitude);
        }

        [Test]
        public void ShouldThrowIPServiceNotAvailableException()
        {
            var ex = Assert.ThrowsAsync<IPServiceNotAvailableException>(() => _ipInfoProvider.GetDetails("134.201"));
            Assert.AreEqual("Service not available, Please try again.", ex.Message);
        }
    }
}