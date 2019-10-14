using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using Newtonsoft.Json;

using System.Text;

using GoNTrip.Model;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public abstract class QueryFactory
    {
        protected string ExtractJsonQueryBody<T, R>(T item) where T : ModelElement
                                                            where R : ExportField
        {
            string body = "{ ";
            foreach (PropertyInfo PI in item.GetType().GetProperties())
                if (PI.CustomAttributes.Select(A => A.AttributeType).Contains(typeof(R)))
                {
                    string propertyValue = JsonConvert.SerializeObject(PI.GetValue(item));
                    body += '"' + PI.Name + '"' + " : " + propertyValue + ", ";
                }

             return body.Length <= 2 ? "{ }" : (body.Substring(0, body.Length - 2) + " }");
        }

        protected IDictionary<string, string> ExtractQueryParameters<T, R>(T item) where T : ModelElement
                                                                                   where R : ExportField
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            foreach (PropertyInfo PI in item.GetType().GetProperties())
                if (PI.CustomAttributes.Select(A => A.AttributeType).Contains(typeof(R)))
                {
                    string propertyValue = JsonConvert.SerializeObject(PI.GetValue(item));
                    data.Add(PI.Name, propertyValue.Substring(1, propertyValue.Length - 2));
                }

            return data;
        }
    }
}
