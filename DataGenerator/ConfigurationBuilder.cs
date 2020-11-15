﻿using System;
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
            return _instance ?? (_instance = new ConfigurationBuilder<T>());
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

        private const string ExpressionCannotBeNullMessage = "The expression cannot be null.";
        private const string InvalidExpressionMessage = "Invalid expression.";

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
    }
}
