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
    public partial class Shares_Sell
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

        private InputModel Input { get; set; } = new();
        private readonly List<PredefinedBroker> brokers = BrokerLookup.Brokers();
        private PredefinedBroker? SelectedBroker { get; set; }
        private List<EventSell> events = [];
        private string? saveMessage;
        private string tickerFilter = string.Empty;
        private int? selectedFinancialYearEndYear;
        private int currentPage;

        private bool showAllocationModal;
        private List<BuyAllocationChoice> allocationChoices = [];
        private Broker? allocationBroker;
        private string? allocationValidationMessage;
        private IEnumerable<int> FinancialYearEndYears => events
            .Select(buyEvent => GetFinancialYearEndYear(buyEvent.ContractDate))
            .Distinct()
            .OrderByDescending(year => year);

        private IEnumerable<EventSell> FilteredEvents => events
            .Where(sellEvent => string.IsNullOrWhiteSpace(tickerFilter)
                || sellEvent.Symbol.Contains(
                        tickerFilter.Trim(),
                        StringComparison.OrdinalIgnoreCase))
            .Where(sellEvent => selectedFinancialYearEndYear is null
                || GetFinancialYearEndYear(sellEvent.ContractDate) == selectedFinancialYearEndYear);

        private int FilteredEventCount => FilteredEvents.Count();

        private IEnumerable<EventSell> CurrentPageEvents => FilteredEvents
            .Skip(currentPage * PageSize)
            .Take(PageSize);

        protected override async Task OnInitializedAsync()
        {
            await LoadSellEventsAsync();
        }

        private void UpdateBrokerageFee()
        {
            if (Input.BrokerDefaultFee && SelectedBroker is not null)
            {
                Input.BrokerageFee = SelectedBroker.BrokerageFee(Input.Quantity * Input.AverageSalePrice);
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

        private async Task AddSellEvent()
        {
            allocationValidationMessage = null;
            string symbol = Input.Symbol.Trim().ToUpperInvariant();
            ApplicationUser user = await GetCurrentUserAsync();

            List<EventBuy> buyEvents = await DbContext.BuyEvents
                .AsNoTracking()
                .Include(buy => buy.SellAllocations)
                .Where(buy => buy.UserId == user.Id && buy.Symbol == symbol)
                .ToListAsync();

            Broker? broker = await DbContext.Brokers
                .SingleOrDefaultAsync(existingBroker => existingBroker.Name == Input.BrokerName);

            if (broker is null)
            {
                broker = new Broker { Name = Input.BrokerName };
                DbContext.Brokers.Add(broker);
            }
            allocationBroker = broker;

            allocationChoices = buyEvents
                .Select(buy => new BuyAllocationChoice
                {
                    BuyEventId = buy.Id,
                    ContractDate = buy.ContractDate,
                    PurchasePrice = buy.AveragePurchasePrice,
                    RemainingQuantity = buy.Quantity - buy.SellAllocations.Sum(a => a.Quantity),
                    Quantity = buy.Quantity - buy.SellAllocations.Sum(a => a.Quantity)
                })
                .Where(choice => choice.RemainingQuantity > 0)
                .ToList();

            if (allocationChoices.Count == 0)
            {
                saveMessage = $"There are no unsold {symbol} buy events to allocate.";
                return;
            }

            showAllocationModal = true;
        }

        private async Task LoadSellEventsAsync()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            events = await DbContext.SellEvents
                .AsNoTracking()
                .Include(sellEvent => sellEvent.SellAllocations)
                    .ThenInclude(allocation => allocation.EventBuy)
                .Include(sellEvent => sellEvent.Broker)
                .Where(sellEvent => sellEvent.UserId == user.Id)
                .OrderBy(sellEvent => sellEvent.ContractDate)
                .ToListAsync();
            currentPage = 0;
        }

        private async Task DeleteSellEvent(EventSell sellEvent)
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>(
                "confirm",
                $"Delete the {sellEvent.Symbol} sell event for {sellEvent.Quantity:0.######} shares? Associated sell allocations will also be deleted.");
            if (!confirmed)
            {
                return;
            }

            ApplicationUser user = await GetCurrentUserAsync();
            EventSell? eventToDelete = await DbContext.SellEvents
                .SingleOrDefaultAsync(existingEvent => existingEvent.Id == sellEvent.Id && existingEvent.UserId == user.Id);
            if (eventToDelete is null)
            {
                return;
            }

            DbContext.SellEvents.Remove(eventToDelete);
            await DbContext.SaveChangesAsync();
            saveMessage = "Sell event deleted.";
            await LoadSellEventsAsync();
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
            [Display(Name = "Average sale price")]
            public decimal AverageSalePrice { get; set; }

            public bool BrokerDefaultFee { get; set; } = true;

            [Range(typeof(decimal), "0", "999999999")]
            [Display(Name = "Brokerage fee")]
            public decimal BrokerageFee { get; set; }

            [Display(Name = "Contract date")]
            public DateTime ContractDate { get; set; } = DateTime.Today;
        }

        private sealed class BuyAllocationChoice
        {
            public int BuyEventId { get; set; }
            public DateTime ContractDate { get; set; }
            public decimal PurchasePrice { get; set; }
            public decimal RemainingQuantity { get; set; }
            public bool Selected { get; set; }
            public decimal Quantity { get; set; }
        }
        private async Task ConfirmSellAllocations()
        {
            List<BuyAllocationChoice> selectedAllocations = allocationChoices
                .Where(choice => choice.Selected)
                .ToList();

            decimal allocatedQuantity = selectedAllocations.Sum(choice => choice.Quantity);

            if (allocatedQuantity != Input.Quantity
                || selectedAllocations.Any(choice =>
                    choice.Quantity <= 0 || choice.Quantity > choice.RemainingQuantity))
            {
                allocationValidationMessage =
                    $"Selected allocation quantity ({allocatedQuantity:0.######}) must equal the sale quantity ({Input.Quantity:0.######}), and each allocation must be within the available quantity.";
                return;
            }

            ApplicationUser user = await GetCurrentUserAsync();
            int[] buyEventIds = selectedAllocations
                .Select(choice => choice.BuyEventId)
                .ToArray();

            Dictionary<int, EventBuy> buyEvents = await DbContext.BuyEvents
                .Where(buy => buyEventIds.Contains(buy.Id))
                .ToDictionaryAsync(buy => buy.Id);

            if (buyEvents.Count != selectedAllocations.Count)
            {
                allocationValidationMessage =
                    "One or more selected buy events are no longer available. Reopen the allocation dialog and try again.";
                return;
            }

            if (allocationBroker is not Broker selectedBroker)
            {
                allocationValidationMessage =
                    "The selected broker is no longer available. Reopen the allocation dialog and try again.";
                return;
            }

            List<(BuyAllocationChoice Choice, EventSell SellEvent)> pendingSaves = [];

            foreach (BuyAllocationChoice choice in selectedAllocations)
            {
                EventSell sellEvent = new()
                {
                    UserId = user.Id,
                    Broker = selectedBroker,
                    Quantity = choice.Quantity,
                    AverageSalePrice = Input.AverageSalePrice,
                    BrokerageFee = Input.BrokerageFee,
                    ContractDate = Input.ContractDate
                };

                DbContext.SellEvents.Add(sellEvent);
                pendingSaves.Add((choice, sellEvent));
            }

            await DbContext.SaveChangesAsync();

            foreach ((BuyAllocationChoice choice, EventSell sellEvent) in pendingSaves)
            {
                DbContext.EventSellAllocations.Add(new EventSellAllocation
                {
                    EventSellId = sellEvent.Id,
                    EventBuyId = choice.BuyEventId,
                    Quantity = choice.Quantity
                });
            }

            await DbContext.SaveChangesAsync();

            showAllocationModal = false;
            allocationValidationMessage = null;
            await LoadSellEventsAsync();
        }
    }
}