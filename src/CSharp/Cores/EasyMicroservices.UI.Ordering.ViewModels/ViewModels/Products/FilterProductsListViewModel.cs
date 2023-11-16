using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Core;
using EasyMicroservices.UI.Core.Commands;
using Ordering.GeneratedServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyMicroservices.UI.Ordering.ViewModels.Products
{
    public class FilterProductsListViewModel : BaseViewModel
    {
        public FilterProductsListViewModel(ProductClient productClient)
        {
            _productClient = productClient;
            SearchCommand = new TaskRelayCommand(this, Search);
            DeleteCommand = new TaskRelayCommand<ProductContract>(this, Delete);
            SearchCommand.Execute(null);
        }

        public ICommand SearchCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public Action<ProductContract> OnDelete { get; set; }
        readonly ProductClient _productClient;
        ProductContract _SelectedProductContract;
        public ProductContract SelectedProductContract
        {
            get => _SelectedProductContract;
            set
            {
                _SelectedProductContract = value;
                OnPropertyChanged(nameof(SelectedProductContract));
            }
        }

        public ObservableCollection<ProductContract> Products { get; set; } = new ObservableCollection<ProductContract>();

        private async Task Search()
        {
            var filteredResult = await _productClient.FilterAsync(new FilterRequestContract()
            {
                IsDeleted = false
            }).AsCheckedResult(x => (x.Result, x.TotalCount));

            Products.Clear();

            foreach (var product in filteredResult.Result)
            {
                Products.Add(product);
            }
        }

        public async Task Delete(ProductContract contract)
        {
            await _productClient.SoftDeleteByIdAsync(new Int64SoftDeleteRequestContract()
            {
                Id = contract.Id,
                IsDelete = true
            }).AsCheckedResult(x => x);
            Products.Remove(contract);
            OnDelete?.Invoke(contract);
        }

        public override Task OnError(Exception exception)
        {
            return base.OnError(exception);
        }

        public override Task DisplayFetchError(ServiceContracts.ErrorContract errorContract)
        {
            return base.DisplayFetchError(errorContract);
        }
    }
}

