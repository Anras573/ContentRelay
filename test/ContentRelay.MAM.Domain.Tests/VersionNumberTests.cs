namespace ContentRelay.MAM.Domain.Tests;

[TestClass]
public class VersionNumberTests
{
    [TestMethod]
    [DataRow("1.0")]
    [DataRow("1.1")]
    [DataRow("2.0")]
    [DataRow("2.1")]
    [DataRow("2.8")]
    public void From_ValidVersion_ReturnsVersionNumber(string version)
    {
        var result = VersionNumber.From(version);
        
        result.Switch(versionNumber =>
        {
            Assert.AreEqual(int.Parse(version.Split('.')[0]), versionNumber.Major);
            Assert.AreEqual(int.Parse(version.Split('.')[1]), versionNumber.Minor);
            Assert.AreEqual(version, versionNumber.Value);
        }, _ => Assert.Fail());
    }
    
    [TestMethod]
    [DataRow("1")]
    [DataRow("1.1.1")]
    [DataRow("alpha")]
    [DataRow("1.0.0")]
    public void From_InvalidVersion_ReturnsVersionNumberError(string version)
    {
        var result = VersionNumber.From(version);
        
        result.Switch(_ => Assert.Fail(), error =>
        {
            Assert.AreEqual(VersionNumberError.InvalidVersion.Message, error.Message);
        });
    }
    
    [TestMethod]
    [DataRow("alpha.0")]
    [DataRow("beta.1")]
    public void From_InvalidMajor_ReturnsVersionNumberError(string version)
    {
        var result = VersionNumber.From(version);
        
        result.Switch(_ => Assert.Fail(), error =>
        {
            Assert.AreEqual(VersionNumberError.InvalidMajor.Message, error.Message);
        });
    }
    
    [TestMethod]
    [DataRow("1.alpha")]
    [DataRow("1.beta")]
    public void From_InvalidMinor_ReturnsVersionNumberError(string version)
    {
        var result = VersionNumber.From(version);
        
        result.Switch(_ => Assert.Fail(), error =>
        {
            Assert.AreEqual(VersionNumberError.InvalidMinor.Message, error.Message);
        });
    }
}