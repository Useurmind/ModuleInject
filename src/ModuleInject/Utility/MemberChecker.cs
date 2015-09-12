using ModuleInject.Common.Exceptions;
using ModuleInject.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ModuleInject.Utility
{
	public class ModuleMemberExpressionChecker<TIModule, TModule>
		where TModule : TIModule
	{
		public string CheckExpressionDescribesDirectMemberAndGetMemberName<TObject, IComponent>(Expression<Func<TObject, IComponent>> moduleProperty)
		{
			int depth;
			string path = LinqHelper.GetMemberPath(moduleProperty, out depth);
			if (depth > 1)
			{
				ExceptionHelper.ThrowPropertyAndTypeException<TModule>(Errors.InjectionModule_CannotRegisterPropertyOrMethodsWhichAreNotMembersOfTheModule, path);
			}

			ParameterExpression parameterExpression = moduleProperty.Parameters[0];

			ParameterExpression paramExp = null;
			MemberExpression propExpression = moduleProperty.Body as MemberExpression;
			MethodCallExpression methodExpression = moduleProperty.Body as MethodCallExpression;
			if (propExpression != null)
			{
				PropertyInfo propInfo = propExpression.Member as PropertyInfo;
				if (propInfo == null || !IsTypeOfModule(propInfo.DeclaringType))
				{
					ThrowNoPropertyOrMethodOfModuleException(moduleProperty);
				}
				paramExp = CommonLinq.GetParameterExpressionWithPossibleConvert(propExpression.Expression, parameterExpression.Type);
			}
			else if (methodExpression != null)
			{
				MethodInfo methodInfo = methodExpression.Method;
				if (!IsTypeOfModule(methodInfo.DeclaringType))
				{
					ThrowNoPropertyOrMethodOfModuleException(moduleProperty);
				}
				paramExp = CommonLinq.GetParameterExpressionWithPossibleConvert(methodExpression.Object, parameterExpression.Type);
			}
			else
			{
				ThrowNoPropertyOrMethodOfModuleException(moduleProperty);
			}

			if (paramExp == null || !IsTypeOfModule(paramExp.Type))
			{
				ThrowNoPropertyOrMethodOfModuleException(moduleProperty);
			}

			return path;
		}

		private void ThrowNoPropertyOrMethodOfModuleException<TObject, IComponent>(Expression<Func<TObject, IComponent>> expression)
		{
			ExceptionHelper.ThrowTypeException<TModule>(Errors.InjectionModule_NeitherPropertyNorMethodExpression, expression);
		}

		private bool IsTypeOfModule(Type type)
		{
			return type.IsAssignableFrom(typeof(TIModule)) || type.IsAssignableFrom(typeof(TModule));
		}
	}
}
