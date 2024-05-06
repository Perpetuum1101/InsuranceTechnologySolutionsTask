using Application.Repository.Contracts;
using AutoMapper;
using Claims.UnitTests.Utility;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace Claims.UnitTests;

public class CrudServiceTest
{
    private TestService _testService;
    private Mock<IValidator<TestDTO>> _validator;
    private Mock<ITestRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;

    public CrudServiceTest()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _repository = new Mock<ITestRepository>();
        _validator = new Mock<IValidator<TestDTO>>();
        var result = new Mock<ValidationResult>();
        result.Setup(x => x.IsValid).Returns(true);
        _validator.Setup(x => x.ValidateAsync(
            It.IsAny<TestDTO>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(result.Object);
        var config = new MapperConfiguration(cfg => {
            cfg.CreateMap<TestDTO, TestEntity>();
            cfg.CreateMap<TestEntity, TestDTO>();
        });
        var mapper = config.CreateMapper();
        _unitOfWork.Setup(x => x.SaveChangesAsync());
        
        _testService = new TestService(
                           _unitOfWork.Object,
                           _repository.Object,
                           _validator.Object,
                           mapper);
    }

    [Fact]
    public async Task CreateShouldValidateDTO()
    {
        // Arrange
        var dto = new TestDTO(Guid.NewGuid());

        // Act
        await _testService.Create(dto);

        // Assert
        _validator.Verify(x => x.ValidateAsync(It.Is<TestDTO>(t => t.Id == dto.Id), default), Times.Once);
    }

    [Fact]
    public async Task CreateShouldSaveChanges()
    {
        // Arrange
        var dto = new TestDTO(Guid.NewGuid());

        // Act
        await _testService.Create(dto);

        // Assert
        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateShouldPassEntityToDb()
    {
        // Arrange
        var dto = new TestDTO(Guid.NewGuid());

        // Act
        await _testService.Create(dto);

        // Assert
        _repository.Verify(x => x.Create(It.Is<TestEntity>(x => x.Id == dto.Id)), Times.Once);
    }

    [Fact]
    public async Task DeleteShouldPassIdToDb()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _testService.Delete(id);

        // Assert
        _repository.Verify(x => x.Delete(It.Is<Guid>(x => x == id)), Times.Once);
    }

    [Fact]
    public async Task DeleteShouldSaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        await _testService.Delete(id);

        // Assert
        _unitOfWork.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task GetShouldReturnDTOWithMatchingId()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repository.Setup(x => x.Get(id)).ReturnsAsync(new TestEntity { Id =  id });

        // Act
        var result = await _testService.Get(id);

        // Assert
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async Task GetAllShouldReturnDTOsWithMatchingIds()
    {
        // Arrange
        var ids = new[] {
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        };
        _repository.Setup(x => x.GetAll()).ReturnsAsync(
        [
            new() { Id = ids[0] },
            new() { Id = ids[1] },
            new() { Id = ids[2] }
        ]);

        // Act
        var result = await _testService.GetAll();

        // Assert
        var resultIds = result.Select(x => x.Id).ToArray();
        Assert.Equal(ids[0], resultIds[0]);
        Assert.Equal(ids[1], resultIds[1]);
        Assert.Equal(ids[2], resultIds[2]);
    }
}
