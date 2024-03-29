using System;
using System.Collections.Generic;
using System.Text;

using Symbols;

namespace Main {
    //using Test;
    using GuidedTour;
    using Pirate;
    using PirateType=Pirate.Type;
    using PirateLiteral=Pirate.Literal;
    using Util;
    using Sexp;
    using Flat;
    using Flat2Pirate;
    using FlatType=Flat.Type;
    using FlatLiteral=Flat.Literal;
    using DynamicVisitor;
    using ConstructLang;
    class Program {
#if false
        static string Example1()
        {
            Pirate p = new Pirate();
            StringLiteral s1 = new StringLiteral("H\ae\bl\fl\vo\0 \'P\"arrot!\n");
            Call c1 = new Call("print", s1);
            StmtList sl = new StmtList();
            CallStmt cs1 = new CallStmt(c1);
            sl.Add(cs1);
            Sub main = new Sub("main", sl);
            p.Add(main);

            StringWriter sw = new StringWriter();
            PirateWriter pv = new PirateWriter(sw);

            DynamicVisitor.accept(p, pv);

            return sw.ToString();
        }

        static string Example2()
        {
            NamedReg a = new NamedReg("a");
            NamedReg b = new NamedReg("b");
            NamedReg c = new NamedReg("c");
            NamedReg det = new NamedReg("det");

            IdList rl1 = new IdList();
            rl1.Add(a);
            rl1.Add(b);
            rl1.Add(c);
            rl1.Add(det);

            LocalDecl ld1 = new LocalDecl(new NumType(), rl1);

            IntLiteral il3 = new IntLiteral(2);
            Assign a12 = new Assign(a, il3);

            IntLiteral il4 = new IntLiteral(-3);
            Assign a13 = new Assign(b, il4);

            IntLiteral il5 = new IntLiteral(-2);
            Assign a14 = new Assign(c, il5);

            UnaryNeg un1 = new UnaryNeg(b);
            TmpNumReg tnr0 = new TmpNumReg(0);
            Assign a1 = new Assign(tnr0, un1);

            TmpNumReg tnr1 = new TmpNumReg(1);
            BinaryMul bm1 = new BinaryMul(b, b);
            Assign a2 = new Assign(tnr1, bm1);

            TmpNumReg tnr2 = new TmpNumReg(2);
            IntLiteral il1 = new IntLiteral(4);
            BinaryMul bm2 = new BinaryMul(il1, a);
            Assign a3 = new Assign(tnr2, bm2);

            BinaryMul bm3 = new BinaryMul(tnr2, c);
            Assign a4 = new Assign(tnr2, bm3);

            TmpNumReg tnr3 = new TmpNumReg(3);
            IntLiteral il2 = new IntLiteral(2);
            BinaryMul bm4 = new BinaryMul(il2, a);
            Assign a5 = new Assign(tnr3, bm4);

            BinarySub bs1 = new BinarySub(tnr1, tnr2);
            Assign a6 = new Assign(det, bs1);

            TmpNumReg tnr4 = new TmpNumReg(4);
            Call sqrt = new Call("sqrt", det);
            Assign a7 = new Assign(tnr4, sqrt);

            NamedReg x1 = new NamedReg("x1");
            NamedReg x2 = new NamedReg("x2");

            IdList rl2 = new IdList();
            rl2.Add(x1);
            rl2.Add(x2);

            LocalDecl ld2 = new LocalDecl(new NumType(), rl2);

            BinaryAdd ba1 = new BinaryAdd(tnr0, tnr4);
            Assign a8 = new Assign(x1, ba1);

            BinaryDiv bd1 = new BinaryDiv(x1, tnr3);
            Assign a9 = new Assign(x1, bd1);

            BinarySub bs2 = new BinarySub(tnr0, tnr4);
            Assign a10 = new Assign(x2, bs2);

            AssignDiv a11 = new AssignDiv(x2, tnr3);

            StringLiteral s1 = new StringLiteral("Answers to ABC formula are:\n");
            Call c1 = new Call("print", s1);
            CallStmt print1 = new CallStmt(c1);

            StringLiteral s2 = new StringLiteral("x1 = ");
            Call c2 = new Call("print", s2);
            CallStmt print2 = new CallStmt(c2);

            Call c3 = new Call("print", x1);
            CallStmt print3 = new CallStmt(c3);

            StringLiteral s4 = new StringLiteral("\nx2 = ");
            Call c4 = new Call("print", s4);
            CallStmt print4 = new CallStmt(c4);

            Call c5 = new Call("print", x2);
            CallStmt print5 = new CallStmt(c5);

            StringLiteral s6 = new StringLiteral("\n");
            Call c6 = new Call("print", s6);
            CallStmt print6 = new CallStmt(c6);

            StmtList sl1 = new StmtList();
            sl1.Add(ld1);
            sl1.Add(a12);
            sl1.Add(a13);
            sl1.Add(a14);
            sl1.Add(a1);
            sl1.Add(a2);
            sl1.Add(a3);
            sl1.Add(a4);
            sl1.Add(a5);
            sl1.Add(a6);
            sl1.Add(a7);
            sl1.Add(ld2);
            sl1.Add(a8);
            sl1.Add(a9);
            sl1.Add(a10);
            sl1.Add(a11);
            sl1.Add(print1);
            sl1.Add(print2);
            sl1.Add(print3);
            sl1.Add(print4);
            sl1.Add(print5);
            sl1.Add(print6);

            Sub foo = new Sub("foo", sl1);

            Pirate p = new Pirate();
            p.Add(foo);

            StringWriter sw = new StringWriter();
            PirateWriter pv = new PirateWriter(sw);

            DynamicVisitor.accept(p, pv);

            return sw.ToString();
        }

#if false
        static string Example3()
        {
            Pirate p = new Pirate();

            StmtList sl1 = new StmtList();

            Sub joe = new Sub("joe", sl1);

            p.Add(joe);

            LocalDecl ld1 = new LocalDecl();
            ld1.type = new StringType();

            NamedReg name = new NamedReg();
            name.name = "name";
            IdList idl1 = new IdList();
            idl1.Add(name);

            ld1.id_list = idl1;

            sl1.Add(ld1);

            Assign a1 = new Assign();
            a1.lval = name;

            StringLiteral s1 = new StringLiteral();
            s1.value = " Joe!";

            a1.rval = s1;

            sl1.Add(a1);

            Assign a2 = new Assign();
            StringLiteral s2 = new StringLiteral();
            s2.value = "Hi!";

            TmpStringReg tsr0 = new TmpStringReg();
            tsr0.number = 0;

            a2.lval = tsr0;
            a2.rval = s2;

            sl1.Add(a2);

            Assign a3 = new Assign();
            TmpStringReg tsr1 = new TmpStringReg();
            tsr1.number = 1;

            BinaryCat bc1 = new BinaryCat();
            bc1.a = tsr0;
            bc1.b = name;

            a3.lval = tsr1;
            a3.rval = bc1;

            sl1.Add(a3);

            AssignCat a4 = new AssignCat();
            a4.lval = tsr1;
            StringLiteral s3 = new StringLiteral();
            s3.value = "\n";

            a4.rval = s3;

            sl1.Add(a4);

            CallStmt cs1 = new CallStmt();
            Call c1 = new Call();
            c1.func = "print";
            c1.args = tsr1;
            cs1.call = c1;
            sl1.Add(cs1);

            StringWriter sw = new StringWriter();
            PirateWriter pv = new PirateWriter(sw);

            DynamicVisitor.accept(p, pv);

            return sw.ToString();
        }

