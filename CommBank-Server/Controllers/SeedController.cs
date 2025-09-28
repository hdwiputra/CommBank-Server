using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using CommBank.Models;
using TagModel = CommBank.Models.Tag;

namespace CommBank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly IMongoDatabase _database;

        public SeedController(IMongoDatabase database)
        {
            _database = database;
        }

        [HttpPost("all")]
        public async Task<IActionResult> SeedAll()
        {
            try
            {
                var basePath = @"c:\Users\theol\OneDrive\Documents\Hacktiv8\codingPascaLulus\forage\commonwealth\CommBank-Server\commbank-program\data";
                
                // Seed Users
                await SeedCollection<User>("Users", Path.Combine(basePath, "Users.json"));
                
                // Seed Accounts
                await SeedCollection<Account>("Accounts", Path.Combine(basePath, "Accounts.json"));
                
                // Seed Tags
                await SeedCollection<TagModel>("Tags", Path.Combine(basePath, "Tags.json"));
                
                // Seed Goals
                await SeedCollection<Goal>("Goals", Path.Combine(basePath, "Goals.json"));
                
                // Seed Transactions
                await SeedCollection<Transaction>("Transactions", Path.Combine(basePath, "Transactions.json"));

                return Ok(new { message = "Database seeded successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private async Task SeedCollection<T>(string collectionName, string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            var jsonData = await System.IO.File.ReadAllTextAsync(filePath);
            var documents = BsonSerializer.Deserialize<BsonDocument[]>(jsonData);
            
            var collection = _database.GetCollection<BsonDocument>(collectionName);
            
            // Clear existing data (optional - remove if you want to keep existing data)
            await collection.DeleteManyAsync(new BsonDocument());
            
            // Insert new data
            await collection.InsertManyAsync(documents);
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetSeedStatus()
        {
            try
            {
                var collections = new[] { "Users", "Accounts", "Tags", "Goals", "Transactions" };
                var status = new Dictionary<string, long>();

                foreach (var collectionName in collections)
                {
                    var collection = _database.GetCollection<BsonDocument>(collectionName);
                    var count = await collection.CountDocumentsAsync(new BsonDocument());
                    status[collectionName] = count;
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("add-icons")]
        public async Task<IActionResult> AddIconsToGoals()
        {
            try
            {
                var goalsCollection = _database.GetCollection<BsonDocument>("Goals");
                
                // Sample icons for different goal types
                var iconUpdates = new Dictionary<string, string>
                {
                    ["House Down Payment"] = "🏠",
                    ["Tesla Model Y"] = "🚗",
                    ["Trip to London"] = "✈️",
                    ["Trip to NYC"] = "🗽"
                };

                foreach (var iconUpdate in iconUpdates)
                {
                    var filter = Builders<BsonDocument>.Filter.Eq("Name", iconUpdate.Key);
                    var update = Builders<BsonDocument>.Update.Set("Icon", iconUpdate.Value);
                    await goalsCollection.UpdateOneAsync(filter, update);
                }

                return Ok(new { message = "Icons added to goals successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}