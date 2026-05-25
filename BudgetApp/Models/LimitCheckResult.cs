namespace BudgetApp.Models;

public class LimitCheckResult
{
    public decimal Limit { get; set; }
    public bool IsExceeded { get; set; }
    public bool IsNearing { get; set; }
    public decimal SpentAmount { get; set; }

    public double ProgressPercent => Limit > 0 ? (double)(SpentAmount / Limit) * 100 : 0;
}
