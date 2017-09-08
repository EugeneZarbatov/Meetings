using System;
using System.Collections.Generic;
using System.Linq;
using Meetings.Data.Models;
using Meetings.Data.Factories;
using System.Data.SqlClient;
using System.Configuration;

namespace Meetings.Data.Repositories
{
    /// <summary>
    /// Репозиторий для встреч.
    /// </summary>
    class MeetingDbRepository : IDbRepository<Meeting>
    {
        /// <summary>
        /// Подключение к базе данных SQL.
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MeetingDbRepository()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["DbConnection"];
            _connectionString = settings.ConnectionString;
        }

        /// <summary>
        /// Извлекает все встречи репозитория.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Meeting> FindAll()
        {
            IMeetingFactory meetingFactory = new MeetingFactory();
            List<Meeting> meetings = new List<Meeting>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT * FROM Meetings", connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime? note;
                            if (reader.IsDBNull(3)) note = null;
                            else note = reader.GetDateTime(3);
                            meetings.Add(meetingFactory.Create(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2), note));
                        }
                    }
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return meetings;
        }

        /// <summary>
        /// Выполняет поиск встречи с идентификаторм id и возвращает первое вхождение в пределах репозитория.
        /// </summary>
        /// <param name="id">Идентификатор искомой встречи.</param>
        /// <returns></returns>
        public Meeting Find(int id)
        {
            Meeting meeting = null;
            IEnumerable<Meeting> meetings = FindAll();
            foreach (Meeting item in meetings)
            {
                if (item.Id == id)
                {
                    meeting = item;
                }
            }

            return meeting;
        }

        /// <summary>
        /// Добавляет встречу в конец репозитория.
        /// </summary>
        /// <param name="meeting">Добавляемая встреча.</param>
        public void Add(Meeting meeting)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"INSERT INTO Meetings ([BeginDateTime], [EndDateTime], NoteDateTime) VALUES (@BeginDateTime, @EndDateTime, @NoteDateTime)", connection);
                    command.Parameters.AddWithValue("@BeginDateTime", meeting.BeginDateTime);
                    command.Parameters.AddWithValue("@EndDateTime", meeting.EndDateTime);
                    if (meeting.NoteDateTime != null) command.Parameters.AddWithValue("@NoteDateTime", meeting.NoteDateTime);
                    else command.Parameters.AddWithValue("@NoteDateTime", DBNull.Value);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Удаляет встречу с идентификаром id из репозитория.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой встречи.</param>
        public void Remove(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"DELETE FROM Meetings WHERE Id = @Id", connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Редактирует встречу с идентификатором id.
        /// </summary>
        /// <param name="id">Идентификатор редактируемой встречи.</param>
        /// <param name="meeting">Встреча, заменяющая старую встречу репозитория.</param>
        public void Edit(int id, Meeting meeting)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"UPDATE Meetings SET [BeginDateTime] = @BeginDateTime, [EndDateTime] = @EndDateTime, NoteDateTime = @NoteDateTime WHERE Id = {id}", connection);
                    command.Parameters.AddWithValue("@BeginDateTime", meeting.BeginDateTime);
                    command.Parameters.AddWithValue("@EndDateTime", meeting.EndDateTime);
                    if (meeting.NoteDateTime != null) command.Parameters.AddWithValue("@NoteDateTime", meeting.NoteDateTime);
                    else command.Parameters.AddWithValue("@NoteDateTime", DBNull.Value);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Получает число встреч, содержащихся в репозитории.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            object count = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Meetings", connection);
                    count = command.ExecuteScalar();
                    connection.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return (int)count;
        }
    }
}