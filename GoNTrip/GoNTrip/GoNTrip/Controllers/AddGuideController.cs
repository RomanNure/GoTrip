using System.Threading.Tasks;
using System.Collections.Generic;

using Autofac;

using GoNTrip.Model;
using GoNTrip.Pages;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.QueryFactories;
using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.Controllers
{
    public class AddGuideController
    {
        private const string KEYWORDS_GLUE = ",";

        public async Task<Guide> AddGuide(IEnumerable<string> keywrods, string card)
        {
            Guide newGuide = new Guide(string.Join(KEYWORDS_GLUE, keywrods), card);
            User currentUser = App.DI.Resolve<Session>().CurrentUser;

            IQuery addGuideQuery = await App.DI.Resolve<AddGuideQueryFactory>().AddGuide(currentUser, newGuide);
            IServerResponse response = await App.DI.Resolve<IServerCommunicator>().SendQuery(addGuideQuery);
            return App.DI.Resolve<IResponseParser>().Parse<Guide>(response);
        }
    }
}
