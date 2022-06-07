using Marmara.API.Models;
using Marmara.Common;
using Marmara.Data;
using Marmara.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Marmara.API.Controllers
{
    [Authorize(Roles = Const.RoleAdmin)]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {

        [HttpPost]
        [Route("create")]
        public IActionResult Create(SchTaskModel model)
        {
            using var dbContext = new MarmaraDbContext();
            var data = new SchTask
            {
                ObjectName = model.ObjectName.ToString(),
                ObjectStatus = model.ObjectStatus.ToString(),
                ScheduleTime = model.ScheduleTime.AddHours(3),
                TaskStatus = TaskStatusEnum.WillWork,
            };
            dbContext.SchTasks.Add(data);
            return Ok(dbContext.SaveChanges());
        }

        [HttpGet]
        [Route("remove/{id?}")]
        public IActionResult Remove(int id)
        {
            using var dbContext = new MarmaraDbContext();
            dbContext.SchTasks.Remove(dbContext.SchTasks.Find(id));
            return Ok(dbContext.SaveChanges());
        }

        [HttpGet]
        [Route("getList")]
        public IActionResult GetList()
        {
            using var dbContext = new MarmaraDbContext();
            var list = dbContext.SchTasks.OrderByDescending(x=>x.CreatedDate).ToList();
            return Ok(list);
        }

    }
}
