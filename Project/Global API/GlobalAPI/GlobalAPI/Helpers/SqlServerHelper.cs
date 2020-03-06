using GlobalAPI.Exceptions;
using GlobalAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace GlobalAPI.Helpers
{
    public class SqlServerHelper
    {
        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["GlobalAPI.Properties.Settings.ConnStr"].ConnectionString;

        public SqlServerHelper(){

        }

        // ----------------------------------------------------<USERS>----------------------------------------------

        // Verifys if user exists
        // Returns true if user existes
        // Returns false if user does not exist
        public static bool UserExistsAndValid(string email, string password)
        {
            string queryString = "SELECT * FROM Users where Email = @email AND PasswordHash = @passwordHash";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();

                try
                {
                    byte[] byteHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    string hashPassword = Convert.ToBase64String(byteHash);

                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.AddWithValue("passwordHash", hashPassword);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return (int)reader["Valid"] == 1 ? true : false;
                    }
                    reader.Close();

                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }

                    if (sha256 != null)
                    {
                        sha256.Dispose();
                    }
                }
            }
        }

        // Creates a new User
        // email: has to match email format
        // password: more than 2 caracters
        // Returns -1 if password or email formats are invalid number or number of rows updated 0 or 1
        public static void CreateUser(string email, string password)
        {
            string queryString = "INSERT INTO Users(Email, PasswordHash) VALUES (@email, @passwordHash)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();

                try
                {
                    if (!Regex.IsMatch(email, @"[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@my.ipleiria.pt") || password.Trim().Length < 3)
                    {
                        throw new InvalidUserFieldsException("User credentials format is incorrect, email must be ?@my.ipleiria.pt and password must have more than 2 caracters");
                    }

                    byte[] byteHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    string hashPassword = Convert.ToBase64String(byteHash);

                    command.Parameters.AddWithValue("email", email);
                    command.Parameters.AddWithValue("passwordHash", hashPassword);

                    connection.Open();
                    int rowsAffected = (int)command.ExecuteNonQuery();
                    connection.Close();

                    //return rowsAffected;
                }
                catch (InvalidUserFieldsException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }

                    if (sha256 != null)
                    {
                        sha256.Dispose();
                    }
                }
            }
        }

        // Gets the id of a user
        // Returns the id or 0 if an error ocours
        public static int GetUserId(string email)
        {
            string queryString = "SELECT Id FROM Users WHERE Email = @email";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("email", email);

                try
                {
                    connection.Open();
                    int id = (int) command.ExecuteScalar();
                    connection.Close();

                    return id;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        // ----------------------------------------------------</USERS>----------------------------------------------

        // ----------------------------------------------------<SENSORS>----------------------------------------------

        // Gets information about all sensors
        // Returns a list of all the Sensors or null
        public static List<Sensor> GetAllSensors()
        {
            List<Sensor> sensors = new List<Sensor>();

            string queryString = "SELECT * FROM SensorInfo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Sensor sensor = LoadSensor(reader);
                        sensors.Add(sensor);
                    }
                    reader.Close();

                    if (sensors.Count == 0)
                    {
                        throw new NoDataFoundException("Sensors not found");
                    }

                    return sensors;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        
        // Gets the sensor matching the passed id
        // Returns a Sensor or null
        public static Sensor GetSensorById(int id)
        {
            string queryString = "SELECT * FROM SensorInfo WHERE Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("id", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Sensor sensor = LoadSensor(reader);
                        return sensor;
                    }
                    reader.Close();

                    return null;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<Sensor> GetSensorsByLocation(string location)
        {
            List<Sensor> sensors = new List<Sensor>();

            string queryString = "SELECT * FROM SensorInfo WHERE Location = @location";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("location", location);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Sensor sensor = LoadSensor(reader);
                        sensors.Add(sensor);
                    }
                    reader.Close();

                    if (sensors.Count == 0)
                    {
                        throw new NoDataFoundException("Sensors not found");
                    }

                    return sensors;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<Sensor> GetSensorsByLocationAndFloor(string location, int floor)
        {
            List<Sensor> sensors = new List<Sensor>();

            string queryString = "SELECT * FROM SensorInfo WHERE Location = @location AND Floor = @floor";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("location", location);
                command.Parameters.AddWithValue("floor", floor);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Sensor sensor = LoadSensor(reader);
                        sensors.Add(sensor);
                    }
                    reader.Close();

                    if (sensors.Count == 0)
                    {
                        throw new NoDataFoundException("Sensors not found");
                    }

                    return sensors;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static int GetSensorBattery(int id)
        {
            string queryString = "SELECT TOP 1 Battery FROM SensorData WHERE SensorId = @id AND Valid = 1 ORDER BY TimeStamp DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("id", id);

                try
                {
                    connection.Open();
                    var battery = command.ExecuteScalar();

                    connection.Close();

                    if (battery == DBNull.Value)
                    {
                        throw new BatteryNotFoundException("No battery value found");
                    }

                    return (int) battery;
                }
                catch (BatteryNotFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        // Created new sensor
        // Returns 1 if successful else 0
        public static int CreateSensor(int id, int floor, string location, int userId, string latitude, string longitude)
        {
            string querry = "INSERT INTO SensorInfo(Id, Floor, Location, UserId, Latitude, Longitude) VALUES (@id, @floor, @location, @userId, @latitude, @longitude)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(querry, connection);
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("floor", floor);
                command.Parameters.AddWithValue("location", location);
                command.Parameters.AddWithValue("userId", userId);
                if (latitude.Length == 0)
                {
                    command.Parameters.AddWithValue("latitude", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("latitude", latitude);
                }

                if (longitude.Length == 0)
                {
                    command.Parameters.AddWithValue("longitude", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("longitude", longitude);
                }

                try
                {
                    Sensor sensor = GetSensorById(id);

                    if (sensor != null)
                    {
                        throw new SensorExistsException("Sensor with id " + id + " already exists");
                    }

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected;
                }
                catch (SensorExistsException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        private static Sensor LoadSensor(SqlDataReader reader)
        {
            return new Sensor() {
                Id = (int) reader["Id"],
                Floor = (int) reader["Floor"],
                Location = (string) reader["Location"],
                Latitude = (reader["Latitude"] == DBNull.Value) ? "" : (string)reader["Latitude"],
                Longitude = (reader["Longitude"] == DBNull.Value) ? "" : (string)reader["Longitude"]
            };
        }

        // ----------------------------------------------------</SENSORS>----------------------------------------------

        // ----------------------------------------------------</SENSORDATA>----------------------------------------------

        public static List<SensorData> GetAllSensorData()
        {
            List<SensorData> sensorsData = new List<SensorData>();

            string queryString = "SELECT * FROM SensorData WHERE Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SensorData sensorData = LoadSensorData(reader); ;
                        sensorsData.Add(sensorData);
                    }
                    reader.Close();

                    if (sensorsData.Count == 0)
                    {
                        throw new NoDataFoundException("Sensor data not found");
                    }

                    return sensorsData;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<SensorData> GetSensorDataBySensorId(int id)
        {
            List<SensorData> sensorsData = new List<SensorData>();

            string queryString = "SELECT * FROM SensorData WHERE SensorId = @sensorId AND Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("sensorId", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SensorData sensorData = LoadSensorData(reader);
                        sensorsData.Add(sensorData);

                    }
                    reader.Close();

                    if (sensorsData.Count == 0)
                    {
                        throw new NoDataFoundException("Sensor data not found");
                    }

                    return sensorsData;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<SensorData> GetSensorDataByLocation(string location)
        {
            List<SensorData> sensorsData = new List<SensorData>();

            string queryString = "SELECT * FROM SensorData D JOIN SensorInfo I ON D.SensorId = I.Id WHERE I.Location = @location AND Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("location", location);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SensorData sensorData = LoadSensorData(reader); ;
                        sensorsData.Add(sensorData);
                    }
                    reader.Close();

                    if (sensorsData.Count == 0)
                    {
                        throw new NoDataFoundException("Sensor data not found");
                    }

                    return sensorsData;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<SensorData> GetSensorDataByLocationAndFloor(string location, int floor)
        {
            List<SensorData> sensorsData = new List<SensorData>();

            string queryString = "SELECT * FROM SensorData D JOIN SensorInfo I ON D.SensorId = I.Id WHERE I.Location = @location AND I.Floor = @floor AND Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("location", location);
                command.Parameters.AddWithValue("floor", floor);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SensorData sensorData = LoadSensorData(reader); ;
                        sensorsData.Add(sensorData);
                    }
                    reader.Close();

                    if (sensorsData.Count == 0)
                    {
                        throw new NoDataFoundException("Sensor data not found");
                    }

                    return sensorsData;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<SensorData> GetSensorDataByLocationFloorInterval(string location, int floor, DateTime startTimestamp, DateTime endTimestamp)
        {
            List<SensorData> sensorsData = new List<SensorData>();

            string queryString = "SELECT * FROM SensorData D JOIN SensorInfo I ON D.SensorId = I.Id WHERE I.Location = @location AND I.Floor = @floor AND D.TimeStamp >= @startTimestamp AND D.TimeStamp <= @endTimestamp AND Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("location", location);
                command.Parameters.AddWithValue("floor", floor);
                command.Parameters.AddWithValue("startTimestamp", startTimestamp);
                command.Parameters.AddWithValue("endTimestamp", endTimestamp);

                try
                {
                    if (startTimestamp > endTimestamp)
                    {
                        throw new InvalidIntervalException("EndTimestamp must be greater or equal than StartTimestamp");
                    }

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        SensorData sensorData = LoadSensorData(reader); ;
                        sensorsData.Add(sensorData);

                    }
                    reader.Close();

                    if (sensorsData.Count == 0)
                    {
                        throw new NoDataFoundException("Sensor data not found");
                    }

                    return sensorsData;
                }
                catch (InvalidIntervalException)
                {
                    throw;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static int CreateSensorData(int sensorId, double temperature, double humidity, DateTime timeStamp, string currentLocation, int currentFloor)
        {
            
            string querry = "INSERT INTO SensorData(SensorId, Temperature, Humidity, TimeStamp) VALUES (@sensorId, @temperature, @humidity, @timeStamp)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(querry, connection);
                command.Parameters.AddWithValue("sensorId", sensorId);
                command.Parameters.AddWithValue("temperature", temperature);
                command.Parameters.AddWithValue("humidity", humidity);
                command.Parameters.AddWithValue("timeStamp", new DateTime(timeStamp.Year, timeStamp.Month, timeStamp.Day, timeStamp.Hour, timeStamp.Minute, timeStamp.Second));

                try
                {
                    if (!VerifySensorLocationMatchesLocation(sensorId, currentLocation, currentFloor))
                    {
                        throw new UserAndSensorLocationsDontMatchException("User location and sensor location do no match");
                    }

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    return rowsAffected;
                }
                catch (UserAndSensorLocationsDontMatchException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static void InvalidateSensorData(int id, DateTime startTimestamp, DateTime endTimestamp)
        {
            string querry = "UPDATE SensorData SET Valid = @valid WHERE SensorId = @sensorId AND TimeStamp >= @startTimestamp AND TimeStamp <= @endTimestamp";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(querry, connection);
                command.Parameters.AddWithValue("valid", 0);
                command.Parameters.AddWithValue("sensorId", id);
                command.Parameters.AddWithValue("startTimestamp", startTimestamp);
                command.Parameters.AddWithValue("endTimestamp", endTimestamp);

                try
                {
                    if (startTimestamp > endTimestamp)
                    {
                        throw new InvalidIntervalException("EndTimestamp must be greater or equal than StartTimestamp");
                    }

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connection.Close();

                    if (rowsAffected <= 0)
                    {
                        throw new NoDataFoundException("0 dados invalidados");
                    }
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (InvalidIntervalException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }


        private static bool VerifySensorLocationMatchesLocation(int id, string location, int floor)
        {
            Sensor sensor = GetSensorById(id);

            if (sensor != null && sensor.Location.Equals(location) && sensor.Floor == floor)
            {
                return true;
            }

            return false;
        }

        private static SensorData LoadSensorData(SqlDataReader reader)
        {
            SensorData sensorData = new SensorData();
            sensorData.Id = (int) reader["Id"];
            sensorData.SensorId = (int) reader["SensorId"];
            sensorData.TimeStamp = (reader["TimeStamp"] == DBNull.Value) ? new DateTime(0, 0, 0) : (DateTime) reader["TimeStamp"];
            if (reader["Temperature"] != DBNull.Value)
            {
                sensorData.Temperature = (double) reader["Temperature"];
            }
            if (reader["Humidity"] != DBNull.Value)
            {
                sensorData.Humidity = (double) reader["Humidity"];
            }

            return sensorData;
        }

        // ----------------------------------------------------</SENSORDATA>----------------------------------------------

        // ----------------------------------------------------</ALARMS>----------------------------------------------

        public static List<Alarm> GetAllAlarms()
        {
            List<Alarm> alarms = new List<Alarm>();

            string queryString = "SELECT Temperature, Humidity, TimeStamp, Reason, Floor, Location FROM Alarms A JOIN SensorData D ON A.SensorDataId = D.Id JOIN SensorInfo I ON D.SensorId = I.Id WHERE D.Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Alarm alarm = LoadAlarm(reader);
                        alarms.Add(alarm);
                    }
                    reader.Close();

                    if (alarms.Count == 0)
                    {
                        throw new NoDataFoundException("Alarms not found");
                    }

                    return alarms;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<Alarm> GetAlarmsBySensor(int id)
        {
            List<Alarm> alarms = new List<Alarm>();

            string queryString = "SELECT Temperature, Humidity, TimeStamp, Reason, Floor, Location FROM Alarms A JOIN SensorData D ON A.SensorDataId = D.Id JOIN SensorInfo I ON D.SensorId = I.Id WHERE D.SensorId = @sensorId AND D.Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("sensorId", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Alarm alarm = LoadAlarm(reader);
                        alarms.Add(alarm);
                    }
                    reader.Close();

                    if (alarms.Count == 0)
                    {
                        throw new NoDataFoundException("Alarms not found");
                    }

                    return alarms;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<Alarm> GetAlarmsByLocation(string location)
        {
            List<Alarm> alarms = new List<Alarm>();

            string queryString = "SELECT Temperature, Humidity, TimeStamp, Reason, Floor, Location FROM Alarms A JOIN SensorData D ON A.SensorDataId = D.Id JOIN SensorInfo I ON D.SensorId = I.Id WHERE I.Location = @location AND D.Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("location", location);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Alarm alarm = LoadAlarm(reader);
                        alarms.Add(alarm);
                    }
                    reader.Close();

                    if (alarms.Count == 0)
                    {
                        throw new NoDataFoundException("Alarms not found");
                    }

                    return alarms;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        public static List<Alarm> GetAlarmsByLocationAndFloor(string location, int floor)
        {
            List<Alarm> alarms = new List<Alarm>();

            string queryString = "SELECT Temperature, Humidity, TimeStamp, Reason, Floor, Location FROM Alarms A JOIN SensorData D ON A.SensorDataId = D.Id JOIN SensorInfo I ON D.SensorId = I.Id WHERE I.Location = @location AND I.Floor = @floor AND D.Valid = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("location", location);
                command.Parameters.AddWithValue("floor", floor);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Alarm alarm = LoadAlarm(reader);
                        alarms.Add(alarm);
                    }
                    reader.Close();

                    if (alarms.Count == 0)
                    {
                        throw new NoDataFoundException("Alarms not found");
                    }

                    return alarms;
                }
                catch (NoDataFoundException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                    }

                    if (command != null)
                    {
                        command.Dispose();
                    }
                }
            }
        }

        private static Alarm LoadAlarm(SqlDataReader reader)
        {
            return new Alarm()
            {
                Temperature = (double)reader["Temperature"],
                Humidity = (double)reader["Humidity"],
                TimeStamp = (reader["TimeStamp"] == DBNull.Value) ? new DateTime(2019, 1, 1) : (DateTime)reader["TimeStamp"],
                Reason = (string)reader["Reason"],
                Floor = (int)reader["Floor"],
                Location = (string)reader["Location"]
            };
        }

        // ----------------------------------------------------</ALARMS>----------------------------------------------
    }
}