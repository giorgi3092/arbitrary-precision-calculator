using System;
using System.Collections.Generic;

/*   Author: Giorgi Aptsiauri    */

namespace inf_calculator_namespace
{
    public class InfInt : IComparable<InfInt>
    {
        // data members
        public List<int> Numbers = new List<int>();
        public bool Sign { get; set; }


        // used for adding numbers to the ints list
        public void Addnum(int digit)
        {
            Numbers.Add(digit);
        }

        //  this method actually handles adding two numbers
        private InfInt Addition(InfInt Other)
        {
            int OtherLastElement = Other.Numbers.Count - 1;
            int ThisLastElement = this.Numbers.Count - 1;
            int carry = 0;
            int DigitSum = 0;

            InfInt Sum = new InfInt();  // final sum goes here


            for (int i = 0; carry != 0 || i <= OtherLastElement || i <= ThisLastElement; i++)
            {
                if ((OtherLastElement - i) >= 0)
                {
                    DigitSum = Other.Numbers[OtherLastElement - i];
                }

                if ((ThisLastElement - i) >= 0)
                {
                    DigitSum += this.Numbers[ThisLastElement - i];
                }

                DigitSum += carry;  // apply carry
                carry = 0;
                if (DigitSum >= 10)
                {
                    carry = 1;
                }
                Sum.Numbers.Add(DigitSum % 10); // extract LSD and add to the Sum
                DigitSum = 0;

            }

            // reverse is necessary because items were added from LSD to MSD
            Sum.Numbers.Reverse();
            return Sum;
        }

        // this actually handles subtraction of two numbers
        private InfInt Subtraction(InfInt Other)
        {
            int Borrow = 0, Difference = 0, LSD = 0;
            InfInt Result = new InfInt();

            int ThisElementLastIndex = this.Numbers.Count - 1;
            int OtherElementLastIndex = Other.Numbers.Count - 1;

            for (int i = 0; i <= ThisElementLastIndex; i++)
            {
                Difference = this.Numbers[ThisElementLastIndex - i];  // assign the number of the first digit
                if ((OtherElementLastIndex - i) >= 0)
                {
                    Difference -= Other.Numbers[OtherElementLastIndex - i]; // if conditions apply, subtract a digit of the other number
                }
                Difference -= Borrow;   // apply borrow

                Borrow = 0;

                if (Difference < 0)  // keeping the carry when difference is negative
                {
                    Difference += 10;
                    LSD = Difference % 10;  // extract the digit to be added to Difference
                    Borrow = 1;
                }
                else
                {
                    LSD = Difference;   // digit is just difference is it was not negative
                    Borrow = 0; // borrow is zero in this case
                }


                Result.Numbers.Add(LSD);    // add digit to the numbers
            }

            // the following removes starting trailing zeros
            for (int i = Result.Numbers.Count - 1; i >= 0; i--)
            {
                if (Result.Numbers[i] != 0)
                {
                    break;
                }

                if (Result.Numbers[i] == 0)
                {
                    Result.Numbers.RemoveAt(i);
                }
            }

            // reverse is needed just as described above in addition
            Result.Numbers.Reverse();

            // results must be returned
            return Result;
        }

        private InfInt Multiply(InfInt Other)
        {
            int thisSize = this.Numbers.Count;
            int OtherSize = Other.Numbers.Count;

            int ThisLastIndex = thisSize - 1;
            int OtherLastIndex = OtherSize - 1;

            int MaxSize = 0;
            int MinSize = 0;
            int product = 0;
            int LS = 0; // Least Significant 
            int carry = 0;

            InfInt Result = new InfInt();

            if (thisSize > OtherSize)
            {
                MaxSize = thisSize;
                MinSize = OtherSize;
            }
            else if (thisSize < OtherSize)
            {
                MaxSize = OtherSize;
                MinSize = thisSize;
            }
            else
            {
                MaxSize = thisSize;
                MinSize = thisSize;
            }

            var ListOfLists = new List<InfInt>();
            var Temp = new InfInt();

            for (int i = 0; i <= OtherLastIndex; i++)
            {
                for (int j = 0; j <= ThisLastIndex; j++)
                {
                    product = (Other.Numbers[OtherLastIndex - i] * this.Numbers[ThisLastIndex - j]) + carry;    // find the product of two corresponding digits
                    LS = product % 10;  // extract the LSD to be added to sum
                    carry = product / 10;   // extract the carry
                    Temp.Addnum(LS);    // add to the list of temp numbers
                }

                // carry must be added at the end of multipication if it is not zero
                if (carry != 0)
                {
                    Temp.Addnum(carry);
                }

                carry = 0;

                Temp.Numbers.Reverse(); // reverse needed again

                // increase the magnitude of intermediary numbers to be added by powers of 10. 
                // this is done by adding extra zeros at the end.
                if (i >= 0)
                {
                    for (int k = 0; k < i; k++)
                    {
                        Temp.Numbers.Add(0);
                    }
                }

                ListOfLists.Add(Temp);
                Temp = new InfInt();    // new instance for new iteration
            }

            // add all the intermediary numbers together.
            for (int i = 0; i < ListOfLists.Count; i++)
            {
                Result = Result.Plus(ListOfLists[i]);
            }

            return Result;
        }

