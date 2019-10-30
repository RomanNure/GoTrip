using System;

using Xamarin.Forms;

namespace GoNTrip.Pages.Additional.PageMementos
{
    public class ObjectBuilder
    {
        private Type type { get; set; }
        private Type[] constructorParametersTypes { get; set; }
        private object[] constructorParameters { get; set; }

        public ObjectBuilder(Type type, Type[] constructorParametersTypes, object[] constructorParameters)
        {
            this.type = type;
            this.constructorParametersTypes = constructorParametersTypes;
            this.constructorParameters = constructorParameters;
        }

        public object Build() => type.GetConstructor(constructorParametersTypes).Invoke(constructorParameters);
    }
}
