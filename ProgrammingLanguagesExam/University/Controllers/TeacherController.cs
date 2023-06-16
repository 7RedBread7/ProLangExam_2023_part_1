using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;
using University.DataAccess;

namespace University.Controllers;

public class TeacherController:Controller
{
    private readonly UniversityDbContext _universityDbContext;

    public TeacherController(UniversityDbContext universityDbContext) => _universityDbContext = universityDbContext;

    [HttpGet]
    public string Test(string message) => $"Echo reply for: {message}";

	[HttpGet]
	public string bruh(string message) => $"Test: database contains {_universityDbContext.Teachers.Count()} teachers with {_universityDbContext.Skills.Count()} skills in total";

	[HttpGet("/Teacher/All")]
	public IActionResult All()
	{
		return View(_universityDbContext.Teachers.ToList());
	}

	[HttpGet("/Teacher/{id}")]
	public IActionResult GetTeacher(int id)
	{
		Teacher teacher = _universityDbContext.Teachers.FirstOrDefault(t => t.Id == id);
		if (teacher == null)
		{
			return NotFound();
		}

		List<Skill> teachersSkills = new List<Skill>();
		foreach (Skill skill in _universityDbContext.Skills)
		{
			if (skill.TeacherId == id)
			{
				teachersSkills.Add(skill);
			}
		}

		MyViewModel model = new MyViewModel
		{
			skills = teachersSkills,
			teacher = _universityDbContext.Teachers.ToList().Find(x => x.Id == id)
		}; 

		return View(model);
	}

	[HttpGet("/Teacher/Create")]
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost("/Teacher/Create")]
	public IActionResult Create(TeacherCreate model)
	{
		if (!ModelState.IsValid) {
			return View(model);
		}
		var teachers = _universityDbContext.Teachers.ToList();
		var maxId = teachers.Max(x => x.Id);
		var newTeacher = new Teacher();
		newTeacher.Id = maxId + 1;
		newTeacher.FirstName = model.FirstName;
		newTeacher.LastName = model.LastName;
		newTeacher.Age = model.Age;
		_universityDbContext.Teachers.Add(newTeacher);
		var skillMaxId = _universityDbContext.Skills.Max(x => x.Id) + 1;
		var skillNames = model.Skills.Split(",").ToList();
		foreach (var skillName in skillNames) {
			_universityDbContext.Skills.Add(new Skill()
			{
				Id = skillMaxId++,
				Name = skillName,
				TeacherId = newTeacher.Id
			});
		}

		_universityDbContext.SaveChanges();
		return RedirectToAction("All");
	}

	[HttpGet]
    [Route("teacher/put/{id},{_firstName},{_skills}")]
    public IActionResult Put([FromRoute] int id, [FromRoute] string _firstName, [FromRoute] string _skills)
	{
		Teacher? teacher = _universityDbContext.Teachers.ToList().Find(x => x.Id == id);
		if (teacher == null)
		{
			return NotFound();
		}

		_universityDbContext.Update(teacher);
		if (!string.IsNullOrEmpty(_firstName))
			teacher.FirstName = _firstName;

		if (!string.IsNullOrEmpty(_skills))
		{
			var skills = new List<Skill>();
			var id_iter = _universityDbContext.Skills.ElementAt(_universityDbContext.Skills.Count()).Id + 1;
			foreach (var thing in _skills.Split(",").ToList())
			{
				skills.Add(new Skill()
				{
					Id = id_iter++,
					Name = thing,
					TeacherId = teacher.Id
				});
			}
			foreach (var thing in _universityDbContext.Skills)
			{
				if (thing.TeacherId == teacher.Id)
					_universityDbContext.Skills.Remove(thing);
			}
			_universityDbContext.Skills.AddRange(skills);
		}
		_universityDbContext.SaveChanges();
		return RedirectToAction("All");
	}

	[HttpDelete("teacher/delete/{id}")]
	public IActionResult Delete(int id)
	{
		Teacher? teacher = _universityDbContext.Teachers.ToList().Find(x => x.Id == id);
		if (teacher == null)
		{
			return NotFound();
		}

		foreach (var thing in _universityDbContext.Skills)
		{
			if (thing.TeacherId == teacher.Id)
				_universityDbContext.Skills.Remove(thing);
		}
		_universityDbContext.Teachers.Remove(teacher);
		_universityDbContext.SaveChanges();
		return RedirectToAction("All");
	}
}

public class MyViewModel
{
	public List<Skill> skills { get; set; }
	public Teacher teacher { get; set; }
}

public class TeacherCreate
{
	[Required(ErrorMessage = "A teacher needs a name")]
	public string FirstName { get; set; }
	[Required(ErrorMessage = "A teacher needs a last name")]
	public string LastName { get; set; }
	[Required(ErrorMessage = "A teacher needs an age number")]
	public int Age { get; set; }
	[Required(ErrorMessage = "A teacher needs a comma seperated skills")]
	public string Skills { get; set; }
}