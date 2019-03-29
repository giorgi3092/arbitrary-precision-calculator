using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/*   Author: Giorgi Aptsiauri */

namespace inf_calculator_namespace
{
    class Program
    {
        static void Main(string[] args)
        {
            var A = new List<InfInt>();
            var B = new List<InfInt>();
            InfInt Result = new InfInt();  // keeps the result
            InfInt TempInf1 = new InfInt();
            InfInt TempInf2 = new InfInt();

            int digit = 0;
            int ifNegative = 0;

            int TempOp = 0;

            string currDir = "infint.txt";

            string[] AllLines = File.ReadAllLines(currDir, Encoding.UTF8);  // read the text file

            for (int i = 0; i < AllLines.Length; i += 3)
            {
                //  clear the numbers for after-use
                TempInf1.Numbers.Clear();
                TempInf2.Numbers.Clear();

                // index offset changes when negative number is encountered
                ifNegative = 0;

                // check for negative number for the first operand
                if (AllLines[i][0] == '-')
                {
                    TempInf1.Sign = true;
                    ifNegative = 1;
                }
                else if (AllLines[i][0] != '-')
                {
                    TempInf1.Sign = false;
                }

                //  convert the first operand to the digits and store them in InfInt A
                for (int j = 0; j < AllLines[i].Length - ifNegative; j++)
                {
                    digit = AllLines[i][j + ifNegative] - '0';
                    TempInf1.Numbers.Add(digit);
                }


                // Repeat the same for the second operand
                ifNegative = 0;
                TempInf2.Numbers.Clear();

                if (AllLines[i + 1][0] == '-')
                {
                    TempInf2.Sign = true;
                    ifNegative = 1;
                }
                else if (AllLines[i + 1][0] != '-')
                {
                    TempInf2.Sign = false;
                }

                for (int j = 0; j < AllLines[i + 1].Length - ifNegative; j++)
                {
                    digit = AllLines[i + 1][j + ifNegative] - '0';
                    TempInf2.Numbers.Add(digit);
                }

                // check which operation is to be done
                if (AllLines[i + 2][0] == '+')
                {
                    TempOp = 0;
                }
                else if (AllLines[i + 2][0] == '-')
                {
                    TempOp = 1;
                }
                else if (AllLines[i + 2][0] == '*')
                {
                    TempOp = 2;
                }

                /* Handle the operation and output it */

                TempInf1.display();
                Console.Write(' ');

                if (TempOp == 0)
                {
                    Console.Write("+ ");
                    Result = TempInf1.Plus(TempInf2);
                }
                else if (TempOp == 1)
                {
                    Console.Write("- ");
                    Result = TempInf1.Minus(TempInf2);
                }
                else if (TempOp == 2)
                {
                    Console.Write("* ");
                    Result = TempInf1.Times(TempInf2);
                }
                TempInf2.display();

                Console.Write(" = ");
                Result.display();

                Console.WriteLine();

            }
        }
    }
}