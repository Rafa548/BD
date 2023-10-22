using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto
{

    [Serializable()]
    public class Location
    {
        private String _Location_ID;
        private String _L_Name;
        private String _L_Address;
        private String _L_City;

        public String Location_ID
        {
            get { return _Location_ID; }
            set { _Location_ID = value; }
        }

        public String L_Name
        { 
            get { return _L_Name; }
            set { _L_Name = value; }
        }

        public String L_Address
        {
            get { return _L_Address; }
            set { _L_Address = value; }
        }

        public String L_City
        {
            get { return _L_City; }
            set { _L_City = value; }
        }

        public override string ToString()
        {
            string formattedString = $"{Location_ID,-5} {L_Name,-40} {L_Address,-30} {L_City,-20}";
            return formattedString;
        }

        public Location() : base()
        {
        }
    }
}
