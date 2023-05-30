namespace AspNet7.Infrastructures
{
    public interface ILog
    {
        void Execute(string message);
    }
    public class ConsoleLog : ILog
    {
        public void Execute(string message) 
        {
            Console.WriteLine(message);
        }
    }

}
