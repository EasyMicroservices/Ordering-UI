using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using EasyMicroservices.UI.Cores.Interfaces;
using Ordering.GeneratedServices;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyMicroservices.UI.Ordering.ViewModels.CountingUnits
{
    public class FilterCountingUnitsListViewModel : BaseViewModel
    {
        public FilterCountingUnitsListViewModel(CountingUnitClient countingUnitClient)
        {
            _countingUnitClient = countingUnitClient;
            SearchCommand = new TaskRelayCommand(this, Search);
            DeleteCommand = new TaskRelayCommand<CountingUnitContract>(this, Delete);
            SearchCommand.Execute(null);
        }

        public ICommandAsync SearchCommand { get; set; }
        public ICommandAsync DeleteCommand { get; set; }

        public Action<CountingUnitContract> OnDelete { get; set; }
        readonly CountingUnitClient _countingUnitClient;
        CountingUnitContract _SelectedCountingUnitContract;
        public CountingUnitContract SelectedCountingUnitContract
        {
            get => _SelectedCountingUnitContract;
            set
            {
                _SelectedCountingUnitContract = value;
                OnPropertyChanged(nameof(SelectedCountingUnitContract));
            }
        }
        
        public int Index { get; set; } = 0;
        public int Length { get; set; } = 10;
        public int TotalCount { get; set; }
        public string SortColumnNames { get; set; }
        public ObservableCollection<CountingUnitContract> CountingUnits { get; set; } = new ObservableCollection<CountingUnitContract>();

        private async Task Search()
        {
            var filteredResult = await _countingUnitClient.FilterAsync(new FilterRequestContract()
            {
                IsDeleted = false,
                Index = Index,
                Length = Length,
                SortColumnNames = SortColumnNames
            }).AsCheckedResult(x => (x.Result, x.TotalCount));

            CountingUnits.Clear();
            TotalCount = (int)filteredResult.TotalCount;
            foreach (var countingUnit in filteredResult.Result)
            {
                CountingUnits.Add(countingUnit);
            }
        }

        public async Task Delete(CountingUnitContract contract)
        {
            await _countingUnitClient.SoftDeleteByIdAsync(new Int64SoftDeleteRequestContract()
            {
                Id = contract.Id,
                IsDelete = true
            }).AsCheckedResult(x => x);
            CountingUnits.Remove(contract);
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

