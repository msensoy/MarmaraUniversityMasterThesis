using Marmara.Common;
using Marmara.Common.Model;
using Marmara.Data;
using Marmara.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Marmara.API.Controllers
{
    //[Authorize(Roles = Const.RoleAdmin)]
    [Route("api/[controller]")]
    [ApiController]
    public class RuleController : ControllerBase
    {
        [HttpGet]
        [Route("getRules")]
        public IActionResult GetRules()
        {
            using var dbContext = new MarmaraDbContext();
            var ruleForLux = dbContext.Rules.FirstOrDefault(x => x.Name == Const.LUX);
            if (ruleForLux == null)
            {
                dbContext.Rules.Add(new Rule()
                {
                    Name = Const.LUX,
                    IsActive = false,
                    IsGreaterThen = false,
                    Value = 0,
                });
                dbContext.Rules.Add(new Rule()
                {
                    Name = Const.TEMPERATURE,
                    IsActive = false,
                    IsGreaterThen = false,
                    Value = 0,
                });
                dbContext.SaveChanges();
            }

            return Ok(dbContext.Rules.ToList());
        }

        [HttpPost]
        [Route("save")]
        public IActionResult Save(List<RuleModel> models)
        {
            System.Console.WriteLine(models[0].ToString());
            System.Console.WriteLine(models[1].ToString());
            //I-i harfi tr karakterde küçültüldüğü için hata veriyormuş. O yüzden burası böyle yapılmıştı.  Helper class ında  streamWriter.Write(value.ToLower()); bu kod yüzünden..
            models[0].IsActive = models[0].ValueActive ==1?true:false; 
            models[1].IsActive = models[1].ValueActive ==1?true:false;  
            models[0].IsGreaterThen = models[0].ValueGreaterThen ==1?true:false;
            models[1].IsGreaterThen = models[1].ValueGreaterThen == 1?true:false;
       
    
            using var dbContext = new MarmaraDbContext();
            var ruleList = new List<Rule>();
            for (int i = 0; i < models.Count; i++)
            {
                var rule = dbContext.Rules.FirstOrDefault(x => x.Name == models[i].Name);
                rule.IsActive = models.FirstOrDefault(x => x.Name == models[i].Name).IsActive;
                rule.IsGreaterThen = models.FirstOrDefault(x => x.Name == models[i].Name).IsGreaterThen;
                rule.Value = models.FirstOrDefault(x => x.Name == models[i].Name).Value;
                ruleList.Add(rule);
            }

            dbContext.Rules.UpdateRange(ruleList);
            var cnt = dbContext.SaveChanges();
            return Ok(cnt);
        }
    }
}