        static string Example4()
        {
            StmtList sl1 = new StmtList();

            Sub foo = new Sub("foo", sl1);

            Pirate p = new Pirate();
            p.Add(foo);

            ParamDecl pd1 = new ParamDecl();

            pd1.type = new IntType();

            IdList idl1 = new IdList();
            NamedReg n = new NamedReg();
            n.name = "n";
            idl1.Add(n);

            pd1.id_list = idl1;
            sl1.Add(pd1);

            ParamDecl pd2 = new ParamDecl();

            pd2.type = new StringType();

            IdList idl2 = new IdList();
            NamedReg message = new NamedReg();
            message.name = "message";
            idl2.Add(message);

            pd2.id_list = idl2;
            sl1.Add(pd2);

            StringWriter sw = new StringWriter();
            PirateWriter pv = new PirateWriter(sw);

            DynamicVisitor.accept(p, pv);

            return sw.ToString();
        }

        static string Example5()
        {
            NamedReg x1 = new NamedReg();
            x1.name = "x1";

            NamedReg x2 = new NamedReg();
            x2.name = "x2";

            IdList idl1 = new IdList();
            idl1.Add(x1);
            idl1.Add(x2);

            LocalDecl ld1 = new LocalDecl();
            ld1.type = new NumType();
            ld1.id_list = idl1;

            AtomExprList ael1 = new AtomExprList();
            ael1.Add(x1);
            ael1.Add(x2);

            ReturnStmt rs1 = new ReturnStmt();
            rs1.rv = ael1;

            StmtList sl1 = new StmtList();
            sl1.Add(ld1);
            sl1.Add(rs1);

            Sub abc = new Sub("abc", sl1);

            Pirate p = new Pirate();
            p.Add(abc);

            StringWriter sw = new StringWriter();
            PirateWriter pv = new PirateWriter(sw);

            DynamicVisitor.accept(p, pv);

            return sw.ToString();
        }
#endif
        static string Example6()
        {
            AtomExprList ael1 = new AtomExprList();
            Call c1 = new Call("foo", ael1);

            CallStmt cs1 = new CallStmt(c1);

            NumLiteral n1 = new NumLiteral(3.14);
            TmpNumReg tnr0 = new TmpNumReg(0);
            Assign a1 = new Assign(tnr0, n1);

            TmpIntReg tir0 = new TmpIntReg(0);

            IntLiteral i1 = new IntLiteral(42);
            StringLiteral s1 = new StringLiteral("hi");

            AtomExprList ael2 = new AtomExprList();
            ael2.Add(tir0);
            ael2.Add(i1);
            ael2.Add(s1);

            Call c2 = new Call("bar", ael2);
            CallStmt cs2 = new CallStmt(c2);

            NamedReg a = new NamedReg("a");
            LocalDecl ld1 = new LocalDecl(new IntType(), a);

            NamedReg b = new NamedReg("b");
            LocalDecl ld2 = new LocalDecl(new NumType(), b);

            NamedReg c = new NamedReg("c");
            LocalDecl ld3 = new LocalDecl(new StringType(), c);

            TmpNumReg tnr2 = new TmpNumReg(2);
            NumLiteral n2 = new NumLiteral(2.7);
            Assign a2 = new Assign(tnr2, n2);

            StringLiteral s2 = new StringLiteral("hello yourself");
            AtomExprList ael3 = new AtomExprList();
            ael3.Add(tnr2);
            ael3.Add(s2);
            Call c3 = new Call("baz", ael3);

            RegList rl4 = new RegList();
            rl4.Add(a);
            rl4.Add(b);
            rl4.Add(c);

            Assign a3 = new Assign(rl4, c3);

            StmtList sl1 = new StmtList();
            sl1.Add(cs1);
            sl1.Add(a1);
            sl1.Add(cs2);
            sl1.Add(ld1);
            sl1.Add(ld2);
            sl1.Add(ld3);
            sl1.Add(a2);
            sl1.Add(a3);

            Sub main = new Sub("main", sl1);

            StringLiteral s3 = new StringLiteral("Foo!\n");
            Call c4 = new Call("print", s3);
            CallStmt cs3 = new CallStmt(c4);

            StmtList sl2 = new StmtList();
            sl2.Add(cs3);

            Sub foo = new Sub("foo", sl2);

            NamedReg i = new NamedReg("i");
            ParamDecl pd1 = new ParamDecl(new NumType(), i);

            NamedReg answer = new NamedReg("answer");
            ParamDecl pd2 = new ParamDecl(new IntType(), answer);

            NamedReg message = new NamedReg("message");
            ParamDecl pd3 = new ParamDecl(new StringType(), message);

            StringLiteral s4 = new StringLiteral("Bar!\n");
            Call print1 = new Call("print", s4);
            CallStmt cs4 = new CallStmt(print1);

            Call print2 = new Call("print", i);
            CallStmt cs5 = new CallStmt(print2);

            StringLiteral s5 = new StringLiteral("\n");
            Call print3 = new Call("print", s5);
            CallStmt cs6 = new CallStmt(print3);

            Call print4 = new Call("print", answer);
            CallStmt cs7 = new CallStmt(print4);

            CallStmt cs8 = new CallStmt(print3);

            Call print5 = new Call("print", message);
            CallStmt cs9 = new CallStmt(print5);

            StmtList sl3 = new StmtList();
            sl3.Add(pd1);
            sl3.Add(pd2);
            sl3.Add(pd3);
            sl3.Add(cs4);
            sl3.Add(cs5);
            sl3.Add(cs6);
            sl3.Add(cs7);
            sl3.Add(cs8);
            sl3.Add(cs9);

            Sub bar = new Sub("bar", sl3);

            NamedReg e = new NamedReg("e");
            ParamDecl pd4 = new ParamDecl(new NumType(), e);

            NamedReg msg = new NamedReg("msg");
            ParamDecl pd5 = new ParamDecl(new StringType(), msg);

            StringLiteral s6 = new StringLiteral("Baz!\n");
            Call print7 = new Call("print", s6);
            CallStmt cs10 = new CallStmt(print7);

            Call print8 = new Call("print", e);
            CallStmt cs11 = new CallStmt(print8);

            Call print9 = new Call("print", s5);
            CallStmt cs12 = new CallStmt(print9);

            Call print10 = new Call("print", msg);
            CallStmt cs13 = new CallStmt(print10);

            AtomExprList ael4 = new AtomExprList();
            ael4.Add(new IntLiteral(1000));
            ael4.Add(new NumLiteral(1.23));
            ael4.Add(new StringLiteral("hi from baz"));
            ReturnStmt rs1 = new ReturnStmt(ael4);

            StmtList sl4 = new StmtList();
            sl4.Add(pd4);
            sl4.Add(pd5);
            sl4.Add(cs10);
            sl4.Add(cs11);
            sl4.Add(cs12);
            sl4.Add(cs13);
            sl4.Add(rs1);

            Sub baz = new Sub("baz", sl4);

            Pirate p = new Pirate();
            p.Add(main);
            p.Add(foo);
            p.Add(bar);
            p.Add(baz);

            StringWriter sw = new StringWriter();
            PirateWriter pv = new PirateWriter(sw);

            DynamicVisitor.accept(p, pv);

            return sw.ToString();
        }
#endif
#if true
        static void test_scan(string path)
        {
            using (Reader file = new Reader(path)) {
                Scanner scanner = new Scanner(file);
                Attributes attrib;

                Console.WriteLine("scanning: {0}", path);
                do {
                    attrib = scanner.scan();

                    //if (attrib.token != Token.ERROR) {
                    //    if (Token.NUM == attrib.token) {
                    //        Console.WriteLine("{0} {1} {3} {4}", attrib.loc.PathPoint(), Enum.GetName(attrib.token.GetType(), attrib.token), (attrib.literal==null)?(""):("= "+attrib.literal), (attrib.error == null)?(""):("warning: "+attrib.error));
                    //    }
                    //} else {
                    Console.WriteLine("{0} error: {1} = {2}", attrib.loc.PathPoint(), attrib.error, attrib.literal);
                    //}

                } while (attrib.token != Token.EOF);
            }
        }

