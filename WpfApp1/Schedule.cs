using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Schedule
    {
        public List<Employee> chromosome;
        public int fitness;
        public Schedule(List<Employee> chromosome_parameter)
        {
            chromosome = chromosome_parameter;
            fitness = calcFitness();
        }
        public Schedule mate(Schedule parent2)
        {
            //crossover and mutation goes here
            List<Employee> child_chromosome = new List<Employee>();
            //make the size of child same as the parent
            int len = chromosome.Count();
            for (int i = 0; i < len; ++i)
            {
                Random rnd = new Random(Guid.NewGuid().GetHashCode());
                int p = rnd.Next(0, 101);
                if (p < 45)
                    child_chromosome.Add(chromosome[i]);
                else if (p < 90)
                    child_chromosome.Add(parent2.chromosome[i]);
                else
                    child_chromosome.Add(Globals.mutated_genes());
            }
            Schedule child_schedule = new Schedule(child_chromosome);
            return (child_schedule);
        }
        public int calcFitness()
        {

            //fitness calculation function goes here
            var mem = new Dictionary<int, int>();
            int i = 0;
            for (int j = 0; j < chromosome.Count(); ++j)
                mem[chromosome[j].id] = 0;

            //initialize fitness value
            int fit = 0;
            int offset = 0;

            //code for paired integer
            if (Globals.atleastTwo)
            {
                offset = 0;         //don't ignore fitness calculation
            }
            else
            {
                offset = -1;        //ignore fitness calculation
            }
            while (i < chromosome.Count())
            {
                if (i == 0)
                {
                    ++mem[chromosome[i].id];
                    if (chromosome[i + 1].id != chromosome[i].id)
                        fit += 1 + offset;
                    ++i;
                }
                else
                {
                    if (chromosome[i].id == chromosome[i - 1].id)
                    {
                        ++i;
                    }
                    else
                    {
                        if (i < chromosome.Count() - 1 && chromosome[i].id != chromosome[i + 1].id)
                            fit += 1 + offset;
                        else if (i == chromosome.Count() - 1)
                            fit += 1 + offset; ;
                        mem[chromosome[i].id]++;
                        ++i;
                    }
                }
            }


            //code for repetitive integer
            if (Globals.nonRepetitive)
            {
                for (int j = 0; j < chromosome.Count(); ++j)
                {
                    if (mem[chromosome[j].id] != 0)
                    {
                        fit += (mem[chromosome[j].id] - 1);
                        mem[chromosome[j].id] = 0;
                    }
                }
            }

            //code for employee time consideration
            if (Globals.empTimeConsideration)
            {
                for (int j = 0; j < chromosome.Count(); ++j)
                {
                    if (j + Globals.STARTING_HOUR < chromosome[j].getStartShift() || j + Globals.STARTING_HOUR > chromosome[j].getEndShift())
                    {
                        ++fit;
                    }
                }
            }

            //code for max working hour per day
            if (Globals.maxWorkPerDay)
            {
                for (int j = 0; j < chromosome.Count(); ++j)
                    mem[chromosome[j].id] = 0;
                for (int j = 0; j < chromosome.Count(); ++j)
                {
                    if (++mem[chromosome[j].id] > Globals.maxWorkHourPerDay)
                        ++fit;
                }
            }

            return fit;
        }

        public int[] returnSchedule()
        {
            int[] schedule = new int[Globals.WORKING_HOUR];
            for (int i = 0; i < chromosome.Count(); ++i)
                schedule[i] = chromosome[i].id;
            return schedule;
        }
    }
}
