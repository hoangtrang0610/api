using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.CukCuk.Api.Models;
using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace MISA.CukCuk.Api.Controllers
{
    [Route("api/v1/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        /// <summary>
        /// lấy toàn bộ khách hàng
        /// </summary>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: HTTrang(08/01/2021)
        [HttpGet]
        public IActionResult Get()
        {
            var connectionString = "";
            IDbConnection dbConnection = new MySqlConnection(connectionString);

            return Ok(new string[] { "value1", "value2" });
        }

        /// <summary>
        /// lấy danh sách khách hàng theo id và tên
        /// </summary>
        /// <param name="id">id của khách hàng</param>
        /// <param name="name">tên của khách ahngf</param>
        /// <returns>Danh sách khách hàng</returns>
        /// CreatedBy: HTTrang(08/01/2021) 
        [HttpGet("{filter}")]
        public IActionResult Get([FromHeader] int id, [FromQuery] string name)
        {
            return Ok("value");
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post(Customer customer)
        {
            return Ok(Created("Add Customer", customer));
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return Ok(1);
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok("delete");
        }
    }
}

