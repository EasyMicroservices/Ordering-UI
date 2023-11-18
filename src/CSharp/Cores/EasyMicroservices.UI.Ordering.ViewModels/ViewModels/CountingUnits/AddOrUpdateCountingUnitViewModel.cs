using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using Ordering.GeneratedServices;

namespace EasyMicroservices.UI.Ordering.ViewModels.CountingUnits
{
    public class AddOrUpdateCountingUnitViewModel : BaseViewModel
    {
        public AddOrUpdateCountingUnitViewModel(CountingUnitClient countingUnitClient)
        {
            _countingUnitClient = countingUnitClient;
            SaveCommand = new TaskRelayCommand(this, Save);
            Clear();
        }

        public TaskRelayCommand SaveCommand { get; set; }

        readonly CountingUnitClient _countingUnitClient;

        public Action OnSuccess { get; set; }
        CountingUnitContract _UpdateCountingUnitContract;
        /// <summary>
        /// for update
        /// </summary>
        public CountingUnitContract UpdateCountingUnitContract
        {
            get
            {
                return _UpdateCountingUnitContract;
            }
            set
            {
                if (value is not null)
                {
                    Name = value.Name;
                }
                _UpdateCountingUnitContract = value;
            }
        }

        string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }


        decimal _PriceAmount;
        public decimal PriceAmount
        {
            get => _PriceAmount;
            set
            {
                _PriceAmount = value;
                OnPropertyChanged(nameof(PriceAmount));
            }
        }


        CountingUnitType _CountingUnitType = CountingUnitType.Number;
        public CountingUnitType CountingUnitType
        {
            get
            {
                return _CountingUnitType;
            }
            set
            {
                _CountingUnitType = value;
            }
        }

        public async Task Save()
        {
            if (UpdateCountingUnitContract is not null)
                await UpdateCountingUnit();
            else
                await AddCountingUnit();
            OnSuccess?.Invoke();
        }

        public async Task AddCountingUnit()
        {
            await _countingUnitClient.AddAsync(new CreateCountingUnitRequestContract()
            {
                Names = GetNames(),
            }).AsCheckedResult(x => x.Result);
            Clear();
        }

        public override Task OnError(Exception exception)
        {
            return base.OnError(exception);
        }

        public override Task DisplayFetchError(ServiceContracts.ErrorContract errorContract)
        {
            return base.DisplayFetchError(errorContract);
        }

        public async Task UpdateCountingUnit()
        {
            await _countingUnitClient.UpdateChangedValuesOnlyAsync(new UpdateCountingUnitRequestContract()
            {
                Id = UpdateCountingUnitContract.Id,
                Names = GetNames(),
            }).AsCheckedResult(x => x.Result);
            Clear();
        }

        T GetCurrentProperty<T>(Func<CountingUnitContract, T> func)
        {
            return UpdateCountingUnitContract == null ? default : func(UpdateCountingUnitContract);
        }

        List<LanguageDataContract> GetNames()
        {
            return new List<LanguageDataContract>()
            {
                new LanguageDataContract()
                {
                    Data = Name,
                    Language = "fa-IR"
                }
            };
        }

        public void Clear()
        {
            Name = "";
            PriceAmount = 0;
            UpdateCountingUnitContract = default;
            CountingUnitType = CountingUnitType.Number;
        }
    }
}
