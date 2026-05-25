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
    Task UpdateTransactionAsync(Transaction transaction);

    decimal CalculateTotal(IEnumerable<Transaction> transactions);
    Dictionary<string, decimal> GetCategoryStats(IEnumerable<Transaction> transactions);

    Task<Dictionary<string, decimal>> GetLimitsAsync();
    Task SaveLimitAsync(string category, decimal limit);
}