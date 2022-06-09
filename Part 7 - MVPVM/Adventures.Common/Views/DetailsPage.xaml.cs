#pragma warning disable CA1416

using Adventures.Common.Controls;
using Adventures.Common.Interfaces;

namespace Adventures.Common.Views;

public partial class DetailsPage : ContentPageBase
{
	public DetailsPage(IDetailViewModel viewModel) 
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}
