using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto
{

    [Serializable()]
    public class Event
    {
        private String _eventID;
        private String _eventName;
        private String _n_tickets_vip;
        private String _n_tickets_public;
        private String _price_vip;
        private String _price_public;
        private String _location_id;
        private String _location_name;
        private String _organizer_name;
        private String _supplier_name;
        private String _sponsor_name;
        private String _sponsor_id;
        private String _organizer_id;
        private String _supplier_id;

        public String EventID
        {
            get { return _eventID; }
            set {
                if (value == null | String.IsNullOrEmpty(value))
                {
                    throw new Exception("Event ID field can’t be empty");
                    return;
                }
                _eventID = value; 
            }
        }

        public String EventName
        {
            get { return _eventName; }
            set
            {
                if (value == null | String.IsNullOrEmpty(value))
                {
                    throw new Exception("Event Name field can’t be empty");
                    return;
                }
                _eventName = value;
            }

        }

        public String N_tickets_vip
        {
            get { return _n_tickets_vip; }
            set
            {
                _n_tickets_vip = value;
            }
        }

        public String N_tickets_public
        {
            get { return _n_tickets_public; }
            set
            {
                
                _n_tickets_public = value;
            }
        }

        public String Price_vip
        {
            get { return _price_vip; }
            set
            {
                _price_vip = value;
            }
        }


        public String Price_public
        {
            get { return _price_public; }
            set
            {
                if (value == null | String.IsNullOrEmpty(value) | value == "0")
                {
                    throw new Exception("Price of public tickets field can’t be empty or 0");
                    return;
                }
                _price_public = value;
            }
        }

        public String Location_ID
        {
            get { return _location_id; }
            set {
                if (value == null | String.IsNullOrEmpty(value) | value == "0")
                {
                    throw new Exception("Location_ID field can’t be empty or 0");
                    return;
                }
                _location_id = value; }
        }

        public String Location_Name
        {
            get { return _location_name; }
            set {
                _location_name = value;
            }
        }

        public String Sponsor_Name
        {
            get { return _sponsor_name; }
            set { _sponsor_name = value; }
        }

        public String Organizer_Name
        {
            get { return _organizer_name; }
            set { _organizer_name = value; }
        }

        public String Supplier_Name
        {
            get { return _supplier_name; }
            set
            {
                _supplier_name = value;
            }
        }

        public String Sponsor_ID
        {
            get { return _sponsor_id; }
            set { _sponsor_id = value; }
        }

        public String Supplier_ID
        {
            get { return _supplier_id; }
            set { _supplier_id = value; }
        }

        public String Organizer_ID
        {
            get { return _organizer_id; }
            set { _organizer_id = value; }
        }

        public override String ToString()
            {
            string formattedString = $"{_eventID,-5} {_eventName,-30} {Location_Name,-30} {_supplier_name,-25} {_sponsor_name,-25} {_organizer_name,-25}";
            return formattedString;
            }



        public Event() : base()
        {
        }

    }
}
