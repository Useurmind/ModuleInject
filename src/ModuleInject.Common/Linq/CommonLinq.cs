using System.Linq;

namespace ModuleInject.Common.Linq
{
    using System;
    using System.Linq.Expressions;

    public static class CommonLinq
    {
        public static ParameterExpression GetParameterExpressionWithPossibleConvert(Expression expression, Type expectedParameterType)
        {
            Expression paramExpBase = expression;
            UnaryExpression convertExp = paramExpBase as UnaryExpression;
            if (convertExp != null && convertExp.NodeType == ExpressionType.Convert)
            {
                if (expectedParameterType == null || convertExp.Type.IsAssignableFrom(expectedParameterType))
                {
                    paramExpBase = convertExp.Operand;
                }
            }

            ParameterExpression paramExp = paramExpBase as ParameterExpression;
            return paramExp;
        }
    }
}