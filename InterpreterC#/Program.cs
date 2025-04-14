using InterpreterC_;
using System.Reflection.Metadata.Ecma335;

String code = readCode("C:\\Users\\mikaa\\Desktop\\Tor Browser\\Browser\\TorBrowser\\Docs\\test.txt");

Interpreter interpreter = new();
if(code != null)
{
    interpreter.interpret(code);
}

static String readCode(String path)
{
    String code;
    String line;
    
    StreamReader sr = new(path);
    line = sr.ReadLine();
    
    if (line == null) //assert
    {
        throw new Exception("MainErr0: reading of file in specified directory failed OR file is empty");
    }
    
    code = line;
    while (line != null)
    {
        line = sr.ReadLine();
        code += "\n";
        code += line;
    }
    sr.Close();

    return code;
}
