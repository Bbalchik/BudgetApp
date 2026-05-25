using System;

namespace BudgetApp.Models;

public class Transaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; } = "Інше";
    public TransactionType Type { get; set; } = TransactionType.Expense;
}