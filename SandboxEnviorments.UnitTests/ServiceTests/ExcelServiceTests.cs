using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SandBoxEnviorments;
using SandBoxEnviorments.Repositories;
using SandBoxEnviorments.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SandboxEnviorments.UnitTests.ServiceTests
{
    [TestClass]
    public class ExcelServiceTests
    {
        private SandboxInfoExcelService sandboxInfoExcelService;

        private Mock<IRepository> mockedExcelRepository;

        private ObservableCollection<Sandbox> boxes;

        private ObservableCollection<Sandbox> nullSandboxList = null;

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

            mockedExcelRepository = new Mock<IRepository>();
            sandboxInfoExcelService = new SandboxInfoExcelService(mockedExcelRepository.Object);
        }

        [TestMethod]
        public void SandBoxes_OnGetSandboxesInfo_IsPopulated()
        {
            // Arrange
            mockedExcelRepository.Setup(x => x.GetSandboxesInfo()).Returns(boxes);

            // Act
            var sandboxCollectionResult = sandboxInfoExcelService.GetSandboxesInfo();

            // Assert
            Assert.IsNotNull(sandboxCollectionResult);
            Assert.AreEqual(boxes, sandboxCollectionResult);
            mockedExcelRepository.Verify();
        }

        [TestMethod]
        public void SandBoxes_OnGetSandboxesInfo_IsEmpty()
        {
            // Arrange
            mockedExcelRepository.Setup(x => x.GetSandboxesInfo()).Returns(new ObservableCollection<Sandbox>());

            // Act
            var sandboxCollectionResult = sandboxInfoExcelService.GetSandboxesInfo();

            // Assert
            Assert.IsNotNull(sandboxCollectionResult);
            Assert.IsTrue(!sandboxCollectionResult.Any());
            Assert.AreNotEqual(boxes, sandboxCollectionResult);
            mockedExcelRepository.Verify();
        }

        [TestMethod]
        public void SandBoxes_OnGetSandboxesInfo_IsNull()
        {
            // Arrange
            mockedExcelRepository.Setup(x => x.GetSandboxesInfo()).Returns(nullSandboxList);

            // Act
            var sandboxCollectionResult = sandboxInfoExcelService.GetSandboxesInfo();

            // Assert
            Assert.IsNull(sandboxCollectionResult);
            Assert.AreNotEqual(boxes, sandboxCollectionResult);
            mockedExcelRepository.Verify();
        }

        [TestMethod]
        public void UpdateSandboxInfo_WhenCallingUpdateSandboxInfo_VerifyIsCalled()
        {
            // Arrange
            mockedExcelRepository.Setup(x => x.UpdateSandboxInfo(boxes[0]));

            // Act
            sandboxInfoExcelService.UpdateSandboxInfo(boxes[0]);

            // Assert
            mockedExcelRepository.Verify(x => x.UpdateSandboxInfo(boxes[0]), Times.Once);
        }

        [TestMethod]
        public void AddNewSandboxFile_WhenCallingAddNewSandboxFile_VerifyIsCalled()
        {
            // Arrange
            var fileInfo = new FileInfo("hello");
            mockedExcelRepository.Setup(x => x.AddNewSandboxFile(fileInfo));

            // Act
            sandboxInfoExcelService.AddNewSandboxFile(fileInfo);

            // Assert
            mockedExcelRepository.Verify(x => x.AddNewSandboxFile(fileInfo), Times.Once);
        }

        [TestMethod]
        public void SignOffOnSandBox_WhenCallingSignOffSandBox_ReturnsTrue()
        {
            // Arrange
            mockedExcelRepository.Setup(x => x.SignOffOnSanbox(boxes[0])).Returns(true);

            // Act
            bool signedOff = sandboxInfoExcelService.SignOffOnSanbox(boxes[0]);

            // Assert
            Assert.IsTrue(signedOff);
            mockedExcelRepository.Verify(x => x.SignOffOnSanbox(boxes[0]), Times.Once);
        }

        [TestMethod]
        public void SignOffOnSandBox_WhenCallingSignOffSandBox_ReturnsFalse()
        {
            // Arrange
            mockedExcelRepository.Setup(x => x.SignOffOnSanbox(boxes[0])).Returns(false);

            // Act
            bool signedOff = sandboxInfoExcelService.SignOffOnSanbox(boxes[0]);

            // Assert
            Assert.IsFalse(signedOff);
            mockedExcelRepository.Verify(x => x.SignOffOnSanbox(boxes[0]), Times.Once);
        }
    }
}
