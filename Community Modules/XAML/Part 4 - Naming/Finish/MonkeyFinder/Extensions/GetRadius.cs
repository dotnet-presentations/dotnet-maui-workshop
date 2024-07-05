using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyFinder.Extensions;

public class GetRadius : BindableObject, IMarkupExtension<double>
{
	public double Diameter
	{
		get => (double)GetValue(DiameterProperty);
		set => SetValue(DiameterProperty, value);
	}

	public static readonly BindableProperty DiameterProperty =
		BindableProperty.Create(nameof(Diameter), typeof(double), typeof(GetRadius), defaultValue: 0.0);

	public double ProvideValue(IServiceProvider serviceProvider)
	{
		return Diameter / 2;
	}

	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
	{
		return (this as IMarkupExtension<double>).ProvideValue(serviceProvider);
	}
}
