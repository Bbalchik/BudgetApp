using System;
using System.Collections.Generic;

namespace BudgetApp.Models;

public class BudgetReport
{
    public ReportPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal Balance => TotalIncome - TotalExpenses;
    public decimal ExpenseToIncomeRatio => TotalIncome > 0 ? Math.Round((TotalExpenses / TotalIncome) * 100, 1) : 0;
    public Dictionary<string, decimal> ExpensesByCategory { get; set; } = new();
    public Dictionary<string, decimal> IncomeByCategory { get; set; } = new();
    public ForecastResult? Forecast { get; set; }
}
