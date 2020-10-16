using BotVide.Classes;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BotVide
{
    public class saveload
    {
        // Méthode de chargement dans le config.json pour la string de connection MongoDB
        public async void ConfigSave()
        {
            // Chargement des configs
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            // Transfers la string de connection
            clientmongo = configJson.ConnectionDB.ToString();
        }

        // Connection pour la BD
        private string clientmongo = "";

        // Save / Load / Delete
        // CHANGER LA STRING CHANGERMOI POUR LA CLASSE REQUISE
        public void Save(String changermoi, String idfile)
        {
            ConfigSave();
            var client = new MongoClient(clientmongo);
            //var database = client.GetDatabase("DATABASE");
            //var collection = database.GetCollection<CLASSE>("COLLECTION");

            //var id = CLASSE.ID;

            //CLASSE ptest = collection.Find(s => s.ID == idfile).FirstOrDefault();

            /*
            if (ptest == null)
                collection.InsertOne(CLASSE);
            else
                collection.ReplaceOne(s => s.ID == ptest.ID,CLASSE);
            */
        }

        // CHANGER "VOID" EN LA CLASSE QUE VOUS VOULEZ SORTIR
        public void Load(String idfile)
        {
            ConfigSave();
            var client = new MongoClient(clientmongo);
            //var database = client.GetDatabase("DATABASE");
            //var collection = database.GetCollection<CLASSE>("COLLECTION");

            //CLASSE load = collection.Find(s => s.ID == idfile).FirstOrDefault();

            //return load;
        }

        public void Delete(String idfile)
        {
            ConfigSave();
            var client = new MongoClient(clientmongo);
            //var database = client.GetDatabase("DATABASE");
            //var collection = database.GetCollection<CLASSE>("COLLECTION");

            //collection.DeleteOne(s => s.ID == idfile);
        }
    }
}
