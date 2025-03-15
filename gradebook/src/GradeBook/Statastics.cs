namespace GradeBook
{
    public class Statastics
    {
        public double Average
        {
            get{
                return Sum/Count;
            }
        }   
        public double High;
        public double Low;
        public char Letter
        {
            get
            {
                switch(Average)
                {
                    case var d when d >=90:
                        return 'A';

                    case var d when d >=80:
                        return 'B';

                    case var d when d >=70:
                        return 'C';

                    case var d when d >=60:
                        return 'D';

                    default:
                        return 'F';;
                    
                }
            }
        }
        
        public double Sum;
        public int Count;

        public void Add(double number)
        {
            Sum += number;
            Count += 1;
            High = Math.Max(number, High);
            Low = Math.Min(number, Low);
        }

        public Statastics()
        {
            Count = 0;
            Sum = 0.0;
            High = double.MinValue;
            Low = double.MaxValue;
        }
        
    }
}