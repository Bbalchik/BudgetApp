namespace BudgetApp.Models;

public class ForecastResult
{
    public decimal ProjectedMonthlyExpenses { get; private set; }
    public decimal ProjectedMonthlyIncome { get; private set; }
    public decimal DailyAverageExpense { get; private set; }
    public decimal DailyAverageIncome { get; private set; }

    public decimal ProjectedBalance => ProjectedMonthlyIncome - ProjectedMonthlyExpenses;

    public ForecastResult(decimal projectedMonthlyExpenses, decimal projectedMonthlyIncome, decimal dailyAverageExpense, decimal dailyAverageIncome)
    {
        ProjectedMonthlyExpenses = projectedMonthlyExpenses >= 0 ? projectedMonthlyExpenses : 0;
        ProjectedMonthlyIncome = projectedMonthlyIncome >= 0 ? projectedMonthlyIncome : 0;
        DailyAverageExpense = dailyAverageExpense >= 0 ? dailyAverageExpense : 0;
        DailyAverageIncome = dailyAverageIncome >= 0 ? dailyAverageIncome : 0;
    }
}