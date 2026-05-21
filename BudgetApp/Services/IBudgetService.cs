using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetApp.Models;

namespace BudgetApp.Services;

public interface IBudgetService
{
    Task<List<Transaction>> GetTransactionsAsync();
    Task AddTransactionAsync(Transaction transaction);
    Task DeleteTransactionAsync(Guid id); 
}