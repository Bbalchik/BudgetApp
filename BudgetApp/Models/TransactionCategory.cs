using System;

namespace BudgetApp.Models;

public class TransactionCategory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public TransactionType Type { get; set; } = TransactionType.Expense;
}