        static void test_parse(string path)
        {
            using (Reader file = new Reader(path)) {
                StringWriter writer = new StringWriter();
                VectVisitor visitor = GetTopLevelWriter.create(writer);
                Parser parser = new Parser(file, visitor);

                Console.WriteLine("parsing: {0}", path);
                parser.SafeRead();
                Console.WriteLine(writer.ToString());
                //Console.WriteLine("{0} error{1}", parser.errors, parser.errors!=1?"s":"");
            }
        }

        static void test_safe_parse(string path)
        {
            using (Reader file = new Reader(path)) {
                //StringWriter writer = new StringWriter();
                //TopLevelWriter visitor = new TopLevelWriter(writer);
                VectVisitor visitor = new SafeVectorVisitor(null);
                Parser parser = new Parser(file, visitor);

                Console.WriteLine("parsing: {0}", path);
                parser.SafeRead();
                //Console.WriteLine(writer.ToString());
                //Console.WriteLine("{0} error{1}", parser.errors, parser.errors!=1?"s":"");
            }
        }

        static void double_parse(string path)
        {
            using (Reader file = new Reader(path)) {
                StringWriter writer = new StringWriter();
                VectVisitor visitor = GetTopLevelWriter.create(writer);
                Parser parser = new Parser(file, visitor);

                parser.SafeRead();
                //Console.WriteLine("{0} error{1}", parser.errors, parser.errors!=1?"s":"");

                //if (parser.errors == 0) {
                using (System.IO.StreamWriter output1 = new System.IO.StreamWriter(path + ".parsed.txt", false)) {
                    output1.Write(writer.ToString());
                    //    }
                }
            }
        }

