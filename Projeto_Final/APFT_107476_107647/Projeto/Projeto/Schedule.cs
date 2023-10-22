using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto
{
    [Serializable()]
    public class Schedule
    {
        private string _eventId;
        private string _day;
        private string _activityId;
        private string _aName;
        private string _hour;
        private string _scheduleId;
        private string _cost;
        private string _objective;

        public string EventId
        {
            get { return _eventId; }
            set {
                _eventId = value; }
        }

        public string Day
        {
            get { return _day; }
            set { _day = value; }
        }

        public string ActivityId
        {
            get { return _activityId; }
            set { _activityId = value; }
        }

        public string Hour
        {
            get { return _hour; }
            set { _hour = value; }
        }

        public string ScheduleId
        {
            get { return _scheduleId; }
            set { _scheduleId = value; }
        }

        public string Name
        {
            get { return _aName; }
            set { _aName = value; }
        }

        public string Objective
        {
            get { return _objective; }
            set { _objective = value; }
        }

        public string Cost 
        {
            get { return _cost; }
            set
            {
                _cost = value;
            }
        }

        public override string ToString()
        {
            string formattedString = $"{ScheduleId,-5} {ActivityId,-5} {Name,-25} {Day,-5} {Hour}";
            return formattedString;
        }
    }
}
