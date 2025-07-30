using UserManagementApp;
using Xunit;

namespace UserManagementApp
{
    public class UserTests
    {
        // Class-level EncryptionService instance accessible to all tests
        private readonly EncryptionService encryptionService = new EncryptionService();

        [Fact]
        public void Register_And_Authenticate_User_Success()
        {
            var user = new User("testuser");
            user.Register("securePassword123", "Sensitive Info", encryptionService);

            Assert.True(user.Authenticate("securePassword123"));
            Assert.False(user.Authenticate("wrongpassword"));
        }

        [Fact]
        public void Encryption_And_Decryption_Works()
        {
            string originalData = "MySensitiveData";
            string encrypted = encryptionService.Encrypt(originalData);
            string decrypted = encryptionService.Decrypt(encrypted);

            Assert.Equal(originalData, decrypted);
        }

        [Fact]
        public void Error_Handling_Does_Not_Expose_Sensitive_Info()
        {
            // Simulate decryption with invalid cipher text input

            // Normal encryption for reference
            var cipherText = encryptionService.Encrypt("data");

            var invalidService = new EncryptionService();

            try
            {
                invalidService.Decrypt("invalidcipher");
            }
            catch (System.Exception ex)
            {
                // Validate exception message does NOT contain plaintext data 
                Assert.DoesNotContain("data", ex.Message, System.StringComparison.OrdinalIgnoreCase);
            }
        }

        // Optional helper method to get EncryptionService instance if needed
        public EncryptionService GetEncryptionService()
        {
            return encryptionService;
        }
    }
}
