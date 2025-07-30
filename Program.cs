using Microsoft.Win32;
using Serilog;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using UserManagementApp;
using System.IO;

namespace UserManagementApp
{
    public class Program
    {
        static void Main()
        {
            // Configure logging
            Log.Logger = new LoggerConfiguration().WriteTo.File("logs.txt").CreateLogger();

            EncryptionService encryption = new EncryptionService();
            List<User> users = new();

            try
            {
                Console.WriteLine("Register New User");
                Console.Write("Username: ");
                string username = Console.ReadLine();
                Console.Write("Password: ");
                string password = Console.ReadLine();
                Console.Write("Enter Details (sensitive): ");
                string details = Console.ReadLine();

                var user = new User(username);
                user.Register(password, details, encryption);
                users.Add(user);

                Log.Information("User {Username} registered successfully at {Time}", username, DateTime.Now);

                // Login
                Console.WriteLine("\nLogin User");
                Console.Write("Username: ");
                string loginUsername = Console.ReadLine();
                Console.Write("Password: ");
                string loginPassword = Console.ReadLine();

                var foundUser = users.FirstOrDefault(u => u.UserName == loginUsername);
                if (foundUser != null && foundUser.Authenticate(loginPassword))
                {
                    Log.Information("User {Username} logged in successfully at {Time}", loginUsername, DateTime.Now);

                    // Display sensitive info
                    string decryptedDetails = encryption.Decrypt(foundUser.EncryptedDetails);
                    Console.WriteLine($"Welcome {foundUser.UserName}! Your data: {decryptedDetails}");
                }
                else
                {
                    Log.Warning("Unsuccessful login attempt for {Username} at {Time}", loginUsername, DateTime.Now);
                    Console.WriteLine("Invalid credentials.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred: {Message}", ex.Message);
                Console.WriteLine("An unexpected error occurred. Please contact support.");
            }
        }
    }
}