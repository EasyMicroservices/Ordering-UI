using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using Ordering.GeneratedServices;

namespace EasyMicroservices.UI.Ordering.ViewModels.Products
{
    public class AddOrUpdateProductViewModel : BaseViewModel
    {
        public AddOrUpdateProductViewModel(ProductClient productClient, CountingUnitClient countingUnitClient)
        {
            _productClient = productClient;
            _countingUnitClient = countingUnitClient;
            SaveCommand = new TaskRelayCommand(this, Save);
            Clear();
        }

        public TaskRelayCommand SaveCommand { get; set; }

        readonly ProductClient _productClient;
        readonly CountingUnitClient _countingUnitClient;

        public Action OnSuccess { get; set; }
        ProductContract _UpdateProductContract;
        /// <summary>
        /// for update
        /// </summary>
        public ProductContract UpdateProductContract
        {
            get
            {
                return _UpdateProductContract;
            }
            set
            {
                if (value is not null)
                {
                    Name = value.Name;
                    PriceAmount = value.Prices.Select(x => x.Amount).DefaultIfEmpty(0).FirstOrDefault();
                    SelectedCountingUnitId = value.CountingUnitId.GetValueOrDefault();
                }
                _UpdateProductContract = value;
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

        AmountType _AmountType = AmountType.Percent;
        public AmountType AmountType
        {
            get => _AmountType;
            set
            {
                _AmountType = value;
                OnPropertyChanged(nameof(AmountType));
            }
        }

        ICollection<CountingUnitContract> _CountingUnits;
        public ICollection<CountingUnitContract> CountingUnits
        {
            get => _CountingUnits;
            set
            {
                _CountingUnits = value;
                OnPropertyChanged(nameof(CountingUnits));
            }
        }

        public long SelectedCountingUnitId { get; set; }

        public async Task Save()
        {
            if (UpdateProductContract is not null)
                await UpdateProduct();
            else
                await AddProduct();
            OnSuccess?.Invoke();
        }

        public async Task AddProduct()
        {
            await _productClient.AddAsync(new CreateProductRequestContract()
            {
                Prices = GetPrices(),
                Names = GetNames(),
                CountingUnitId = SelectedCountingUnitId
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

        public async Task UpdateProduct()
        {
            await _productClient.UpdateChangedValuesOnlyAsync(new UpdateProductRequestContract()
            {
                Id = UpdateProductContract.Id,
                Prices = GetPrices(),
                Names = GetNames(),
                CountingUnitId = SelectedCountingUnitId
            }).AsCheckedResult(x => x.Result);
            Clear();
        }

        List<ProductPriceContract> GetPrices()
        {
            return new List<ProductPriceContract>()
            {
                new ProductPriceContract()
                {
                    Id = GetCurrentProperty(x => x.Prices.Select(x=> x.Id).DefaultIfEmpty(0).FirstOrDefault()) ,
                    ProductId = GetCurrentProperty(x => x.Prices.Select(x=> x.ProductId).DefaultIfEmpty(0).FirstOrDefault()),
                    UniqueIdentity = GetCurrentProperty(x => x.Prices.Select(x=> x.UniqueIdentity).DefaultIfEmpty(null).FirstOrDefault()),
                    Amount = PriceAmount,
                    CurrencyCode = CurrencyCodeType.IRR,
                    AmountType = AmountType,
                    Type = PriceType.ValueAddedTax
                }
            };
        }

        T GetCurrentProperty<T>(Func<ProductContract, T> func)
        {
            return UpdateProductContract == null ? default : func(UpdateProductContract);
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

        public async Task LoadConfig()
        {
            var items = await _countingUnitClient.GetAllByLanguageAsync(new GetByLanguageRequestContract()
            {
                Language = "fa-IR"
            }).AsCheckedResult(x => x.Result);
            CountingUnits = items;
        }

        public void Clear()
        {
            Name = "";
            PriceAmount = 0;
            AmountType = AmountType.Percent;
            UpdateProductContract = default;
            CountingUnits = null;
        }
    }
}
