using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataGenerator
{
    public class ConfigurationBuilder<T>
    {
        private readonly OptionBuilder _optionBuilder = new OptionBuilder();
        private static ConfigurationBuilder<T> _instance;
        private readonly Dictionary<string, Option> _configuration = new Dictionary<string, Option>();
        public delegate Option OptionConfigurator(OptionBuilder prop);
        public delegate Func<dynamic> Option();

        private ConfigurationBuilder() { }

        public static ConfigurationBuilder<T> GetBuilder()
        {
            if (_instance == null)
            {

                _instance = new ConfigurationBuilder<T>();

            }

            return _instance;
        }

        public ConfigurationBuilder<T> ForProperty(Expression<Func<T, object>> expression, OptionConfigurator option)
        {
            var propertyName = GetMemberName(expression.Body);
            _configuration.Add(propertyName, option.Invoke(_optionBuilder));
            return _instance;
        }

        public Dictionary<string, Option> GetConfiguration()
        {
            return _configuration;
        }

        private static readonly string expressionCannotBeNullMessage = "The expression cannot be null.";
        private static readonly string invalidExpressionMessage = "Invalid expression.";
        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException(expressionCannotBeNullMessage);
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException(invalidExpressionMessage);
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
    }
}
