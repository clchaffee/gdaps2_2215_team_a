using System;
using System.Collections.Generic;

namespace Spawn_Testing
{
    class Program
    {
        /// <summary>
        /// To simplify this all, this uses the formula of e^(x*dampener)
        /// X will always be a number divisible by 5 (0, 5, 10, etc)
        /// The dampener is a decimal that is used to make sure the product is not too high (so there's not 10 trillion enemies at round 2
        /// There's two ways that the spawning can work:
        /// ADDING: Uses the formula and adds to the previous number (like going from 1 to (plus 1) to 2 to (plus 2) 4)
        /// REPLACING: Uses the formula to replace the previous number (like going from 1 to 2 to 4 to 8 using x^2)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int start = 0;
            int end = 30;
            int wave = 1;
            double limiter = .04;
            List<decimal> rValue = new List<decimal>();
            int valueTotal = 0;
            int interval = 0;

            Console.WriteLine("Intervals with ADDING the number of enemies:\n");

            //Console.WriteLine("Wave 1:");
            //SpawnRateAdd(start, end);
            //foreach(decimal d in rValue)
            //{
            //    Console.WriteLine($"Interval {rValue.IndexOf(d)+1}: {d}");
            //}
            //start = start + 5;
            //end = end + 5;
            //rValue.Clear();

            while (wave < 13)
            {
                for (int i = 0; i < 6; i++)
                {
                    Console.WriteLine($"Interval {i + 1}: {SpawnRateAdd(interval)}");

                    interval += 5;
                }
                Console.WriteLine();
                //Console.WriteLine($"Wave : {wave}");
                //SpawnRateAdd();
                //foreach (decimal d in rValue)
                //{
                //    Console.WriteLine($"Interval {rValue.IndexOf(d) + 1}: {d}");
                //}



                start = start + 5;
                end = end + 5;
                wave++;
                limiter -= .001;
                //rValue.Clear();
                valueTotal = 0;
                interval = start;
            }
            #region //replacing
            //Console.WriteLine("Wave 2:");
            //SpawnRateAdd(start, end);
            //foreach (decimal d in rValue)
            //{
            //    Console.WriteLine($"Interval {rValue.IndexOf(d)+1}: {d}");
            //}
            //start = 0;
            //end = 30;
            //rValue.Clear();

            //Console.WriteLine("\nIntervals with REPLACING the number of enemies:");

            //while (wave < max + 1)
            //{
            //    Console.WriteLine($"\nWave {wave}:");
            //    SpawnRateReplace(start, end);
            //    foreach (decimal d in rValue)
            //    {
            //        Console.WriteLine($"Interval {rValue.IndexOf(d) + 1}: {d}");
            //    }
            //    start = start + 5;
            //    end = end + 5;
            //    rValue.Clear();
            //    wave++;
            //}
            #endregion


            int SpawnRateAdd(int interval)
            {

                        //if (wave > 1)
                        //{
                            valueTotal += (int)Math.Ceiling((decimal)Math.Exp(((start + end) / 15) * 2*limiter));
                        //}
                        //if (wave < 6)
                        //{
                            valueTotal += (int)Math.Ceiling((decimal)Math.Exp(interval * limiter));
                        //if (i > 1)
                        //{
                        //    value -= Math.Ceiling((decimal)Math.Exp((i - 5) * .05));
                        //}
                        //}
                        //else
                        //{
                        //value += Math.Ceiling((decimal)Math.Exp(i * .06));
                        //if (i > 1)
                        //{
                        //value -= Math.Ceiling((decimal)Math.Exp((i - 5) * .005));
                        //}
                        //}


                        //rValue.Add(value);
                return valueTotal;
            }

            List<decimal> SpawnRateReplace(int s, int e)
            {

                for (int i = start; i < end; i++)
                {
                    if (i % 5 == 0)
                    {

                        decimal value = Math.Ceiling((decimal)Math.Exp(i * .1));

                        rValue.Add(value);
                    }
                }
                return rValue;
            }
        }
    }
}
