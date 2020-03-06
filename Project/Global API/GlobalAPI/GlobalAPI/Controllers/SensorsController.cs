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
    public class SensorsController : ApiController
    {
        [NoDataFoundExceptionFilter]
        [Route("api/sensors")]
        // GET: api/Sensors
        public IHttpActionResult GetAllSensors()
        {
            return Ok(SqlServerHelper.GetAllSensors());
        }

        /*
        [NoDataFoundExceptionFilter]
        [Route("api/sensors/{sensorId:int}")]
        // GET: api/Sensors/5
        public IHttpActionResult GetSensorById(int id)
        {
            return Ok(SqlServerHelper.GetSensorById(id));
        }*/

        [NoDataFoundExceptionFilter]
        [Route("api/sensors/{location}")]
        public IHttpActionResult GetSensorsByLocation(string location)
        {
            return Ok(SqlServerHelper.GetSensorsByLocation(location));
        }

        [NoDataFoundExceptionFilter]
        [Route("api/sensors/{location}/{floor}")]
        public IHttpActionResult GetSensorsByLocationAndFloor(string location, int floor)
        {
            return Ok(SqlServerHelper.GetSensorsByLocationAndFloor(location, floor));
        }

        [BatteryNotFoundExceptionFilter]
        [Route("api/sensors/{id}/battery")]
        public IHttpActionResult GetSensorBatteryByID(int id)
        {
            return Ok(new BatteryHelper { Battery = SqlServerHelper.GetSensorBattery(id) });
        }

        [SensorExistsExceptionFilter]
        [Route("api/sensors")]
        // POST: api/Sensors
        public IHttpActionResult PostSensor([FromBody]Sensor sensor)
        {
            int userId = SqlServerHelper.GetUserId(ControllerContext.RequestContext.Principal.Identity.Name);
            
            int response = SqlServerHelper.CreateSensor(sensor.Id, sensor.Floor, sensor.Location, userId, sensor.Latitude, sensor.Longitude);

            if (response > 0)
            {
                return Content(HttpStatusCode.Created, new MessageHelper { Message = "Sensor created" });
            }

            return InternalServerError();
        }
    }
}
