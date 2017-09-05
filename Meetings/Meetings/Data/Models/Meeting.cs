using System;

namespace Meetings.Data.Models
{
    /// <summary>
    /// Класс "встреча".
    /// </summary>
    public class Meeting
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Время начала.
        /// </summary>
        public DateTime BeginDateTime { get; set; }
        
        /// <summary>
        /// Время окончания.
        /// </summary>
        public DateTime EndDateTime { get; set; }
        
        /// <summary>
        /// Время уведомления.
        /// </summary>
        public DateTime? NoteDateTime { get; set; }

        public Meeting() { }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="beginDateTime">Время начала.</param>
        /// <param name="endDateTime">Время окончания.</param>
        /// <param name="noteDateTime">Время уведомления.</param>
        public Meeting(DateTime beginDateTime, DateTime endDateTime, DateTime? noteDateTime)
        {
            BeginDateTime = beginDateTime;
            EndDateTime = endDateTime;
            NoteDateTime = noteDateTime;
        }
        
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="beginDateTime">Время начала.</param>
        /// <param name="endDateTime">Время окончания.</param>
        /// <param name="noteDateTime">Время уведомления.</param>
        public Meeting(int id, DateTime beginDateTime, DateTime endDateTime, DateTime? noteDateTime) : this(beginDateTime, endDateTime, noteDateTime)
        {
            Id = id;
        }
    }
}