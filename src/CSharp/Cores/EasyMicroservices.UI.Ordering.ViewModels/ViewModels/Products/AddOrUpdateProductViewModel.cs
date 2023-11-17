using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Core;
using EasyMicroservices.UI.Core.Commands;
using Ordering.GeneratedServices;

namespace EasyMicroservices.UI.Ordering.ViewModels.Products
{
    public class AddOrUpdateProductViewModel : BaseViewModel
    {
        public AddOrUpdateProductViewModel(ProductClient productClient)
        {
            _productClient = productClient;
            SaveCommand = new TaskRelayCommand(this, Save);
            Clear();
        }

        public TaskRelayCommand SaveCommand { get; set; }

        readonly ProductClient _productClient;

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
                CountingUnitType = CountingUnitType,
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
                CountingUnitType = CountingUnitType,
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
                    AmountType = AmountType.Decimal,
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

        public void Clear()
        {
            Name = "";
            PriceAmount = 0;
            UpdateProductContract = default;
            CountingUnitType = CountingUnitType.Number;
        }
    }
}
