// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/accessing-attributes-by-using-reflection

using System;
using System.Linq;
using System.Reflection;

namespace Reflections
{
    [AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
    public class AuthorAttribute : Attribute
    {
        private string name;
        public string version;

        public AuthorAttribute(string name)
        {
            this.name = name;
            version = "1.0";
        }

        public string GetName()
        {
            return name;
        }
    }

    [Author("P. Ackerman", version = "1.1")]
    [Author("R. Koch", version = "1.2")]
    class SampleClassWithMultipleAttributes
    {
        // P. Ackerman's code goes here...  
        // R. Koch's code goes here...  

        public bool BringMeMoreAttributes() { return false; }
    }

    [Author("P. Ackerman", version = "1.3")]
    class SampleClassWithSingleAttribute
    {
        // P. Ackerman's code goes here...  

        public bool BringMeOneAttribute() { return false; }
    }

    [Obsolete("That kind of class - without attributes - should be redesigned.", false)]
    public class SampleClassWithoutAttributes
    {
        // ...  
    }

    class Program
    {
        private static void PrintAuthorInfo(System.Type t)
        {
            System.Console.WriteLine("Author information for {0}", t);

            // Using reflection.  
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);  // Reflection.  

            // Displaying output.  
            foreach (System.Attribute attr in attrs)
            {
                if (attr is AuthorAttribute)
                {
                    AuthorAttribute a = (AuthorAttribute)attr;
                    System.Console.WriteLine("   {0}, version {1}", a.GetName(), a.version);
                }
            }
        }

        private static void ReviewSomeAttributes(System.Type t)
        {
            string[] trivial = { "ToString", "Equals", "GetHashCode", "GetType" };

            // Iterate through all the methods of the class.
            foreach (MethodInfo m in t.GetMethods())
            {
                if (!(m.IsGenericMethod || trivial.Contains(m.Name)))
                    Console.WriteLine("Type {0} has a non-generic, non-standard method {1}.", t.Name, m.Name);
            }

            // Iterate through all the interfaces of the class.
            foreach (var i in t.GetInterfaces())
            {
                Console.WriteLine("Type {0} implements the interface {1}.", t.Name, i.Name);
            }
        }

        public static void Main(string[] args)
        {
            PrintAuthorInfo(typeof(SampleClassWithMultipleAttributes));
            PrintAuthorInfo(typeof(SampleClassWithSingleAttribute));
            PrintAuthorInfo(typeof(SampleClassWithoutAttributes));

            ReviewSomeAttributes(typeof(SampleClassWithMultipleAttributes));
            ReviewSomeAttributes(typeof(SampleClassWithSingleAttribute));
            ReviewSomeAttributes(typeof(SampleClassWithoutAttributes));
        }
    }
}
