using GoNTrip.ServerAccess;
using System.Collections.Generic;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetCompanyByIdQueryFactory : QueryFactory
    {
        private const string GET_COMPANY_BI_ID_QUERY_FACTORY = "company/get";
        private const string COMPANY_ID_PARAM_NAME = "id";

        public IQuery GetCompanyById(long id) =>
            new Query(QueryMethod.GET, GET_COMPANY_BI_ID_QUERY_FACTORY,
                      parameters: new Dictionary<string, string>() { { COMPANY_ID_PARAM_NAME, id.ToString() } });
    }
}
