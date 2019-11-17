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
                                                                        where R : QueryField => 
            WrapBody(await ExtractBody<T, R>(item));

        protected async Task<string> ExtractJsonQueryBody<T, Q, R>(T item1, Q item2) where T : ModelElement
                                                                                     where Q : ModelElement
                                                                                     where R : QueryField =>
            WrapBody(await ExtractBody<T, R>(item1) + await ExtractBody<Q, R>(item2));

        private async Task<string> ExtractBody<T, R>(T item) where T : ModelElement
                                                             where R : QueryField
        {
            return await Task.Run(() =>
            {
                string body = "";
                foreach (PropertyInfo PI in item.GetType().GetProperties())
                {
                    R attribute = PI.GetCustomAttribute<R>();
                    if (attribute != null)
                    {
                        string propertyName = attribute.Name == null || attribute.Name == "" ? PI.Name : attribute.Name;
                        string propertyValue = JsonConvert.SerializeObject(PI.GetValue(item));
                        body += '"' + propertyName + '"' + " : " + propertyValue + ", ";
                    }
                }
                return body;
            });
        }

        private string WrapBody(string body) =>
            "{" + (body.Length <= 2 ? "" : body.Substring(0, body.Length - 2)) + " }";



        protected async Task<IDictionary<string, string>> ExtractQueryParameters<T, R>(T item) where T : ModelElement
                                                                                               where R : QueryField =>
            await ExtractParameters<T, R>(item);

        protected async Task<IDictionary<string, string>> ExtractQueryParameters<T, Q, R>(T item1, Q item2) where T : ModelElement
                                                                                                            where Q : ModelElement
                                                                                                            where R : QueryField
        {
            IDictionary<string, string> params1 = await ExtractParameters<T, R>(item1);
            IDictionary<string, string> params2 = await ExtractParameters<Q, R>(item2);

            return params1.Concat(params2).ToDictionary(P => P.Key, P => P.Value);
        }

        private async Task<IDictionary<string, string>> ExtractParameters<T, R>(T item) where T : ModelElement
                                                                                where R : QueryField
        {
            IDictionary<string, string> data = new Dictionary<string, string>();

            await Task.Run(() =>
            {
                foreach (PropertyInfo PI in item.GetType().GetProperties())
                {
                    R attribute = PI.GetCustomAttribute<R>();
                    if (attribute != null)
                    {
                        string propertyName = attribute.Name == null || attribute.Name == "" ? PI.Name : attribute.Name;
                        string propertyValue = JsonConvert.SerializeObject(PI.GetValue(item)).Trim('\"');
                        data.Add(propertyName, propertyValue);
                    }
                }
            });

            return data;
        }
    }
}
