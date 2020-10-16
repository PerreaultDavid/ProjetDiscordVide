using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BotVide.Classes
{
    public class Vide
    {
        // ID BSON
        [BsonId]
        public ObjectId OD { get; set; }

        // ID Discord
        [BsonElement("ID")]
        public string ID { get; set; }

        // Exemple info
        [BsonElement("nom")]
        public string Nom { get; set; }
        [BsonElement("titre")]
        public string Titre { get; set; }
    }
}