        static void parse_and_write(string in_path, string out_path)
        {
            Console.WriteLine("parsing {0} and writing it out to {1}", in_path, out_path);
            using (Reader file = new Reader(in_path)) {
                using (FileWriter writer = new FileWriter(out_path)) {
                    VectVisitor visitor = GetTopLevelWriter.create(writer);
                    Parser parser = new Parser(file, visitor);
                    Console.WriteLine(parser.SafeRead() ? "OK" : "FAILED");
                    //Console.WriteLine("{0} error{1}", parser.errors, parser.errors!=1?"s":"");
                    //Console.WriteLine(parser.errors == 0 ? "OK" : "FAILED!");
                }
            }
            Console.WriteLine();
        }

        static void compare_files(string path1, string path2)
        {
            Console.WriteLine("comparing the token stream of {0} and {1}", path1, path2);
            using (Reader file1 = new Reader(path1)) {
                using (Reader file2 = new Reader(path2)) {
                    Scanner s1 = new Scanner(file1);
                    Scanner s2 = new Scanner(file2);
                    Comparer comp = new Comparer(s1, s2);
                    comp.run();
                }
            }
            Console.WriteLine();
        }

        static object[] build(string path)
        {
            using (Reader file = new Reader(path)) {
                VectBox top = new VectBox();
                VectBuilder visitor = new VectBuilder(top);
                Parser parser = new Parser(file, visitor);

                Console.WriteLine("parsing: {0}", path);
                Console.WriteLine(parser.SafeRead() ? "OK" : "FAILED");
                //Console.WriteLine("{0} error{1}", parser.errors, parser.errors!=1?"s":"");
                //Console.WriteLine(parser.errors == 0 ? "OK" : "FAILED!");
                return top.value;
            }
        }

