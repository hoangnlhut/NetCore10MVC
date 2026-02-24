using Microsoft.AspNetCore.Mvc;

namespace MiddlewareDemo.Controllers
{
    public class ModelBindingController : Controller
    {
        //https://localhost:7237/ModelBinding/Index?idx=10&name=hoang&year=2000
        [HttpGet]
        public IActionResult Index(int idx, string name, int year)
        {
            return Ok($"Id: {idx}, Name: {name}, Year: {year}");
        }

        //https://localhost:7237/GetById/Index?id=20&name=hoang1&year=1000
        [HttpGet("{id}")]
        public IActionResult GetById(int id, string name, int year)
        {
            return Ok($"Id: {id}, Name: {name}, Year: {year}");
        }

        //binding in 2 ways: with priority decrease with form data with prefix.property_name (prefix = person objext below, property_name is property of person object)
        // if there is not have prefix, later it will find property such as Id, Name, Year....
        // ModelState to check whether our input can cast the value we defined in our Person model
        // if you pass Id like 10abc -> model.state is false....
        //1. https://localhost:7237/GetWithObject/Index?person.Id=20&person.Name=hoang1&yperson.Year=1000
        //2/ https://localhost:7237/GetWithObject/Index?Id=20&Name=hoang1&Year=1000
        [HttpPost]
        public IActionResult GetWithObject(Person person)
        {
            return Ok($"Id: {person.Id}, Name: {person.Name}, Year: {person.Year} Model State: {ModelState.IsValid}");
        }

        // https://localhost:7237/ModelBinding/GetArray?stringInput[0]=hoang&stringInput[2]=hoan2g&stringInput[1]=hoang3 or
        // https://localhost:7237/ModelBinding/GetArray?stringInput=hoang&stringInput=hoan2g&stringInput=hoang3
        [HttpGet]
        public IActionResult GetArray(string[] stringInput)
        {
            return Ok($"all array String: {string.Join(",", stringInput)}");
        }

        // https://localhost:7237/ModelBinding/GetList?stringInput[0]=hoang&stringInput[2]=hoan2g&stringInput[1]=hoang3 or
        // https://localhost:7237/ModelBinding/GetList?stringInput=hoang&stringInput=hoan2g&stringInput=hoang3
        [HttpGet]
        [HttpGet]
        public IActionResult GetList(List<string> stringInput)
        {
            return Ok($"all list String: {string.Join(",", stringInput)}");
        }
    }
}
