using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1
{
    class Globals
    {
        public const int POPULATION_SIZE = 100;
        public static int STARTING_HOUR = 10;
        public static int ENDING_HOUR = 18;
        public static int WORKING_HOUR = ENDING_HOUR - STARTING_HOUR;
        public static int maxWorkHourPerDay = 4;
        public static int maxWorkHourPerWeek = 24;

        public static bool nonRepetitive = true;
        public static bool atleastTwo = true;
        public static bool empTimeConsideration = true;
        public static bool maxWorkPerDay = true;
        public static bool maxWorkPerWeek = true;

        public static List<Employee> domain = new List<Employee>();

        public static Employee mutated_genes()
        {
            int len = domain.Count();
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int i = rnd.Next(0, len);
            return domain[i];
        }
    }
}
