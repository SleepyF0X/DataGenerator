using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataGenerator
{
    public class ConfigurationBuilder<T>
    {
        private const string ExpressionCannotBeNullMessage = "The expression cannot be null.";
        private const string InvalidExpressionMessage = "Invalid expression.";
        private readonly OptionBuilder _optionBuilder = new OptionBuilder();
        private static ConfigurationBuilder<T> _instance;
        private readonly Dictionary<string, dynamic> _configuration = new Dictionary<string, dynamic>();
        public delegate dynamic OptionConfigurator(OptionBuilder prop);
        //public delegate Func<dynamic> Option();
        //public delegate Func<dynamic> OptionWithParams(params dynamic[] param);

        private ConfigurationBuilder() { }

        public static ConfigurationBuilder<T> GetBuilder()
        {
            return _instance ?? (_instance = new ConfigurationBuilder<T>());
        }
        public Dictionary<string, dynamic> GetConfiguration()
        {
            return _configuration;
        }
        public ConfigurationBuilder<T> ForProperty(Expression<Func<T, object>> property, OptionConfigurator optionConfig)
        {
            var propertyName = GetMemberName(property.Body);
            var option = optionConfig.Invoke(_optionBuilder);
            ConfigurationAdd(propertyName, option);
            return _instance;
        }
        public ConfigurationBuilder<T> WithParent<T2>(Expression<Func<T, object>> property, Expression<Func<T2, object>> parentProperty, T2 model)
        {
            var propertyName = GetMemberName(property.Body);
            var parentPropertyName = GetMemberName(parentProperty.Body);
            Func<dynamic> option = () => model.GetType().GetProperty(parentPropertyName)?.GetValue(model);
            ConfigurationAdd(propertyName, option);
            return _instance;
        }
        public ConfigurationBuilder<T> WithParent<T2>(Expression<Func<T, object>> property, Expression<Func<T, object>> propertyObject, Expression<Func<T2, object>> parentProperty, T2 model)
        {
            var propertyName = GetMemberName(property.Body);
            var propertyObjectName = GetMemberName(propertyObject.Body);
            var parentPropertyName = GetMemberName(parentProperty.Body);
            Func<dynamic> propertyOption = () => model.GetType().GetProperty(parentPropertyName)?.GetValue(model);
            Func<dynamic> propertyObjectOption = () => model;
            ConfigurationAdd(propertyName, propertyOption);
            ConfigurationAdd(propertyObjectName, propertyObjectOption);
            return _instance;
        }

        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(ExpressionCannotBeNullMessage);
            }

            if (expression is MemberExpression memberExpression)
            {
                // Reference type property or field
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression methodCallExpression)
            {
                // Reference type method
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression unaryExpression)
            {
                // Property, field of method returning value type
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException(InvalidExpressionMessage);
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression methodExpression)
            {
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }

        private void ConfigurationAdd(string key, Func<dynamic> option)
        {
            if (_configuration.ContainsKey(key))
            {
                _configuration.Remove(key);
            }
            _configuration.Add(key, option);
        }
    }
}
