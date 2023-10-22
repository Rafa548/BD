using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto
{
    
    [Serializable()]
    public class Sales
    {
        private String _saleID;
        private String _eventID;
        private String _n_tickets;
        private String _ssn;
        private String _t_type;

        public String SaleID
        {
            get { return _saleID; }
            set {
                if (value == null | String.IsNullOrEmpty(value))
                {
                    throw new Exception("Sale ID field can’t be empty");
                    return;
                }
                _saleID = value; 
            }
        }

        public String EventID
        {
            get { return _eventID; }
            set { _eventID = value; }
        }

        public String N_tickets
        {
            get { return _n_tickets; }
            set {
                if (value == null | String.IsNullOrEmpty(value))
                {
                    throw new Exception("Number of Tickets field can’t be empty");
                    return;
                }
                _n_tickets = value; }
        }

        public String Ssn
        {
            get { return _ssn; }
            set {
                if (value == null | String.IsNullOrEmpty(value))
                {
                    throw new Exception("Ssn field can’t be empty");
                    return;
                }
                _ssn = value;
            }
        }

        public string T_type
        {
            get { return _t_type; }
            set {
                if (value == null | String.IsNullOrEmpty(value))
                {
                    throw new Exception("Type field can’t be empty");
                    return;
                }
                _t_type = value; }
        }

        public override string ToString()
        {
            string formattedString = $"{SaleID,-5} {Ssn,-15} {N_tickets,-15} {T_type}";
            return formattedString;
        }

        public string ToString2()
        {
            string formattedString = $"{Ssn,-15} {N_tickets,-15} {T_type}";
            return formattedString;
        }

        public Sales() : base()
        {
        }
       }
   }

