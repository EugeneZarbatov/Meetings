using System;
using Meetings.Data.Models;

namespace Meetings.Data.Factories
{
    /// <summary>
    /// Интерфейс фабрики для создания встреч.
    /// </summary>
    public interface IMeetingFactory
    {
        /// <summary>
        /// Возвращает созданный объект встречи.
        /// </summary>
        /// <param name="BeginDateTime">Время начала.</param>
        /// <param name="EndDateTime">Время окончания.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        /// <returns></returns>
        Meeting Create(DateTime BeginDateTime, DateTime EndDateTime, DateTime? NoteDateTime);

        /// <summary>
        /// Возвращает созданный объект встречи.
        /// </summary>
        /// <param name="Id">Идентификатор.</param>
        /// <param name="BeginDateTime">Время начала.</param>
        /// <param name="EndDateTime">Время окончания.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        /// <returns></returns>
        Meeting Create(int Id, DateTime BeginDateTime, DateTime EndDateTime, DateTime? NoteDateTime);
    }
}
