using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Core;
using EasyMicroservices.UI.Core.Commands;
using Ordering.GeneratedServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyMicroservices.UI.Ordering.ViewModels.Orders
{
    public class FilterOrdersListViewModel : BaseViewModel
    {
        public FilterOrdersListViewModel(OrderClient orderClient)
        {
            _orderClient = orderClient;
            SearchCommand = new TaskRelayCommand(this, Search);
            DeleteCommand = new TaskRelayCommand<OrderContract>(this, Delete);
            SearchCommand.Execute(null);
        }


        public ICommand SearchCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public Action<OrderContract> OnDelete { get; set; }
        readonly OrderClient _orderClient;
        OrderContract _SelectedOrderContract;
        public OrderContract SelectedOrderContract
        {
            get => _SelectedOrderContract;
            set
            {
                _SelectedOrderContract = value;
                OnPropertyChanged(nameof(SelectedOrderContract));
            }
        }

        public ObservableCollection<OrderContract> Orders { get; set; } = new ObservableCollection<OrderContract>();

        private async Task Search()
        {
            var filteredResult = await _orderClient.FilterAsync(new FilterRequestContract()
            {
                IsDeleted = false
            }).AsCheckedResult(x => (x.Result, x.TotalCount));

            Orders.Clear();

            foreach (var order in filteredResult.Result)
            {
                Orders.Add(order);
            }
        }

        public async Task Delete(OrderContract contract)
        {
            await _orderClient.SoftDeleteByIdAsync(new Int64SoftDeleteRequestContract()
            {
                Id = contract.Id,
                IsDelete = true
            }).AsCheckedResult(x=>x);
            Orders.Remove(contract);
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
