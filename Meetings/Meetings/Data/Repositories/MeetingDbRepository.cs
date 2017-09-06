using System;
using System.Collections.Generic;
using System.Linq;
using Meetings.Data.Models;
using Meetings.Data.Factories;
using System.Data.SqlClient;

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
        private SqlConnection _connection = new SqlConnection();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="connectionString">строка подключения.</param>
        public MeetingDbRepository(string connectionString)
        {
            _connection.ConnectionString = connectionString;
        }

        /// <summary>
        /// Извлекает все встречи репозитория.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Meeting> FindAll()
        {
            _connection.Open();
            List<Meeting> meetings = new List<Meeting>();
            IMeetingFactory meetingFactory = new MeetingFactory();
            SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Meetings", _connection);
            object count = command.ExecuteScalar();
            command.CommandText = "SELECT * FROM Meetings";
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
            _connection.Close();

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
            _connection.Open();
            SqlCommand command = new SqlCommand($"INSERT INTO Meetings ([BeginDateTime], [EndDateTime], NoteDateTime) VALUES (@BeginDateTime, @EndDateTime, @NoteDateTime)", _connection);
            command.Parameters.AddWithValue("@BeginDateTime", meeting.BeginDateTime);
            command.Parameters.AddWithValue("@EndDateTime", meeting.EndDateTime);
            if (meeting.NoteDateTime != null) command.Parameters.AddWithValue("@NoteDateTime", meeting.NoteDateTime);
            else command.Parameters.AddWithValue("@NoteDateTime", DBNull.Value);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        /// <summary>
        /// Удаляет встречу с идентификаром id из репозитория.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой встречи.</param>
        public void Remove(int id)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand($"DELETE FROM Meetings WHERE Id = @Id", _connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        /// <summary>
        /// Редактирует встречу с идентификатором id.
        /// </summary>
        /// <param name="id">Идентификатор редактируемой встречи.</param>
        /// <param name="meeting">Встреча, заменяющая старую встречу репозитория.</param>
        public void Edit(int id, Meeting meeting)
        {
            _connection.Open();
            SqlCommand command = new SqlCommand($"UPDATE Meetings SET [BeginDateTime] = @BeginDateTime, [EndDateTime] = @EndDateTime, NoteDateTime = @NoteDateTime WHERE Id = {id}", _connection);
            command.Parameters.AddWithValue("@BeginDateTime", meeting.BeginDateTime);
            command.Parameters.AddWithValue("@EndDateTime", meeting.EndDateTime);
            if (meeting.NoteDateTime != null) command.Parameters.AddWithValue("@NoteDateTime", meeting.NoteDateTime);
            else command.Parameters.AddWithValue("@NoteDateTime", DBNull.Value);
            command.ExecuteNonQuery();
            _connection.Close();
        }

        /// <summary>
        /// Получает число встреч, содержащихся в репозитории.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            IEnumerable<Meeting> meetings = FindAll();
            return meetings.Count();
        }
    }
}
