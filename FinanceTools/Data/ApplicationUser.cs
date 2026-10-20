using FinanceTools.Data.UserItems;
using Microsoft.AspNetCore.Identity;

namespace FinanceTools.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ICollection<EventBuy> BuyEvents { get; } = new List<EventBuy>();
    }

}
