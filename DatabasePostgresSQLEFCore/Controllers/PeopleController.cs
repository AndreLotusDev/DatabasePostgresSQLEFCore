using DatabasePostgresSQLEFCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatabasePostgresSQLEFCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public PeopleController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        [Route("add-people-legal")]
        public async Task<IActionResult> AddPeopleLegal(PeopleLegal peopleLegalToInsert)
        {
            await dbContext.PeopleLegal.AddAsync(peopleLegalToInsert);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("add-people-physical")]
        public async Task<IActionResult> AddPeoplePhysical(PeoplePhysical peoplePhysicalToInsert)
        {
            await dbContext.PeoplePhysical.AddAsync(peoplePhysicalToInsert);
            await dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        [Route("get-all-people-legal")]
        public async Task<IActionResult> GetAllPeopleLegal()
        {
            var allToReturn = await dbContext.PeopleLegal.ToListAsync();

            return Ok(allToReturn);
        }

        [HttpGet]
        [Route("get-all-people-physical")]
        public async Task<IActionResult> GetAllPeoplePhysical()
        {
            var allToReturn = await dbContext.PeoplePhysical.ToListAsync();

            return Ok(allToReturn);
        }

        [HttpGet]
        [Route("get-all-people")]
        public async Task<IActionResult> GetAllPeople()
        {
            var allToReturn = await dbContext.People.ToListAsync();

            return Ok(allToReturn);
        }
    }
}
