using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Test
{
    public class UserEncryptionKeyTest
    {
        [Fact]
        public void ChallengeInvalidUserEncryptionKey()
        {
            var userEncryptionKey = new UserEncryptionKey
            {
                Password = "KeyTest"
            };
            Assert.False(userEncryptionKey.IsValid);
        }

        [Fact]
        public void IsUserEncryptionKeyValid()
        {
            var userEncryptionKey = new UserEncryptionKey
            {
                Password = "SystemTest123456"
            };
            Assert.True(userEncryptionKey.IsValid);
        }
    }
}
