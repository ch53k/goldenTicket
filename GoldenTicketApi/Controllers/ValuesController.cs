using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace GoldenTicketApi.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class GoldenTicketController : ControllerBase
    {
        private SqlConnection _connection;
        private const string sql = "SELECT * FROM Ticket WHERE id=@id";

        public GoldenTicketController()
        {
            _connection = new SqlConnection("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=GoldenTicket;Integrated Security=SSPI");
        }
        // GET api/values
        [Route("")]
        public async Task<ActionResult<IEnumerable<int>>> Get()
        {
            var ticket = await _connection.QueryAsync<int>("SELECT Id FROM Ticket");
            return Ok(ticket);
        }
        // GET api/values
        [Route("sync/{id}")]
        public ActionResult<bool> GetSync(int id)
        {
            var ticket = _connection.QueryFirst<Ticket>(sql, new { id });
            return ticket.IdGolden;
        }

        // GET api/values/5
        [Route("async/{id}")]
        public async Task<bool> GetAsync(int id)
        {
            var ticket = await _connection.QueryFirstAsync<Ticket>(sql, new {id});
            return ticket.IdGolden;
        }
    }

    public class Ticket
    {
        public int Id { get; set; }
        public bool IdGolden { get; set; }
    }
}
