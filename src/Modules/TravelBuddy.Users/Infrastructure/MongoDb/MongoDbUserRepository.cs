using MongoDB.Driver;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Linq;
using TravelBuddy.Users.Models;
using TravelBuddy.Users.Infrastructure; // Access the database context

namespace TravelBuddy.Users
{
    // MongoDB-specific representation of a user document
    internal class UserDocument
    {
        // Map directly to MongoDB _id (which is an Int32 in your data)
        [BsonId]
        public int UserId { get; set; }

        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        // Birthdate is stored as a BSON DateTime, so use DateTime? here
        public DateTime? Birthdate { get; set; }

        public bool IsDeleted { get; set; }
        public string Role { get; set; } = null!;
    }

    [BsonIgnoreExtraElements]
    internal class UserAuditDocument
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int AuditId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; } = null!;
        public string? FieldChanged { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public int? ChangedBy { get; set; }
        public DateTime? Timestamp { get; set; }
    }

    public class MongoDbUserRepository : IUserRepository
    {
        // IMPORTANT: this collection is UserDocument, NOT User
        private readonly IMongoCollection<UserDocument> _usersCollection;

        public MongoDbUserRepository(IMongoClient client)
        {
            // Same DB name as your migrator / other Mongo repos
            var database = client.GetDatabase("travel_buddy_mongo");
            _usersCollection = database.GetCollection<UserDocument>("users");
        }

        // ---------- Helper: map Mongo document -> domain User ----------

        private static User MapToEntity(UserDocument doc)
        {
            return new User
            {
                UserId = doc.UserId,
                Name = doc.Name,
                Email = doc.Email,
                PasswordHash = doc.PasswordHash,
                Birthdate = doc.Birthdate.HasValue
                    ? DateOnly.FromDateTime(doc.Birthdate.Value)
                    : default,
                IsDeleted = doc.IsDeleted,
                Role = doc.Role
            };
        }

        // ---------- Helper: simple auto-increment for UserId ----------

        private async Task<int> GetNextUserIdAsync()
        {
            var last = await _usersCollection
                .Find(FilterDefinition<UserDocument>.Empty)
                .SortByDescending(u => u.UserId)
                .Limit(1)
                .FirstOrDefaultAsync();

            return (last?.UserId ?? 0) + 1;
        }

        // --------------------------------------------------------
        // Get user by email (used for login, etc.)
        // --------------------------------------------------------
        public async Task<User?> GetByEmailAsync(string email)
        {
            var filter = Builders<UserDocument>.Filter.Eq(u => u.Email, email);

            var doc = await _usersCollection
                .Find(filter)
                .FirstOrDefaultAsync();

            return doc == null ? null : MapToEntity(doc);
        }


        // --------------------------------------------------------
        // Get user by id
        // --------------------------------------------------------
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            var filter = Builders<UserDocument>.Filter.Eq(u => u.UserId, userId);

            var doc = await _usersCollection
                .Find(filter)
                .FirstOrDefaultAsync();

            return doc == null ? null : MapToEntity(doc);
        }

        // --------------------------------------------------------
        // Register: insert new user
        // --------------------------------------------------------
        public async Task AddAsync(User user)
        {
            // If UserId is not set, generate one
            if (user.UserId == 0)
            {
                user.UserId = await GetNextUserIdAsync();
            }

            var doc = new UserDocument
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Birthdate = user.Birthdate != default
                    ? user.Birthdate.ToDateTime(TimeOnly.MinValue)
                    : null,
                IsDeleted = user.IsDeleted,
                Role = user.Role
            };

            // IMPORTANT: insert doc, not user
            await _usersCollection.InsertOneAsync(doc);
        }

        // --------------------------------------------------------
        // Delete: soft-delete user (and update password hash)
        // --------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(int userId, string passwordHash)
        {
            try {
                var filter = Builders<UserDocument>.Filter.Eq(u => u.UserId, userId);

                var update = Builders<UserDocument>.Update
                    .Set(u => u.IsDeleted, true)
                    .Set(u => u.PasswordHash, passwordHash);

                var result = await _usersCollection.UpdateOneAsync(filter, update);

                return (result.IsAcknowledged && result.ModifiedCount == 1, null);

            }
            catch (Exception ex) {
                return (false, ex.Message ?? "An error occurred while deleting the user.");
            }
        }

        // --------------------------------------------------------
        // Change password
        // --------------------------------------------------------
        public async Task<(bool Success, string? ErrorMessage)> UpdatePasswordAsync(int userId, string passwordHash)
        {
            try {
                var filter = Builders<UserDocument>.Filter.And(
                    Builders<UserDocument>.Filter.Eq(u => u.UserId, userId),
                    Builders<UserDocument>.Filter.Eq(u => u.IsDeleted, false)
                );

                var update = Builders<UserDocument>.Update
                    .Set(u => u.PasswordHash, passwordHash);

                await _usersCollection.UpdateOneAsync(filter, update);
                return (true, null);

            } catch (Exception ex) {
                return (false, ex.Message ?? "An error occurred while updating the password.");
            }
        }

        // --------------------------------------------------------
        // Get all users (non-deleted)
        // --------------------------------------------------------
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var filter = Builders<UserDocument>.Filter.Eq(u => u.IsDeleted, false);

            var docs = await _usersCollection
                .Find(filter)
                .ToListAsync();

            // docs is IEnumerable<UserDocument>, MapToEntity(UserDocument) fits perfectly
            return docs.Select(MapToEntity).ToList();
        }
        // ------------------------------- AUDIT TABLES -------------------------------
        public async Task<IEnumerable<UserAudit>> GetUserAuditsAsync()
        {
            var database = _usersCollection.Database;
            var userAuditCollection = database.GetCollection<UserAuditDocument>("user_audits");
            
            var docs = await userAuditCollection
                .Find(FilterDefinition<UserAuditDocument>.Empty)
                .ToListAsync();
            
            return docs.Select(doc => new UserAudit
            {
                AuditId = doc.AuditId,
                UserId = doc.UserId,
                Action = doc.Action,
                FieldChanged = doc.FieldChanged,
                OldValue = doc.OldValue,
                NewValue = doc.NewValue,
                ChangedBy = doc.ChangedBy,
                Timestamp = doc.Timestamp
            }).ToList();
        }
    }
}