using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto
{
    [Serializable()]
    public class Client
    {
        private String ssn;
        private String name;
        private String age;

        public String Ssn
        {
            get { return ssn; }
            set { ssn = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Age
        { 
            get { return age; }
            set { age = value; }
        }

        public override String ToString()
        {
            string formattedString = $"{Ssn,-10} {Name,-25} {Age,-5}";
            return formattedString;
        }
    }
}
