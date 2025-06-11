using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using Lookup;

namespace InterpreterC_
{
    [MemoryDiagnoser]
    [SimpleJob(
    RuntimeMoniker.Net80,
    launchCount: 1,     // L
    warmupCount: 10,     // W
    iterationCount: 100,    // I
    invocationCount: 100000000   // N
)]
    public class Benchmark_NO_InstAndInit
    {
        // TODO: make good, solid benchmarks and stuff...

        const String varInput =
              "var x = 5; \n"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var foobar = 838383;";

        Program PrepedProgram = new();
        [GlobalSetup]
        public void Setup()
        {
            parLexMan.init_lexer(varInput);
            walLexMan.init_lexer(varInput);

            parPar = new(parLexMan);
            walPar = new(walLexMan);

            walPro = walPar.parse_program();
        }


        LexerManager parLexMan = new();
        Parser? parPar;
        [Benchmark]
        public void parser_pure()
        {
            parPar!.parse_program();
        }

        [Benchmark]
        public void parser()
        {
            LexerManager lexMan = new();
            lexMan.init_lexer(varInput);
            Parser par = new(lexMan);
            Program pro = par!.parse_program();
        }
        LexerManager walLexMan = new();
        Parser? walPar;
        Program walPro;
        /* [Benchmark]
        public void Walker()
        {
            walk(walPro!);
        }
        */
    }
    [MemoryDiagnoser]
    [SimpleJob(
    RuntimeMoniker.Net80,
    launchCount: 1,     // L
    warmupCount: 10,     // W
    iterationCount: 100,    // I
    invocationCount: 100000   // N
    )]
    public class Benchmark_WITH_InstAndInit
    {
        // TODO: make good, solid benchmarks and stuff...

        const String varInput =
              "var x = 5; \n"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var y = 10;\n"
            + "var foobar = 838383;"
            + "var foobar = 838383;";

        Program PrepedProgram = new();
        [GlobalSetup]
        public void Setup()
        {

        }

        [Benchmark]
        public void parser()
        {
            LexerManager lexMan = new();
            lexMan.init_lexer(varInput);
            Parser par = new(lexMan);
            Program pro = par!.parse_program();
        }
    }
}
