using System.Security.Claims;

namespace ECOMMERCE2.Helper
{
    public class UserHelper
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            string userIdValue = user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(userIdValue, out int userId))
            {
                return userId;
            }

            return 0;
        }
    }
}