using System.Collections.Generic;
using BudgetApp.Models;

namespace BudgetApp.Services;

public interface IBudgetAnalyzer
{
    BudgetReport BuildReport(IEnumerable<Transaction> transactions, ReportPeriod period);
    ForecastResult BuildForecast(IEnumerable<Transaction> allMonthTransactions);
    LimitCheckResult CheckLimit(string category, decimal spentAmount, Dictionary<string, decimal> limits);
    string BuildDonutGradient(Dictionary<string, decimal> stats, decimal total);
}
