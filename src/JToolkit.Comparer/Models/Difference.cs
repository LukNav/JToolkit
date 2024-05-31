namespace JToolkit.Comparer.Models;

public record Difference
{
    public string Key { get; set; }
    public DifferenceReason Reason { get; set; }
    public object Expected { get; set; }
    public object Actual { get; set; }
}
public enum DifferenceReason
{
    TypesMismatch,
    ValuesMismatch,
    MissingValues,
    ExtraValues
}