using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BudgetApp.Models;

namespace BudgetApp.Services;

public class BudgetService : IBudgetService
{
    private readonly string _filePath = "budget_data.json";
    private readonly string _limitsPath = "limits_data.json";

    private List<Transaction> _transactions = new List<Transaction>();
    private Dictionary<string, decimal> _categoryLimits = new Dictionary<string, decimal>();

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        try
        {
            if (!File.Exists(_filePath)) return _transactions;
            string json = await File.ReadAllTextAsync(_filePath);
            _transactions = JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка читання транзакцій: {ex.Message}");
        }
        return _transactions;
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        _transactions.Add(transaction);
        await SaveToFileAsync();
    }

    public async Task DeleteTransactionAsync(Guid id)
    {
        var item = _transactions.FirstOrDefault(t => t.Id == id);
        if (item != null)
        {
            _transactions.Remove(item);
            await SaveToFileAsync();
        }
    }

    public async Task UpdateTransactionAsync(Transaction updatedTransaction)
    {
        var index = _transactions.FindIndex(t => t.Id == updatedTransaction.Id);
        if (index != -1)
        {
            updatedTransaction.Date = _transactions[index].Date;
            _transactions[index] = updatedTransaction;
            await SaveToFileAsync();
        }
    }

    private async Task SaveToFileAsync()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_transactions, options);
            await File.WriteAllTextAsync(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка запису транзакцій: {ex.Message}");
        }
    }

    public async Task<Dictionary<string, decimal>> GetLimitsAsync()
    {
        try
        {
            if (!File.Exists(_limitsPath))
            {
                _categoryLimits = new Dictionary<string, decimal>
                {
                    { "🛒 Продукти харчування", 5000 },
                    { "🏠 Комунальні послуги", 3000 },
                    { "🚕 Транспорт (пальне/квитки)", 1500 }
                };
                await SaveLimitsToFileAsync();
                return _categoryLimits;
            }

            string json = await File.ReadAllTextAsync(_limitsPath);
            _categoryLimits = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json) ?? new Dictionary<string, decimal>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка читання лімітів: {ex.Message}");
        }
        return _categoryLimits;
    }

    public async Task SaveLimitAsync(string category, decimal limit)
    {
        _categoryLimits[category] = limit;
        await SaveLimitsToFileAsync();
    }

    private async Task SaveLimitsToFileAsync()
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_categoryLimits, options);
            await File.WriteAllTextAsync(_limitsPath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка запису лімітів: {ex.Message}");
        }
    }

    public decimal CalculateTotal(IEnumerable<Transaction> transactions)
    {
        return transactions.Sum(t => t.Amount);
    }

    public Dictionary<string, decimal> GetCategoryStats(IEnumerable<Transaction> transactions)
    {
        return transactions
            .GroupBy(t => t.Category)
            .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount))
            .OrderByDescending(kv => kv.Value)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }
}