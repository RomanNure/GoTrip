using Microsoft.Extensions.Configuration;
using Android.Content.Res;

namespace GoNTrip.InternalDataAccess
{
    public interface IConfigManager
    {
        string this[params string[] keys] { get; }
        IConfigurationProvider ConfigProvider { get; }
        AssetManager Assets { get; }
    }
}
