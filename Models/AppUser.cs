using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace JobFinder.Models
{
    public class AppUser : IdentityUser
    {
       public int ProfileStatus { get; set; } = 0;

      public string? Avatar {  get; set; }
    }
}
