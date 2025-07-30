using System;
using System.Security.Cryptography;
using System.Text;
using UserManagementApp;


namespace UserManagementApp
{
    public class User
    {
        public string UserName { get; }
        public string HashedPassword { get; private set; }
        public string EncryptedDetails { get; private set; }

        public User(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username cannot be empty.", nameof(userName));

            UserName = userName;
        }

        /// <summary>
        /// Registers the user by hashing the password and encrypting sensitive details.
        /// </summary>
        /// <param name="password">Plain-text password to hash</param>
        /// <param name="details">Sensitive user details to encrypt</param>
        /// <param name="encryptionService">Service used to encrypt details</param>
        public void Register(string password, string details, EncryptionService encryptionService)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be empty.", nameof(password));

            if (encryptionService == null)
                throw new ArgumentNullException(nameof(encryptionService));

            HashedPassword = HashPassword(password);
            EncryptedDetails = encryptionService.Encrypt(details ?? string.Empty);
        }

        /// <summary>
        /// Authenticates the user by comparing the hash of the input password with the stored hash.
        /// </summary>
        /// <param name="password">Password to check</param>
        /// <returns>True if password matches; otherwise, false.</returns>
        public bool Authenticate(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            return HashedPassword == HashPassword(password);
        }

        /// <summary>
        /// Creates a SHA-256 hash of the input password.
        /// </summary>
        /// <param name="password">Password string to hash</param>
        /// <returns>Base64 encoded hash string</returns>
        private static string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = sha.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}
