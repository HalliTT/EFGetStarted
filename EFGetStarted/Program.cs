using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using var db = new BloggingContext();

//seedTeam(db);
//seedWorker(db);
//connectWorkerAndTeam(db);

//seedTasks(db);
//seedTodo(db);
//displayTasksAndTodos(db);
//printIncompleteTasksAndTodos(db);

//connectTaskAndTeam(db);
//connectTodoAndWorker(db);
PrintTeamsWithoutTasks(db);
PrintTeamsCurrentTask(db);


static void seedTeam(BloggingContext db)
{
    Console.WriteLine("Seed teams...");
    createTeam("Frontend", db);
    createTeam("Backend", db);
    createTeam("Testere", db);
}
static void createTeam(string name, BloggingContext db)
{
    var team = new Team
    {
        Name = name,
    };
    db.Team.Add(team);
    Console.WriteLine("Create team " + team.Name);
    db.SaveChanges();
}

static void seedWorker(BloggingContext db)
{
    Console.WriteLine("Seed workers...");
    var frontendTasks = db.Team.FirstOrDefault(t => t.Name == "Frontend");
    var backendTasks = db.Team.FirstOrDefault(t => t.Name == "Backend");
    var testereTasks = db.Team.FirstOrDefault(t => t.Name == "Testere");

    if(frontendTasks == null)
    {
        return;
    }

    createWorker("Steen Secher", db);
    createWorker("Ejvind Møller", db);
    createWorker("Konrad Sommer", db);
    createWorker("Sofus Lotus", db);
    createWorker("Remo Lademann", db);
    createWorker("Ella Fanth", db);
    createWorker("Anne Dam", db);
}

static void createWorker(string name, BloggingContext db)
{
    var worker = new Worker
    {
        Name = name,
    };

    db.Worker.Add(worker);
    Console.WriteLine("Create worker " + worker.Name);
    db.SaveChanges();
}

static void seedTasks(BloggingContext db)
{
    Console.WriteLine("Seed task...");

    createTask("Produce software", db);
    createTask("Brew coffee", db);
}

static void createTask(string name, BloggingContext db)
{
    var task = new Task
    {
        Name = name,
    };
    db.Task.Add(task);
    Console.WriteLine("Create task " + task.Name);
    db.SaveChanges();
}

static void seedTodo(BloggingContext db)
{
    Console.WriteLine("Seed todos...");
    var softwareTasks = db.Task.FirstOrDefault(t => t.Name == "Produce software");
    var brewTask = db.Task.FirstOrDefault(t => t.Name == "Brew coffee");

    if (softwareTasks == null || brewTask == null) {
        return;
    }

    createTodo("Write code", true, softwareTasks.TaskId, db);
    createTodo("Compile source", false, softwareTasks.TaskId, db);
    createTodo("Test program", false, softwareTasks.TaskId, db);

    createTodo("Pour water", false, brewTask.TaskId, db);
    createTodo("Pour coffee", true, brewTask.TaskId, db);
    createTodo("Turn on", false, brewTask.TaskId, db);
}

static void createTodo(string name, bool completed, int taskId, BloggingContext db)
{
    var todo = new Todo
    {
        Name = name,
        IsComplete = completed,
        TaskId = taskId,
    };

    db.Todo.Add(todo);
    Console.WriteLine("Create todo " + todo.Name);

    db.SaveChanges();
}

static void connectWorkerAndTeam(BloggingContext db)
{
    Console.WriteLine("Connect worket to team...");

    var frontend = db.Team.FirstOrDefault(t => t.Name == "Frontend");
    var backend = db.Team.FirstOrDefault(t => t.Name == "Backend");
    var testere = db.Team.FirstOrDefault(t => t.Name == "Testere");

    var steen = db.Worker.FirstOrDefault(w => w.Name == "Steen Secher");
    var ejvind = db.Worker.FirstOrDefault(w => w.Name == "Ejvind Møller");
    var konrad = db.Worker.FirstOrDefault(w => w.Name == "Konrad Sommer");
    var sofus = db.Worker.FirstOrDefault(w => w.Name == "Sofus Lotus");
    var remo = db.Worker.FirstOrDefault(w => w.Name == "Remo Lademann");
    var ella = db.Worker.FirstOrDefault(w => w.Name == "Ella Fanth");
    var anne = db.Worker.FirstOrDefault(w => w.Name == "Anne Dam");

    if (frontend == null || backend == null || testere == null)
    {
        return;
    }
    if (steen == null || ejvind == null || konrad == null || sofus == null || remo == null || ella == null || anne == null)
    {
        return;
    }

    frontend.Workers.AddRange([steen, ejvind, konrad]);
    Console.WriteLine("Connect " + steen.Name + " " + ejvind.Name + " " + konrad.Name + " to " + frontend.Name);

    backend.Workers.AddRange([konrad, sofus, remo]);
    Console.WriteLine("Connect " + konrad.Name + " " + sofus.Name + " " + remo.Name + " to " + backend.Name);


    testere.Workers.AddRange([ella, anne, steen]);
    Console.WriteLine("Connect " + ella.Name + " " + anne.Name + " " + steen.Name + " to " + testere.Name);

    db.SaveChanges();
}

