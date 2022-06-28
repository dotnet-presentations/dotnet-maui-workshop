using System;

namespace Caliburn.Light
{
    /// <summary>
    /// The default implementation to coerce a parameter.
    /// </summary>
    /// <typeparam name="TParameter">The type of the command parameter.</typeparam>
    public static class CoerceParameter<TParameter>
    {
        /// <summary>
        /// Default method to coerce a parameter.
        /// </summary>
        /// <param name="parameter">The supplied parameter value.</param>
        /// <returns>The converted parameter value.</returns>
        public static TParameter Default(object parameter)
        {
            if (parameter is null)
                return default;

            if (parameter is ISpecialValue specialValue)
                parameter = specialValue.Resolve(new CommandExecutionContext());

            if (parameter is TParameter typedParameter)
                return typedParameter;

            return (TParameter)Convert.ChangeType(parameter, typeof(TParameter));
        }
    }
}
