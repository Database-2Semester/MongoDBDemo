using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBConsoleApp
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }

    class Program
    {
        // create a static variable to store the collection of users
        private static IMongoCollection<User> _usersCollection;

        static async Task Main(string[] args)
        {
            // Setup MongoDB Connection and initialize database and collection
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("DemoDatabase");

            // Check if the "Users" collection exists, and create it if it doesn't
            var collectionNames = await database.ListCollectionNamesAsync().Result.ToListAsync();
            if (!collectionNames.Any(name => name == "Users"))
            {
                database.CreateCollection("Users");
                Console.WriteLine("Created 'Users' collection.");
            }
            _usersCollection = database.GetCollection<User>("Users");

            // Step 1: Create Users (Tom and Donna)
            Console.WriteLine("Step 1: Creating users named Tom and Donna...");

            var tom = new User
            {
              Id = 1,
              Name = "Tom",
              Age = 17,
              Email = "tom@example.com"
            };
            await CreateUser(tom);

            var donna = new User
            {
              Id = 2,
              Name = "Donna",
              Age = 25,
              Email = "donna@example.com"
            };
            await CreateUser(donna);

            Console.WriteLine("Users 'Tom' and 'Donna' created. \nPress Enter to proceed to Step 2.");
            Console.ReadLine();

            // Step 2: Read Users
            Console.WriteLine("Step 2: Reading all users in the 'Users' collection...");
            await ReadUsers();
            Console.WriteLine("Press Enter to proceed to Step 3.");
            Console.ReadLine();

            // Step 3: Update User (Tom)
            Console.WriteLine("Step 3: Updating user 'Tom's age to 24...");

            // create variable to store the user with name "Tom", using a filter on the users collection.
            var tomUser = await _usersCollection.Find(u => u.Name == "Tom").FirstOrDefaultAsync();
            if (tomUser != null)
            {
              tomUser.Age = 24;
              await UpdateUser(tomUser.Id, tomUser);
              Console.WriteLine("User 'Tom' updated. Press Enter to proceed to Step 4.");
            }
            else
            {
              Console.WriteLine("User 'Tom' not found.");
            }
            Console.ReadLine();

            // Step 4: Delete User (Tom)
            Console.WriteLine("Step 4: Deleting user 'Tom'...");
            if (tomUser != null)
            {
              await DeleteUser(tomUser.Id);
              Console.WriteLine("User 'Tom' deleted. Press Enter to view remaining users.");
            }
            else
            {
              Console.WriteLine("User 'Tom' not found for deletion.");
            }
            Console.ReadLine();

            // Confirm deletion
            Console.WriteLine("Final check: Listing users after deletion of 'Tom'.");
            await ReadUsers();
        }

        // CRUD Operations helper methods
        private static async Task CreateUser(User user)
        {
            await _usersCollection.InsertOneAsync(user);
            Console.WriteLine($"Created User: {user.Name}");
        }

        private static async Task ReadUsers()
        {
            var users = await _usersCollection.Find(new BsonDocument()).ToListAsync();
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Age: {user.Age}, Email: {user.Email}");
            }
        }

        private static async Task UpdateUser(int id, User updatedUser)
        {
            // Builders is a helper class that provides static methods to create update definitions 
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var update = Builders<User>.Update
                .Set(u => u.Name, updatedUser.Name)
                .Set(u => u.Age, updatedUser.Age)
                .Set(u => u.Email, updatedUser.Email);

            var result = await _usersCollection.UpdateOneAsync(filter, update);
            if (result.ModifiedCount > 0)
                Console.WriteLine($"Updated User: {updatedUser.Name}");
            else
                Console.WriteLine("User not found for update.");
        }

        private static async Task DeleteUser(int id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var result = await _usersCollection.DeleteOneAsync(filter);
            if (result.DeletedCount > 0)
                Console.WriteLine($"Deleted User with ID: {id}");
            else
                Console.WriteLine("User not found for deletion.");
        }
    }
}
