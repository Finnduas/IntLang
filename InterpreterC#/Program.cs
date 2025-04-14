using InterpreterC_;
using System.Reflection.Metadata.Ecma335;

String code;
String line;

    StreamReader sr = new("C:\\Users\\mikaa\\Desktop\\Tor Browser\\Browser\\TorBrowser\\Docs\\ChangeLog.monkey");

    line = sr.ReadLine();
    
    if(line == null) {
        Console.WriteLine("MainErr0: line == null <=> true => reading of file in specified directory failed ");
        return;
    }

    code = line;
    while (line != null)
    {
        line = sr.ReadLine();
        code += "\n";
        code += line;
    }

    sr.Close();

    Interpreter interpreter = new();
    if(code != null)
    {
        interpreter.interpret(code);
    }
