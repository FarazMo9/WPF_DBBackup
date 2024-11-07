
using Business.Entities;

namespace Business.Test
{
    public class DatabaseInfoTest
    {
        [Fact]
        public void ChallengeInvalidDatabaseInfo()
        {
            var database = new DatabaseInfo
            {
                Database=Database.MySQL
            };

            Assert.False(database.IsValid);
        }

        [Fact]
        public void IsDatabaseInfoValid()
        {
            var database = new DatabaseInfo
            {
                Database = Database.MySQL,
                Name = "test",
                DecryptedConnectionString = "test"
            };

            Assert.True(database.IsValid);
        }
    }
}