static void connectTaskAndTeam(BloggingContext db)
{
    Console.WriteLine("Connect task to team...");

    var softwareTask = db.Task.FirstOrDefault(t => t.Name == "Produce software");
    var brewTask = db.Task.FirstOrDefault(t => t.Name == "Brew coffee");

    var frontend = db.Team.FirstOrDefault(t => t.Name == "Frontend");
    var backend = db.Team.FirstOrDefault(t => t.Name == "Backend");
    var testere = db.Team.FirstOrDefault(t => t.Name == "Testere");

    if (softwareTask == null || brewTask == null || frontend == null || backend == null || testere == null)
    {
        return;
    }

    frontend.Tasks.Add(softwareTask);
    Console.WriteLine("Connect " + softwareTask.Name + " to " + frontend.Name);
    frontend.CurrentTask = softwareTask;
    Console.WriteLine("Connect current task " + softwareTask.Name + " to " + frontend.Name);
    backend.Tasks.Add(brewTask);
    Console.WriteLine("Connect " + brewTask.Name + " to " + backend.Name);
    backend.CurrentTask = brewTask;
    Console.WriteLine("Connect current task " + brewTask.Name + " to " + backend.Name);

    db.SaveChanges();
}

static void connectTodoAndWorker(BloggingContext db)
{
    Console.WriteLine("Connect todo to worker...");

    var teamFrontend = db.Team
        .Include(t => t.Workers)
        .Include(t => t.Tasks)
        .FirstOrDefault(t => t.Name == "Frontend");

    if (teamFrontend != null && teamFrontend.CurrentTaskId != null)
    {
        var todosFrontend = db.Todo
          .Where(todo => todo.TaskId == teamFrontend.CurrentTaskId.Value)
          .ToList();
        foreach (var worker in teamFrontend.Workers)
        {
            var todoIndex = teamFrontend.Workers.IndexOf(worker) % todosFrontend.Count;
            worker.Todos.Add(todosFrontend[todoIndex]);
            Console.WriteLine($"Connect Worker ID: {worker.WorkerId}, Name: {worker.Name}, to - Todo ID: {todosFrontend[todoIndex].TodoId}");
        }
    }

    var teamBackend = db.Team
        .Include(t => t.Workers)
        .Include(t => t.Tasks)
        .FirstOrDefault(t => t.Name == "Backend");

    if (teamBackend != null && teamBackend.CurrentTaskId != null)
    {
        var todosBackend = db.Todo
        .Where(todo => todo.TaskId == teamBackend.CurrentTaskId.Value)
        .ToList();
        foreach (var worker in teamBackend.Workers)
        {
            var todoIndex = teamBackend.Workers.IndexOf(worker) % todosBackend.Count;
            worker.Todos.Add(todosBackend[todoIndex]);
            Console.WriteLine($"Connect Worker ID: {worker.WorkerId}, Name: {worker.Name}, to - Todo ID: {todosBackend[todoIndex].TodoId}");
        }
    }

    var teamTest = db.Team
        .Include(t => t.Workers)
        .Include(t => t.Tasks)
        .FirstOrDefault(t => t.Name == "Test");
    if (teamTest != null && teamTest.CurrentTaskId != null)
    {
        var todosTest = db.Todo
        .Where(todo => todo.TaskId == teamTest.CurrentTaskId.Value)
        .ToList();
        foreach (var worker in teamTest.Workers)
        {
            var todoIndex = teamTest.Workers.IndexOf(worker) % todosTest.Count;
            worker.Todos.Add(todosTest[todoIndex]);
            Console.WriteLine($"Connect Worker ID: {worker.WorkerId}, Name: {worker.Name}, to - Todo ID: {todosTest[todoIndex].TodoId}");
        }
    }

    db.SaveChanges();
}


static void displayTasksAndTodos(BloggingContext db)
{
    Console.WriteLine("Displaying all tasks and their associated todos:");

    var tasks = db.Task.Include(t => t.Todos).ToList();
    foreach (var task in tasks)
    {
        Console.WriteLine($"Task: {task.Name} (ID: {task.TaskId})");

        foreach (var todo in task.Todos)
        {
            Console.WriteLine($"  - Todo: {todo.Name} (ID: {todo.TodoId}, Completed: {todo.IsComplete})");
        }
    }
}

static void printIncompleteTasksAndTodos(BloggingContext db)
{
    Console.WriteLine("Displaying all tasks and their associated undone todos:");

    var tasksWithIncompleteTodos = db.Task
        .Where(t => t.Todos.Any(todo => todo.IsComplete == false))
        .Include(t => t.Todos)
    .ToList();

    foreach (var task in tasksWithIncompleteTodos)
    {
        Console.WriteLine($"Task: {task.Name} (ID: {task.TaskId})");

        foreach (var todo in task.Todos.Where(todo => todo.IsComplete == false))
        {
            Console.WriteLine($"  - Todo: {todo.Name} (ID: {todo.TodoId}, Completed: {todo.IsComplete})");
        }
    }
}

static void PrintTeamsWithoutTasks(BloggingContext db)
{
    var teamsWithoutTasks = db.Team
        .Where(t => !t.Tasks.Any())
        .ToList();

    if (!teamsWithoutTasks.Any())
    {
        Console.WriteLine("All teams have tasks.");
    }
    else
    {
        Console.WriteLine("Teams without tasks:");
        foreach (var team in teamsWithoutTasks)
        {
            Console.WriteLine($"Team ID: {team.TeamId}, Name: {team.Name}");
        }
    }
}


static void PrintTeamsCurrentTask(BloggingContext db)
{
    var teamsWithCurrentTasks = db.Team
        .Include(t => t.CurrentTask)
        .ToList();

    Console.WriteLine("Teams and their current tasks:");
    foreach (var team in teamsWithCurrentTasks)
    {
        Console.WriteLine($"Team ID: {team.TeamId}, Name: {team.Name}");

        if (team.CurrentTask != null)
        {
            Console.WriteLine($"\tCurrent Task ID: {team.CurrentTask.TaskId}, Name: {team.CurrentTask.Name}");
        }
        else
        {
            Console.WriteLine("\tNo current task assigned.");
        }
    }
}

