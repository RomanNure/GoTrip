using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Threading.Tasks;

using Newtonsoft.Json;

using GoNTrip.Model;
using GoNTrip.ServerAccess;
using GoNTrip.ServerInteraction.ModelFieldAttributes;

namespace GoNTrip.ServerInteraction.QueryFactories
{
    public abstract class QueryFactory
    {
        protected async Task<string> ExtractJsonQueryBody<T, R>(T item) where T : ModelElement
                                                            where R : ExportField
        {
            string body = "{ ";

            await Task.Run(() =>
            {
                foreach (PropertyInfo PI in item.GetType().GetProperties())
                    if (PI.CustomAttributes.Select(A => A.AttributeType).Contains(typeof(R)))
                    {
                        string propertyValue = JsonConvert.SerializeObject(PI.GetValue(item));
                        body += '"' + PI.Name + '"' + " : " + propertyValue + ", ";
                    }

            });

            return body.Length <= 2 ? "{ }" : (body.Substring(0, body.Length - 2) + " }");
        }

        protected async Task<IDictionary<string, string>> ExtractQueryParameters<T, R>(T item) where T : ModelElement
                                                                                               where R : ExportField
        {
            IDictionary<string, string> data = new Dictionary<string, string>();

            await Task.Run(() =>
            {
                foreach (PropertyInfo PI in item.GetType().GetProperties())
                    if (PI.CustomAttributes.Select(A => A.AttributeType).Contains(typeof(R)))
                    {
                        string propertyValue = JsonConvert.SerializeObject(PI.GetValue(item)).Trim('\"');
                        data.Add(PI.Name, propertyValue);
                    }
            });

            return data;
        }
    }
}
