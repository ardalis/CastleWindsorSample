using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace CastleWindsorConsole
{
    public interface IGreeting
    {
        string Greet(string name);
    }

    public class HelloGreeting : IGreeting
    {
        public string Greet(string name)
        {
            return String.Format("Hello, {0}", name);
        }
    }

    public interface IWriter
    {
        void WriteLine(string message);
    }

    public class ConsoleWriter : IWriter
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Greeter
    {
        private readonly IGreeting _greeting;
        private readonly IWriter _writer;

        public Greeter(IGreeting greeting, IWriter writer)
        {
            Name = "Unknown Greeter";
            _greeting = greeting;
            _writer = writer;
        }

        public string Name { get; set; }

        public void Execute(string personToGreet)
        {
            _writer.WriteLine("Executing for Greeter " + Name);
            _writer.WriteLine(_greeting.Greet(personToGreet));
        }
    }

    class Program
    {
        // To get started, run install-package Castle.Windsor in Package Manager Console
        static void Main(string[] args)
        {
            var container = new WindsorContainer();

            // register interfaces and their implementations
            //container.Register(Component.For<IGreeting>()
            //    .ImplementedBy<HelloGreeting>());
            //container.Register(Component.For<IWriter>()
            //    .ImplementedBy<ConsoleWriter>());

            // register one type at a time
            // container.Register(Component.For<Greeter>());

            // or register all types in an assembly matching some criteria
            container.Register(Classes.FromThisAssembly()
                //.Where(t => t.Name.EndsWith("Greeter")));
                .InNamespace("CastleWindsorConsole")
                .WithServiceDefaultInterfaces()); // similar to StructureMap's WithDefaultConventions

            // show contents of container
            foreach (var handler in container.Kernel
                .GetAssignableHandlers(typeof(object)))
            {
                Console.WriteLine("{0} {1}",
                   handler.ComponentModel.Services,
                   handler.ComponentModel.Implementation);
            }

            var greeter = container.Resolve<Greeter>();
            greeter.Name = "Bob the Greeter";
            greeter.Execute("Steve");

            Console.ReadLine();
        }
    }
}
