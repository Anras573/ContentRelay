namespace ContentRelay.MAM.Domain.Tests;

[TestClass]
public class AssetIdTests
{
    [TestMethod]
    [DataRow("ASSET001")]
    [DataRow("asset001")] // Case-insensitive
    [DataRow("ASSET002")]
    [DataRow("ASSET003")]
    public void From_ValidId_ReturnsSome(string id)
    {
        var assetId = AssetId.From(id);
        
        Assert.IsTrue(assetId.Match(_ => true, _ => false));
    }
    
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    [DataRow("ASSET")]
    [DataRow("ASSET0001")]
    public void From_InvalidId_ReturnsNone(string id)
    {
        var assetId = AssetId.From(id);
        
        Assert.IsFalse(assetId.Match(_ => true, _ => false));
    }
}