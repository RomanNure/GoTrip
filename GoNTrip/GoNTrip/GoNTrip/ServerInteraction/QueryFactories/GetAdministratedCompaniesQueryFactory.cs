using System.Threading.Tasks;
using System.Collections.Generic;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetAdministratedCompaniesQueryFactory : QueryFactory
    {
        private const string GET_ADMINISTRATED_COMPANIES_SERVER_METHOD_NAME = "user/get/companies";
        public async Task<IQuery> GetAdministratedCompanies(User user) =>
            new Query(QueryMethod.GET, GET_ADMINISTRATED_COMPANIES_SERVER_METHOD_NAME, parameters: await ExtractQueryParameters<User, GetAdministratedCompaniesField>(user));
    }
}