        static void parse_build_write(string in_path, string out_path)
        {
            Console.WriteLine("parsing {0} into memory then writing it out to {1}", in_path, out_path);
            using (FileWriter writer = new FileWriter(out_path)) {
                object[] top = build(in_path);
                if (top != null) DynamicVisitor.accept(top, GetTopLevelWriter.create(writer));
            }
            Console.WriteLine();
        }

        static void compare_logs(string path1, string path2)
        {
            Console.WriteLine("comparing the visitation logs for files {0} and {1}", path1, path2);
            using (Reader file1 = new Reader(path1)) {
                using (Reader file2 = new Reader(path2)) {
                    Log log1 = new Log();
                    Log log2 = new Log();
                    Util.TxtLocation loc1 = new Util.TxtLocation(path1);
                    Util.TxtLocation loc2 = new Util.TxtLocation(path2);
                    VectorLogger logger1 = new VectorLogger(log1, loc1, new SafeVectorVisitor(null));
                    VectorLogger logger2 = new VectorLogger(log2, loc2, new SafeVectorVisitor(null));
                    Parser p1 = new Parser(file1, logger1, loc1);
                    Parser p2 = new Parser(file2, logger2, loc2);
                    p1.SafeRead();
                    p2.SafeRead();
                    LogComparer.compare_logs(log1, log2);
                }
            }
            Console.WriteLine();
        }

        static void compare_logs2(string path1, string path2)
        {
            Console.WriteLine("testing the multivisitor with files {0} and {1}", path1, path2);
            using (Reader file1 = new Reader(path1)) {
                using (Reader file2 = new Reader(path2)) {
                    Log log1 = new Log();
                    Log log2 = new Log();
                    Util.TxtLocation loc1 = new Util.TxtLocation(path1);
                    Util.TxtLocation loc2 = new Util.TxtLocation(path2);
                    VectorLogger logger1 = new VectorLogger(log1, loc1, new SafeVectorVisitor(null));
                    VectorLogger logger2 = new VectorLogger(log2, loc2, new SafeVectorVisitor(null));
                    VectorMultiVisitor mv = new VectorMultiVisitor(logger1, logger2);
                    Parser p1 = new Parser(file1, mv, loc1);
                    p1.SafeRead();
                    LogComparer.compare_logs(log1, log2);
                }
            }
            Console.WriteLine();
        }

