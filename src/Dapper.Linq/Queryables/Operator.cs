using System.Collections.Generic;
using System.Linq.Expressions;

namespace Dapper
{
    /// <summary>
    /// Database operators
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// in
        /// </summary>
        /// <typeparam name="T">Type Inference</typeparam>
        /// <param name="column">field</param>
        /// <param name="values">parameters</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool In<T>(T column, IEnumerable<T> values) => default;

        /// <summary>
        /// in (low performance)
        /// </summary>
        /// <typeparam name="T">Type Inference</typeparam>
        /// <param name="column">field</param>
        /// <param name="values">parameters</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool In<T>(T column, params T[] values) => default;
        /// <summary>
        /// not in
        /// </summary>
        /// <typeparam name="T">Type Inference</typeparam>
        /// <param name="column">field</param>
        /// <param name="values">parameters</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool NotIn<T>(T column, IEnumerable<T> values) => default;
        /// <summary>
        /// not in (low performance)
        /// </summary>
        /// <typeparam name="T">Type Inference</typeparam>
        /// <param name="column">field</param>
        /// <param name="values">parameters</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool NotIn<T>(T column, params T[] values) => default;
        /// <summary>
        /// like %value%
        /// </summary>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool Contains(string column, string value) => default;
        /// <summary>
        /// not like %value%
        /// </summary>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool NotContains(string column, string value) => default;
        /// <summary>
        /// like value%
        /// </summary>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool StartsWith(string column, string value) => default;
        /// <summary>
        /// not like value%
        /// </summary>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool NotStartsWith(string column, string value) => default;
        /// <summary>
        /// like %value
        /// </summary>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool EndsWith(string column, string value) => default;
        /// <summary>
        /// not like %value
        /// </summary>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool NotEndsWith(string column, string value) => default;
        /// <summary>
        /// regex value
        /// </summary>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool Regexp(string column, string value) => default;
        /// <summary>
        /// not regex value
        /// </summary>
        /// <param name="column">field</param>
        /// <param name="value">parameter</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060: Remove unused parameters", Justification = "<Suspended>")]
        public static bool NotRegexp(string column, string value) => default;
        /// <summary>
        /// Analytical expression
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
		internal static string ResolveExpressionType(ExpressionType type)
        {
            var condition = string.Empty;
            switch (type)
            {
                case ExpressionType.Add:
                    condition = "+";
                    break;
                case ExpressionType.Subtract:
                    condition = "-";
                    break;
                case ExpressionType.Multiply:
                    condition = "*";
                    break;
                case ExpressionType.Divide:
                    condition = "/";
                    break;
                case ExpressionType.Modulo:
                    condition = "%";
                    break;
                case ExpressionType.Equal:
                    condition = "=";
                    break;
                case ExpressionType.NotEqual:
                    condition = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    condition = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    condition = ">=";
                    break;
                case ExpressionType.LessThan:
                    condition = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    condition = "<=";
                    break;
                case ExpressionType.OrElse:
                    condition = "OR";
                    break;
                case ExpressionType.AndAlso:
                    condition = "AND";
                    break;
                case ExpressionType.Not:
                    condition = "NOT";
                    break;
            }
            return condition;
        }
        /// <summary>
        /// Analytical expression
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static string ResolveExpressionType(string type)
        {
            switch (type)
            {
                case nameof(Operator.In):
                    type = "IN";
                    break;
                case nameof(Operator.NotIn):
                    type = "NOT IN";
                    break;
                case nameof(Operator.Contains):
                case nameof(Operator.StartsWith):
                case nameof(Operator.EndsWith):
                    type = "LIKE";
                    break;
                case nameof(Operator.NotContains):
                case nameof(Operator.NotStartsWith):
                case nameof(Operator.NotEndsWith):
                    type = "NOT LIKE";
                    break;
                case nameof(Operator.Regexp):
                    type = "REGEXP";
                    break;
                case nameof(Operator.NotRegexp):
                    type = "NOT REGEXP";
                    break;
            }
            return type;
        }
    }
}