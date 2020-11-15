using System;
using System.Collections.Generic;
using System.Text;

namespace DataGenerator
{
    public class ModelBuilder<T> where T : class
    {
        private readonly List<T> _modeList;
        private readonly ConfigurationBuilder<T> _configurationBuilder;

        public ModelBuilder(ConfigurationBuilder<T> configurationBuilder)
        {
            _modeList = new List<T>();
            _configurationBuilder = configurationBuilder;
            
        }

        public List<T> Build(int count)
        {
            var configuration = _configurationBuilder.GetConfiguration();
            for (int i = 0; i < count; i++)
            {
                var model = Activator.CreateInstance<T>();
                var props = model.GetType().GetProperties();
                foreach (var propertyInfo in props)
                {
                    var propertyName = propertyInfo.Name;
                    propertyInfo.SetValue(model,
                        configuration.ContainsKey(propertyName)
                            ? configuration[propertyName].Invoke()
                            : default(object));
                }
                _modeList.Add(model);
            }
            return _modeList;
        }
    }
}
