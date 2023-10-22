using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto
{
    [Serializable()]
    public class Ad
	{
		private String event_ID;//foreign key
		private String duration;
		private String ad_ID;
		private String cost;
		
		public String EventID
		{
            get { return event_ID; }
            set { event_ID = value; }
        }
		public String Duration
		{
            get { return duration; }
            set { duration = value; }
        }
		public String AdID
		{
            get { return ad_ID; }
            set { ad_ID = value; }
        }
		public String Cost
		{
			get { return cost; }
            set { cost = value; }
		}
		public override string ToString()
		{
            string formattedString = $"{AdID,-5} {Duration,-10} {Cost}";
            return formattedString;
        }
		public Ad() :base()
		{
		}
	}
}
