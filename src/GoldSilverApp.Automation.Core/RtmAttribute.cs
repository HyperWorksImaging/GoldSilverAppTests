namespace GoldSilverApp.Automation.Core;

/// <summary>
/// Links an automated test to its source-of-truth row in the RTM
/// (GoldSilver-Test-Suite-Inventory). Read at runtime via reflection
/// for reporting; also serves as human-readable documentation in code.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class RtmTraceAttribute : Attribute
{
    public string TcId { get; }
    public string FeatureId { get; }
    public string Priority { get; }

    public RtmTraceAttribute(string tcId, string featureId, string priority)
    {
        TcId = tcId;
        FeatureId = featureId;
        Priority = priority;
    }
}