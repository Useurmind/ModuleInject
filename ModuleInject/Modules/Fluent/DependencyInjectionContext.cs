using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Linq;
using ModuleInject.Container.Dependencies;
using ModuleInject.Interfaces.Fluent;
using ModuleInject.Utility;

namespace ModuleInject.Modules.Fluent
{
    internal class DependencyInjectionContext : IDependencyInjectionContext
    {
        private RegistrationContext registrationContext;

        public LambdaExpression SourceExpression { get; private set; }

        public IRegistrationContext RegistrationContext { get { return registrationContext; } }

        public DependencyInjectionContext(RegistrationContext registrationContext, LambdaExpression sourceExpression)
        {
            this.registrationContext = registrationContext;
            this.SourceExpression = sourceExpression;
        }

        public IRegistrationContext IntoProperty(LambdaExpression dependencyTargetExpression)
        {
            var component = this.registrationContext;
            IRegistrationTypes types = component.RegistrationTypes;

            ParameterExpression componentParameterExpression = Expression.Parameter(dependencyTargetExpression.Parameters[0].Type, "component");
            ParameterExpression moduleParameterExpression = Expression.Parameter(this.SourceExpression.Parameters[0].Type, "module");

            var componentParameterVisitor = new ParameterExpressionReplacementVisitor(componentParameterExpression);
            var moduleParameterVisitor = new ParameterExpressionReplacementVisitor(moduleParameterExpression);

            Expression targetBody = componentParameterVisitor.ReplaceParameter(dependencyTargetExpression.Body);
            Expression sourceBody = moduleParameterVisitor.ReplaceParameter(this.SourceExpression.Body);

            MemberExpression targetMemberExpression = targetBody as MemberExpression;
            if(targetMemberExpression == null)
            {
                ExceptionHelper.ThrowFormatException(Errors.DependencyInjectionContext_NoMemberAccessInTargetExpression);
            }

            PropertyInfo propertyInfo = targetMemberExpression.Member as PropertyInfo;
            if (!propertyInfo.CanWrite)
            {
                ExceptionHelper.ThrowFormatException(Errors.DependencyInjectionContext_IntoPropertyExpressionNotWritable, propertyInfo.Name, propertyInfo.DeclaringType);
            }

            var assignmentLambdaExpression = Expression.Lambda(Expression.Assign(targetBody, sourceBody), 
                componentParameterExpression, 
                moduleParameterExpression);

            Delegate assignmentDelegate = assignmentLambdaExpression.Compile();
            
            var lambdaInjection = new LambdaDependencyInjection(this.registrationContext.Container, (cont, comp) =>
            {
                assignmentDelegate.DynamicInvoke(comp, this.registrationContext.Module);
            });


            var dependencyEvaluator = new ParameterMemberAccessEvaluator(this.SourceExpression, 0, this.RegistrationContext.Module);

            component.AddPrerequisites(dependencyEvaluator);
            component.Container.Inject(component.RegistrationName, types.IComponent, lambdaInjection);

            return component;
        }
    }
}
