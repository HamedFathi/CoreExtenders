using System.Linq.Expressions;

namespace ExpressionToString
{
    public static class ExpressionToStringExtensions
    {
        public static string ToExpressionString(this Expression expression, bool trimLongArgumentList = false)
        {
            return ExpressionStringBuilder.ToString(expression, trimLongArgumentList);
        }
    }
}
