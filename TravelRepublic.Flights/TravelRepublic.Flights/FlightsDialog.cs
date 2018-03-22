using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TravelRepublic.FlightCodingTest
{
    public partial class FlightsDialog : Form
    {
        public FlightsDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FlightBuilder fb = new FlightBuilder();
            var flights = fb.GetFlights();

            //Depart before the current date/time.
            var departBefore = flights
                    .SelectMany(s => s.Segments
                    .Where(i => DateTime.Compare(i.DepartureDate, DateTime.Now) < 0));
            
            foreach (var f in departBefore)
            {
                Console.WriteLine("Arrival Date: " + f.ArrivalDate.ToString() + " departure date: " + f.DepartureDate.ToString() );
            }

            FlightGridView.DataSource = departBefore.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FlightBuilder fb = new FlightBuilder();
            var flights = fb.GetFlights();

            //Have a segment with an arrival date before the departure date.
            var arrivalBefore = flights
                    .SelectMany(s => s.Segments
                    .Where(i => DateTime.Compare(i.ArrivalDate, i.DepartureDate) < 0));

            foreach (var f in arrivalBefore)
            {
                Console.WriteLine("Arrival Date: " + f.ArrivalDate.ToString() + " departure date: " + f.DepartureDate.ToString());
            }
            FlightGridView.DataSource = arrivalBefore.ToList();
        }

        private bool FilterFlights(Flight arg)
        {
            bool result = false;
            for (int i = 0; i < arg.Segments.Count() - 1; i++)
            {
                result = ((arg.Segments[i + 1].DepartureDate - arg.Segments[i].ArrivalDate).TotalHours >= 2);
            }
            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FlightBuilder fb = new FlightBuilder();
            var flights = fb.GetFlights();

            //3.Spend more than 2 hours on the ground. i.e those with a total gap of over two hours between the 
            //arrival date of one segment and the departure date of the next.
            var gapGround = flights
                   .Where(s => (s.Segments.Count >= 2))
                   .Where(FilterFlights);

            foreach (var f in gapGround)
            {
                //Console.WriteLine("Arrival Date: " + f.Segments.ToString() + " departure date: " + f.Segments.ToString());
            }
            FlightGridView.DataSource = gapGround.SelectMany(s => s.Segments).ToList();
        }
    }
}
