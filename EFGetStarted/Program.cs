using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using var db = new BloggingContext();



// Note: This sample requires the database to be created before running.
Console.WriteLine($"Database path: {db.DbPath}.");

// Create
Console.WriteLine("Inserting a new blog");
db.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
db.SaveChanges();

// Read
Console.WriteLine("Querying for a blog");
var blog = db.Blogs
    .OrderBy(b => b.BlogId)
    .First();

// Update
Console.WriteLine("Updating the blog and adding a post");
blog.Url = "https://devblogs.microsoft.com/dotnet";
blog.Posts.Add(
    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
db.SaveChanges();

// Delete
Console.WriteLine("Delete the blog");
db.Remove(blog);
db.SaveChanges();

// Tasks & Todos
//seedTasks();
displayTasksAndTodos(db);
printIncompleteTasksAndTodos(db);

//deleteAllTasksAndTodos(db)



static void seedTasks()
{
    using var db = new BloggingContext();

    Console.WriteLine("Creating tasks and todos");
    Todo wirt = createTodo("Write code", true, db);
    Todo compile = createTodo("Compile source", false, db);
    Todo test = createTodo("Test program", false, db);

    Todo water = createTodo("Pour water", false, db);
    Todo coffe = createTodo("Pour coffee", true, db);
    Todo on = createTodo("Turn on", false, db);


    Task one = createTask("Produce software", new List<Todo> { wirt, compile, test }, db);
    Task Two = createTask("Brew coffee", new List<Todo> { water, coffe, on }, db);
}

static Todo createTodo(string name, bool completed,BloggingContext db)
{
    var todo = new Todo
    {
        Name = name,
        IsComplete = completed
    };

    db.Todos.Add(todo);
    db.SaveChanges();
    return todo;
}

static Task createTask(string name, List<Todo> todo, BloggingContext db)
{
    var task = new Task
    {
        Name = name,
        Todos = todo
    };
    db.Tasks.Add(task);
    db.SaveChanges();
    return task;
}

static void displayTasksAndTodos(BloggingContext db)
{
    Console.WriteLine("Displaying all tasks and their associated todos:");

    var tasks = db.Tasks.Include(t => t.Todos).ToList();
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

    var tasksWithIncompleteTodos = db.Tasks.Where(t => t.Todos.Any(todo => !todo.IsComplete))
                                     .Include(t => t.Todos)
                                     .ToList();

    foreach (var task in tasksWithIncompleteTodos)
    {
        Console.WriteLine($"Task: {task.Name} (ID: {task.TaskId})");

        foreach (var todo in task.Todos.Where(todo => !todo.IsComplete))
        {
            Console.WriteLine($"  - Todo: {todo.Name} (ID: {todo.TodoId}, Completed: {todo.IsComplete})");
        }
    }
}

static void deleteAllTasksAndTodos(BloggingContext db)
{
    // Delete all Todos
    db.Todos.RemoveRange(db.Todos);

    // Delete all Tasks
    db.Tasks.RemoveRange(db.Tasks);

    // Save changes to the database
    db.SaveChanges();
}