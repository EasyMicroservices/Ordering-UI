using EasyMicroservices.ServiceContracts;
using EasyMicroservices.UI.Cores;
using EasyMicroservices.UI.Cores.Commands;
using Ordering.GeneratedServices;

namespace EasyMicroservices.UI.Ordering.ViewModels.Orders
{
    public class AddOrUpdateOrderViewModel : BaseViewModel
    {
        public AddOrUpdateOrderViewModel(OrderClient orderClient)
        {
            _orderClient = orderClient;
            SaveCommand = new TaskRelayCommand(this, Save);
            Clear();
        }

        public TaskRelayCommand SaveCommand { get; set; }

        readonly OrderClient _orderClient;

        public Action OnSuccess { get; set; }
        OrderContract _UpdateOrderContract;
        /// <summary>
        /// for update
        /// </summary>
        public OrderContract UpdateOrderContract
        {
            get
            {
                return _UpdateOrderContract;
            }
            set
            {
                if (value is not null)
                {
                    Name = value.Name;
                    PriceAmount = value.Prices.Select(x => x.Amount).DefaultIfEmpty(0).FirstOrDefault();
                }
                _UpdateOrderContract = value;
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

        public async Task Save()
        {
            if (UpdateOrderContract is not null)
                await UpdateOrder();
            else
                await AddOrder();
            OnSuccess?.Invoke();
        }

        public async Task AddOrder()
        {
            await _orderClient.AddAsync(new CreateOrderRequestContract()
            {
                Prices = GetPrices(),
                Names = GetNames(),
            }).AsCheckedResult(x => x.Result);
            Clear();
        }

        public async Task UpdateOrder()
        {
            await _orderClient.UpdateAsync(new UpdateOrderRequestContract()
            {
                Id = UpdateOrderContract.Id,
                ParentId = _UpdateOrderContract.ParentId,
                ProductId = _UpdateOrderContract.ProductId,
                UniqueIdentity = _UpdateOrderContract.UniqueIdentity,
                Prices = GetPrices(),
                Names = GetNames(),
            }).AsCheckedResult(x => x.Result);
            Clear();
        }

        List<OrderPriceContract> GetPrices()
        {
            return new List<OrderPriceContract>()
            {
                new OrderPriceContract()
                {
                    Amount = PriceAmount,
                    CurrencyCode = CurrencyCodeType.IRR
                }
            };
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
            UpdateOrderContract = default;
        }
    }
}
