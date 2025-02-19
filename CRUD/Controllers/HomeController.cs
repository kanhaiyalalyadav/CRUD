using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CRUD.Models;
using CRUD.AppData;

namespace CRUD.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IEmpRepo _empRepo;

    public HomeController(ILogger<HomeController> logger,IEmpRepo empRepo)
    {
        _logger = logger;
        _empRepo = empRepo;
    }

    public IActionResult Index()
    {
        var emp = _empRepo.GetEmployees();
        return View(emp);
    }
    public IActionResult Create()
    {
        ViewBag.Departments = _empRepo.GetDepartments();
        return View();
    }
    [HttpPost]
    public IActionResult Create(Employee emp)
    {
        if (ModelState.IsValid){
            _empRepo.AddEmployee(emp);
            return RedirectToAction("Index");
        };
        ViewBag.Departments = _empRepo.GetDepartments();
        return View(emp);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
