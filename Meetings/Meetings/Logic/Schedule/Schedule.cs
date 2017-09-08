using System;
using Meetings.Data.Repositories;
using Meetings.Data.Models;
using Meetings.Data.Factories;
using Meetings.Logic.Printer;
using System.Collections.Generic;

namespace Meetings.Logic.Shedule
{
    /// <summary>
    /// Расписание.
    /// </summary>
    class Schedule : ISchedule
    {
        /// <summary>
        /// Репозиторий.
        /// </summary>
        private IDbRepository<Meeting> _meetings = new MeetingDbRepository();
        /// <summary>
        /// Фабрика для встреч.
        /// </summary>
        public IMeetingFactory meetingFactory = new MeetingFactory();

        /// <summary>
        /// Добавляет всречу в расписание.
        /// </summary>
        /// <param name="BeginDateTime">Время начала.</param>
        /// <param name="EndDateTime">Время окончания.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        public void Add(DateTime BeginDateTime, DateTime EndDateTime, DateTime? NoteDateTime)
        {
            Meeting meeting = meetingFactory.Create(BeginDateTime, EndDateTime, NoteDateTime);
            if (Validate(meeting, FindAll()))
            {
                _meetings.Add(meeting);
            }
            else
            {
                throw new Exception("Ошибка создания встречи!");
            }
        }

        /// <summary>
        /// Редактирует встречу.
        /// </summary>
        /// <param name="id">Идентификатор встречи.</param>
        /// <param name="BeginDateTime">Время начала.</param>
        /// <param name="EndDateTime">Время окончания.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        public void Edit(int id, DateTime BeginDateTime, DateTime EndDateTime, DateTime? NoteDateTime)
        {
            List<Meeting> meetings = FindAll() as List<Meeting>;
            Meeting item = meetings.Find(x => x.Id == id);
            meetings.Remove(item);
            Meeting meeting = meetingFactory.Create(BeginDateTime, EndDateTime, NoteDateTime);
            if (Validate(meeting, meetings))
            {
                _meetings.Edit(id, meeting);
            }
            else
            {
                throw new Exception("Ошибка редактирования встречи!");
            }
        }

        /// <summary>
        /// Проверяет создаваемую/редактируемую встречу на корректность. 
        /// </summary>
        /// <param name="newMeeting">Создаваемая встреча.</param>
        /// <param name="meetings">Расписание встреч.</param>
        /// <returns></returns>
        public bool Validate(Meeting newMeeting, IEnumerable<Meeting> meetings)
        {
            bool isIntersect = false;
            foreach (Meeting meeting in meetings)
            {
                if (((newMeeting.BeginDateTime >= meeting.BeginDateTime) & (newMeeting.BeginDateTime < meeting.EndDateTime)) || ((newMeeting.EndDateTime > meeting.BeginDateTime) & (newMeeting.EndDateTime <= meeting.EndDateTime)))
                    isIntersect = true;
            }
            return !(isIntersect || newMeeting.BeginDateTime < DateTime.Now || newMeeting.BeginDateTime >= newMeeting.EndDateTime || newMeeting.NoteDateTime >= newMeeting.BeginDateTime);
        }

        /// <summary>
        /// Добавляет уведомление встрече с идентификатором id.
        /// </summary>
        /// <param name="id">Идентификатор встречи.</param>
        /// <param name="NoteDateTime">Время уведомления.</param>
        public void AddNotification(int id, DateTime NoteDateTime)
        {
            var meeting = Find(id);
            if (meeting.NoteDateTime != null) throw new Exception($"Встреча № {id} уже имеет уведомление!");
            if (NoteDateTime < meeting.BeginDateTime && meeting.BeginDateTime > DateTime.Now)
            {
                _meetings.Edit(id, meetingFactory.Create(meeting.BeginDateTime, meeting.EndDateTime, NoteDateTime));
            }
            else throw new Exception("Ошибка добавления уведомления!");
        }

        /// <summary>
        /// Удаляет встречу с идентификатором id.
        /// </summary>
        /// <param name="id">Идентификатор удаляемой встречи.</param>
        public void Remove(int id)
        {
            if (Find(id) == null) throw new Exception($"Встреча № {id} не найдена!");
            _meetings.Remove(id);
        }

        /// <summary>
        /// Осуществляет подписку встреч на события.
        /// </summary>
        /// <param name="updater">Наблюдатель.</param>
        public void Update(Observer.IObserver<Meeting> observer)
        {
            //if (Count() != 0)
            //{
                observer.Update(FindAll());
            //}
        }

        /// <summary>
        /// Выводит текстовое представление встреч расписания, соответствующих указанной дате, в консоль.
        /// Для вывода используется консольный принтер.
        /// </summary>
        /// <param name="printer">Принтер для вывода элементов в консоль.</param>
        /// <param name="day">Дата.</param>
        public void ConsolePrint(IConsoleSchedulePrinter<Meeting> printer, DateTime day)
        {
            printer.Print(FindAll(), day);
        }

        /// <summary>
        /// Записывает текстовое представление встреч расписания, соответствующих указанной дате, в файл по заданнному пути.
        /// Для записи используется файловый принтер.
        /// </summary>
        /// <param name="printer">Принтер для записи элементов в файл.</param>
        /// <param name="day">Дата.</param>
        /// <param name="path">Путь сохранения файла.</param>
        public void FilePrint(IFileSchedulePrinter<Meeting> printer, DateTime day, string path)
        {
            printer.Print(FindAll(), day, path);
        }

        /// <summary>
        /// Выполняет поиск встречи с идентификаторм id и возвращает первое вхождение в пределах расписания.
        /// </summary>
        /// <param name="id">Идентификатор искомой встречи.</param>
        /// <returns></returns>
        public Meeting Find(int id)
        {
            return _meetings.Find(id);
        }

        /// <summary>
        /// Извлекает все встречи репозитория.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Meeting> FindAll()
        {
            return _meetings.FindAll();
        }

        /// <summary>
        /// Получает число встреч, содержащихся в расписании.
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return _meetings.Count();
        }
    }
}