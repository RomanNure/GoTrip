using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.PageMementos
{
    public class RestorableAttribute : Attribute { }
    public class RestorableConstructorAttribute : Attribute { }
    public class RestorableCtorNotAssignedException : Exception { }

    public class PageMemento : IMemento<ContentPage>
    {
        private ContentPage Page { get; set; }
        private object[] RestoreParams { get; set; }

        public void Save(ContentPage page, params object[] restoreParams)
        {
            Page = page;
            RestoreParams = restoreParams;
        }

        public ContentPage Restore()
        {
            Type pageType = Page.GetType();

            List<PropertyInfo> props = pageType.GetRuntimeProperties().Where(P => P.GetCustomAttribute<RestorableAttribute>(true) != null).ToList();
            ConstructorInfo restoreCtor = pageType.GetConstructors().SingleOrDefault(CT => CT.GetCustomAttribute<RestorableConstructorAttribute>() != null);

            if (restoreCtor == null)
                throw new RestorableCtorNotAssignedException();

            ContentPage page = restoreCtor.Invoke(RestoreParams) as ContentPage;

            foreach (PropertyInfo prop in props)
                prop.SetValue(page, prop.GetValue(Page));

            return page;
        }
    }
}
