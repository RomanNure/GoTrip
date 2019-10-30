using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetOwnedCompaniesQueryFactory : QueryFactory
    {
        private const string GET_OWNED_COMPANIES_SERVER_METHOD_NAME = "company/get/owner";
        public async Task<IQuery> GetOwnedCompanies(User user) =>
            new Query(QueryMethod.GET, GET_OWNED_COMPANIES_SERVER_METHOD_NAME, parameters: await ExtractQueryParameters<User, GetOwnedCompaniesField>(user));
    }
}
