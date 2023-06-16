using Microsoft.AspNetCore.Mvc;
using University.DataAccess;

namespace University.Controllers;

[Route("{controller}")]
public class TestController
{
    private readonly UniversityDbContext _universityDbContext;

    public TestController(UniversityDbContext universityDbContext) => _universityDbContext = universityDbContext;

    [HttpGet]
    public string Test(string message) => $"Test: database contains {_universityDbContext.Teachers.Count()} teachers with {_universityDbContext.Skills.Count()} skills in total";
}