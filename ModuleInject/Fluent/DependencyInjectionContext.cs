﻿using Microsoft.Practices.Unity;
using ModuleInject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ModuleInject.Fluent
{
    using ModuleInject.Common.Exceptions;
    using ModuleInject.Common.Linq;
    using ModuleInject.Container.Dependencies;
    using ModuleInject.Container.Resolving;
    using ModuleInject.Interfaces.Fluent;
    using System.Reflection;

    internal class DependencyInjectionContext
    {
        public LambdaExpression SourceExpression { get; private set; }

        public RegistrationContext RegistrationContext { get; private set; }

        public DependencyInjectionContext(RegistrationContext registrationContext, LambdaExpression sourceExpression)
        {
            this.RegistrationContext = registrationContext;
            this.SourceExpression = sourceExpression;
        }

        public RegistrationContext IntoProperty(LambdaExpression dependencyTargetExpression)
        {
            RegistrationContext component = this.RegistrationContext;
            IRegistrationTypes types = component.RegistrationTypes;

            ParameterExpression componentParameterExpression = Expression.Parameter(dependencyTargetExpression.Parameters[0].Type, "component");
            ParameterExpression moduleParameterExpression = Expression.Parameter(SourceExpression.Parameters[0].Type, "module");

            var componentParameterVisitor = new ParameterExpressionReplacementVisitor(componentParameterExpression);
            var moduleParameterVisitor = new ParameterExpressionReplacementVisitor(moduleParameterExpression);

            Expression targetBody = componentParameterVisitor.ReplaceParameter(dependencyTargetExpression.Body);
            Expression sourceBody = moduleParameterVisitor.ReplaceParameter(SourceExpression.Body);

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
            
            var lambdaInjection = new LambdaDependencyInjection(this.RegistrationContext.Container, (cont, comp) =>
            {
                assignmentDelegate.DynamicInvoke(comp, this.RegistrationContext.Module);
            });


            var dependencyEvaluator = new ParameterMemberAccessEvaluator(SourceExpression, 0, this.RegistrationContext.Module);

            component.AddPrerequisites(dependencyEvaluator);
            component.Container.Inject(component.RegistrationName, types.IComponent, lambdaInjection);

            return component;
        }
    }
}
