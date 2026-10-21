using FinanceTools.Data;
using FinanceTools.Data.UserItems;
using FinanceTools.Static;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;

namespace FinanceTools.Components.Pages
{
    public partial class Shares_Buy
    {
        private const int PageSize = 10;

        [Inject]
        private ApplicationDbContext DbContext { get; set; } = default!;

        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        private UserManager<ApplicationUser> UserManager { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        private List<EventBuy> events = [];
        private readonly List<PredefinedBroker> brokers = BrokerLookup.Brokers();
        private InputModel Input { get; set; } = new();
        private PredefinedBroker? SelectedBroker { get; set; }
        private string? saveMessage;
        private string tickerFilter = string.Empty;
        private int? selectedFinancialYearEndYear;
        private bool hideFullySold = false;
        private int currentPage;

        private IEnumerable<int> FinancialYearEndYears => events
            .Select(buyEvent => GetFinancialYearEndYear(buyEvent.ContractDate))
            .Distinct()
            .OrderByDescending(year => year);

        private IEnumerable<EventBuy> FilteredEvents => events
            .Where(buyEvent => string.IsNullOrWhiteSpace(tickerFilter)
                || buyEvent.Symbol.Contains(tickerFilter.Trim(), StringComparison.OrdinalIgnoreCase))
            .Where(buyEvent => selectedFinancialYearEndYear is null
                || GetFinancialYearEndYear(buyEvent.ContractDate) == selectedFinancialYearEndYear)
            .Where(buyEvent => !hideFullySold
                || buyEvent.SellAllocations.Sum(allocation => allocation.Quantity) < buyEvent.Quantity);

        private int FilteredEventCount => FilteredEvents.Count();

        private IEnumerable<EventBuy> CurrentPageEvents => FilteredEvents
            .Skip(currentPage * PageSize)
            .Take(PageSize);

        protected override async Task OnInitializedAsync()
        {
            await LoadBuyEventsAsync();
        }

        private void UpdateBrokerageFee()
        {
            if (Input.BrokerDefaultFee && SelectedBroker is not null)
            {
                Input.BrokerageFee = SelectedBroker.BrokerageFee(Input.Quantity * Input.AveragePurchasePrice);
            }
        }

        private void OnBrokerChanged()
        {
            SelectedBroker = brokers.FirstOrDefault(broker => broker.Name == Input.BrokerName);
            Input.BrokerName = SelectedBroker?.Name ?? string.Empty;
            UpdateBrokerageFee();
        }

        private void OnTickerChanged()
        {
            TickerLookup tickerLookup = new();
            Input.CompanyName = tickerLookup.GetCompanyName(Input.Symbol);
        }

        private async Task AddBuyEvent()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            Broker? broker = await DbContext.Brokers
                .SingleOrDefaultAsync(existingBroker => existingBroker.Name == Input.BrokerName);

            if (broker is null)
            {
                broker = new Broker { Name = Input.BrokerName };
                DbContext.Brokers.Add(broker);
            }

            DbContext.BuyEvents.Add(new EventBuy
            {
                UserId = user.Id,
                Broker = broker,
                Symbol = Input.Symbol.Trim().ToUpperInvariant(),
                CompanyName = Input.CompanyName.Trim(),
                Quantity = Input.Quantity,
                AveragePurchasePrice = Input.AveragePurchasePrice,
                BrokerageFee = Input.BrokerageFee,
                ContractDate = Input.ContractDate
            });

            await DbContext.SaveChangesAsync();
            Input = new();
            SelectedBroker = null;
            saveMessage = "Buy event saved.";
            await LoadBuyEventsAsync();
        }

        private async Task LoadBuyEventsAsync()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            events = await DbContext.BuyEvents
                .AsNoTracking()
                .Include(buyEvent => buyEvent.Broker)
                .Include(buyEvent => buyEvent.SellAllocations)
                .Where(buyEvent => buyEvent.UserId == user.Id)
                .OrderBy(buyEvent => buyEvent.Symbol)
                    .ThenBy(buyEvent => buyEvent.ContractDate)
                .ToListAsync();
            currentPage = 0;
        }

        private async Task DeleteBuyEvent(EventBuy buyEvent)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>(
                "confirm",
                $"Delete the {buyEvent.Symbol} buy event for {buyEvent.Quantity:0.######} shares? Associated sell allocations will also be deleted.");
            if (!confirmed)
            {
                return;
            }

            ApplicationUser user = await GetCurrentUserAsync();
            EventBuy? eventToDelete = await DbContext.BuyEvents
                .SingleOrDefaultAsync(existingEvent => existingEvent.Id == buyEvent.Id && existingEvent.UserId == user.Id);
            if (eventToDelete is null)
            {
                return;
            }

            DbContext.BuyEvents.Remove(eventToDelete);
            await DbContext.SaveChangesAsync();
            saveMessage = "Buy event deleted.";
            await LoadBuyEventsAsync();
        }

        private void ResetPagination()
        {
            currentPage = 0;
        }

        private void PreviousPage()
        {
            if (currentPage > 0)
            {
                currentPage--;
            }
        }

        private void NextPage()
        {
            if ((currentPage + 1) * PageSize < FilteredEventCount)
            {
                currentPage++;
            }
        }

        private static int GetFinancialYearEndYear(DateTime date)
        {
            return date.Month >= 7 ? date.Year + 1 : date.Year;
        }

        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            AuthenticationState authenticationState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            return await UserManager.GetUserAsync(authenticationState.User)
                ?? throw new InvalidOperationException("The authenticated user could not be loaded.");
        }

        private sealed class InputModel
        {
            [Required]
            [StringLength(10)]
            public string Symbol { get; set; } = "";

            [Required]
            [StringLength(200)]
            [Display(Name = "Company name")]
            public string CompanyName { get; set; } = "";

            [Required]
            [Display(Name = "Broker")]
            public string BrokerName { get; set; } = "";

            [Range(typeof(decimal), "0.000001", "999999999")]
            public decimal Quantity { get; set; }

            [Range(typeof(decimal), "0.0001", "999999999")]
            [Display(Name = "Average purchase price")]
            public decimal AveragePurchasePrice { get; set; }

            public bool BrokerDefaultFee { get; set; } = true;

            [Range(typeof(decimal), "0", "999999999")]
            [Display(Name = "Brokerage fee")]
            public decimal BrokerageFee { get; set; }

            [Display(Name = "Contract date")]
            public DateTime ContractDate { get; set; } = DateTime.Today;
        }
    }
}