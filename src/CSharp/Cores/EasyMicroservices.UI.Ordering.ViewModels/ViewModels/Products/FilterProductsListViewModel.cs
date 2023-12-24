using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using EasyMicroservices.UI.Cores.Interfaces;
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

        public IAsyncCommand SearchCommand { get; set; }
        public IAsyncCommand DeleteCommand { get; set; }

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
        
        public int Index { get; set; } = 0;
        public int Length { get; set; } = 10;
        public int TotalCount { get; set; }
        public string SortColumnNames { get; set; }
        public ObservableCollection<ProductContract> Products { get; set; } = new ObservableCollection<ProductContract>();

        private async Task Search()
        {
            var filteredResult = await _productClient.FilterAsync(new FilterRequestContract()
            {
                IsDeleted = false,
                Index = Index,
                Length = Length,
            }).AsCheckedResult(x => (x.Result, x.TotalCount));

            Products.Clear();
            TotalCount = (int)filteredResult.TotalCount;
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
    }
}

