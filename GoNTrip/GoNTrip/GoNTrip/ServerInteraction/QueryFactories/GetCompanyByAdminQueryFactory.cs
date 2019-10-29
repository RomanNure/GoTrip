using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetCompanyByAdminQueryFactory : QueryFactory
    {
        private const string GET_COMPANY_BY_ADMIN_SERVER_METHOD_NAME = "company/get/admin";
        public async Task<IQuery> GetCompanyByAdmin(Admin admin) =>
            new Query(QueryMethod.GET, GET_COMPANY_BY_ADMIN_SERVER_METHOD_NAME, parameters: await ExtractQueryParameters<Admin, GetCompanyByAdminField>(admin));
    }
}
