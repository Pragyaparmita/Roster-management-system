using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Program
    {
        public static int[] generateSchedule()
        {
            int[] schedule=new int[Globals.WORKING_HOUR];
          //  string inp; //used to store console input
         //   string nm;  //name of employee
            int generation = 0;   //keep count of generation
            List<Schedule> population = new List<Schedule>();    //main population that evolves with time
          //  int idVal, fst, lst;  //variables to hold employee details
            bool found = false;
            //bool quit = false;   //to keep the loop running

            /*
			//for checking
            Employee neema = new Employee("neema", 1, 10, 18);
            Employee krimesh = new Employee("krimesh",2,12, 15);
            Employee Pragya = new Employee("pragya", 3, 14, 18);
            Employee prajwal = new Employee("prajwal", 4, 16, 18);
            Employee alina = new Employee("alina", 5, 10, 14);

            Globals.domain.Add(neema);
            Globals.domain.Add(krimesh);
            Globals.domain.Add(Pragya);
            Globals.domain.Add(prajwal);
            Globals.domain.Add(alina);
			*/

            
            /*while (!quit)
            {
                //The following commented code was used for console i/o
                Console.WriteLine("");
                Console.WriteLine("Enter command:");
                inp = Console.ReadLine();
                if (inp == "add")
                {
                    Console.Write("About to add employee \n");
                    Console.Write("Enter name, id, start hour and end hour: \t");
                    nm = Console.ReadLine();
                    idVal = Convert.ToInt32(Console.ReadLine());
                    fst = Convert.ToInt32(Console.ReadLine());
                    lst = Convert.ToInt32(Console.ReadLine());
                    Employee e = new Employee(nm, idVal, fst, lst);
                    Globals.domain.Add(e);
                }
                else if (inp == "show")
                {
                    for (int i = 0; i < Globals.domain.Count(); ++i)
                    {
                        Globals.domain[i].showDetails();
                    }
                }
                else if (inp == "quit")
                    quit = true;
            }
            */


            for (int i = 0; i < Globals.POPULATION_SIZE; ++i)
            {
                List<Employee> gnome = createGnome(Globals.WORKING_HOUR);    //creates a random schedule of length WORKING_HOUR
                population.Add(new Schedule(gnome));      //add the random schedule to population
            }

            while (!found)
            {
                population.Sort(new sortSchedule()); //sort according to fitness, lower fitness value -> best answer
                if (population[0].fitness <= 0 || generation>1000)
                {      //if fitness=0, we've found the answer
                       /* Here "<=" is used instead of "=="
                       to make sure that our loop terminates once answer is found*/
                    found = true;
                    break;
                }
                List<Schedule> new_generation = new List<Schedule>();    //new generation of population

                //Send the fittest 10% population to the next generation directly
                int s = (10 * Globals.POPULATION_SIZE) / 100;
                for (int i = 0; i < s; ++i)
                    new_generation.Add(population[i]);

                //mate the 2 random parents for rest 90% of the new_generation
                s = (90 * Globals.POPULATION_SIZE) / 100;

                for (int i = 0; i < s; ++i)
                {
                    Random rnd = new Random();
                    int r = rnd.Next(0, 100);           //select random 1st parent
                    Schedule parent2 = population[r];
                    r = rnd.Next(0, 100);              //select random 2nd parent
                    Schedule parent1 = population[r];
                    Schedule offspring = parent1.mate(parent2);
                    new_generation.Add(offspring);
                }

                
                population = new_generation;      //new_generation becomes the current population
                //Console.Write("Generation: " + generation + "\t");

                //Console.Write("Schedule: ");
                //population[0].displaySchedule();
                //Console.Write("\t");

                //Console.WriteLine("Fitness: " + (population[0]).fitness);
                ++generation;           //next generation
                
            }

                /*
                Console.WriteLine("\nSOLUTION:");

                Console.Write("Generation: " + generation + "\t");

                Console.Write("Schedule: ");
                population[0].displaySchedule();        
                Console.Write("\t");

                Console.WriteLine("Fitness: " + (population[0]).fitness + "\n");
                Console.ReadKey();
                */
                schedule=population[0].returnSchedule();
                return schedule;
            
        }

        //creates a random schedule (individual)
        public static List<Employee> createGnome(int len)
        {
            List<Employee> gnome = new List<Employee>();
            for (int i = 0; i < len; ++i)
            {
                Employee result = Globals.mutated_genes();
                gnome.Add(result);
            }
            return gnome;
        }

        //create a method for comparision for sorting
        public class sortSchedule : IComparer<Schedule>
        {
            public int Compare(Schedule x, Schedule y)
            {
                if (x.fitness > y.fitness)
                    return 1;
                else if (x.fitness < y.fitness)
                    return -1;
                else
                    return 0;
            }
        }

    }
}
