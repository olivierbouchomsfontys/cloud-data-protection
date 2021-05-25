﻿using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CloudDataProtection.Data.Context
{
    public interface IAuthenticationDbContext
    {
        DbSet<Entities.User> User { get; set; }

        Task<bool> SaveAsync();
    }
}