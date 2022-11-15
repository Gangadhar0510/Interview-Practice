using System;

namespace Singleton_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            ApplicationState state = ApplicationState.GetState();
            state.LoginId = "Gangadhar";
            state.RoleId = "eedhala";

            ApplicationState st2 = ApplicationState.GetState();
            var lId = st2.LoginId;
            var rId = st2.RoleId;
            var b = (state == st2).ToString();

            Console.WriteLine(lId + "\n" + rId + "\n"+ b);
        }

        public class ApplicationState
        {
            private static ApplicationState Instance = null;            
            public string LoginId
            {
                get;
                set;
            }
            public string RoleId
            {
                get;
                set;
            }

            public ApplicationState()
            {

            }

            private static object lockThis = new object();

            public static ApplicationState GetState()
            {
                lock (lockThis)
                {
                    if(ApplicationState.Instance == null)
                    {
                        Instance = new ApplicationState();
                    }
                }
                return Instance;
            }

        }
    }
}
