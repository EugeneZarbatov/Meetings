using System;
using Meetings.Data.Models;
using Meetings.Logic.Printer;

namespace Meetings.Logic.Shedule
{
    /// <summary>
    /// Интерфейс расписания.
    /// </summary>
    public interface ISchedule
    {
        /// <summary>
        /// Добавляет всречу в расписание.
        /// </summary>
        /// <param name="BeginDateTime">Время начала.</param>
        /// <param name="EndDateTime">Время окончания.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        void Add(DateTime BeginDateTime, DateTime EndDateTime, DateTime? NoteDateTime);

        /// <summary>
        /// Выполняет поиск встречи с идентификаторм id и возвращает первое вхождение в пределах расписания.
        /// </summary>
        /// <param name="id">Идентификатор искомой встречи.</param>
        /// <returns></returns>
        Meeting Find(int id);

        /// <summary>
        /// Добавляет уведомление встрече с идентификатором id.
        /// </summary>
        /// <param name="id">Идентификатор встречи.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        void AddNotification(int id, DateTime NoteDateTime);

        /// <summary>
        /// Удаляет встречу с идентификатором id.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой встречи.</param>
        void Remove(int id);

        /// <summary>
        /// Редактирует встречу.
        /// </summary>
        /// <param name="id">Идентификатор встречи.</param>
        /// <param name="BeginDateTime">Время начала.</param>
        /// <param name="EndDateTime">Время окончания.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        void Edit(int id, DateTime BeginDateTime, DateTime EndDateTime, DateTime? NoteDateTime);

        /// <summary>
        /// Выводит текстовое представление встреч расписания, соответствующих указанной дате, в консоль.
        /// Для вывода используется консольный принтер.
        /// </summary>
        /// <param name="printer">Принтер для вывода элементов в консоль.</param>
        /// <param name="day">Дата.</param>
        void ConsolePrint(IConsoleSchedulePrinter<Meeting> printer, DateTime day);

        /// <summary>
        /// Записывает текстовое представление встреч расписания, соответствующих указанной дате, в файл по заданнному пути.
        /// Для записи используется файловый принтер.
        /// </summary>
        /// <param name="printer">Принтер для записи элементов в файл.</param>
        /// <param name="day">Дата.</param>
        /// <param name="path">Путь сохранения файла.</param>
        void FilePrint(IFileSchedulePrinter<Meeting> printer, DateTime day, string path);

        /// <summary>
        /// Получает число встреч, содержащихся в расписании.
        /// </summary>
        /// <returns></returns>
        int Count();
    }
}
