﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Dapper.Expressions
{
    /// <summary>
    /// Expression tree generator
    /// </summary>
    public class ExpressionActivator
    {
        private Dictionary<string, string> Factorization(string expression)
        {
            var experssions = new Dictionary<string, string>();
            var pattern = @"\([^\(\)]+\)";
            var text = $"({expression})";
            while (Regex.IsMatch(text, pattern))
            {
                var key = $"${experssions.Count}";
                var value = Regex.Match(text, pattern).Value;
                experssions.Add(key, value);
                text = text.Replace(value, key);
            }
            return experssions;
        }

        private Expression CreateExpression(ParameterExpression parameter, string expression)
        {
            var expressions1 = Factorization(expression);
            var expressions2 = new Dictionary<string, Expression>();
            foreach (var item in expressions1)
            {
                var subexpr = item.Value.Trim('(', ')');
                var @opterator = ResolveOperator(item.Value);
                var opt = GetExpressionType(@opterator);
                if (opt == ExpressionType.Not)
                {
                    Expression exp;
                    var text = subexpr.Split(new string[] { @opterator }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    if (expressions2.ContainsKey(text))
                    {
                        exp = expressions2[text];
                    }
                    else if (parameter.Type.GetProperties().Any(a => a.Name == text))
                    {
                        var property = parameter.Type.GetProperty(text);
                        exp = Expression.MakeMemberAccess(parameter, property);
                    }
                    else
                    {
                        exp = Expression.Constant(Convert.ToBoolean(text));
                    }
                    expressions2.Add(item.Key, Expression.MakeUnary(opt, exp, null));
                }
                else
                {
                    var text1 = subexpr
                        .Split(new string[] { @opterator }, StringSplitOptions.RemoveEmptyEntries)[0]
                        .Trim();
                    var text2 = subexpr
                        .Split(new string[] { @opterator }, StringSplitOptions.RemoveEmptyEntries)[1]
                        .Trim();
                    string temp = null;
                    Expression exp1, exp2;
                    // always put the variable in the first operand
                    if (parameter.Type.GetProperties().Any(a => a.Name == text2))
                    {
                        temp = text1;
                        text1 = text2;
                        text2 = temp;
                    }
                    //Whether it is the last fraction
                    if (expressions2.ContainsKey(text1))
                    {
                        exp1 = expressions2[text1];
                    }
                    else if (parameter.Type.GetProperties().Any(a => a.Name == text1))
                    {
                        //Is it a variable
                        var property = parameter.Type.GetProperty(text1);
                        exp1 = Expression.MakeMemberAccess(parameter, property);
                    }
                    else
                    {
                        exp1 = ResolveConstantExpression(text1);
                    }
                    //Whether it is the last fraction
                    if (expressions2.ContainsKey(text2))
                    {
                        exp2 = expressions2[text2];
                    }
                    //if the first operand is a variable
                    else if (parameter.Type.GetProperties().Any(a => a.Name == text1))
                    {
                        var constantType = parameter.Type.GetProperty(text1).PropertyType;
                        exp2 = ResolveConstantExpression(text2, constantType);
                    }
                    else
                    {
                        exp2 = ResolveConstantExpression(text1, (exp1 as ConstantExpression)?.Type);
                    }
                    expressions2.Add(item.Key, Expression.MakeBinary(opt, exp1, exp2));
                }
            }
            return expressions2.Last().Value;
        }

        public ExpressionContextResult<T> Create<T>(string expression)
        {
            expression = Initialization(expression);
            var parameter = Expression.Parameter(typeof(T), "p");
            var body = CreateExpression(parameter, expression);
            var lambda = Expression.Lambda(body, parameter);
            var func = lambda.Compile() as Func<T, bool>;
            return new ExpressionContextResult<T>()
            {
                Func = func,
                LambdaExpression = lambda
            };
        }
        private string Initialization(string expression)
        {
            expression = Regex.Replace(expression, @"(?<=[^\'][\s]+)and(?=\s+[^\'])", "&&", RegexOptions.IgnoreCase);
            expression = Regex.Replace(expression, @"(?<=[^\'][\s]+)or(?=\s+[^\'])", "||", RegexOptions.IgnoreCase);
            expression = Regex.Replace(expression, @"(?<=[^\'][\s]+)not(?=\s+[^\'])", "!", RegexOptions.IgnoreCase);
            return expression;
        }

        private ExpressionType GetExpressionType(string text)
        {
            switch (text)
            {
                case "+": return ExpressionType.Add;
                case "-": return ExpressionType.Subtract;
                case "*": return ExpressionType.Multiply;
                case "/": return ExpressionType.Divide;
                case "%": return ExpressionType.Modulo;
                case "!": return ExpressionType.Not;
                case ">": return ExpressionType.GreaterThan;
                case "<": return ExpressionType.LessThan;
                case ">=": return ExpressionType.GreaterThanOrEqual;
                case "<=": return ExpressionType.LessThanOrEqual;
                case "==": return ExpressionType.Equal;
                case "!=": return ExpressionType.NotEqual;
                case "&&": return ExpressionType.AndAlso;
                case "||": return ExpressionType.OrElse;
                default:
                    throw new InvalidOperationException(text);
            }
        }

        private Expression ResolveConstantExpression(string expression, Type type)
        {
            //Generate an expression of the specified type
            if (expression == "null")
            {
                return Expression.Constant(null, type);
            }
            else if (type == typeof(string))
            {
                return Expression.Constant(expression.Trim('\'', '\''), type);
            }
            else
            {
                if (Nullable.GetUnderlyingType(type) == null)
                {
                    var value = Convert.ChangeType(expression, type);
                    return Expression.Constant(value, type);
                }
                else
                {
                    var undertype = Nullable.GetUnderlyingType(type);
                    var value = Convert.ChangeType(expression, undertype);
                    var expr = Expression.Constant(value, undertype);
                    return Expression.MakeUnary(ExpressionType.Convert, expr, type);
                }
            }
        }

        private Expression ResolveConstantExpression(string expression)
        {
            //Automatic type inference generates expressions
            if (expression.StartsWith("'") && expression.EndsWith("'"))
            {
                // string constant
                return Expression.Constant(expression.Trim('\''), typeof(string));
            }
            else if (expression == "true" || expression == "false")
            {
                return Expression.Constant(expression, typeof(bool));
            }
            else if (Regex.IsMatch(expression, @"^\d+$"))
            {
                //int type constant
                return Expression.Constant(expression, typeof(int));
            }
            else if (Regex.IsMatch(expression, @"^\d*\.\d*$"))
            {
                //double
                return Expression.Constant(expression, typeof(int));
            }
            else if (expression == "null")
            {
                return Expression.Constant(null, typeof(object));
            }
            return Expression.Constant(expression, typeof(object));
        }

        private string ResolveOperator(string text)
        {
            var operators = new string[] { "!", "*", "/", "%", "+", "-", "<", ">", "<=", ">=", "==", "!=", "&&", "||" };
            for (int i = 0; i < text.Length - 1; i++)
            {
                var opt1 = text[i].ToString();
                var opt2 = text.Substring(i, 2);
                if (operators.Contains(opt2))
                {
                    return opt2;
                }
                else if (operators.Contains(opt1))
                {
                    return opt1;
                }
            }
            throw new Exception("resolve operator eroor");
        }
    }

    public class ExpressionContextResult<T>
    {
        public Func<T, bool> Func { get; internal set; }
        public Expression LambdaExpression { get; internal set; }
    }
}