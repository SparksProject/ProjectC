using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ProjectC.Data.Models;
using ProjectC.DTO;

namespace ProjectC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeyannameController : ControllerBase
    {
        private readonly IHostingEnvironment Environment;

        public BeyannameController(IHostingEnvironment hostingenvironment)
        {
            Environment = hostingenvironment;
        }



        [HttpPost("List")]
        public IActionResult Index([FromForm] BeyannameDTO obj)
        {
            try
            {
                using (ProjectCContext context = new ProjectCContext())
                {
                    var sqlUserId = new SqlParameter("@Id", SqlDbType.Int) { Value = obj.UserId };
                    var sqlDosyaNo = new SqlParameter("@Dosya_No", SqlDbType.NVarChar) { Value = obj.Dosya_No };
                    var sqlTip = new SqlParameter("@Tip", SqlDbType.NVarChar) { Value = obj.Tip };
                    var sqlBeyannameNo = new SqlParameter("@VarisGumrukA_2",SqlDbType.NVarChar) { Value = obj.VarisGumrukA_2 };
                    //var sqlFaturaNo = new SqlParameter("@FaturaNo", SqlDbType.NVarChar) { Value = obj.FaturaNo };
                    var sqlBeyannameTarihiBaslangic = new SqlParameter("@BeyannameTarihi1", SqlDbType.DateTime) { Value = obj.BeyannameTarihiBaslangic };
                    var sqlBeyannameTarihiBitis = new SqlParameter("@BeyannameTarihi2", SqlDbType.DateTime) { Value = obj.BeyannameTarihiBitis };



                    var parameters = new List<SqlParameter> { sqlUserId };

                    var query = $"SELECT * FROM vw_beyannamebasim WHERE UserId = @Id";

                    if (!string.IsNullOrEmpty(obj.Dosya_No))
                    {
                        query += " AND Dosya_No = @Dosya_No";
                        parameters.Add(sqlDosyaNo);
                    }
                    if (!string.IsNullOrEmpty(obj.VarisGumrukA_2))
                    {
                        query += " AND VarisGumrukA_2 = @VarisGumrukA_2";
                        parameters.Add(sqlBeyannameNo);
                    }
                    if (!string.IsNullOrEmpty(obj.Tip))
                    {
                        query += " AND Tip = @Tip";
                        parameters.Add(sqlTip);
                    }
                    if (obj.BeyannameTarihiBaslangic.HasValue && obj.BeyannameTarihiBitis.HasValue)
                    {
                        query += " AND VarisGumrukA_3 BETWEEN @BeyannameTarihi1 and @BeyannameTarihi2";

                        parameters.Add(sqlBeyannameTarihiBaslangic);
                        parameters.Add(sqlBeyannameTarihiBitis);
                    }




                    var target = new List<Beyanname>();



                    foreach (var item in context.Beyanname.FromSql(query, parameters.ToArray<object>()))
                    {
                        target.Add(item);
                    }



                    return Ok(target);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(string id)
        {
            try
            {
                using (ProjectCContext context = new ProjectCContext())
                {
                    var DosyaNoParameter = new SqlParameter("@VarisGumrukA_2", SqlDbType.NVarChar);
                    DosyaNoParameter.Value = id;


                    var Entity = context.Beyanname.FromSql($"Select * from vw_beyannamebasim where (VarisGumrukA_2 = {DosyaNoParameter})").FirstOrDefault();

                    return Ok(Entity);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);

            }
            
        }
    }
}