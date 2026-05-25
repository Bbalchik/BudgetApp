namespace BudgetApp.Models;

public class ForecastResult
{
    public decimal ProjectedMonthlyExpenses { get; set; }
    public decimal ProjectedMonthlyIncome { get; set; }
    public decimal ProjectedBalance => ProjectedMonthlyIncome - ProjectedMonthlyExpenses;
    public decimal DailyAverageExpense { get; set; }
    public decimal DailyAverageIncome { get; set; }
}
