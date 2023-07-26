using MSO687_CONSOLE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSO687_CONSOLE
{
    class Program
    {
        DB_Plant_PPE_ConsoleDataContext db = new DB_Plant_PPE_ConsoleDataContext();
        //static void Main(string[] args)
        //{
        //}
        static void Main(string[] args)
        {
            // Put your desired logic here, which will run at 6 AM
            Console.WriteLine("The program is now running at 6 AM!");
            DateTime currentDate = DateTime.Today;
            using (DB_Plant_PPE_ConsoleDataContext db = new DB_Plant_PPE_ConsoleDataContext())
            {
                // Fetch data from TBL_T_PPE with DATE_RECEIVED_SM equal to today's date
                var dataToProcess = db.TBL_T_PPEs.Where(a => a.DATE_RECEIVED_SM == currentDate).ToList();

                // Process the fetched data (you can replace this with your actual processing logic)
                foreach (var item in dataToProcess)
                {
                    Console.WriteLine($"Processing data: ID={item.ID}, Date Received={item.DATE_RECEIVED_SM}");
                    // Add your processing logic here
                }
            }

            // Get the current time
            DateTime currentTime = DateTime.Now;

            // Calculate the time until 6 AM the next day
            DateTime nextRunTime = currentTime.Date.AddDays(1).AddHours(6);

            // Calculate the delay until 6 AM
            int delayMilliseconds = (int)(nextRunTime - currentTime).TotalMilliseconds;

            // Create a new timer that repeats every 24 hours and calls the ExecuteLogic method
            Timer timer = new Timer(ExecuteLogic, null, delayMilliseconds, 24 * 60 * 60 * 1000);
            //// Get the current time
            //DateTime currentTime = DateTime.Now;

            //// Calculate the time until 6 AM the next day
            //DateTime nextRunTime = currentTime.Date.AddDays(1).AddHours(6);

            //// Calculate the delay until 6 AM
            //int delayMilliseconds = (int)(nextRunTime - currentTime).TotalMilliseconds;

            //// If the current time is after 6 AM, schedule the run for the next day
            //if (currentTime.Hour >= 6)
            //{
            //    delayMilliseconds += 24 * 60 * 60 * 1000; // Add one day in milliseconds
            //}

            //// Create a Timer object that calls the ExecuteLogic method when the time is due
            //Timer timer = new Timer(ExecuteLogic, null, delayMilliseconds, Timeout.Infinite);

            //// Keep the application running
            //Console.ReadLine();
        }

        static void ExecuteLogic(object state)
        {
            // Put your desired logic here, which will run at 6 AM
            Console.WriteLine("The program is now running at 6 AM!");
            DateTime currentDate = DateTime.Today;
            using (DB_Plant_PPE_ConsoleDataContext db = new DB_Plant_PPE_ConsoleDataContext())
            {
                // Fetch data from TBL_T_PPE with DATE_RECEIVED_SM equal to today's date
                var dataToProcess = db.TBL_T_PPEs.Where(a => a.DATE_RECEIVED_SM == currentDate).ToList();

                // Process the fetched data (you can replace this with your actual processing logic)
                foreach (var item in dataToProcess)
                {
                    Console.WriteLine($"Processing data: ID={item.ID}, Date Received={item.DATE_RECEIVED_SM}");
                    // Add your processing logic here
                }
            }

            // Get the current time
            DateTime currentTime = DateTime.Now;

            // Calculate the time until 6 AM the next day
            DateTime nextRunTime = currentTime.Date.AddDays(1).AddHours(6);

            // Calculate the delay until 6 AM
            int delayMilliseconds = (int)(nextRunTime - currentTime).TotalMilliseconds;

            // Create a new timer that repeats every 24 hours and calls the ExecuteLogic method
            Timer timer = new Timer(ExecuteLogic, null, delayMilliseconds, 24 * 60 * 60 * 1000);
        }

    }
}
