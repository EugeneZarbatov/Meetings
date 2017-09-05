using System;
using System.Collections.Generic;
using Meetings.Data.Models;

namespace Meetings.Logic.Updater
{
    /// <summary>
    /// Наблюдатель встреч расписания.
    /// </summary>
    class MeetingUpdater : IUpdater<Meeting>
    {
        /// <summary>
        /// Делегат для обработки событий: уведомление о встрече, начало/окончание встречи.
        /// </summary>
        /// <param name="str"></param>
        public delegate void Updater(string str);
        /// <summary>
        /// Встреча начнется.
        /// </summary>
        public event Updater Notified;
        /// <summary>
        /// Встреча началась.
        /// </summary>
        public event Updater Started;
        /// <summary>
        /// Встреча окончилась.
        /// </summary>
        public event Updater Finished;

        /// <summary>
        /// Конструктор наблюдателя. 
        /// </summary>
        /// <param name="updater">Метод, реагирующий на происходящие события.</param>
        public MeetingUpdater(Updater updater)
        {
            Notified = updater;
            Started = updater;
            Finished = updater;
        }

        /// <summary>
        /// Осуществляет подписку встреч на события.
        /// Notified - подошло время уведомления встречи.
        /// Started - подошло время начала встречи.
        /// Finished - подошло время окончания встречи.
        /// </summary>
        /// <param name="meetings">Список встреч.</param>
        public void Update(IEnumerable<Meeting> meetings)
        {
            foreach (Meeting meeting in meetings)
            {
                if ((meeting.NoteDateTime != null) && (meeting.NoteDateTime.ToString() == DateTime.Now.ToString()))
                {
                    Notified($"Встреча № {meeting.Id} начнется {meeting.BeginDateTime}");
                }
                if (meeting.BeginDateTime.ToString() == DateTime.Now.ToString())
                {
                    Started($"Встреча № {meeting.Id} началась в {meeting.BeginDateTime.ToLongTimeString()}");
                }
                if (meeting.EndDateTime.ToString() == DateTime.Now.ToString())
                {
                    Finished($"Встреча № {meeting.Id} закончилась в {meeting.EndDateTime.ToLongTimeString()}");
                }
            }
        }
    }
}