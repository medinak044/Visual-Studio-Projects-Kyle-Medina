using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Practice_WebAPI_01.Controllers;
using Practice_WebAPI_01.DTOs;
using Practice_WebAPI_01.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controllers;

public class HeroControllerTests
{
    //private readonly IHeroRepository _heroRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HeroControllerTests()
    {
        // "Mock"
        //_heroRepository = A.Fake<IHeroRepository>();
        _unitOfWork = A.Fake<IUnitOfWork>();
        _mapper = A.Fake<IMapper>();
    }

    // Class_Method_ExpectedResult
    [Fact]
    public void HeroController_GetHeroes_ReturnOk()
    {
        // Arrange
        var heroes = A.Fake<IEnumerable<HeroDto>>();
        var heroList = A.Fake<List<HeroDto>>();
        A.CallTo(() => _mapper.Map<List<HeroDto>>(heroes));
        var controller = new HeroController(_mapper, _unitOfWork); // "New" a controller, with its dependencies

        // Act
        var result = controller.GetHeroes();

        // Assert 
        result.Should().NotBeNull(); // This uses FluentAssertions
        result.Should().BeOfType(typeof(OkObjectResult));
    }
}
