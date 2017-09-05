using System;
using Meetings.Data.Models;

namespace Meetings.Data.Factories
{
    /// <summary>
    /// Фабрика для создания встреч.
    /// </summary>
    public class MeetingFactory : IMeetingFactory
    {
        /// <summary>
        /// Возвращает созданный объект встречи.
        /// </summary>
        /// <param name="BeginDateTime">Время начала.</param>
        /// <param name="EndDateTime">Время окончания.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        /// <returns></returns>
        public Meeting Create(DateTime BeginDateTime, DateTime EndDateTime, DateTime? NoteDateTime)
        {
            return new Meeting(BeginDateTime, EndDateTime, NoteDateTime);
        }
        /// <summary>
        /// Возвращает созданный объект встречи.
        /// </summary>
        /// <param name="Id">Идентификатор.</param>
        /// <param name="BeginDateTime">Время начала.</param>
        /// <param name="EndDateTime">Время окончания.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        /// <returns></returns>
        public Meeting Create(int Id, DateTime BeginDateTime, DateTime EndDateTime, DateTime? NoteDateTime)
        {
            return new Meeting(Id, BeginDateTime, EndDateTime, NoteDateTime);
        }
    }
}
