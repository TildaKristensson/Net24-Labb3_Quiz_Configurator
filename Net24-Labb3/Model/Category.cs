using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net24_Labb3.Model
{
    public class Category
    {
        
        public Category(string name)
        {
            Name = name;
        }

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public string Name { get; set; }


    }
}