        static void compare_logs(string path)
        {
            Console.WriteLine("comparing the visitation logs for the parser and dynamic visitor for file {0}", path);

            Log log1 = new Log();

            using (Reader file = new Reader(path)) {
                Util.TxtLocation loc = new Util.TxtLocation(path);
                VectorLogger logger = new VectorLogger(log1, loc, new SafeVectorVisitor(null));
                Parser p = new Parser(file, logger, loc);
                p.SafeRead();
            }

            Log log2 = new Log();

            using (Reader file = new Reader(path)) {
                VectBox top = new VectBox();
                VectBuilder builder = new VectBuilder(top);
                Parser p = new Parser(file, builder);
                Util.TxtLocation loc = new Util.TxtLocation(path);
                p.SafeRead();

                VectorLogger logger = new VectorLogger(log2, loc, new SafeVectorVisitor(null));
                if (top.value != null) DynamicVisitor.accept(top.value, logger);
            }

            LogComparer.compare_logs(log1, log2);

            Console.WriteLine();
        }
#endif
        static void code_builder_test(Code code)
        {
            PirateCodeBuilder pir = new PirateCodeBuilder();
            DynamicVisitor.accept(code, pir);
            StringWriter sw = new StringWriter();
            DynamicVisitor.accept(pir.getPirate(), new PirateWriter(sw));
            Console.Write(sw.ToString());
        }

        static Code code_builder_test1()
        {
#if false
            ; hello world
            (code
                (type string)
                (prototype print_prototype (string) ())
                (global print print_prototype)
                (constant "Hello World!\\n" string)

                (lambda main
                    (call print_prototype print () ("Hello World!\\n"))))
#endif
            CodeBuilder cb = new CodeBuilder();
            FlatType string_type = cb.defineType("string");
            Prototype print_prototype = cb.definePrototype(null, new Types(string_type), null);
            Global print_global = cb.defineGlobal("print", print_prototype);
            FlatLiteral hello_world = cb.defineLiteral(string_type, @"Hello World!"+System.Environment.NewLine);
            LambdaBuilder lb = cb.getLambdaBuilder("main", null);
            lb.addCall(print_global, null, new Operands(hello_world));
            cb.defineLambda(lb.getLambda(null));
            return cb.getCode();
        }

        static Code code_builder_test2()
        {
            CodeBuilder cb = new CodeBuilder();

            FlatType string_type = cb.defineType("string");
            FlatType single_type = cb.defineType("single");

            Prototype print_prototype1 = cb.definePrototype(null, new Types(string_type), null);
            Prototype print_prototype2 = cb.definePrototype(null, new Types(single_type), null);
            Prototype sqrt_prototype = cb.definePrototype(null, new Types(single_type), new Types(single_type));

            Global print_global1 = cb.defineGlobal("print1", print_prototype1);
            Global print_global2 = cb.defineGlobal("print2", print_prototype2);
            Global sqrt_global = cb.defineGlobal("sqrt", sqrt_prototype);

            Operator neg = cb.defineOperator("neg", new Types(single_type), new Types(single_type));
            Operator add = cb.defineOperator("+", new Types(single_type), new Types(single_type, single_type));
            Operator sub = cb.defineOperator("-", new Types(single_type), new Types(single_type, single_type));
            Operator mul = cb.defineOperator("*", new Types(single_type), new Types(single_type, single_type));
            Operator div = cb.defineOperator("/", new Types(single_type), new Types(single_type, single_type));
            Operator div_assign = cb.defineOperator("/=", new Types(single_type), new Types(single_type));

            LambdaBuilder lb = cb.getLambdaBuilder("foo", null);
            Local a = lb.defineLocal("a", single_type);
            Local b = lb.defineLocal("b", single_type);
            Local c = lb.defineLocal("c", single_type);
            Local det = lb.defineLocal("det", single_type);
            Local n0 = lb.defineLocal("n0", single_type);
            Local n1 = lb.defineLocal("n1", single_type);
            Local n2 = lb.defineLocal("n2", single_type);
            Local n3 = lb.defineLocal("n3", single_type);
            Local n4 = lb.defineLocal("n4", single_type);

            FlatLiteral _2 = cb.defineLiteral(single_type, 2L);
            lb.addMove(new Lvalues(a), new Operands(_2));

            FlatLiteral _n3 = cb.defineLiteral(single_type, -3L);
            lb.addMove(new Lvalues(b), new Operands(_n3));

            FlatLiteral _n2 = cb.defineLiteral(single_type, -2L);
            lb.addMove(new Lvalues(c), new Operands(_n2));

            lb.addDo(neg, new Lvalues(n0), new Operands(b));
            lb.addDo(mul, new Lvalues(n1), new Operands(b, b));

            FlatLiteral _4 = cb.defineLiteral(single_type, 4L);
            lb.addDo(mul, new Lvalues(n2), new Operands(_4, a));

            lb.addDo(mul, new Lvalues(n2), new Operands(n2, c));
            lb.addDo(sub, new Lvalues(det), new Operands(n1, n2));

            lb.addCall(sqrt_global, new Lvalues(n4), new Operands(det));

            Local x1 = lb.defineLocal("x1", single_type);
            Local x2 = lb.defineLocal("x2", single_type);

            lb.addDo(add, new Lvalues(x1), new Operands(n0, n4));
            lb.addDo(div, new Lvalues(x1), new Operands(x1, n3));
            lb.addDo(sub, new Lvalues(x2), new Operands(n0, n4));
            lb.addDo(div_assign, new Lvalues(x2), new Operands(n3));

            Constant answer1 = cb.defineConstant(null, string_type, @"Answers to ABC formula are:"/*+System.Environment.NewLine*/);
            Constant answer2 = cb.defineConstant(null, string_type, @"x1 = ");
            Constant answer3 = cb.defineConstant(null, string_type, /*System.Environment.NewLine+*/@"x2 = ");
            Constant answer4 = cb.defineConstant(null, string_type, @""/*System.Environment.NewLine*/);

            lb.addCall(print_global1, null, new Operands(answer1));
            lb.addCall(print_global1, null, new Operands(answer2));
            lb.addCall(print_global2, null, new Operands(x1));
            lb.addCall(print_global1, null, new Operands(answer3));
            lb.addCall(print_global2, null, new Operands(x2));
            lb.addCall(print_global1, null, new Operands(answer4));

            cb.defineLambda(lb.getLambda(null));

            return cb.getCode();
        }

