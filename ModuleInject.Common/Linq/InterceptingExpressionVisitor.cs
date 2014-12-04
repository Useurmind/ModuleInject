using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Common.Linq
{
    /// <summary>
    /// <see cref="System.Linq.Expressions.ExpressionVisitor"/> with the possibility to intercept 
    /// general expression visits.
    /// </summary>
    public class InterceptingExpressionVisitor : System.Linq.Expressions.ExpressionVisitor
    {
        protected virtual void OnVisiting(Expression expression) { }
        protected virtual void OnVisited(Expression expression) { }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitBinary(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitBlock(BlockExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitBlock(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitConditional(ConditionalExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitConditional(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitConstant(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitDebugInfo(DebugInfoExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitDebugInfo(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitDefault(DefaultExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitDefault(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitDynamic(DynamicExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitDynamic(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitExtension(Expression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitExtension(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitGoto(GotoExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitGoto(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitIndex(IndexExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitIndex(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitInvocation(InvocationExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitInvocation(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitLabel(LabelExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitLabel(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitLambda<T>(Expression<T> node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitLambda<T>(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitListInit(ListInitExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitListInit(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitLoop(LoopExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitLoop(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitMember(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitMemberInit(MemberInitExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitMemberInit(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitMethodCall(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitNew(NewExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitNew(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitNewArray(NewArrayExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitNewArray(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitParameter(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitRuntimeVariables(RuntimeVariablesExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitRuntimeVariables(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitSwitch(SwitchExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitSwitch(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitTry(TryExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitTry(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitTypeBinary(TypeBinaryExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitTypeBinary(node);
            }
            finally
            {
                OnVisited(node);
            }
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            try
            {
                OnVisiting(node);

                return base.VisitUnary(node);
            }
            finally
            {
                OnVisited(node);
            }
        }
    }
}