        // this method just decides the signs and either Addition or Subtraction depending on the signs and magnitudes of the input numbers
        public InfInt Plus(InfInt Other)
        {
            InfInt Result = new InfInt();

            //  + + +
            if (this.Sign == false && Other.Sign == false)
            {
                Result = this.Addition(Other);
                Result.Sign = false;
            }

            //  - + -
            if (this.Sign == true && Other.Sign == true)
            {
                Result = this.Addition(Other);
                Result.Sign = true;
            }

            //  + + -
            if (this.Sign == false && Other.Sign == true)
            {
                int compareResult = this.CompareTo(Other);

                if (compareResult < 0)
                {
                    Result = Other.Subtraction(this);
                    Result.Sign = true;
                }

                if (compareResult > 0)
                {
                    Result = this.Subtraction(Other);
                    Result.Sign = false;
                }

                if (compareResult == 0)
                {
                    Result.Addnum(0);
                    Result.Sign = false;
                }
            }

            //  - + +
            if (this.Sign == true && Other.Sign == false)
            {
                int compareResult = this.CompareTo(Other);

                if (compareResult < 0)
                {
                    Result = Other.Subtraction(this);
                    Result.Sign = false;
                }

                if (compareResult > 0)
                {
                    Result = this.Subtraction(Other);
                    Result.Sign = true;
                }

                if (compareResult == 0)
                {
                    Result.Addnum(0);
                    Result.Sign = false;
                }
            }

            return Result;
        }

        // this method just decides the signs and either Addition or Subtraction depending on the signs and magnitudes of the input numbers
        public InfInt Minus(InfInt Other)
        {
            InfInt Result = new InfInt();

            //  +A - -B is the same as +A + +B
            if (this.Sign == false && Other.Sign == true)
            {
                Result = this.Addition(Other);
                Result.Sign = false;
            }

            // -A - +B is the same as -(A + B)
            if (this.Sign == true && Other.Sign == false)
            {
                Result = this.Addition(Other);
                Result.Sign = true;
            }

            //  + - +
            if (this.Sign == false && Other.Sign == false)
            {
                int compareResult = this.CompareTo(Other);

                if (compareResult < 0)
                {
                    Result = Other.Subtraction(this);
                    Result.Sign = true;
                }

                if (compareResult > 0)
                {
                    Result = this.Subtraction(Other);
                    Result.Sign = false;
                }

                if (compareResult == 0)
                {
                    Result.Addnum(0);
                    Result.Sign = false;
                }
            }


            //  - - -
            if (this.Sign == true && Other.Sign == true)
            {
                int compareResult = this.CompareTo(Other);

                if (compareResult < 0)
                {
                    Result = Other.Subtraction(this);
                    Result.Sign = false;
                }

                if (compareResult > 0)
                {
                    Result = this.Subtraction(Other);
                    Result.Sign = true;
                }

                if (compareResult == 0)
                {
                    Result.Addnum(0);
                    Result.Sign = false;
                }
            }

            return Result;
        }

        // this method just decides the signs depending on the signs of the input numbers
        public InfInt Times(InfInt Other)
        {
            int ThisCount = this.Numbers.Count;
            int ThatCount = Other.Numbers.Count;

            InfInt Result = new InfInt();

            if (ThisCount > ThatCount)
            {
                Result = this.Multiply(Other);
            }
            if (ThisCount < ThatCount)
            {
                Result = Other.Multiply(this);
            }
            else
            {
                Result = this.Multiply(Other);
            }

            if ((this.Sign == false && Other.Sign == true) || (this.Sign == true && Other.Sign == false))
            {
                Result.Sign = true;
            }
            else
            {
                Result.Sign = false;
            }
            return Result;
        }

        // display the list of ints that current instance of InfInt contains
        public void display()
        {
            Console.Write($"{(Sign ? "-" : "")}");
            for (int i = 0; i < Numbers.Count; i++)
            {
                Console.Write(Numbers[i]);
            }
        }

        //  implementation of Compareto method coming from IComparable interface
        public int CompareTo(InfInt other)
        {
            if (this.Numbers.Count > other.Numbers.Count)
            {
                return 1;
            }
            else if (this.Numbers.Count < other.Numbers.Count)
            {
                return -1;
            }
            else
            {
                int value = 0;
                for (int I = 0; I < Numbers.Count; I++)
                {
                    if (this.Numbers[I] > other.Numbers[I])
                    {
                        value = 1;
                        return value;
                    }
                    else if (this.Numbers[I] < other.Numbers[I])
                    {
                        value = -1;
                        return value;
                    }
                }
                return value;
            }
        }
    }
}

