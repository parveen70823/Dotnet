using System.Reflection.Metadata;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args);
    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public interface IBook
    {
        void AddGrade(double grade);
        Statastics GetStatistics();
        string Name{get;}
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book: NamedObject, IBook//here we can not inherit the multiple classes but we can inherit the interface and we have to implement all the method present inside of the interface and we can achieve 100 percent abstraction.
    {                                                                             
        public Book(string name) : base(name)
        {
        }

        public abstract event GradeAddedDelegate GradeAdded;

        public abstract void AddGrade(double grade);

        public abstract Statastics GetStatistics();
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {
        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            // var Writer = File.AppendText($"{Name}.txt");
            // Writer.WriteLine(grade);
            // Writer.Dispose();

            //here writer obj is being used by using and it is disposed once it will
            //be free and it is like we using namespace(system.io) and it is overloaded 
            //with the Idisposable interface
            using(var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);
                if(GradeAdded !=null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
        }

        public override Statastics GetStatistics()
        {
            var result = new Statastics();
            using (var reader = File.OpenText($"{Name}.txt"))
            {
                var line = reader.ReadLine();
                while(line != null)
                {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }
            return result;
        }
    }

    public class InMemoryBook:Book
    {
        public InMemoryBook(string name) : base(name)
        //base is new keyword in c# which invoke the constructor of the base class constructor
        {
            // this.Name = name;
            grades = new List<double>();
            Name = name;
        }

        public void AddGrade(char letter)
        {
            switch(letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                default:
                    AddGrade(0);
                    break;
            }
        }
        public override void AddGrade(double grade)
        {
            if(grade<=100 && grade>=0)
            {
                grades.Add(grade);
                if(GradeAdded != null)
                {
                    GradeAdded(this,new EventArgs()); 
                }
            }
            else{
                // System.Console.WriteLine("Invalid grade");
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        public override event GradeAddedDelegate GradeAdded;

        private List<double> grades;

        // private string name; //backing field
        // public String Name
        // {
        //     get
        //     {
        //         return name;
        //     }
        //     set
        //     {
        //         if(!String.IsNullOrEmpty(value))
        //         {
        //             name = value;
        //         }
        //     }
        // }//can also be done in shorter format ms team made this
        
        // public string Name
        // {
        //     get;
        //     set;
        //     // private set;
        // }

        public const string CATEGORY = "science";//const keyword is like static field which can
                                                // not be accessible using obje ref but can be 
                                                //using direct class ref. this is use to when we want to don't change the value in lifetime of the object

        // readonly string category = "science";// a readonly field is only accessible or editable
                                             // within constructor and inside a variable initializer.
    

        public override Statastics GetStatistics()
        {
            // var x = 60.8;
            // var y =9.39;
            // var result =0.0;
            // var numbers = new double[]{1.2 ,3.2 , 4.43, 34.4};
            // List<double> grades = new List<double>(){0.2 ,3.2 , 4.43, 34.4};

            var res = new Statastics();
            
            foreach(double i in grades)
            {
                res.Add(i);
            }
            
            return res;
        }
    }
}