using System.Diagnostics;

// i've modified this at 1334
// See https://aka.ms/new-console-template for more information
//
// Goal: track the status of long-running asynchronous tasks and inform the user of the overall progress
//

Console.WriteLine("starting heavy lifting...");
Heavy.Lifting();

static public class Heavy
{
    public delegate void DelegatedWork();

    static public void Lifting()
    {
        string status;
        Queries queries = new Queries(); // instantiate class
        DelegatedWork? delegate1, delegate2, multicastdelegate; // create delegates

        // assign delegates to methods
        delegate1 = queries.query1;
        delegate2 = queries.query2;
        multicastdelegate = delegate1 + delegate2;

        // create 3 tasks 
        Task t1 = new Task(queries.query1);
        Task t2 = new Task(queries.query2);
        Task t3 = new Task(new Action(multicastdelegate));

        // start the two methods independent of the delegate
        t1.Start();
        t2.Start();

        while (!t1.IsCompleted || !t2.IsCompleted)
        {
            status = "query1 timer:" + queries.ElapsedTimer1 + "query2 timer:" + queries.ElapsedTimer2;
            Console.WriteLine(status + " task t1:" + t1.Status.ToString() + " task t2:" + t2.Status.ToString());
        }
        Console.WriteLine("done" + " task t1:" + t1.Status.ToString() + " task t2:" + t2.Status.ToString());
        Thread.Sleep(1000);

        // start the method for the delegate (will run syncrhonously as multicast delegates do)
        t3.Start();
        while (!t3.IsCompleted)
        {
            status = "query1 timer:" + queries.ElapsedTimer1.ToString() + " query2 timer:" + queries.ElapsedTimer2.ToString();
            Console.WriteLine(status + " task t3:" + t3.Status.ToString());
        }
        Console.WriteLine("done" + " task t3:" + t3.Status.ToString());
    }
}

public class Queries
{
    public string ElapsedTimer1
    {
        get
        {
            string elapsedString = "";
            TimeSpan ts = stopwatch1.Elapsed;

            // Format and display the TimeSpan value.
            elapsedString = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            return elapsedString;
        } 
        private set {  } }
    public string ElapsedTimer2
    {
        get
        {
            string elapsedString = "";
            TimeSpan ts = stopwatch2.Elapsed;

            // Format and display the TimeSpan value.
            elapsedString = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            return elapsedString;
        }
        private set {  }
    }
    Stopwatch stopwatch1 = new Stopwatch();
    Stopwatch stopwatch2 = new Stopwatch();

    public Queries()
    {
        ElapsedTimer1 = "";
        ElapsedTimer2 = "";
    }

    public void query1()
    {
        stopwatch1.Start();
        Thread.Sleep(5000);
        stopwatch1.Stop();
        stopwatch1.Reset();
    }

    public void query2()
    {
        stopwatch2.Start();
        Thread.Sleep(10000);
        stopwatch2.Stop();
        stopwatch2.Reset();
    }
}


