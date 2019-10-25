using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GoNTrip.ServerAccess;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public class GetToursQueryFactory : QueryFactory
    {
        public const string GET_TOURS_SERVER_METHOD_NAME = "tours/get";
        public IQuery GetAllTours() => new Query(QueryMethod.GET, GET_TOURS_SERVER_METHOD_NAME);
        //public IQuery GetToursFiltered()...
    }
}
