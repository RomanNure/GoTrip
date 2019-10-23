using System;
using System.IO;

using Microsoft.Extensions.Configuration.Json;

using Android.Content.Res;
using Microsoft.Extensions.Configuration;

namespace GoNTrip.InternalDataAccess
{
    public class JsonConfigurationManager : IConfigManager, IDisposable
    {
        public IConfigurationProvider ConfigProvider { get; private set; }
        public AssetManager Assets { get; private set; }

        public string this[params string[] keys]
        {
            get
            {
                ConfigProvider.TryGet(String.Join(":", keys), out string temp);
                return temp;
            }
        }

        public JsonConfigurationManager(string filename)
        {
            JsonConfigurationProvider JCP = new JsonConfigurationProvider(new JsonConfigurationSource());

            ConfigProvider = JCP;
            Assets = Android.App.Application.Context.Assets;

            Stream assetStream = Android.App.Application.Context.Assets.Open(filename);
            JCP.Load(assetStream);
            assetStream.Close();
        }

        public void Dispose() => Assets.Dispose();
    }
}