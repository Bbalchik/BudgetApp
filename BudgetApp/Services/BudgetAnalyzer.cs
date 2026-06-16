using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BudgetApp.Models;

namespace BudgetApp.Services;

public class BudgetAnalyzer : IBudgetAnalyzer
{
    private static readonly string[] ChartColors = { "#ff9f43", "#54a0ff", "#5f27cd", "#00d2d3", "#ff6b6b", "#1dd1a1", "#feca57", "#48dbfb" };

    public BudgetReport BuildReport(IEnumerable<Transaction> transactions, ReportPeriod period)
    {
        var list = transactions.ToList();
        var (start, end) = GetPeriodBounds(period);

        var inPeriod = list.Where(t => t.Date.Date >= start && t.Date.Date <= end).ToList();

        var expenses = inPeriod.Where(t => t.Type == TransactionType.Expense).ToList();
        var income = inPeriod.Where(t => t.Type == TransactionType.Income).ToList();

        var report = new BudgetReport
        {
            Period = period,
            StartDate = start,
            EndDate = end,
            TotalExpenses = expenses.Sum(t => t.Amount),
            TotalIncome = income.Sum(t => t.Amount),
            ExpensesByCategory = expenses
                .GroupBy(t => t.Category)
                .OrderByDescending(g => g.Sum(t => t.Amount))
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount)),
            IncomeByCategory = income
                .GroupBy(t => t.Category)
                .OrderByDescending(g => g.Sum(t => t.Amount))
                .ToDictionary(g => g.Key, g => g.Sum(t => t.Amount))
        };

        if (period == ReportPeriod.Month)
        {
            var monthExpenses = list.Where(t =>
                t.Type == TransactionType.Expense &&
                t.Date.Month == DateTime.Now.Month &&
                t.Date.Year == DateTime.Now.Year).ToList();

            var monthIncome = list.Where(t =>
                t.Type == TransactionType.Income &&
                t.Date.Month == DateTime.Now.Month &&
                t.Date.Year == DateTime.Now.Year).ToList();

            report.Forecast = BuildForecast(monthExpenses.Concat(monthIncome));
        }

        return report;
    }

    public ForecastResult BuildForecast(IEnumerable<Transaction> allMonthTransactions)
    {
        var list = allMonthTransactions.ToList();
        int currentDay = DateTime.Now.Day;
        int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

        if (currentDay == 0) return new ForecastResult(0, 0, 0, 0);

        decimal totalExp = list.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
        decimal totalInc = list.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);

        decimal dailyExp = totalExp / currentDay;
        decimal dailyInc = totalInc > 0 ? totalInc / currentDay : 0;

        return new ForecastResult(
            Math.Round(dailyExp * daysInMonth, 2),
            Math.Round(dailyInc * daysInMonth, 2),
            Math.Round(dailyExp, 2),
            Math.Round(dailyInc, 2)
        );
    }

    public LimitCheckResult CheckLimit(string category, decimal spentAmount, Dictionary<string, decimal> limits)
    {
        if (limits.TryGetValue(category, out decimal limit) && limit > 0)
        {
            return new LimitCheckResult
            {
                Limit = limit,
                SpentAmount = spentAmount,
                IsExceeded = spentAmount >= limit,
                IsNearing = spentAmount >= limit * 0.8m && spentAmount < limit
            };
        }
        return new LimitCheckResult { Limit = 0, SpentAmount = spentAmount };
    }

    public string BuildDonutGradient(Dictionary<string, decimal> stats, decimal total)
    {
        if (!stats.Any() || total == 0) return "conic-gradient(#2d3436 0% 100%)";

        string gradient = "conic-gradient(";
        decimal currentPercent = 0;
        int colorIndex = 0;

        foreach (var stat in stats)
        {
            decimal percent = (stat.Value / total) * 100;
            decimal nextPercent = currentPercent + percent;
            string color = ChartColors[colorIndex % ChartColors.Length];

            gradient += $"{color} {currentPercent.ToString("0.##", CultureInfo.InvariantCulture)}% {nextPercent.ToString("0.##", CultureInfo.InvariantCulture)}%, ";
            currentPercent = nextPercent;
            colorIndex++;
        }

        return gradient.TrimEnd(',', ' ') + ")";
    }

    private static (DateTime Start, DateTime End) GetPeriodBounds(ReportPeriod period)
    {
        var today = DateTime.Now.Date;

        return period switch
        {
            ReportPeriod.Day => (today, today),
            ReportPeriod.Week => GetWeekBounds(today),
            ReportPeriod.Month => (new DateTime(today.Year, today.Month, 1), new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month))),
            _ => (today, today)
        };
    }

    private static (DateTime, DateTime) GetWeekBounds(DateTime today)
    {
        int dow = (int)today.DayOfWeek == 0 ? 7 : (int)today.DayOfWeek;
        DateTime start = today.AddDays(-(dow - 1));
        return (start, start.AddDays(6));
    }
}
