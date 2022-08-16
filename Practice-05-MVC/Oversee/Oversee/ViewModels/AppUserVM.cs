using Oversee.Models;

namespace Oversee.ViewModels;

public class AppUserVM
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public IEnumerable<UserConnectionRequest>? UserConnectionRequests { get; set; }
    public IEnumerable<Item>? Items { get; set; }
    public IEnumerable<ItemRequest>? ItemRequests { get; set; }
}