        class IntVisitor {
            Int64 i = 0;
            int scount = 0;

            public void visit()
            {
                Console.WriteLine("summing ints");
            }

            public void visit(long v)
            {
                i += v;
            }

            public void visit_value(Symbol s)
            {
                scount++;
            }

            public void visitEnd()
            {
                Console.WriteLine("total is {0}", i);
                Console.WriteLine("symbol m_count = {0}", scount);
            }
        }

        public static class GetTopLevelWriter {
            public static VectorWriter create(Writer writer, Format fmt)
            {
                return create(writer, fmt, null);
            }

            public static VectorWriter create(Writer writer, Format fmt, GetConfig get_new_config)
            {
                Config config = new Config(writer, fmt, get_new_config);
                return new VectorWriter(config.file, config);
            }

            public static VectorWriter create(Writer writer)
            {
                return create(writer, new Format());
            }
        }

        public static class ConstructFormat {
            static Config m_cfg;
            static Format m_fmt;

            static readonly Symbol[] print_one_line = {
                Symbol.get_symbol(".alias"),
                Symbol.get_symbol(".namespace"),
                Symbol.get_symbol(".type"),
                Symbol.get_symbol(".prototype"),
                Symbol.get_symbol(".operator"),
                Symbol.get_symbol(".relation"),
                Symbol.get_symbol(".constant"),
                Symbol.get_symbol(".global"),
                Symbol.get_symbol(".param"),
                Symbol.get_symbol(".local"),
                Symbol.get_symbol(".do"),
                Symbol.get_symbol(".move"),
                Symbol.get_symbol(".call"),
                Symbol.get_symbol(".gosub"),
                Symbol.get_symbol(".if"),
            };

            static public void init(Writer writer)
            {
                m_fmt = new Format();
                m_fmt.format_appl = false;
                m_fmt.format_data = false;
                m_fmt.format_head = false;
                m_fmt.format_vect = false;

                m_cfg = new Config(writer, m_fmt);
            }

            static public bool get_construct_lang_config(object atom, out Config config, out bool is_appl)
            {
                if (atom is Symbol) {
                    Symbol sym = (Symbol)atom;

                    foreach (Symbol s in print_one_line) {
                        if (s == sym) {
                            config = m_cfg;
                            is_appl = true;
                            return true;
                        }
                    }
                }

                config = null;
                is_appl = true;

                return false;
            }
        }

