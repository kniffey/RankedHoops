using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Script.Serialization;

namespace RankedHoops
{

    public class Result
    {
        public int total_count { get; set; }
        public Items items { get; set; }
    }

    public class Items
    {
        public Entities entities { get; set; }
    }

    public class Entities
    {
        public List<Standing> standing { get; set; }
        public List<Entrant> entrants { get; set; }
    }

    public class Standing
    {
        public int entityId { get; set; }
        public int standing { get; set; }
    }

    public class Entrant
    {
        public int id { get; set; }
        public string name { get; set; }
        public int finalPlacement { get; set; }
        public List<int> participantIds { get; set; }
    }

    public class TourneyDataRetriever
    {
        private const string URL = "https://api.smash.gg/tournament/{0}/event/{1}/standings";
        private const string urlParameters = "?entityType=event&expand%5B%5D=entrants&mutations%5B%5D=playerData&mutations%5B%5D=standingLosses";

        //example tournament:2v2-hoops-tournament-02-18-17
        //example event:2v2-hoops-tournament
        public static Result parseResults(string tournament, string eventName){
            Result result = null;
            string url = String.Format(URL, tournament, eventName);

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("appolication/json"));

            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                result = jsSerializer.Deserialize<Result>(data);
                Entities entities = result.items.entities;

                foreach (Standing stand in entities.standing){
                    Console.WriteLine("{0}: {1}", stand.entityId, stand.standing);
                }
                foreach (Entrant entrant in entities.entrants) { 
                    Console.WriteLine("{0}({1}): {2}", entrant.name, entrant.id, entrant.finalPlacement);
                }
            }
            return result;
        }
        
        static void Main (string[] args)
        {
            TourneyDataRetriever.parseResults("2v2-hoops-tournament-02-18-17", "2v2-hoops-tournament");
        }
    }

}
