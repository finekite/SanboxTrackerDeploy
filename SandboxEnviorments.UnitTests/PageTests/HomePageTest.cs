using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SandBoxEnviorments;
using SandBoxEnviorments.Services;
using System.Collections.ObjectModel;

namespace SandboxEnviorments.UnitTests
{
    [TestClass]
    public class HomePageTest
    {
        Mock<ISandboxInfoService> mockSandboxInfoService;

        Mock<IDeployService> mockDeployService;

        ObservableCollection<Sandbox> boxes;

        [TestInitialize]
        public void Setup()
        {
            boxes = new ObservableCollection<Sandbox>
            {
                new Sandbox
                {
                    BranchToDeploy = "Brnach",
                    Deployable = true,
                    Developer = "me",
                    LocalPathToSandBox = "local path",
                    Status = "smoke",
                    UserStory = "user story"
                },
                new Sandbox
                {
                    BranchToDeploy = "Brnach",
                    Deployable = true,
                    Developer = "me",
                    LocalPathToSandBox = "local path",
                    Status = "smoke",
                    UserStory = "user story"
                }
            };

            mockSandboxInfoService = new Mock<ISandboxInfoService>();
            mockDeployService = new Mock<IDeployService>();
        }

        [TestMethod]
        public void Boxes_OnConstructorLoad_IsPopulated()
        {
            // Arrange
            mockSandboxInfoService.Setup(x => x.GetSandboxesInfo()).Returns(boxes);

            // Act
            var homePage = new HomePage(mockSandboxInfoService.Object, mockDeployService.Object);

            // Assert
            Assert.IsNotNull(homePage.Boxes);
            Assert.AreEqual(2, homePage.Boxes.Count);
        }
    }
}
