using Business.Entities;

namespace Business.Test
{
    public class ConfigTest
    {
        [Fact]
        public void ChallengeInvalidConfig()
        {
            var config = new Config
            {
                HostSizeInput=1,
                IntervalInput=1,
            };
            Assert.False(config.IsValid);

        }

        [Fact]
        public void IsConfigValid()
        {
            var config = new Config
            {
                HostSizeInput = 15,
                IntervalInput = 10,
            };
            Assert.True(config.IsValid);

        }
    }
}