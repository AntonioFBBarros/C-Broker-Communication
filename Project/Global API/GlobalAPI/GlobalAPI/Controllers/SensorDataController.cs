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
    public class SensorDataController : ApiController
    {
        [NoDataFoundExceptionFilter]
        [Route("api/sensordata")]
        // GET: api/SensorData
        public IHttpActionResult GetAllSensorData()
        {
            return Ok(SqlServerHelper.GetAllSensorData());
        }

        [NoDataFoundExceptionFilter]
        [Route("api/sensordata/{sensorId:int}")]
        // GET: api/SensorData
        public IHttpActionResult GetSensorDataBySensorID(int sensorId)
        {
            return Ok(SqlServerHelper.GetSensorDataBySensorId(sensorId));
        }

        [NoDataFoundExceptionFilter]
        [Route("api/sensordata/{location}")]
        public IHttpActionResult GetSensorDataByLocation(string location)
        {
            return Ok(SqlServerHelper.GetSensorDataByLocation(location));
        }

        [NoDataFoundExceptionFilter]
        [Route("api/sensordata/{location}/{floor}")]
        public IHttpActionResult GetSensorDataByLocationAndFloor(string location, int floor)
        {
            return Ok(SqlServerHelper.GetSensorDataByLocationAndFloor(location, floor));
        }

        [NoDataFoundExceptionFilter]
        [InvalidIntervalExceptionFilter]
        [Route("api/sensordata/{location}/{floor}/{startTimestamp:regex(^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}$)}/{endTimestamp:regex(^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}$)}")]
        public IHttpActionResult GetSensorDataByLocation(string location, int floor, DateTime startTimestamp, DateTime endTimestamp)
        {
            return Ok(SqlServerHelper.GetSensorDataByLocationFloorInterval(location, floor, startTimestamp, endTimestamp));
        }

        [UserAndSensorLocationsDontMatchExceptionFilter]
        [Route("api/sensordata")]
        // POST: api/SensorData
        public IHttpActionResult PostSensorData([FromBody]SensorDataInput sensorDataInput)
        {
            int response = SqlServerHelper.CreateSensorData(sensorDataInput.SensorId, sensorDataInput.Temperature, sensorDataInput.Humidity, DateTime.Now, sensorDataInput.CurrentLocation, sensorDataInput.CurrentFloor);

            if (response > 0)
            {
                return Content(HttpStatusCode.Created, new MessageHelper { Message = "Sensor data created" });
            }

            return InternalServerError();
        }

        [NoDataFoundExceptionFilter]
        [InvalidIntervalExceptionFilter]
        [Route("api/sensordata/{sensorId:int}")]
        // PUT: api/SensorData/5
        public IHttpActionResult PutInvalidateSensorDataInInterval(int sensorId, [FromBody]Interval interval)
        {
            SqlServerHelper.InvalidateSensorData(sensorId, interval.StartTimestamp, interval.EndTimestamp);

            return Content(HttpStatusCode.OK, new MessageHelper { Message = "Success" }); ;
        }
    }
}
