using System;
using System.Collections.Generic;

namespace MasterPice.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ProfilePicture { get; set; }
}