        static void Main(string[] args)
        {
#if false
            Parent p = new Parent("Karen");
            p.children.Add(null);
            p.children.Add(new Son("Jason"));
            p.children.Add(new Daughter("April"));

            ParentVisitor pv = new ParentVisitor();

            DynamicVisitor.accept(p, pv);
#endif
#if false
            Console.WriteLine(Example1());
            //Console.WriteLine(Example2());
            ////Console.WriteLine(Example3());
            ////Console.WriteLine(Example4());
            ////Console.WriteLine(Example5());
            //Console.WriteLine(Example6());
            
            //test_parse("test.txt");
            //test_parse("test2.txt");
            //test_safe_parse("test.txt");
            //test_safe_parse("test2.txt");
#endif

#if false
            parse_and_write("test.txt", "test-output1.txt");
            parse_and_write("test-output1.txt", "test-output2.txt");
            compare_files("test.txt", "test-output1.txt");
            compare_files("test-output1.txt", "test-output2.txt");
            compare_logs("test.txt", "test-output1.txt");
            compare_logs("test-output1.txt", "test-output2.txt");
            parse_build_write("test.txt", "test-output3.txt");
            compare_files("test.txt", "test-output3.txt");
            compare_logs("test.txt", "test-output3.txt");
            compare_logs2("test.txt", "test-output3.txt");
            compare_logs("test.txt");
            compare_logs("test-output1.txt");
            compare_logs("test-output2.txt");
            compare_logs("test-output3.txt");
            compare_logs("test.txt", "test2.txt");
            code_builder_test(code_builder_test1());
            code_builder_test(code_builder_test2());
#endif
#if false
            using (FileWriter writer = new FileWriter("construct1.txt")) {
                Code c = code_builder_test2();
                Dictionary<string, string> aliases = new Dictionary<string, string>();
                //aliases.Add("Sexp.Cons", "cons");
                //aliases.Add("System.Object[]", "vector");
                List<string> ns = new List<string>();
                //                ns.Add("Sexp");
                //                object[] test1 = build("test.txt");
                //                object[] test1 = ConstructLang.tour(build("test.txt"), ns, aliases);
                ns.Add("Flat");

                aliases.Add("Flat.Type", ".type");
                aliases.Add("Flat.Prototype", ".prototype");
                aliases.Add("Flat.Operator", ".operator");
                aliases.Add("Flat.Relation", ".relation");
                aliases.Add("Flat.Constant", ".constant");
                aliases.Add("Flat.Global", ".global");

                aliases.Add("Flat.Local", ".local");
                aliases.Add("Flat.Parameter", ".param");

                aliases.Add("Flat.Do", ".do");
                aliases.Add("Flat.Move", ".move");
                aliases.Add("Flat.Call", ".call");
                aliases.Add("Flat.DoLambda", ".gosub");
                aliases.Add("Flat.If", ".if");

                aliases.Add("Flat.Lambda", ".lambda");
                aliases.Add("Flat.Operands", "<-");
                aliases.Add("Flat.Lvalues", "->");

                aliases.Add("Flat.Types", "types:");
                aliases.Add("Flat.Prototypes", "prototypes:");
                aliases.Add("Flat.Constants", "constants:");
                aliases.Add("Flat.Globals", "globals:");
                aliases.Add("Flat.Operators", "operators:");
                aliases.Add("Flat.Relations", "relations:");
                aliases.Add("Flat.Lambdas", "lambdas:");
                aliases.Add("Flat.Listing", "listing:");

                object[] test1 = ConstructLang.tour(c, ns, aliases);
                //VectorLogger logger = new VectorLogger(new Log(), new TxtLocation(""), GetTopLevelWriter.create(writer));
                Format fmt = new Format();
                //fmt.format_vect = true;
                //fmt.format_data = false;
                //fmt.format_head = false;
                //fmt.format_appl = true;
                //fmt.do_abbrev = true;
                //fmt.do_abbrev = false;
                //fmt.do_debug = true;
                ConstructFormat.init(writer);
                DynamicVisitor.accept(test1, GetTopLevelWriter.create(writer, fmt, ConstructFormat.get_construct_lang_config));
            }
            //            int foo = 2+2;
#endif
#if true
            //System.IO.TextWriter my_out = new System.IO.StreamWriter("output.txt");
            //Console.SetOut(my_out);

            Reader f = new Reader("test4.txt");
            Util.TxtLocation loc = new Util.TxtLocation("test4.txt");
            Parser p = new Parser(f, new SafeVectorVisitor(new Interpreter(new StandardEnvironment(null/*delegate(Symbol sym, out object def) { def = null; return true; }*/), loc)), loc);
            p.Read();

            Console.WriteLine();

            //object[] v = build("test.txt");
            //DynamicVisitor.accept(v, new IntVisitor());
            //DynamicVisitor.accept(v, new Interpreter(new TestEnvironment(), null));
#endif
        }
    }
}
