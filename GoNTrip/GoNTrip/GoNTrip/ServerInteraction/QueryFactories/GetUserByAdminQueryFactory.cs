using System.Threading.Tasks;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetUserByAdminQueryFactory : QueryFactory
    {
        public const string GET_USER_BY_ADMIN_SERVER_METHOD_NAME = "administrator/get/user";
        public async Task<IQuery> GetUserByAdmin(Admin admin) =>
            new Query(QueryMethod.GET, GET_USER_BY_ADMIN_SERVER_METHOD_NAME, parameters: await ExtractQueryParameters<Admin, GetUserByAdminField>(admin));
    }
}
