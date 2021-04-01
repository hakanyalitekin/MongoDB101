using MongoDB.Driver;
using MongoDB101.API.Model;
using MongoDB101.API.Settings;
using System.Collections.Generic;
using System.Linq;

namespace MongoDB101.API.Service
{
    public class UserService
    {
        private IDbSettings _settings;
        private IMongoCollection<User> _user;

        public UserService(IDbSettings settings)
        {
            _settings = settings;
            MongoClient client = new MongoClient(settings.ConnectionString);
            var db = client.GetDatabase(settings.Database);
            _user = db.GetCollection<User>(settings.Collection);
        }

        public List<User> GetAll()
        {
            return _user.Find(u => true).ToList();
        }

        public User GetSingle(string id)
        {
            return _user.Find(u => u.Id == id).FirstOrDefault();
        }

        public User Add(User user)
        {
            _user.InsertOne(user);
            return user;
        }

        public long Update(User currentUser)
        {
            return _user.ReplaceOne(u => u.Id == currentUser.Id, currentUser).ModifiedCount;
        }

        public long Delete(string id)
        {
            return _user.DeleteOne(u => u.Id == id).DeletedCount;
        }

    }
}
