using System;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    public class ModelBuilder<T> where T : class
    {
        private List<T> modeList = new List<T>();
        private readonly Dictionary<string, ConfigurationBuilder<T>.Option> _configuration;
        public ModelBuilder(ConfigurationBuilder<T> configuration, int count)
        {
            _configuration = configuration.GetConfiguration();
            for (int i = 0; i <= count; i++)
            {
                var model = Activator.CreateInstance<T>();
                var props = model.GetType().GetProperties();
                foreach (var propertyInfo in props)
                {
                    var propertyName = propertyInfo.Name;
                    if (_configuration.ContainsKey(propertyName))
                    {
                        propertyInfo.SetValue(model, _configuration[propertyName].Invoke().Invoke());
                    }
                    else
                    {
                        propertyInfo.SetValue(model, default);
                    }
                }
                modeList.Add(model);
            }
        }

        public List<T> Build()
        {
            return modeList;
        }
    }
}
