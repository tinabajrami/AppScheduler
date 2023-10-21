using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppScheduler_v1.Models;
using Microsoft.AspNetCore.Identity;

namespace AppScheduler_v1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AppUser class
public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName  { get; set; }

    public ICollection<Appointment> Appointments { get; set; }
}

