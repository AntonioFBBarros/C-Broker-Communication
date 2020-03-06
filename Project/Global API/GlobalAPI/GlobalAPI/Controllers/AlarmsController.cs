using GlobalAPI.Exceptions;
using GlobalAPI.Helpers;
using GlobalAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GlobalAPI.Controllers
{
    [Authorize]
    public class AlarmsController : ApiController
    {

        // GET: api/Alarms
        [NoDataFoundExceptionFilter]
        [Route("api/alarms")]
        public IHttpActionResult GetAllAlarms()
        {
            return Ok(SqlServerHelper.GetAllAlarms());
        }

        // GET: api/Alarms/5
        [NoDataFoundExceptionFilter]
        [Route("api/alarms/{sensorId:int}")]
        public IHttpActionResult GetAlarmsBySensor(int sensorId)
        {
            return Ok(SqlServerHelper.GetAlarmsBySensor(sensorId));
        }

        [NoDataFoundExceptionFilter]
        [Route("api/alarms/{location}")]
        public IHttpActionResult GetAlarmsByLocation(string location)
        {
            return Ok(SqlServerHelper.GetAlarmsByLocation(location));
        }

        [NoDataFoundExceptionFilter]
        [Route("api/alarms/{location}/{floor}")]
        public IHttpActionResult GetAlarmsByLocationAndFloor(string location, int floor)
        {
            return Ok(SqlServerHelper.GetAlarmsByLocationAndFloor(location, floor));
        }
    }
}
