using System;

namespace WpfApp1
{
    //Employee is a gene; the class contains the info about the gene
    class Employee
    {
        public string name;
        public int startShift, endShift, workHour;
        public int id;
        public Employee(string empName, int idValue, int frstShift, int lstShift)
        {
            name = empName;
            id = idValue;
            startShift = frstShift;
            endShift = lstShift;
            workHour = endShift - startShift;
        }
        public void addShift(int frstShift, int lstShift)
        {
            startShift = frstShift;
            endShift = lstShift;
            workHour = endShift - startShift;
        }
        public void showDetails()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Name: " + name );
            Console.WriteLine("Employee id: " + id );
            Console.WriteLine("Start time: " + startShift);
            Console.WriteLine("End time: " + endShift);
            Console.WriteLine("Total work hour: " + (endShift - startShift));
        }

        public int getStartShift()
        {
            return startShift;
        }

        public int getEndShift()
        {
            return endShift;
        }
    }
}
