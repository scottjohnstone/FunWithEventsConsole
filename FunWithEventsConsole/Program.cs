using System.Diagnostics;


// See https://aka.ms/new-console-template for more information

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

        Console.WriteLine("starting delegated processes...");
        Task t = new Task(delegate { multicastdelegate(); });
        t.Start();
        Console.WriteLine(Task.WhenAll(t).Status);


        //        while (multicastdelegate.GetInvocationList().Count() > 0) // just stuff some property into the class and mark it when it starts, in progress, and done - essentially come up with your own task tracking process until you figure out how to do it right
        while (!queries.QueriesComplete()) // just stuff some property into the class and mark it when it starts, in progress, and done - essentially come up with your own task tracking process until you figure out how to do it right
        {
            foreach (var item in multicastdelegate.GetInvocationList())
            {
                // need to figure out how to access properties within a delegate
                // in the meantime, workaround by just accessing the two properties on the class fileHandler
                status = "query1 timer:" + queries.ElapsedTime1() + "query2 timer:" + queries.ElapsedTime2();
                Console.WriteLine(status + " invo length:" + multicastdelegate.GetInvocationList().Length.ToString() + "done?" + Task.WhenAll(t).IsCompletedSuccessfully.ToString() + "stat:" + Task.WhenAll(t).Status.ToString());
            }
        }
        Console.WriteLine("done");
    }
}

public class Queries
{
    public string ElapsedTimer1 { get; private set; }
    public string ElapsedTimer2 { get; private set; }
    public bool AllQueriesComplete { get; private set; }
    public bool Query1Complete { get; private set; }
    public bool Query2Complete { get; private set; }
    Stopwatch stopwatch1 = new Stopwatch();
    Stopwatch stopwatch2 = new Stopwatch();

    public Queries()
    {
        AllQueriesComplete = false;
        Query1Complete = false;
        Query2Complete = false;
        ElapsedTimer1 = "";
        ElapsedTimer2 = "";
    }

    public bool QueriesComplete()
    {
        if (Query1Complete == true && Query2Complete == true)
            return true;
        else
            return false;
    }
    async public void query1()
    {
        stopwatch1.Start();
        await Task.Delay(TimeSpan.FromSeconds(5));
        stopwatch1.Stop();
        stopwatch1.Reset();
        Query1Complete = true;
    }

    public string ElapsedTime1()
    {
        TimeSpan ts = stopwatch1.Elapsed;

        // Format and display the TimeSpan value.
        ElapsedTimer1 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        return ElapsedTimer1;
    }

    public string ElapsedTime2()
    {
        TimeSpan ts = stopwatch2.Elapsed;

        // Format and display the TimeSpan value.
        ElapsedTimer2 = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
        return ElapsedTimer2;
    }
    async public void query2()
    {
        stopwatch2.Start();
        await Task.Delay(TimeSpan.FromSeconds(10));
        stopwatch2.Stop();
        stopwatch2.Reset();
        Query2Complete = true;
    }
}


