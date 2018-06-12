using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SandBoxEnviorments;
using SandBoxEnviorments.Services;
using System;

namespace SandboxEnviorments.UnitTests
{
    [TestClass]
    public class DeployPageTest
    {
        Mock<ISandboxInfoService> mockSandboxInfoService;

        Mock<IDeployService> mockDeployService;

        Sandbox sandBox;

        [TestInitialize]
        public void Setup()
        {
            sandBox = new Sandbox
            {
                BranchToDeploy = "Brnach",
                Deployable = true,
                Developer = Environment.UserName,
                LocalPathToSandBox = "local path",
                Status = "smoke",
                UserStory = "user story"
            };

            mockSandboxInfoService = new Mock<ISandboxInfoService>();
            mockDeployService = new Mock<IDeployService>();
        }

        [TestMethod]
        public void Sanbox_OnConstructorLoad_IsPopulated()
        {
            // Arrange

            // Act
            var deployPage = new DeployPage(mockSandboxInfoService.Object, mockDeployService.Object, sandBox);

            // Assert
            Assert.IsNotNull(deployPage.SandBoxInfo);
            Assert.AreEqual(sandBox, deployPage.SandBoxInfo);
        }

        [TestMethod]
        public void SanboxDeveloper_OnConstructorLoad_IsEqualToEnviormentUsername()
        {
            // Arrange

            // Act
            var deployPage = new DeployPage(mockSandboxInfoService.Object, mockDeployService.Object, sandBox);

            // Assert
            Assert.IsNotNull(deployPage.SandBoxInfo.Developer);
            Assert.AreEqual(deployPage.SandBoxInfo.Developer, Environment.UserName);
        }
    }
}
