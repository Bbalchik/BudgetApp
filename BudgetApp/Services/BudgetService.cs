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
    private List<Transaction> _transactions = new List<Transaction>();

    public async Task<List<Transaction>> GetTransactionsAsync()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                return _transactions;
            }

            string json = await File.ReadAllTextAsync(_filePath);
            _transactions = JsonSerializer.Deserialize<List<Transaction>>(json) ?? new List<Transaction>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Помилка читання файлу: {ex.Message}");
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
        var itemToRemove = _transactions.FirstOrDefault(t => t.Id == id);
        if (itemToRemove != null)
        {
            _transactions.Remove(itemToRemove);
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
            Console.WriteLine($"Помилка запису у файл: {ex.Message}");
        }
    }
}