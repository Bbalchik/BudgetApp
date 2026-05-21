using System.Collections.Generic;
using System.Threading.Tasks;
using BudgetApp.Models;

namespace BudgetApp.Services;

public class BudgetService : IBudgetService
{
    private readonly List<Transaction> _transactions = new List<Transaction>();

    public Task<List<Transaction>> GetTransactionsAsync()
    {
        return Task.FromResult(_transactions);
    }

    public Task AddTransactionAsync(Transaction transaction)
    {
        _transactions.Add(transaction);

        return Task.CompletedTask;
    }
}