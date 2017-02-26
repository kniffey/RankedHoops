using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace RankedHoops
{
    public class RestService : IRestService
    {
        public Result getTournamentResults(string tournament, string eventName)
        {
            return TourneyDataRetriever.parseResults(tournament, eventName);
        }
    }
}
