using Castle.DynamicProxy;
using FluentValidation;

namespace Core.Aspects.Validation
{
	public class ValidationAspect : Attribute, IInterceptor
	{
		private readonly Type _validatorType;

		public ValidationAspect(Type validatorType)
		{
			_validatorType = validatorType;
		}
		public void Intercept(IInvocation invocation)
		{
			var validator = (IValidator)Activator.CreateInstance(_validatorType)!;
			var entityType = _validatorType.BaseType?.GetGenericArguments()[0];
			var entities = invocation.Arguments
				.Where(arg => arg != null && arg.GetType() == entityType);

			foreach (var entity in entities)
			{
				var context = new ValidationContext<object>(entity);
				var result = validator.Validate(context);

				if (!result.IsValid)
				{
					throw new ValidationException(result.Errors);
				}
			}

			invocation.Proceed();
		}
	}
}
